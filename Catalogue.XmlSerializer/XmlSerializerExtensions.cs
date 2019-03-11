using System.Text;

namespace Catalogue.XmlSerializer
{
    public static class XmlSerializerExtensions
    {
        public static string SerializeToUtfString<T>(this IXmlSerializer xmlSerializer, T data, bool omitXmlDeclaration)
        {
            var encoding = new UTF8Encoding(false);
            return encoding.GetString(xmlSerializer.SerializeToBytes(data, omitXmlDeclaration, encoding));
        }

        public static byte[] SerializeToUtfBytes<T>(this IXmlSerializer xmlSerializer, T data, bool omitXmlDeclaration)
        {
            var encoding = new UTF8Encoding(false);
            return xmlSerializer.SerializeToBytes(data, omitXmlDeclaration, encoding);
        }
    }
}