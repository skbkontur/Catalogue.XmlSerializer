using System;
using System.Globalization;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerialization.Writing;
using SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerialization.CommonDataTypes
{
    public class XmlDateTime : XmlDataType, ICustomRead, ICustomWrite
    {
        public DateTime? Date { get; set; }

        public void Read(IReader xmlReader)
        {
            var dateString = xmlReader.ReadStringValue();
            DateTime datetime;
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, CultureInfo.InvariantCulture,
                                                                       DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out datetime))
                Date = datetime;
            else
                Date = null;
        }

        public void Write(IWriter xmlWriter)
        {
            if (Date != null)
                xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ss.fffK}",
                                                   Date.Value.ToUniversalTime()));
        }
    }
}