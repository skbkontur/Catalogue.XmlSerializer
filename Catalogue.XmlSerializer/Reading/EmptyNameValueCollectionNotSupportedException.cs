using System;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public class EmptyNameValueCollectionNotSupportedException : Exception
    {
        public EmptyNameValueCollectionNotSupportedException()
            : base("An empty name-value collection cannot be deserialized")
        {
        }
    }
}