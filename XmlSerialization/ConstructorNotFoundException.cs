using System;
using System.Collections.Generic;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public class ConstructorNotFoundException : Exception
    {
        private ConstructorNotFoundException(string message)
            : base(message)
        {
        }

        public static Exception Create(Type type, IEnumerable<Type> constructorParameterTypes)
        {
            var message = String.Format("Type {0} has no public instance constructor with types\n", type);
            foreach(var parameterType in constructorParameterTypes)
                message += parameterType.FullName + "\r\n";
            return new ConstructorNotFoundException(message);
        }
    }
}