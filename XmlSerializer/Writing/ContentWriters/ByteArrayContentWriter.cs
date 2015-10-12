using System;

namespace SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ByteArrayContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            var byteArrayValue = (byte[])obj;
            var base64String = Convert.ToBase64String(byteArrayValue);
            writer.WriteValue(base64String);
        }
    }
}