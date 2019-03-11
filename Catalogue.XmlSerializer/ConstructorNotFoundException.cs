using System;
using System.Collections.Generic;

namespace Catalogue.XmlSerializer
{
    public class ConstructorNotFoundException : Exception
    {
        private ConstructorNotFoundException(string message)
            : base(message)
        {
        }

        public static Exception Create(Type type, IEnumerable<Type> constructorParameterTypes)
        {
            var message = $"Type {type} has no public instance constructor with types\n";
            foreach (var parameterType in constructorParameterTypes)
                message += parameterType.FullName + "\r\n";
            return new ConstructorNotFoundException(message);
        }
    }
}