using System.Collections.Specialized;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ReportReaderAttributesTest
    {
        [Test]
        public void TestAttrBugNvc()
        {
            var nvc = new NameValueCollection {{"X.Y$Attr", "1"}, {"Z", "123"}};
            ReportReaderHelpers.Check(
                nvc,
                new CForTestAttrBug {X = new CWithAttrForTestAttrBug {Y = "1"}, Z = 123});
        }

        [Test]
        public void TestOneAttrNvc()
        {
            var nvc = new NameValueCollection {{"A1$Attr", "qqq"}};
            ReportReaderHelpers.Check(
                nvc,
                new CWithAttr {A1 = "qqq"});
        }

        [Test]
        public void TestAttrBug()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <X Y='1' />
    <Z>123</Z>
</Root>
",
                new CForTestAttrBug {X = new CWithAttrForTestAttrBug {Y = "1"}, Z = 123});
        }

        [Test]
        public void TestOneAttr()
        {
            ReportReaderHelpers.Check(
                @"<Root 
A1='qqq' 
/>",
                new CWithAttr {A1 = "qqq"});
        }

        public class CWithAttrForTestAttrBug
        {
            [XmlAttribute]
            public string Y { get; set; }
        }

        public class CForTestAttrBug
        {
            public CWithAttrForTestAttrBug X { get; set; }
            public int Z { get; set; }
        }

        public class CWithAttr
        {
            [XmlAttribute]
            public string A1 { get; set; }
        }
    }
}