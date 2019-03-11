using System;
using System.Globalization;

using SkbKontur.Catalogue.XmlSerializer.Attributes;
using SkbKontur.Catalogue.XmlSerializer.Reading;
using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SkbKontur.Catalogue.XmlSerializer.Writing;
using SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SkbKontur.Catalogue.XmlSerializer.CommonDataTypes
{
    public class XmlDate : XmlDataType, ICustomRead, ICustomWrite
    {
        public void Read(IReader xmlReader)
        {
            var dateString = xmlReader.ReadStringValue();
            if (!string.IsNullOrEmpty(dateString))
            {
                try
                {
                    Date = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    Date = null;
                }
            }
        }

        public void Write(IWriter xmlWriter)
        {
            if (Date != null)
                xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd}", Date));
        }

        public DateTime? Date { get; set; }
    }
}