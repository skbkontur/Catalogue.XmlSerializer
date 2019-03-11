using System;

namespace Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class DateTimeContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            writer.WriteValue(((DateTime)obj).ToUniversalTime().Ticks);
        }
    }
}