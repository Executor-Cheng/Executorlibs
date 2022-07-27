using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Routing.Matching
{
    public sealed class ModifiedAcceptsMatcherPolicy : MatcherPolicy, IEndpointComparerPolicy, INodeBuilderPolicy, IEndpointSelectorPolicy
    {
        public static Type AcceptsMatcherPolicyType => _acceptsMatcherPolicyType;

        private static readonly Type _acceptsMatcherPolicyType;

        public override int Order => _acceptsMatcherPolicy.Order;

        public IComparer<Endpoint> Comparer => EndpointComparerPolicy.Comparer;

        private readonly MatcherPolicy _acceptsMatcherPolicy;

        private ref IEndpointComparerPolicy EndpointComparerPolicy => ref Unsafe.As<MatcherPolicy, IEndpointComparerPolicy>(ref Unsafe.AsRef(in _acceptsMatcherPolicy));

        private ref INodeBuilderPolicy NodeBuilderPolicy => ref Unsafe.As<MatcherPolicy, INodeBuilderPolicy>(ref Unsafe.AsRef(in _acceptsMatcherPolicy));

        private ref IEndpointSelectorPolicy EndpointSelectorPolicy => ref Unsafe.As<MatcherPolicy, IEndpointSelectorPolicy>(ref Unsafe.AsRef(in _acceptsMatcherPolicy));

        static ModifiedAcceptsMatcherPolicy()
        {
            _acceptsMatcherPolicyType = typeof(IEndpointComparerPolicy).Assembly.GetType("Microsoft.AspNetCore.Routing.Matching.AcceptsMatcherPolicy")!;
        }

        public ModifiedAcceptsMatcherPolicy(IServiceProvider services)
        {
            _acceptsMatcherPolicy = (MatcherPolicy)services.GetRequiredService(_acceptsMatcherPolicyType);
        }

        bool INodeBuilderPolicy.AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            return NodeBuilderPolicy.AppliesToEndpoints(endpoints);
        }

        bool IEndpointSelectorPolicy.AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            return EndpointSelectorPolicy.AppliesToEndpoints(endpoints);
        }

        public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            if (IsBodilessRequest(httpContext))
            {
                return Task.CompletedTask;
            }
            return EndpointSelectorPolicy.ApplyAsync(httpContext, candidates);
        }

        public PolicyJumpTable BuildJumpTable(int exitDestination, IReadOnlyList<PolicyJumpTableEdge> edges)
        {
            return new ModifiedConsumesPolicyJumpTable(NodeBuilderPolicy.BuildJumpTable(exitDestination, edges));
        }

        public IReadOnlyList<PolicyNodeEdge> GetEdges(IReadOnlyList<Endpoint> endpoints)
        {
            return NodeBuilderPolicy.GetEdges(endpoints);
        }

        private static bool IsBodilessRequest(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            return HttpMethods.IsGet(method) ||
                   HttpMethods.IsOptions(method) ||
                   HttpMethods.IsDelete(method) ||
                   HttpMethods.IsHead(method) ||
                   HttpMethods.IsTrace(method);
        }

        private sealed class ModifiedConsumesPolicyJumpTable : PolicyJumpTable
        {
            private static readonly FieldInfo _destinationsField;

            static ModifiedConsumesPolicyJumpTable()
            {
                Type consumesPolicyJumpTable = AcceptsMatcherPolicyType.GetNestedType("ConsumesPolicyJumpTable", BindingFlags.NonPublic)!;
                _destinationsField = consumesPolicyJumpTable.GetField("_destinations", BindingFlags.Instance | BindingFlags.NonPublic)!;
            }

            private readonly PolicyJumpTable _consumesPolicyJumpTable;

            private readonly int _bodilessDestination;

            public ModifiedConsumesPolicyJumpTable(PolicyJumpTable consumesPolicyJumpTable)
            {
                _consumesPolicyJumpTable = consumesPolicyJumpTable;
                ITuple firstTuple = (ITuple)((Array)_destinationsField.GetValue(consumesPolicyJumpTable)!).GetValue(0)!;
                _bodilessDestination = (int)firstTuple[1]!;
            }

            public override int GetDestination(HttpContext httpContext)
            {
                if (IsBodilessRequest(httpContext))
                {
                    return _bodilessDestination;
                }
                return _consumesPolicyJumpTable.GetDestination(httpContext);
            }
        }
    }
}
