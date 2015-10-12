using System.Collections.Specialized;

using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerializer.Reading;
using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ArrayContentReaderTest
    {
        [Test]
        public void TestEndOfErray()
        {
            const string source =
                @"<A>1</A>
<A>2</A>
<B>100</B>";
            CheckArray(source, new[] {1, 2});
            CheckArray(ReportReaderHelpers.KillSpaces(source), new[] {1, 2});
        }

        [Test]
        public void TestEndOfErrayNvc()
        {
            var nvc = new NameValueCollection {{"A$0", "1"}, {"A$1", "2"}, {"B", "100"}};
            ReportReaderHelpers.Check(nvc, new C1 {A = new[] {1, 2}});
        }

        [Test]
        public void TestSimple()
        {
            const string source =
                @"<A>1</A>
<A>2</A>";
            CheckArray(source, new[] {1, 2});
            CheckArray(ReportReaderHelpers.KillSpaces(source), new[] {1, 2});
        }

        [Test]
        public void TestSimpleNvc()
        {
            var nvc = new NameValueCollection {{"A$0", "1"}, {"A$1", "2"}};
            ReportReaderHelpers.Check(nvc, new C1 {A = new[] {1, 2}});
        }

        public class C1
        {
            public int[] A { get; set; }
        }

        private static void CheckArray<T>(string source, T[] expected)
        {
            using(var xmlReader = ReportReaderHelpers.CreateXmlReader(source))
            {
                var arrayContentReader = new ArrayContentReader<T>(new ContentReaderCollection(new XmlAttributeInterpretator(), StandardConfigurations.EmptyOnDeserializeConfiguration));
                var value = arrayContentReader.Read(new SimpleXmlReader(xmlReader, true));
                value.ShouldBeEquivalentTo(expected);
            }
        }

        private static void CheckArray<T>(NameValueCollection source, T[] expected)
        {
            var arrayContentReader = new ArrayContentReader<T>(new ContentReaderCollection(new XmlAttributeInterpretator(), StandardConfigurations.EmptyOnDeserializeConfiguration));
            var value = arrayContentReader.Read(new NameValueCollectionReader(source));
            value.ShouldBeEquivalentTo(expected);
        }
    }
}