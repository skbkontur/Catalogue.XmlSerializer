using System;
using System.Globalization;

using SkbKontur.Catalogue.XmlSerializer.Attributes;
using SkbKontur.Catalogue.XmlSerializer.Reading;
using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SkbKontur.Catalogue.XmlSerializer.Writing;
using SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SkbKontur.Catalogue.XmlSerializer.CommonDataTypes
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