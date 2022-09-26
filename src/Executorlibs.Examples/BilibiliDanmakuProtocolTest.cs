using System;
using System.IO;
using System.Net.Http;
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
using Executorlibs.NeteaseMusic.Apis;
using Executorlibs.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Executorlibs.Examples
{
    public class BilibiliDanmakuProtocolTest
    {
        private static string Cookie { get; } =                                                                                                         "_ntes_nuid=7113a7ac97c41c67a4698b570930edc0; NMTID=00ObHG7OMJIDQGtfUkHhs7kurg9kbwAAAF37iEvDw; WM_TID=NjN%2BO2vdTbFEBBVUUBdqf7i9WRYfcNak; WEVNSM=1.0.0; WNMCID=vzntjy.1622821015762.01.0; _ntes_nnid=7113a7ac97c41c67a4698b570930edc0,1646194922335; Qs_lvt_382223=1646021654%2C1648988455; Qs_pv_382223=1449800791018867700%2C4582513436630403000; __csrf=f8d7994f23adfd686fd02cdd60616e5a; MUSIC_U=92dd6bb1b4e19e1fec68a3bd9b780b5ac30606117bcaa8bc4c3e46d40bf89288519e07624a9f0053363000983ab9fbe22bccb84fba7096fed1823d8c0566231a79c79f5283b17b23a0d2166338885bd7; _iuqxldmzr_=32; JSESSIONID-WYYY=Zm3oyb6oZWq%2BFusTJ%2Bst22jOk8CBzpfY%5C7AVMTTZh1F2RMoQ7VjoawJ5nbQ2l6T%5CIpmgd0jo3cobQlUeaeQEk0mFCj%5C%5CHiYv9mlQ700kd8yE4lBo6jchZ8cYmGrBiboYub7o0llTnNNhkIaaIAof3rJiKMkTaSpv8Xo%2Bo%2FYq53yzwI8I%3A1659495187266; WM_NI=NP6Ye09L8EK3FtpXpqoKt7mY162gdE7RHJo2DAaQ1SHduMRlCJq48qwLSHCRfsYXPsvrV8l6wvOgO5KvOygYm8Fgm8P4TUprla6gm8D%2FRVLFtKBfd%2F79Ey0mvHJ1tAbnRks%3D; WM_NIKE=9ca17ae2e6ffcda170e2e6eed0ed6b9ab6c098f84997ac8fb6d14e928a8eb1d84995b488d1c57f918b9fa2b32af0fea7c3b92a8e90fabae83cfb94f8a6ca3986acc0b9f37bad95bcd8f053f3eafe88e74ff58e88abee3a889fafb2b4598cbcbbaed1688196bfd5bb7bb5949da9c25aa6ad97b9d379b6ae9ad9ef3eb5aa8188ce3f89aa819ae653afe88a88f03494ec9cb6f37ab0bffa88f03488f19eb7cf4eb8958e87e725f5e78b9ab367b69886d2f74ba2b29e8cf237e2a3; playerid=22214454";







        public static async Task Main()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            Task<Stream?> t1 = Task.FromException<Stream?>(new InvalidOperationException());
            Task<MemoryStream?> t2 = t1.Cast<Stream, MemoryStream>();
            MemoryStream? s = await t2;
            ;

















            //SocketsHttpHandler handler = new SocketsHttpHandler();
            //handler.CookieContainer = new System.Net.CookieContainer();
            ////handler.CookieContainer.SetCookies(new Uri("https://music.163.com/"), Cookie.Replace(';', ','));
            //HttpClient client = new HttpClient(handler);
            //var date = await NeteaseMusicApis.GetNeteaseMusicUserBirthDayAsync(client, 415275006);
            //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.53 Safari/537.36 Edg/103.0.1264.37");
            //client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            //var x = await NeteaseMusicApis.SearchSongsAsync(client, "しゃろう - 2_23 AM");
            //;
        }

        public static Task Main2()
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
                                              .AddCredentialProvider<DanmakuServerProvider>()
                                              .AddInvoker<BilibiliMessageHandlerInvoker>()
                                              .AddHandler<DanmakuMessageHandler>()
                                              .AddHandler<SendGiftMessageHandler>()
                                              .AddHandler<DisconnectMessageHandler>()
                                              .AddClient<TcpDanmakuClientV2>();
#else
                                      var builder = services.AddBilibiliDanmakuFramework(); // 由于 <.NET 5.0 的运行时不支持协变返回, 所以不能像上边那样链式调用
                                      builder.AddCredentialProvider<DanmakuServerProvider>(); // 需要一个连接时获取所需信息的提供类, 只注册一次
                                      builder.AddInvoker<BilibiliMessageHandlerInvoker>();  // 必须注册一个消息推送器, 只注册一次
                                      builder.AddHandler<DanmakuMessageHandler>(ServiceLifetime.Singleton);    // 注册你自己定义的消息处理类
                                      builder.AddHandler<SendGiftMessageHandler>(ServiceLifetime.Singleton);   // 同上, 可注册多次
                                      builder.AddHandler<DisconnectMessageHandler>(ServiceLifetime.Singleton); // 同上
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
            services.GetRequiredService<IOptionsSnapshot<DanmakuClientOptions>>().Value.RoomId = 34348; // 使用 DanmakuClientOptions 为 IDanmakuClient 提供房间号
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

        public override Task HandleMessageAsync(IDanmakuClient client, IDanmakuMessage message)
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

        public override Task HandleMessageAsync(IDanmakuClient client, ISendGiftMessage message)
        {
            _logger.LogInformation(client.RoomId, $"{message.Time:yyyy-MM-dd HH:mm:ss} {message.UserName}[{message.UserId}]:{message.GiftName}x{message.GiftCount}");
            return Task.CompletedTask;
        }
    }

    public class DisconnectMessageHandler : BilibiliMessageHandler<IDisconnectedMessage>, // 请使用.NET Standard2.0 的用户继承此类, 更高版本的可以只实现接口
                                            IInvarianceBilibiliMessageHandler<IDisconnectedMessage> // 位于 Executorlibs.Bilibili.Protocol.Models.General 下的消息不需要注册 Parser
    {
        public override async Task HandleMessageAsync(IDanmakuClient client, IDisconnectedMessage message)
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
