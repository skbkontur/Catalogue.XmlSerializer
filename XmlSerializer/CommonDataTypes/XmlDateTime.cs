using System;
using System.Globalization;

using SKBKontur.Catalogue.XmlSerializer.Attributes;
using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerializer.Writing;
using SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerializer.CommonDataTypes
{
    public class XmlDateTime : XmlDataType, ICustomRead, ICustomWrite
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
                xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ss.fffK}", Date.Value.ToUniversalTime()));
        }

        public DateTime? Date { get; set; }
    }
}