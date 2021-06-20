using System;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Builders;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Invokers;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Bilibili.Protocol.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Executorlibs.Examples
{
    public class BilibiliDanmakuProtocolTest
    {
        public static Task Main()
        {
            IHostBuilder hb = Host.CreateDefaultBuilder()
                                  .ConfigureLogging((ILoggingBuilder factory) =>
                                  {
                                      factory.AddSimpleConsole(options =>
                                      {
                                          options.IncludeScopes = false;
                                          options.TimestampFormat = "[HH:mm:ss]";
                                      });
                                      factory.SetMinimumLevel(LogLevel.Trace);
                                  })
                                  .ConfigureServices((context, services) =>
                                  {
#if NET5_0_OR_GREATER
                                      services.AddBilibiliDanmakuFramework()
                                              .AddInvoker<BilibiliMessageHandlerInvoker>()
                                              .AddHandler<DanmakuMessageHandler>()
                                              .AddHandler<SendGiftMessageHandler>()
                                              .AddHandler<DisconnectMessageHandler>()
                                              .AddCredentialProvider<DanmakuServerProvider>()
                                              .AddClient<TcpDanmakuClientV2>();
#else
                                      var builder = services.AddBilibiliDanmakuFramework(); // 由于 <.NET 5.0 的运行时不支持协变返回, 所以不能像上边那样链式调用
                                      builder.AddInvoker<BilibiliMessageHandlerInvoker>();  // 必须注册一个消息推送器, 只注册一次
                                      builder.AddHandler<DanmakuMessageHandler>(ServiceLifetime.Singleton);    // 注册你自己定义的消息处理类
                                      builder.AddHandler<SendGiftMessageHandler>(ServiceLifetime.Singleton);   // 同上, 可注册多次
                                      builder.AddHandler<DisconnectMessageHandler>(ServiceLifetime.Singleton); // 同上
                                      builder.AddCredentialProvider<DanmakuServerProvider>(); // 需要一个连接时获取所需信息的提供类, 只注册一次
                                      builder.AddClient<TcpDanmakuClientV2>();                // 注册一个弹幕客户端, 只注册一次
#endif
                                      services.Configure<DanmakuClientOptions>(options => { options.HeartbeatInterval = TimeSpan.FromSeconds(30); }); // 可选, 修改弹幕客户端的默认选项
                                      services.AddHostedService<TestHostedService>();
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
            services.GetRequiredService<IOptionsSnapshot<DanmakuClientOptions>>().Value.RoomId = 7734200; // 使用 DanmakuClientOptions 为 IDanmakuClient 提供房间号
            _client = services.GetRequiredService<IDanmakuClient>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.ConnectAsync(stoppingToken);
        }
    }

    [RegisterBilibiliParser(typeof(DanmakuParser))] // 你现在需要框架给你推送 IDanmakuMessage 类型的消息, 那你就注册相应的 Parser
    public class DanmakuMessageHandler : BilibiliMessageHandler<IDanmakuMessage>,          // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                         IInvarianceBilibiliMessageHandler<IDanmakuMessage> // 这个接口表示仅处理 IDanmakuMessage 类型的消息
                                                                                            // 不处理比其派生程度更高的消息
    {
        private readonly ILogger<DanmakuMessageHandler> _logger;

        public DanmakuMessageHandler(ILogger<DanmakuMessageHandler> logger) // 框架使用依赖注入进行实例化
        {
            _logger = logger;
        }

        public override Task HandleMessage(IDanmakuClient client, IDanmakuMessage message)
        {
            _logger.LogInformation(client.RoomId, $"{message.Time:yyyy-MM-dd HH:mm:ss} {message.UserName}[{message.UserId}]:{message.Comment}");
            return Task.CompletedTask;
        }
    }

    [RegisterBilibiliParser(typeof(SendGiftParser))] // 你现在需要框架给你推送 ISendGiftMessage 类型的消息, 那你就注册相应的 Parser
                                                     // 但是实现的接口是 IContravarianceBilibiliMessageHandler<ISendGiftMessage>
                                                     // 所以你可以注册处理比 ISendGiftMessage 派生程度更高消息的 Parser
    public class SendGiftMessageHandler : BilibiliMessageHandler<ISendGiftMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                          IContravarianceBilibiliMessageHandler<ISendGiftMessage> // 这个接口表示处理 ISendGiftMessage 类型的消息
                                                                                                  // 也处理比其派生程度更高的消息
    {
        private readonly ILogger<SendGiftMessageHandler> _logger;

        public SendGiftMessageHandler(ILogger<SendGiftMessageHandler> logger) // 框架使用依赖注入进行实例化
        {
            _logger = logger;
        }

        public override Task HandleMessage(IDanmakuClient client, ISendGiftMessage message)
        {
            _logger.LogInformation(client.RoomId, $"{message.Time:yyyy-MM-dd HH:mm:ss} {message.UserName}[{message.UserId}]:{message.GiftName}x{message.GiftCount}");
            return Task.CompletedTask;
        }
    }

    public class DisconnectMessageHandler : BilibiliMessageHandler<IDisconnectedMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                            IInvarianceBilibiliMessageHandler<IDisconnectedMessage> // 位于 Executorlibs.Bilibili.Protocol.Models.General 下的消息不需要注册 Parser
    {
        public override async Task HandleMessage(IDanmakuClient client, IDisconnectedMessage message)
        {
            while (true) // 无限重连
            {
                try
                {
                    await client.ConnectAsync();
                    return;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception) // 其它异常就丢掉, 你也可以自己魔改
                {
                    try
                    {
                        await Task.Delay(1000, message.Token); // 等1秒
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }
    }
}
