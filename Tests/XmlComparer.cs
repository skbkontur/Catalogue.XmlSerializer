using System.Linq;
using System.Text;
using System.Xml;

using NUnit.Framework;

namespace SKBKontur.Catalogue.XmlSerializer.Tests
{
    public static class XmlComparer
    {
        public static void AssertEqualsXml(this string actual, string expected, string message = "", params object[] args)
        {
            if(string.IsNullOrEmpty(message))
                Assert.AreEqual(ReformatXml(expected), ReformatXml(actual));
            else
                Assert.AreEqual(ReformatXml(expected), ReformatXml(actual), message, args);
        }

        private static string ReformatXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            var result = new StringBuilder();
            var writer = XmlWriter.Create(result, new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = !HasXmlDeclaration(document)
                });
            document.WriteTo(writer);
            writer.Flush();
            return result.ToString();
        }

        private static bool HasXmlDeclaration(XmlDocument document)
        {
            return document.ChildNodes.OfType<XmlDeclaration>().Any();
        }
    }
}