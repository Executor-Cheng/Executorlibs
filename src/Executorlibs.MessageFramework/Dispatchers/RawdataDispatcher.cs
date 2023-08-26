using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public interface IRawdataDispatcher<in TClient, TRawdata> where TClient : IMessageClient
    {
        Task HandleRawdataAsync(TClient client, TRawdata rawdata);
    }

    public abstract class RawdataDispatcher<TClient, TRawdata> : IRawdataDispatcher<TClient, TRawdata> where TClient : IMessageClient
    {
        protected RawdataDispatcher()
        {

        }

        public abstract Task HandleRawdataAsync(TClient client, TRawdata rawdata);
    }
}
