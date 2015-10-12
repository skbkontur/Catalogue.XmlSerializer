using System.Collections.Specialized;
using System.Text;

namespace SKBKontur.Catalogue.XmlSerializer.Writing
{
    public interface IReportWriter
    {
        string SerializeToString<T>(T data, bool omitXmlDeclaration, Encoding encoding);
        byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding);
        NameValueCollection SerializeToNameValueCollection<T>(T data, bool skipEmpty);
        void Serialize<T>(T data, IWriter writer);
    }
}