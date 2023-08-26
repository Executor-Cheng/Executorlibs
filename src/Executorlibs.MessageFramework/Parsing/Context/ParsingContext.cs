using System;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;

namespace Executorlibs.MessageFramework.Parsing.Context
{
    public interface IParsingContext<in TClient, TRawdata> where TClient : IMessageClient
    {
        bool CanParse(TRawdata rawdata);

        Task InvokeAsync(TClient client, TRawdata rawdata);
    }

    public abstract class ParsingContext<TClient, TRawdata> : IParsingContext<TClient, TRawdata> where TClient : IMessageClient
    {
        public abstract bool CanParse(TRawdata rawdata);

        public abstract Task InvokeAsync(TClient client, TRawdata rawdata);
    }
}
