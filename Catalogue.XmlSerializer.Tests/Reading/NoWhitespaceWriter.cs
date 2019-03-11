using System.IO;
using System.Xml;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    public class NoWhitespaceWriter : XmlTextWriter
    {
        public NoWhitespaceWriter(TextWriter writer, XmlWriterSettings settings)
            : base(writer)
        {
            this.settings = settings;
        }

        public override void WriteWhitespace(string ws)
        {
        }

        public override XmlWriterSettings Settings => settings;
        private readonly XmlWriterSettings settings;
    }
}