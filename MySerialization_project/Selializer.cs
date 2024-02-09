using System.Collections;
using System.Reflection;
using System.Text;

namespace MySerialization_project
{
    internal static class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            var StringBuilder = new StringBuilder(256);
            Serialize(StringBuilder, obj, typeof(T));
            return StringBuilder.ToString();
        }
        
        public static void Serialize(StringBuilder stringBuilder, object obj, Type? type)
        {
            if (type == null) return;

            if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
            {
                stringBuilder.Append('[');
                foreach (var element in (IEnumerable)obj)
                {
                    Serialize(stringBuilder, element, element?.GetType() ?? null);
                    stringBuilder.Append(',');
                }
                stringBuilder.Remove(stringBuilder.Length-1, 1);
                stringBuilder.Append("]");
            }
            else
            {
                stringBuilder.Append('{');

                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);

                    if (value == null)
                    {
                        stringBuilder.Append($"\"{property.Name}\":\"null\"");
                    }
                    else
                    {
                        var valueType = value.GetType();

                        if (valueType.IsClass && valueType != typeof(string))
                        {
                            stringBuilder.Append($"\"{property.Name}\":");
                            Serialize(stringBuilder, value, valueType);
                        }
                        else
                        {
                            stringBuilder.Append($"\"{property.Name}\":{GetTypeValue(valueType, value)}");
                        }
                    }
                    stringBuilder.Append(",");
                }

                stringBuilder.Remove(stringBuilder.Length-1, 1);
                stringBuilder.Append("}");

            }
        }

        private static object GetTypeValue(Type typeCode, object value)
        {
            switch (Type.GetTypeCode(typeCode))
            {
                case TypeCode.Boolean:
                    return value.ToString().ToLower();
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return value;
                default:
                    return $"\"{value}\"";
            }
        }
    
    
    }
}
