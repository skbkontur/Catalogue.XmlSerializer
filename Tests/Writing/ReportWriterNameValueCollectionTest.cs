using System.Collections.Specialized;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ReportWriterNameValueCollectionTest
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
            var actual = writer.SerializeToNameValueCollection(new C1 {PackNumber = 12, C2Prop = new C2 {P = "sss", U = ""}}, true);
            var expected = new NameValueCollection {{"PackNumber", "12"}, {"C2Prop.P", "sss"}};
            expected.AssertAreEqual(actual);
        }

        [Test]
        public void TestSimpleNotSkipEmpty()
        {
            var actual = writer.SerializeToNameValueCollection(new C1 {PackNumber = 12, C2Prop = new C2 {P = "sss", U = ""}}, false);
            var expected = new NameValueCollection {{"PackNumber", "12"}, {"C2Prop.U", null}, {"C2Prop.Q", null}, {"C2Prop.P", "sss"}};
            expected.AssertAreEqual(actual);
        }

        [Test]
        public void TestGenericType()
        {
            var message = new Message<SpecificTransaction>
                {
                    SpecificTrash = "1",
                    Transaction = new SpecificTransaction {SpecificTransactionTrash = "2"},
                };
            var actual = writer.SerializeToNameValueCollection(message, true);
            var expected = new NameValueCollection {{"specificTrash", "1"}, {"SpecificTransaction.specifictransactiontrash", "2"}};
            expected.AssertAreEqual(actual);
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

            var actual = writer.SerializeToNameValueCollection(message, true);
            var expected = new NameValueCollection {{"specificTrash", "st"}, {"SpecificTransaction$0.specifictransactiontrash", "stt1"}, {"SpecificTransaction$1.specifictransactiontrash", "stt2"}};
            expected.AssertAreEqual(actual);
        }

        [Test]
        public void TestByteArray()
        {
            var message = new QZZ
                {
                    ByteArray = new byte[] {0, 1, 2, 3, 4, 5}
                };

            var actual = writer.SerializeToNameValueCollection(message, true);
            var expected = new NameValueCollection {{"ByteArray", "AAECAwQF"}};
            expected.AssertAreEqual(actual);
        }

        private IContentWriterCollection collection;
        private IReportWriter writer;

        public class QZZ
        {
            public byte[] ByteArray { get; set; }
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