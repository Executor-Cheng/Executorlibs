using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Executorlibs.AspNetCore.Mvc.ApplicationModels
{
    public abstract class SubComponentApplicationModelProvider<TSubComponent> : IApplicationModelProvider where TSubComponent : SubComponentAttribute
    {
        public abstract int Order { get; }

        protected abstract string SubComponentName { get; }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            List<TSubComponent> attributes = new List<TSubComponent>();
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                attributes.Clear();
                static void RecursiveFindAttributes(Type? type, List<TSubComponent> attributes)
                {
                    if (type == null)
                    {
                        return;
                    }
                    TSubComponent[] subComponents = (TSubComponent[])type.GetCustomAttributes<TSubComponent>(false);
                    for (int addedCount = subComponents.Length - 1; addedCount >= 0; addedCount--)
                    {
                        attributes.Add(subComponents[addedCount]);
                    }
                    RecursiveFindAttributes(type.BaseType, attributes);
                }
                RecursiveFindAttributes(controller.ControllerType, attributes);
                bool inited = false;
                foreach (SelectorModel selector in controller.Selectors)
                {
                    ReadOnlySpan<char> templateSpan;
                    if (selector.AttributeRouteModel != null && TryGetSubComponentPatternIx(templateSpan = selector.AttributeRouteModel.Template, out int index))
                    {
                        int length = attributes.Count/*, patternLength = 10 * subComponents.Length, j = 1, k = 1, l*/;
                        if (!inited)
                        {
                            inited = true;
                            for (int i = length - 1, j = 0; i >= 0; i--, j++)
                            {
                                controller.RouteValues.Add($"{SubComponentName}{j}", attributes[i].SubComponentName);
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append(templateSpan[..index]);
                        for (int i = 0; i < length; i++)
                        {
                            sb.Append('[');
                            sb.Append(SubComponentName);
                            sb.Append(i);
                            sb.Append("]/");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(templateSpan[(index + SubComponentName.Length + 2)..]);
                        selector.AttributeRouteModel.Template = sb.ToString();
                    }
                }
            }
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {

        }

        private bool TryGetSubComponentPatternIx(ReadOnlySpan<char> pattern, out int index)
        {
            int ix, ix2, ix3 = 0;
            string subComponent = $"[{SubComponentName}]";
            do
            {
                ix = pattern.IndexOf(subComponent);
                if (ix == -1)
                {
                    break;
                }
                if ((ix == 0 || pattern[ix - 1] != '[') &&
                   ((ix2 = ix + subComponent.Length) == pattern.Length || pattern[ix2] != ']'))
                {
                    index = ix + ix3;
                    return true;
                }
                ix += subComponent.Length;
                if (ix >= pattern.Length)
                {
                    break;
                }
                ix3 += ix;
                pattern = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(pattern), ix), pattern.Length - ix);
            }
            while (true);
            index = 0;
            return false;
        }
    }
}
