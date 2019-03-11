using System.IO;
using System.Xml;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    public class NoWhitespaceWriter : XmlTextWriter
    {
        public NoWhitespaceWriter(TextWriter writer, XmlWriterSettings settings)
            : base(writer)
        {
            this.Settings = settings;
        }

        public override void WriteWhitespace(string ws)
        {
        }

        public override XmlWriterSettings Settings { get; }
    }
}