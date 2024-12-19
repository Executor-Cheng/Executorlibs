using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Parsing.Context;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public class DefaultRawdataDispatcher<TClient, TRawdata> : RawdataDispatcher<TClient, TRawdata> where TClient : IMessageClient
    {
        protected readonly IParsingContext<TClient, TRawdata>[] _contexts;

        public DefaultRawdataDispatcher(IEnumerable<IParsingContext<TClient, TRawdata>> contexts) : this(contexts is IParsingContext<TClient, TRawdata>[] array ? array : contexts.ToArray())
        {
            
        }

        protected DefaultRawdataDispatcher(IParsingContext<TClient, TRawdata>[] contexts)
        {
            _contexts = contexts;
        }

        public override Task HandleRawdataAsync(TClient client, TRawdata rawdata)
        {
            var contexts = _contexts;
            int index = 0;
            while (index < contexts.Length)
            {
                var context = contexts[index];
                var task = context.InvokeAsync(client, rawdata);
                index++;
#if NETSTANDARD2_0
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
#else
                if (!task.IsCompletedSuccessfully)
#endif
                {
                    if (index == contexts.Length)
                    {
                        return task;
                    }
                    return HandleRawdataSlowlyAsync(client, rawdata, index, task);
                }
            }
            return Task.CompletedTask;
        }

        protected async Task HandleRawdataSlowlyAsync(TClient client, TRawdata rawdata, int i, Task task)
        {
            await task;
            var contexts = _contexts;
            while (i < contexts.Length)
            {
                var context = contexts[i];
                await context.InvokeAsync(client, rawdata);
                i++;
            }
        }
    }

    public class DefaultRawdataDispatcher<TClient, TRawdata, TContext> : DefaultRawdataDispatcher<TClient, TRawdata> where TClient : IMessageClient
                                                                                                                     where TContext : class, IParsingContext<TClient, TRawdata>
    {
        public DefaultRawdataDispatcher(IEnumerable<TContext> contexts) : base(contexts)
        {

        }
    }
}
