using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Extensions;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Executorlibs.Shared.Extensions;
using Executorlibs.Shared.Exceptions;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;
using Executorlibs.TarsProtocol.Models.Primitives;

namespace Executorlibs.Examples
{
    public class ExecutorDanmakuServerProvider : DanmakuServerProvider
    {
        public ExecutorDanmakuServerProvider(HttpClient client) : base(client)
        {
            
        }

        public override async Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(uint roomId, CancellationToken token = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.live.bilibili.com/xlive/web-room/v1/index/getDanmuInfo?id={roomId}&type=0");
            using JsonDocument j = await _client.SendAsync(req, token).GetJsonAsync(token);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 0)
            {
                JsonElement data = root.GetProperty("data"),
                            server = data.GetProperty("host_list");
                return new DanmakuServerInfo(server.EnumerateArray().Select(p => new DanmakuServerHostInfo(
                    p.GetProperty("host").GetString()!,
                    p.GetProperty("port").GetInt32(),
                    p.GetProperty("ws_port").GetInt32(),
                    p.GetProperty("wss_port").GetInt32()
                    )).Where(p => p.Host != "broadcastlv.chat.bilibili.com").ToArray(), 0, data.GetProperty("token").GetString()!);
            }
            throw new UnknownResponseException(in root);
        }
    }

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
                                      services.AddScoped<DanmakuMessageHandler>();

                                      services.AddBilibiliDanmakuFramework().AddDefaultRawdataDispatcher(rawContext =>
                                      {
                                          //rawContext.WithDefault()
                                          //          .WithDefaultDispatcher()
                                          //          .WithDefaultSubscription()
                                          //          .WithMessage<IPopularityMessage>(
                                          //              parser => parser.AddComponent<PopularityParser>(),
                                          //              handler => handler.AddComponent<PopularityMesssageHandler>());

                                          rawContext.TransistToJson(dispatcher =>
                                          {
                                              dispatcher.AddDefault(jsonContext =>
                                              {
                                                  jsonContext.WithDefault()
                                                             .AddDefaultDispatcher()
                                                             .AddDefaultSubscription()
                                                             .AddMessage<IDanmakuMessage>(
                                                                 parser => parser.AddComponent<DanmakuParser>(),
                                                                 handler => handler.AddComponent(services => services.GetRequiredService<DanmakuMessageHandler>()))
                                                             //.WithMessage<IUnknownJsonMessage>(
                                                             //    parser => parser.AddComponent<UnknownJsonMessageParser>(),
                                                             //    handler => handler.AddComponent<UnknownMessageHandler>())
                                                             ;
                                              });
                                          });
                                                        
                                      })
                                      .WithDanmakuCredentialProvider().AddComponent<ExecutorDanmakuServerProvider>().Builder
                                      .AddClient<TcpDanmakuClientV3>();

                                      services.AddHostedService<TestHostedService>();
                                      services.AddHttpClient(); 
                                  });
            return hb.RunConsoleAsync();
        }
    }

    public class TestHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public TestHostedService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _services.CreateScope();
            var services = scope.ServiceProvider;
            var options = new DanmakuClientOptions((uint)5096, TimeSpan.FromSeconds(30));
            var client = services.GetRequiredService<IDanmakuClient>();
            await client.ConnectAsync(options, stoppingToken);
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

    public static partial class BiliApis
    {
        public static HttpClient Client { get; } = new HttpClient();

        static BiliApis()
        {
            Client.DefaultRequestHeaders.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66");
            Client.DefaultRequestHeaders.SetAccept("*/*");
            Client.DefaultRequestHeaders.SetAcceptLanguage("zh-CN,zh;q=0.9");
        }

        private static void CheckGetAreaRooms(in JsonElement root)
        {
            if (root.GetProperty("code").GetInt32() != 0)
            {
                throw new UnknownResponseException(in root);
            }
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(int areaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsync(Client, areaId, sorting, token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(HttpClient client, int areaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(client, areaId, sorting, token).ToArrayAsync(token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(int areaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsync(Client, page, pageSize, areaId, sorting, token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(HttpClient client, int areaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(client, areaId, page, pageSize, sorting, token).ToArrayAsync(token);
        }

        public static IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(int areaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(Client, page, pageSize, areaId, sorting, token);
        }

        public static async IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(HttpClient client, int areaId, int page, int pageSize, string sorting = "online", [EnumeratorCancellation] CancellationToken token = default)
        {
            using JsonDocument j = await client.GetAsync($"https://api.live.bilibili.com/room/v1/Area/getListByAreaID?areaId={areaId}&sort={sorting}&page={page}&pageSize={pageSize}", token).GetJsonAsync(token);
            JsonElement root = j.RootElement;
            CheckGetAreaRooms(in root);
            foreach (JsonElement roomToken in root.GetProperty("data").EnumerateArray())
            {
                yield return roomToken.GetProperty("roomid").GetInt32();
            }
        }

        public static IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(int areaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(Client, areaId, sorting, token);
        }

        public static async IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(HttpClient client, int areaId, string sorting = "online", [EnumeratorCancellation] CancellationToken token = default)
        {
            for (int page = 1; ; page++)
            {
                long ticks = Stopwatch.GetTimestamp();
                using JsonDocument j = await client.GetAsync($"https://api.live.bilibili.com/room/v1/Area/getListByAreaID?areaId={areaId}&sort={sorting}&page={page}&pageSize=200", token).GetJsonAsync(token);
                JsonElement root = j.RootElement;
                CheckGetAreaRooms(in root);
                JsonElement list = root.GetProperty("data");
                if (!list.HasValues())
                {
                    yield break;
                }
                foreach (JsonElement roomToken in list.EnumerateArray())
                {
                    yield return roomToken.GetProperty("roomid").GetInt32();
                }
                TimeSpan ts = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond * 5 - (Stopwatch.GetTimestamp() - ticks) * ((double)TimeSpan.TicksPerSecond / Stopwatch.Frequency)));
                if (ts > TimeSpan.Zero)
                {
                    await Task.Delay(ts, token);
                }
            }
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(int areaId, int subAreaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsync(Client, areaId, subAreaId, sorting, token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(HttpClient client, int areaId, int subAreaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(client, areaId, subAreaId, sorting, token).ToArrayAsync(token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(int areaId, int subAreaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsync(Client, page, pageSize, areaId, subAreaId, sorting, token);
        }

        public static ValueTask<int[]> GetAreaRoomsAsync(HttpClient client, int areaId, int subAreaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(client, areaId, subAreaId, page, pageSize, sorting, token).ToArrayAsync(token);
        }

        public static IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(int areaId, int subAreaId, int page, int pageSize, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(Client, page, pageSize, areaId, subAreaId, sorting, token);
        }

        public static async IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(HttpClient client, int areaId, int subAreaId, int page, int pageSize, string sorting = "online", [EnumeratorCancellation] CancellationToken token = default)
        {
            using JsonDocument j = await client.GetAsync($"https://api.live.bilibili.com/room/v3/area/getRoomList?platform=web&parent_area_id={areaId}&cate_id=0&area_id={subAreaId}&sort_type={sorting}&page={page}&page_size={pageSize}&tag_version=1", token).GetJsonAsync(token);
            JsonElement root = j.RootElement;
            CheckGetAreaRooms(in root);
            foreach (JsonElement roomToken in root.GetProperty("data").GetProperty("list").EnumerateArray())
            {
                yield return roomToken.GetProperty("roomid").GetInt32();
            }
        }

        public static IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(int areaId, int subAreaId, string sorting = "online", CancellationToken token = default)
        {
            return GetAreaRoomsAsyncEnumerable(Client, areaId, subAreaId, sorting, token);
        }

        public static async IAsyncEnumerable<int> GetAreaRoomsAsyncEnumerable(HttpClient client, int areaId, int subAreaId, string sorting = "online", [EnumeratorCancellation] CancellationToken token = default)
        {
            for (int page = 1; ; page++)
            {
                using JsonDocument j = await client.GetAsync($"https://api.live.bilibili.com/room/v3/area/getRoomList?platform=web&parent_area_id={areaId}&cate_id=0&area_id={subAreaId}&sort_type={sorting}&page={page}&page_size=99&tag_version=1", token).GetJsonAsync(token);
                JsonElement root = j.RootElement;
                CheckGetAreaRooms(in root);
                JsonElement list = root.GetProperty("data").GetProperty("list");
                if (!list.HasValues())
                {
                    yield break;
                }
                foreach (JsonElement roomToken in list.EnumerateArray())
                {
                    yield return roomToken.GetProperty("roomid").GetInt32();
                }
            }
        }
    }
}
