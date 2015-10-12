using System.Collections.Specialized;
using System.Xml;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public interface IReportReader
    {
        T Read<T>(XmlReader reader, bool needTrimValues);
        T Read<T>(NameValueCollection collection);
        T Read<T>(byte[] xmlContent, bool needTrimValues);
    }
}