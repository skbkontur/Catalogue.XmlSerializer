using System;
using System.Collections.Generic;

namespace SKBKontur.Catalogue.XmlSerializer
{
    public static class TypeRecognizerHelper
    {
        public static bool IsList(this Type type)
        {
            return type.IsGenericType && (typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }

        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType && (typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }

        public static Type GetListType(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public static Type GetDictionaryKeyType(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public static Type GetDictionaryValueType(this Type type)
        {
            return type.GetGenericArguments()[1];
        }
    }
}