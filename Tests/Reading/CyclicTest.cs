using System.Collections.Specialized;

using NUnit.Framework;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class CyclicTest
    {
        [Test]
        public void TestCycleInArrayNvc()
        {
            ReportReaderHelpers.Check(
                new NameValueCollection {{"S", "zzz"}, {"C.S", "x"}},
                new C2 {S = "zzz", C = new[] {new C2 {S = "x"},}});
        }

        [Test]
        public void TestMethodNvc()
        {
            ReportReaderHelpers.Check(
                new NameValueCollection {{"S", "zzz"}, {"C.S", "x"}},
                new C1 {S = "zzz", C = new C1 {S = "x"}});
        }

        [Test]
        public void TestCycleInArray()
        {
            ReportReaderHelpers.Check(
                @"
<Root >
    <S>zzz</S>
    <C>
        <S>x</S>
    </C>
</Root>",
                new C2 {S = "zzz", C = new[] {new C2 {S = "x"},}});
        }

        [Test]
        public void TestMethod()
        {
            ReportReaderHelpers.Check(
                @"
<Root >
    <S>zzz</S>
    <C>
        <S>x</S>
    </C>
</Root>",
                new C1 {S = "zzz", C = new C1 {S = "x"}});
        }

        public class C1
        {
            public string S { get; set; }
            public C1 C { get; set; }
        }

        public class C2
        {
            public string S { get; set; }
            public C2[] C { get; set; }
        }
    }
}