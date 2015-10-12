using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SKBKontur.Catalogue.XmlSerializer.Writing;
using SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerializer.CommonDataTypes
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