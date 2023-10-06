using System;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Extensions;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Executorlibs.Examples
{
    public class BilibiliDanmakuProtocolTest
    {
        public static Task Main()
        {
            IHostBuilder hb = Host.CreateDefaultBuilder()
                                  .ConfigureLogging((ILoggingBuilder factory) =>
                                  {
                                      // factory.ClearProviders();
                                      factory.AddSimpleConsole(options =>
                                      {
                                          options.IncludeScopes = false;
                                          options.TimestampFormat = "[HH:mm:ss]";
                                      });
                                      factory.SetMinimumLevel(LogLevel.Information);
                                  })
                                  .ConfigureServices((context, services) =>
                                  {
                                      services.AddBilibiliDanmakuFramework().AddDefaultRawdataDispatcher(rawContext =>
                                      {
                                          rawContext.WithDefault()
                                                    .WithDefaultDispatcher()
                                                    .WithDefaultSubscription()
                                                    .WithMessage<IPopularityMessage>(
                                                        parser => parser.AddComponent<PopularityParser>(),
                                                        handler => handler.AddComponent<PopularityMesssageHandler>());

                                          rawContext.TransistToJson(dispatcher =>
                                          {
                                              dispatcher.AddDefault(jsonContext =>
                                              {
                                                  jsonContext.WithDefault()
                                                             .WithDefaultDispatcher()
                                                             .WithDefaultSubscription()
                                                             .WithMessage<IDanmakuMessage>(
                                                                 parser => parser.AddComponent<DanmakuParser>(),
                                                                 handler => handler.AddComponent<DanmakuMessageHandler>())
                                                             .WithMessage<IUnknownJsonMessage>(
                                                                 parser => parser.AddComponent<UnknownJsonMessageParser>(),
                                                                 handler => handler.AddComponent<UnknownMessageHandler>());
                                              });
                                          });
                                                        
                                      })
                                      .WithDanmakuCredentialProvider().AddComponent<DanmakuServerProvider>().Builder
                                      .AddClient<TcpDanmakuClientV3>();

                                      services.AddHostedService<TestHostedService>();
                                      services.AddHttpClient(); 
                                  });
            return hb.RunConsoleAsync();
        }
    }

    public class TestHostedService : BackgroundService
    {
        private readonly IServiceScope _scope;

        private readonly IDanmakuClient _client;

        public TestHostedService(IServiceProvider services)
        {
            _scope = services.CreateScope();
            services = _scope.ServiceProvider;
            _client = services.GetRequiredService<IDanmakuClient>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new DanmakuClientOptions(5096, TimeSpan.FromSeconds(30));
            return _client.ConnectAsync(options, stoppingToken);
        }
    }

    //[RegisterBilibiliParser(typeof(DanmakuParser))] // 你现在需要框架给你推送 IDanmakuMessage 类型的消息, 那你就注册相应的 Parser
    public class DanmakuMessageHandler : BilibiliMessageHandler<IDanmakuMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                         IBilibiliMessageHandler<IDanmakuMessage>
    {
        private readonly ILogger<DanmakuMessageHandler> _logger;

        public DanmakuMessageHandler(ILogger<DanmakuMessageHandler> logger) // 框架使用依赖注入进行实例化
        {
            _logger = logger;
        }

        public override Task HandleMessageAsync(IDanmakuClient client, IDanmakuMessage message)
        {
            _logger.LogInformation((int)client.RoomId, $"{message.Time:yyyy-MM-dd HH:mm:ss} {message.UserName}[{message.UserId}]:{message.Comment}");
            return Task.CompletedTask;
        }
    }

    public class SendGiftMessageHandler : BilibiliMessageHandler<ISendGiftMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                          IBilibiliMessageHandler<ISendGiftMessage> 
    {
        private readonly ILogger<SendGiftMessageHandler> _logger;

        public SendGiftMessageHandler(ILogger<SendGiftMessageHandler> logger) // 框架使用依赖注入进行实例化
        {
            _logger = logger;
        }

        public override Task HandleMessageAsync(IDanmakuClient client, ISendGiftMessage message)
        {
            _logger.LogInformation((int)client.RoomId, $"{message.Time:yyyy-MM-dd HH:mm:ss} {message.UserName}[{message.UserId}]:{message.GiftName}x{message.GiftCount}");
            return Task.CompletedTask;
        }
    }

    public class GuardBuyMessageHandler : BilibiliMessageHandler<IGuardBuyMessage>
    {
        public override Task HandleMessageAsync(IDanmakuClient client, IGuardBuyMessage message)
        {
            throw new NotImplementedException();
        }
    }

    public class PopularityMesssageHandler : BilibiliMessageHandler<IPopularityMessage>
    {
        private readonly ILogger<PopularityMesssageHandler> _logger;

        public PopularityMesssageHandler(ILogger<PopularityMesssageHandler> logger)
        {
            _logger = logger;
        }

        public override Task HandleMessageAsync(IDanmakuClient client, IPopularityMessage message)
        {
            _logger.LogInformation(message.Popularity.ToString());
            return Task.CompletedTask;
        }
    }

    public class UnknownMessageHandler : BilibiliMessageHandler<IUnknownJsonMessage>
    {
        private readonly ILogger<UnknownMessageHandler> _logger;

        public UnknownMessageHandler(ILogger<UnknownMessageHandler> logger) // 框架使用依赖注入进行实例化
        {
            _logger = logger;
        }

        public override Task HandleMessageAsync(IDanmakuClient client, IUnknownJsonMessage message)
        {
            _logger.LogInformation(message.Rawdata.GetRawText());
            return Task.CompletedTask;
        }
    }

    public class DisconnectMessageHandler : BilibiliMessageHandler<IDisconnectedMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                            IBilibiliMessageHandler<IDisconnectedMessage> // 位于 Executorlibs.Bilibili.Protocol.Models.General 下的消息不需要注册 Parser
    {
        public override async Task HandleMessageAsync(IDanmakuClient client, IDisconnectedMessage message)
        {
            while (true) // 无限重连
            {
                try
                {
                    await client.ConnectAsync(message.ClientOptions);
                    return;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception) // 其它异常就丢掉, 你也可以自己魔改
                {
                    await Task.Delay(1000); // 等1秒
                }
            }
        }
    }
}
