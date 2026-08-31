using System;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients.Coding;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Bilibili.Protocol.Options;

namespace Executorlibs.Bilibili.Protocol.Clients.Contexts
{
    public interface IDanmakuEventContext
    {
        void HandleDisconnect(Exception exception);

        Task HandlePacketAsync(byte[] buffer, uint length);
    }

    public abstract class DanmakuEventContext : IDanmakuEventContext
    {
        public abstract void HandleDisconnect(Exception exception);

        public abstract Task HandlePacketAsync(byte[] buffer, uint length);
    }

    public class DanmakuDecodingContext<TEvent, TDecoder> : DanmakuEventContext where TEvent : IDanmakuEventContext where TDecoder : PayloadDecoder
    {
        public TEvent Context { get; }

        public TDecoder Decoder { get; }

        protected DanmakuDecodingContext(TEvent context, TDecoder decoder)
        {
            Context = context;
            Decoder = decoder;
        }

        public override void HandleDisconnect(Exception exception)
        {
            Context.HandleDisconnect(exception);
        }

        public sealed override Task HandlePacketAsync(byte[] buffer, uint length)
        {
            if (Decoder.TryOpen(buffer))
            {
                return LoopDecodePacketAsync();
            }
            return Context.HandlePacketAsync(buffer, length);
        }

        private async Task LoopDecodePacketAsync()
        {
            try
            {
                while (Decoder.TryProcess(out var decodedRawdata))
                {
                    await Context.HandlePacketAsync(decodedRawdata, (uint)decodedRawdata.Length);
                }
            }
            finally
            {
                Decoder.Close();
            }
        }
    }

    public interface IDanmakuAdapterContext<out TTransport> : IDisposable where TTransport : DanmakuTransport
    {
        TTransport Transport { get; }
    }

    public abstract class DanmakuAdapterContext<TTransport> : IDanmakuAdapterContext<TTransport> where TTransport : DanmakuTransport
    {
        public TTransport Transport { get; }

        protected DanmakuAdapterContext(TTransport transport)
        {
            Transport = transport;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Transport.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class DanmakuClientContext<TTransport> : DanmakuAdapterContext<TTransport> where TTransport : DanmakuTransport
    {
        public DanmakuClientOptions Options { get; }

        public DanmakuServerInfo ServerInfo { get; }

        public DanmakuClientContext(TTransport transport, DanmakuClientOptions options, DanmakuServerInfo serverInfo) : base(transport)
        {
            Options = options;
            ServerInfo = serverInfo;
        }
    }

    public class DanmakuServerContext<TTransport> : DanmakuAdapterContext<TTransport> where TTransport : DanmakuTransport
    {
        public DanmakuServerOptions Options { get; }

        public DanmakuServerContext(TTransport transport, DanmakuServerOptions options) : base(transport)
        {
            Options = options;
        }
    }
}
