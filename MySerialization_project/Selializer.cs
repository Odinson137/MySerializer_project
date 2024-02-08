using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MySerialization_project
{
    internal static class Selializer
    {
        public static StringBuilder Serialize<T>(T obj)
        {
            var type = typeof(T);
            return Serialize(obj, type);
        }

        public static StringBuilder Serialize(object obj, Type type)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append('{');

            foreach (var property in properties)
            {
                strBuilder.Append($",");
                var value = property.GetValue(obj);

                if (value == null)
                {
                    strBuilder.Append($"\"{property.Name}\":");
                    strBuilder.Append($"null");
                }
                else
                {
                    var valueType = value.GetType();

                    if (valueType.IsClass && valueType != typeof(string))
                    {
                        strBuilder.Append($"\"{property.Name}\":");
                        var child = Serialize(value, valueType);
                        strBuilder.Append(child);
                    }
                    else
                    {
                        strBuilder.Append($"\"{property.Name}\":");
                        strBuilder.Append(GetTypeValue(valueType, value));
                    }
                }
 
            }

            strBuilder.Append('}');

            strBuilder.Remove(1, 1);

            return strBuilder;
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
