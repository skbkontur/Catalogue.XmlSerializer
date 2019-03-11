using System.Collections.Specialized;
using System.Xml;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public interface IReportReader
    {
        T Read<T>(XmlReader reader, bool needTrimValues);
        T Read<T>(NameValueCollection collection);
        T Read<T>(byte[] xmlContent, bool needTrimValues);
    }
}