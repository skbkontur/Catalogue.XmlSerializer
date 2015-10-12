using System.Collections.Specialized;

using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class NullableContentReaderTest
    {
        [Test]
        public void TestNothing()
        {
            const string source =
                @"
 <A />

";
            Check(source, (int?)null);
            Check(ReportReaderHelpers.KillSpaces(source), (int?)null);
        }

        [Test]
        public void TestNothingNvc()
        {
            var source = new NameValueCollection {{"A", null}};
            ReportReaderHelpers.Check(source, new C1());
        }

        [Test]
        public void TestSimpleNvc()
        {
            var source = new NameValueCollection {{"A", "1"}};
            ReportReaderHelpers.Check(source, new C1 {A = 1});
        }

        [Test]
        public void TestSimple()
        {
            const string source =
                @"
 <A>1</A>

";
            Check(source, (int?)1);
            Check(ReportReaderHelpers.KillSpaces(source), (int?)1);
        }

        public class C1
        {
            public int? A { get; set; }
        }

        private static void Check<T>(string source, T? expected) where T : struct
        {
            using(var xmlReader = ReportReaderHelpers.CreateXmlReader(source))
            {
                var arrayContentReader = new NullableContentReader<T>(new ContentReaderCollection(new XmlAttributeInterpretator(), StandardConfigurations.EmptyOnDeserializeConfiguration));
                var value = arrayContentReader.Read(new SimpleXmlReader(xmlReader, true));
                value.ShouldBeEquivalentTo(expected);
            }
        }
    }
}