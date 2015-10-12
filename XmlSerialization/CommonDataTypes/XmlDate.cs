using System;
using System.Globalization;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerialization.Writing;
using SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerialization.CommonDataTypes
{
    public class XmlDate : XmlDataType, ICustomRead, ICustomWrite
    {
        public void Read(IReader xmlReader)
        {
            var dateString = xmlReader.ReadStringValue();
            if(!string.IsNullOrEmpty(dateString))
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
            if(Date != null)
                xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd}", Date));
        }

        public DateTime? Date { get; set; }
    }
}