using System.Diagnostics;
using System.Text;

using Catalogue.XmlSerializer.Writing;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ReportWriterSpeedTest
    {
        [SetUp]
        public void SetUp()
        {
            var xmlAttributeInterpretator = new XmlAttributeInterpretator();
            collection = new ContentWriterCollection(xmlAttributeInterpretator);
            writer = new ReportWriter(collection);
        }

        [Test]
        public void TestSimple()
        {
            var пфрсзв2010 = Fill();
            var count = 10000;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding1251 = Encoding.GetEncoding(1251);
            writer.SerializeToString(пфрсзв2010, true, encoding1251);
            var w = Stopwatch.StartNew();
            for (var i = 0; i < count; ++i)
                writer.SerializeToString(пфрсзв2010, true, encoding1251);
            Debug.WriteLine(w.ElapsedMilliseconds);
            Debug.WriteLine(count / w.Elapsed.TotalSeconds + " writes/s");
        }

        private static ФайлПФРСЗВ2010 Fill()
        {
            var сзв2010s = new ТипСЗВ2010[10];
            for (var i = 0; i < сзв2010s.Length; ++i)
                сзв2010s[i] = new ТипСЗВ2010 {Zzz = "sdjksdkjsdkj"};
            var пфрсзв2010 = new ФайлПФРСЗВ2010
                {
                    ЗаголовокФайла = new ТипЗаголовокФайла(),
                    ИмяФайла = "sdjhsdjhsdjhsd",
                    ПачкаВходящихДокументов = new ТипПачкаВходящихДокументовСЗВ2010
                        {
                            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ =
                                new ТипОписьСЗВ2010 {Xxx = "sdjksdksd"},
                            СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ
                                = сзв2010s
                        }
                };
            return пфрсзв2010;
        }

        private IContentWriterCollection collection;
        private IReportWriter writer;
    }
}