using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.Configuration;
using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public class StrictXmlSerializer : IStrictXmlSerializer
    {
        public StrictXmlSerializer()
        {
            var xmlAttributeInterpretator = new XmlAttributeInterpretator();

            var onDeserializeConfiguration = new OnDeserializeConfiguration();
            onDeserializeConfiguration.OnUnexpectedElement += OnUnexpectedElement;
            onDeserializeConfiguration.OnUnexpectedAttribute += OnUnexpectedAttribute;

            reportReader = new ReportReader(new ContentReaderCollection(xmlAttributeInterpretator, onDeserializeConfiguration));
            reportWriter = new ReportWriter(new ContentWriterCollection(xmlAttributeInterpretator));
        }

        public byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding)
        {
            return reportWriter.SerializeToBytes(data, omitXmlDeclaration, encoding);
        }

        public NameValueCollection SerializeToNameValueCollection<T>(T data)
        {
            return reportWriter.SerializeToNameValueCollection(data, true);
        }

        public void Serialize<T>(T data, IWriter writer)
        {
            reportWriter.Serialize(data, writer);
        }

        public T Deserialize<T>(XmlReader reader, bool needTrimValues = true)
        {
            return reportReader.Read<T>(reader, needTrimValues);
        }

        public T Deserialize<T>(NameValueCollection collection)
        {
            return reportReader.Read<T>(collection);
        }

        public T Deserialize<T>(byte[] source, bool needTrimValues = true)
        {
            using (var xmlReader = XmlReader.Create(new MemoryStream(source)))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        public T Deserialize<T>(Stream stream, bool needTrimValues = true)
        {
            using (var xmlReader = XmlReader.Create(stream))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        public T Deserialize<T>(string source, bool needTrimValues = true)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(source)))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        private static void OnUnexpectedAttribute(object sender, DeserializationContext context)
        {
            throw new InvalidOperationException(string.Format("Unexpected attributeName {0}", context.CurrentElementLocalName));
        }

        private static void OnUnexpectedElement(object sender, DeserializationContext context)
        {
            throw new InvalidOperationException(string.Format("Unexpected elementName {0}", context.CurrentElementLocalName));
        }

        private readonly ReportReader reportReader;
        private readonly ReportWriter reportWriter;
    }
}