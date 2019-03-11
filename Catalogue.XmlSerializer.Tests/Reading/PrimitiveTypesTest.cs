using System.Collections.Specialized;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class PrimitiveTypesTest
    {
        [Test]
        public void TestSimple()
        {
            const string source =
                @"
<Root>
    <D>1.21e3</D>
    <S>zzz</S>
    <S>x</S>
    <A>-1</A>
    <B>123456789012345</B>
</Root>
";
            ReportReaderHelpers.Check(source,
                                      new C1
                                          {
                                              A = -1,
                                              B = 123456789012345,
                                              D = 1.21e3,
                                              S = new[] {"zzz", "x"}
                                          });
        }

        [Test]
        public void TestSimpleCData()
        {
            const string source =
                @"
<Root>
    <D>1.21e3</D>
    <S><![CDATA[zzzz]]></S>
    <S>x</S>
    <A>-1</A>
    <B>123456789012345</B>
</Root>
";
            ReportReaderHelpers.Check(source,
                                      new C1
                                          {
                                              A = -1,
                                              B = 123456789012345,
                                              D = 1.21e3,
                                              S = new[] {"zzzz", "x"}
                                          });
        }

        [Test]
        public void TestSimpleNvc()
        {
            var source = new NameValueCollection {{"D", "1.21e3"}, {"S$0", "zzz"}, {"S$1", "x"}, {"A", "-1"}, {"B", "123456789012345"}};
            ReportReaderHelpers.Check(source,
                                      new C1
                                          {
                                              A = -1,
                                              B = 123456789012345,
                                              D = 1.21e3,
                                              S = new[] {"zzz", "x"}
                                          });
        }

        public class C1
        {
            public int A { get; set; }
            public long B { get; set; }
            public string[] S { get; set; }
            public double D { get; set; }
        }
    }
}