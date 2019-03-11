using SkbKontur.Catalogue.XmlSerializer.Reading;
using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;
using SkbKontur.Catalogue.XmlSerializer.Writing;
using SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace SkbKontur.Catalogue.XmlSerializer.CommonDataTypes
{
    public class XmlFragment : ICustomRead, ICustomWrite
    {
        public void Read(IReader reader)
        {
            Data = reader.ReadRawData();
        }

        public void Write(IWriter xmlWriter)
        {
            if (!string.IsNullOrEmpty(Data))
                xmlWriter.WriteRawData(Data);
        }

        public string Data { get; set; }
    }
}