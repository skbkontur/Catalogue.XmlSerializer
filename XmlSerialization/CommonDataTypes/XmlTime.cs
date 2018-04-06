using System;
using System.Globalization;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerialization.Writing;
using SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerialization.CommonDataTypes
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