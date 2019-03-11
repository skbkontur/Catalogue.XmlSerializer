using System;
using System.Globalization;

using Catalogue.XmlSerializer.Attributes;
using Catalogue.XmlSerializer.Reading;
using Catalogue.XmlSerializer.Reading.ContentReaders;
using Catalogue.XmlSerializer.Writing;
using Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.CommonDataTypes
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