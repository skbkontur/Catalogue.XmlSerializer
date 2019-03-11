using System.IO;
using System.Xml;

namespace Catalogue.XmlSerializer.Reading
{
    public static class ReportReaderExtensions
    {
        public static T ReadFromString<T>(this IReportReader reportReader, string source, bool needTrimValues = true)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(source)))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }
    }
}