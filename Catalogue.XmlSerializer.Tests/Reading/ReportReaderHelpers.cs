using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

using Catalogue.XmlSerializer.Reading;
using Catalogue.XmlSerializer.Reading.Configuration;

using FluentAssertions;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    public static class ReportReaderHelpers
    {
        public static void Check<T>(string source, T expected)
        {
            var reportReader = CreateReader();
            var read = reportReader.ReadFromString<T>(source);
            read.Should().BeEquivalentTo(expected);

            read = reportReader.ReadFromString<T>(KillSpaces(source));
            read.Should().BeEquivalentTo(expected);
        }

        public static void Check<T>(NameValueCollection source, T expected)
        {
            var reportReader = CreateReader();
            var read = reportReader.Read<T>(source);
            read.Should().BeEquivalentTo(expected);
        }

        public static ReportReader CreateReader(OnDeserializeConfiguration configuration = null)
        {
            if (configuration == null)
                configuration = StandardConfigurations.EmptyOnDeserializeConfiguration;
            return new ReportReader(new ContentReaderCollection(new XmlAttributeInterpretator(), configuration));
        }

        public static XmlReader CreateXmlReader(string source)
        {
            var xmlReader = XmlReader.Create(new StringReader(source),
                                             new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Fragment});
            Assert.AreEqual(XmlNodeType.Element, xmlReader.MoveToContent());
            return xmlReader;
        }

        public static string KillSpaces(string source)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(source),
                                                    new XmlReaderSettings
                                                        {ConformanceLevel = ConformanceLevel.Fragment}))
            {
                var builder = new StringBuilder();
                using (
                    XmlWriter xmlWriter = new NoWhitespaceWriter(new StringWriter(builder),
                                                                 new XmlWriterSettings
                                                                     {
                                                                         Indent = false,
                                                                         OmitXmlDeclaration = true,
                                                                         NewLineOnAttributes = false
                                                                     }))
                    xmlWriter.WriteNode(xmlReader, false);
                return builder.ToString();
            }
        }
    }
}