using System;
using System.Text;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ReportWriterTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            var xmlAttributeInterpretator = new XmlAttributeInterpretator();
            collection = new ContentWriterCollection(xmlAttributeInterpretator);
            writer = new ReportWriter(collection);
        }

        #endregion

        [Test]
        public void TestGuid()
        {
            var id = new Guid("7FE483B0-B27E-43FB-AC68-D66FD7DEA4DA");
            writer.SerializeToString(new QXX
                {
                    Id = id
                }, true, Encoding.ASCII).AssertEqualsXml(
                    @"
<root>
    <Id>7fe483b0-b27e-43fb-ac68-d66fd7dea4da</Id>
</root>");
        }

        [Test]
        public void TestDecimal()
        {
            writer.SerializeToString(new DCC
                {
                    Z = 23782.323223m
                }, true, Encoding.ASCII).AssertEqualsXml(
                    @"
<root>
    <Z>23782.323223</Z>
</root>");
        }

        [Test]
        public void TestEnum()
        {
            writer.SerializeToString(new Q
                {
                    Z = ZEnum.B
                }, true, Encoding.ASCII).AssertEqualsXml(
                    @"
<root>
    <Z>B</Z>
</root>");
        }

        [Test]
        public void TestSimple()
        {
            writer.SerializeToString(new C1 {PackNumber = 12, C2Prop = new C2 {P = "sss", U = ""}}, true, Encoding.ASCII).AssertEqualsXml(
                @"
<TestRoot>
    <PackNumber>12</PackNumber>
    <C2Prop>
        <P>sss</P>
    </C2Prop>
</TestRoot>
");
        }

        [Test]
        public void TestGenericType()
        {
            var message = new Message<SpecificTransaction>
                {
                    SpecificTrash = "1",
                    Transaction = new SpecificTransaction {SpecificTransactionTrash = "2"},
                };
            writer.SerializeToString(message, true, Encoding.ASCII).AssertEqualsXml(
                @"
                <Message>
                    <specificTrash>1</specificTrash>
                    <SpecificTransaction>
                        <specifictransactiontrash>2</specifictransactiontrash>
                    </SpecificTransaction>
                </Message>");
        }

        [Test]
        public void TestArray()
        {
            var message = new ArrayMessage<SpecificTransaction>
                {
                    SpecificTrash = "st",
                    Transactions = new[]
                        {
                            new SpecificTransaction {SpecificTransactionTrash = "stt1"},
                            new SpecificTransaction {SpecificTransactionTrash = "stt2"}
                        }
                };

            writer.SerializeToString(message, true, Encoding.ASCII).AssertEqualsXml(@"
                <Message>
                    <specificTrash>st</specificTrash>
                    <SpecificTransaction>
                        <specifictransactiontrash>stt1</specifictransactiontrash>
                    </SpecificTransaction>
                    <SpecificTransaction>
                        <specifictransactiontrash>stt2</specifictransactiontrash>
                    </SpecificTransaction>
                </Message>");
        }

        [Test]
        public void TestByteArray()
        {
            var message = new QZZ
                {
                    ByteArray = new byte[] {0, 1, 2, 3, 4, 5}
                };
            writer.SerializeToString(message, true, Encoding.ASCII).AssertEqualsXml(@"
<root>
    <ByteArray>AAECAwQF</ByteArray>
</root>
");
        }

        [Test]
        public void TestByteArrayNull()
        {
            writer.SerializeToString(new QZZ(), true, Encoding.ASCII).AssertEqualsXml(@"
<root />
");
        }

        [Test]
        public void TestClassWithIndexer()
        {
            var actual = writer.SerializeToString(new ClassWithIndexer(), true, Encoding.ASCII);
            actual.AssertEqualsXml(@"
<root />
");
        }

        public enum ZEnum
        {
            A,
            B
        }

        private IContentWriterCollection collection;
        private IReportWriter writer;

        public class ClassWithIndexer
        {
            public virtual Object this[int index] => "djskdjsdk";
        }

        public class QZZ
        {
            public byte[] ByteArray { get; set; }
        }

        public class QXX
        {
            public Guid Id { get; set; }
        }

        public class DCC
        {
            public decimal Z { get; set; }
        }

        public class Q
        {
            public ZEnum Z { get; set; }
        }

        private class C2
        {
            public string U { get; set; }
            public string Q { get; set; }
            public string P { get; set; }
        }

        [XmlElement("TestRoot")]
        private class C1
        {
            public int? PackNumber { get; set; }
            public C2 C2Prop { get; set; }
        }
    }
}