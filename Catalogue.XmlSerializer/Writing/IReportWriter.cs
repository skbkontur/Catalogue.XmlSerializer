using System.Collections.Specialized;
using System.Text;

namespace SkbKontur.Catalogue.XmlSerializer.Writing
{
    public interface IReportWriter
    {
        string SerializeToString<T>(T data, bool omitXmlDeclaration, Encoding encoding, bool skipEmpty);
        byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding, bool skipEmpty);
        NameValueCollection SerializeToNameValueCollection<T>(T data, bool skipEmpty);
        void Serialize<T>(T data, IWriter writer);
    }
}