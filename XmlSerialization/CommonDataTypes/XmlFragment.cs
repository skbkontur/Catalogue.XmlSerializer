using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerialization.Writing;
using SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerialization.CommonDataTypes
{
    public class XmlFragment : ICustomRead, ICustomWrite
    {
        public void Read(IReader reader)
        {
            Data = reader.ReadRawData();
        }

        public void Write(IWriter xmlWriter)
        {
            if(!string.IsNullOrEmpty(Data))
                xmlWriter.WriteRawData(Data);
        }

        public string Data { get; set; }
    }
}