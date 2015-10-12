using System.Collections.Specialized;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ReportReaderCustomReaderTest
    {
        [Test]
        public void TestSimpleNvc()
        {
            var nvc = new NameValueCollection {{"Arr$0", "ABC"}, {"Arr$1", "EEe"}, {"Q", "1"}};
            ReportReaderHelpers.Check(
                nvc,
                new C2 {Arr = new[] {new C1 {S = "abc"}, new C1 {S = "eee"},}, Q = 1});
        }

        [Test]
        public void TestCustomRoot()
        {
            ReportReaderHelpers.Check(
                @"
<Root>AAA</Root>
", new C1 {S = "aaa"});
        }

        [Test]
        public void TestSimple()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Arr>ABC</Arr>
    <Arr>EEe</Arr>
    <Q>1</Q>
</Root>
",
                new C2 {Arr = new[] {new C1 {S = "abc"}, new C1 {S = "eee"},}, Q = 1});
        }

        public class C1 : ICustomRead
        {
            public void Read(IReader reader)
            {
                var readElementString = reader.ReadElementString();
                S = readElementString.ToLower();
            }

            public string S { get; set; }
        }

        public class C2
        {
            public C1[] Arr { get; set; }
            public int Q { get; set; }
        }
    }
}