using System;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class DateTimeContentReader : IContentReader<DateTime>
    {
        public DateTime Read(IReader reader)
        {
            return new DateTime(long.Parse(reader.ReadStringValue()), DateTimeKind.Utc);
        }
    }
}