using Catalogue.XmlSerializer.Reading;
using Catalogue.XmlSerializer.Reading.ContentReaders;
using Catalogue.XmlSerializer.Writing;
using Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.CommonDataTypes
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