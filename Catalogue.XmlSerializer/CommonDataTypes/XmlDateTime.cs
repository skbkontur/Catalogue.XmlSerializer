using System;
using System.Globalization;

using Catalogue.XmlSerializer.Attributes;
using Catalogue.XmlSerializer.Reading;
using Catalogue.XmlSerializer.Reading.ContentReaders;
using Catalogue.XmlSerializer.Writing;
using Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.CommonDataTypes
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