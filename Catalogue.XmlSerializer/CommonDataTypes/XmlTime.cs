using System;
using System.Globalization;

using Catalogue.XmlSerializer.Attributes;
using Catalogue.XmlSerializer.Reading;
using Catalogue.XmlSerializer.Reading.ContentReaders;
using Catalogue.XmlSerializer.Writing;
using Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.CommonDataTypes
{
    public class XmlTime : XmlDataType, ICustomRead, ICustomWrite
    {
        private const string Format = "HH:mm";

        public TimeSpan? Time { get; set; }

        public void Read(IReader xmlReader)
        {
            var dateString = xmlReader.ReadStringValue();
            DateTime datetime;
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, CultureInfo.InvariantCulture,
                                                                       DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out datetime))
                Time = new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second);
            else
                Time = null;
        }

        public void Write(IWriter xmlWriter)
        {
            if (Time != null)
            {
                var datetime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + Time.Value;
                xmlWriter.WriteValue(datetime.ToUniversalTime().ToString(Format));
            }
        }

        public static XmlTime FromDateTime(DateTime datetime)
        {
            return new XmlTime {Time = new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second)};
        }
    }
}