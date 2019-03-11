using System;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public class GroboSerializerException : Exception
    {
        public GroboSerializerException(string format, params object[] parameters)
            : base(string.Format(format, parameters))
        {
        }

        public GroboSerializerException(Exception innerException, string format, params object[] parameters)
            : base(string.Format(format, parameters), innerException)
        {
        }
    }
}