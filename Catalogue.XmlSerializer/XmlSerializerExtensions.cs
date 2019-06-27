using System.Text;

namespace SkbKontur.Catalogue.XmlSerializer
{
    public static class XmlSerializerExtensions
    {
        public static string SerializeToUtfString<T>(this IXmlSerializer xmlSerializer, T data, bool omitXmlDeclaration, bool skipEmpty = true)
        {
            var encoding = new UTF8Encoding(false);
            return encoding.GetString(xmlSerializer.SerializeToBytes(data, omitXmlDeclaration, encoding, skipEmpty));
        }

        public static byte[] SerializeToUtfBytes<T>(this IXmlSerializer xmlSerializer, T data, bool omitXmlDeclaration, bool skipEmpty = true)
        {
            var encoding = new UTF8Encoding(false);
            return xmlSerializer.SerializeToBytes(data, omitXmlDeclaration, encoding, skipEmpty);
        }
    }
}