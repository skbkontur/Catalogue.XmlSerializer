using System.Collections.Specialized;

using NUnit.Framework;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    public class ReportReaderBigArrayTest
    {
        [Test]
        public void TestArrayOfClassNvc()
        {
            var source = new NameValueCollection {{"C1Arr$0.Prpo", "2"}, {"C1Arr$1.Prpo", "3"}};
            ReportReaderHelpers.Check(source, new C3 {C1Arr = new[] {new C1 {Prpo = 2}, new C1 {Prpo = 3},}});
        }

        [Test]
        public void TestArrayOfClass()
        {
            const string source =
                @"
<Root>
        <C1Arr><Prpo>2</Prpo></C1Arr>
        <C1Arr>
    <Prpo>3</Prpo>
</C1Arr>
</Root>";
            ReportReaderHelpers.Check(source, new C3 {C1Arr = new[] {new C1 {Prpo = 2}, new C1 {Prpo = 3},}});
        }

        [Test]
        public void TestInnerArrNvc()
        {
            var source = new NameValueCollection {{"InnerArr.Arr$0", "1"}, {"InnerArr.Arr$1", "2"}, {"InnerArr.Prpo", "10"}};
            ReportReaderHelpers.Check(source, new C2 {InnerArr = new C1 {Arr = new[] {1, 2}, Prpo = 10}});
        }

        [Test]
        public void TestInnerArr()
        {
            const string source =
                @"
<Root>
    <InnerArr>
        <Arr>1</Arr>
        <Arr>2</Arr>
        <Prpo>10</Prpo>
    </InnerArr>
</Root>";
            ReportReaderHelpers.Check(source, new C2 {InnerArr = new C1 {Arr = new[] {1, 2}, Prpo = 10}});
        }

        [Test]
        public void TestSimpleNvc()
        {
            var source = new NameValueCollection {{"Arr$0", "1"}, {"Arr$1", "2"}};
            ReportReaderHelpers.Check(source, new C1 {Arr = new[] {1, 2}});
        }

        [Test]
        public void TestArrayLengthGreaterThan10()
        {
            var source = new NameValueCollection
                {
                    {"Arr$0", "1"}, {"Arr$1", "2"},
                    {"Arr$2", "3"}, {"Arr$3", "4"},
                    {"Arr$4", "5"}, {"Arr$5", "6"},
                    {"Arr$6", "7"}, {"Arr$7", "8"},
                    {"Arr$8", "9"}, {"Arr$9", "10"},
                    {"Arr$10", "11"}, {"Arr$11", "12"},
                };
            ReportReaderHelpers.Check(source, new C1 {Arr = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}});
        }

        [Test]
        public void TestSimple()
        {
            const string source = @"
<Root>
    <Arr>1</Arr>
    <Arr>2</Arr>
</Root>";
            ReportReaderHelpers.Check(source, new C1 {Arr = new[] {1, 2}});
        }

        [Test]
        public void TestByteArray()
        {
            const string source = @"
<root>
    <ByteArray>AAECAwQF</ByteArray>
</root>";
            ReportReaderHelpers.Check(source, new QZZ {ByteArray = new byte[] {0, 1, 2, 3, 4, 5}});
        }

        [Test]
        public void TestByteArrayNvc()
        {
            var source = new NameValueCollection {{"ByteArray", "AAECAwQF"}};
            ReportReaderHelpers.Check(source, new QZZ {ByteArray = new byte[] {0, 1, 2, 3, 4, 5}});
        }

        [Test]
        public void TestByteArrayEmpty()
        {
            const string source = @"
<root>
</root>";
            ReportReaderHelpers.Check(source, new QZZ());
        }

        [Test]
        public void TestByteArrayNotBase64()
        {
            const string source = @"
<root>
    <ByteArray>**%**</ByteArray>
</root>";
            ReportReaderHelpers.Check(source, new QZZ());
        }

        public class QZZ
        {
            public byte[] ByteArray { get; set; }
        }

        public class C1
        {
            public int[] Arr { get; set; }
            public int Prpo { get; set; }
        }

        public class C2
        {
            public C1 InnerArr { get; set; }
        }

        public class C3
        {
            public C1[] C1Arr { get; set; }
        }
    }
}