using System;

namespace Executorlibs.AspNetCore.Identity
{
    public abstract class TypeResolver
    {
        protected static Type? FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            Type? type = currentType;
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                {
                    return type;
                }
            }
            while ((type = type.BaseType) != null);
            return null;
        }
    }
}
