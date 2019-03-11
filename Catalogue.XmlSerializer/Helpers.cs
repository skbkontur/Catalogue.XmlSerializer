using System;
using System.Reflection;

namespace Catalogue.XmlSerializer
{
    public static class Helpers
    {
        public delegate bool TryParseDelegate<T>(string s, out T result);

        public static ConstructorInfo GetPublicConstructor(this Type type, params Type[] constructorParameterTypes)
        {
            return type.GetConstructor(BindingFlags.Public, constructorParameterTypes);
        }

        private static ConstructorInfo GetConstructor(this Type type, BindingFlags constructorFlags, params Type[] constructorParameterTypes)
        {
            var constructorInfo = type.GetConstructor(BindingFlags.Instance | constructorFlags, null, constructorParameterTypes, null);
            if (constructorInfo == null)
            {
                var errorMessage = $"Type {type} has no public instance constructor with types{Environment.NewLine}";
                foreach (var parameterType in constructorParameterTypes)
                    errorMessage += $"{parameterType.FullName}{Environment.NewLine}";
                throw new InvalidOperationException(errorMessage);
            }
            return constructorInfo;
        }
    }
}