using System;

using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class DateTimeContentReader : IContentReader<DateTime>
    {
        public DateTime Read(IReader reader)
        {
            return new DateTime(long.Parse(reader.ReadStringValue()), DateTimeKind.Utc);
        }
    }
}