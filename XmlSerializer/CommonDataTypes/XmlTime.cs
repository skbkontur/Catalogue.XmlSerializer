using SKBKontur.Catalogue.XmlSerializer.Attributes;
using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerializer.Writing;
using SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerializer.CommonDataTypes
{
    public class XmlTime : XmlDataType, ICustomRead, ICustomWrite
    {
        public void Read(IReader xmlReader)
        {
            Time = xmlReader.ReadStringValue();
        }

        public void Write(IWriter xmlWriter)
        {
            if(Time != null)
                xmlWriter.WriteValue(Time);
        }

        public string Time { get; set; }
    }
}