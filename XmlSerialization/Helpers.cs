using System;
using System.Reflection;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public static class Helpers
    {
        public delegate bool TryParseDelegate<T>(string s, out T result);

        public static ConstructorInfo GetPublicConstructor(this Type type, params Type[] constructorParameterTypes)
        {
            return type.GetConstructor(BindingFlags.Public, constructorParameterTypes);
        }

        public static ConstructorInfo GetConstructor(this Type type, BindingFlags constructorFlags,
                                                     params Type[] constructorParameterTypes)
        {
            var constructorInfo = type.GetConstructor(
                BindingFlags.Instance | constructorFlags,
                null, constructorParameterTypes,
                null);
            if (constructorInfo == null)
                throw ConstructorNotFoundException.Create(type, constructorParameterTypes);
            return constructorInfo;
        }
    }
}