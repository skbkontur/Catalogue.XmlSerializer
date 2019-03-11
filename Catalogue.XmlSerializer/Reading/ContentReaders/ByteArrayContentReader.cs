using System;

using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders
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