using System;
using System.ComponentModel;

namespace Npoi.Report.Extends
{
    public static class TypeExtend
    {
        public static bool IsNullableType(this Type type)
        {
            return ((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetUnderlyingType(this Type type)
        {
            if (IsNullableType(type))
            {
                var nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }
    }
}