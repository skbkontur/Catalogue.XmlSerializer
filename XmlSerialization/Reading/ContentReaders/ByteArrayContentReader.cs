using System;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class ByteArrayContentReader : IContentReader<byte[]>
    {
        public byte[] Read(IReader reader)
        {
            var base64String = reader.ReadStringValue();
            if (String.IsNullOrEmpty(base64String))
                return null;
            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}