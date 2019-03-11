using System.Xml;

using Catalogue.XmlSerializer.Tests.Writing;

using FluentAssertions;

using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer;
using SkbKontur.Catalogue.XmlSerializer.Reading;
using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    public class SimpleContentReaderTest
    {
        [Test]
        public void TestCantParse()
        {
            int read;
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>ZZZ</A>"))
            {
                read = ((IContentReader<int>)new SimpleContentReader<int>(int.TryParse)).Read(new SimpleXmlReader(xmlReader, true));
                Assert.AreEqual(XmlNodeType.None, xmlReader.NodeType);
            }
            Assert.AreEqual(0, read);
        }

        [Test]
        public void TestDouble_Dot()
        {
            double read;
            var reader = new FractionalContentReader<double>(double.TryParse);
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>1.23</A>"))
                read = reader.Read(new SimpleXmlReader(xmlReader, true));
            Assert.AreEqual(1.23, read, 1e-10);
        }

        [Test]
        public void TestDouble_Comma()
        {
            double read;
            var reader = new FractionalContentReader<double>(double.TryParse);
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>1,23</A>"))
                read = reader.Read(new SimpleXmlReader(xmlReader, true));
            Assert.AreEqual(1.23, read, 1e-10);
        }

        [Test]
        public void TestDecimal_Dot()
        {
            decimal read;
            var reader = new FractionalContentReader<decimal>(decimal.TryParse);
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>1.23</A>"))
                read = reader.Read(new SimpleXmlReader(xmlReader, true));
            Assert.AreEqual(1.23, read);
        }

        [Test]
        public void TestDecimal_Comma()
        {
            decimal read;
            var reader = new FractionalContentReader<decimal>(decimal.TryParse);
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>1,23</A>"))
                read = reader.Read(new SimpleXmlReader(xmlReader, true));
            Assert.AreEqual(1.23, read);
        }

        [Test]
        public void TestPosition()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A>123</A> <Z />"))
            {
                var read = ((IContentReader<int>)new SimpleContentReader<int>(int.TryParse)).Read(new SimpleXmlReader(xmlReader, true));
                Assert.AreEqual(XmlNodeType.Element, xmlReader.NodeType);
                Assert.AreEqual("Z", xmlReader.LocalName);
                Assert.AreEqual(123, read);
            }
        }

        [Test]
        public void TestReadEmptyTag()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A />"))
            {
                var contentReader = new SimpleContentReader<int>(int.TryParse);
                var read = contentReader.Read(new SimpleXmlReader(xmlReader, true));
                Assert.AreEqual(XmlNodeType.None, xmlReader.NodeType);
                Assert.AreEqual(0, read);
            }
        }

        [Test]
        public void TestReadSimple()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A x='xx'>123</A>"))
            {
                var contentReader = new SimpleContentReader<int>(int.TryParse);
                var read = contentReader.Read(new SimpleXmlReader(xmlReader, true));
                Assert.AreEqual(XmlNodeType.None, xmlReader.NodeType);
                Assert.AreEqual(123, read);
            }
        }

        [Test]
        public void TestStringReaderAndSubtree()
        {
            var stringContentReader = new StringContentReader();
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A><b>234</b></A><z />"))
                Assert.Throws<XmlException>(() => stringContentReader.Read(new SimpleXmlReader(xmlReader, true)));
        }

        [Test]
        public void TestSubtree()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader("<A><b>234</b></A><z />"))
            {
                var contentReader = new SimpleContentReader<int>(int.TryParse);
                Assert.Throws<XmlException>(() => contentReader.Read(new SimpleXmlReader(xmlReader, true)));
            }
        }

        [Test]
        public void TestGenericType()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader(@"<Message><specificTrash>1</specificTrash><SpecificTransaction><specifictransactiontrash>2</specifictransactiontrash></SpecificTransaction></Message>"))
            {
                var interpreter = new XmlAttributeInterpreter();
                var contentReader = new ClassContentReader<Message<SpecificTransaction>>(new ContentReaderCollection(interpreter, StandardConfigurations.EmptyOnDeserializeConfiguration), interpreter, StandardConfigurations.EmptyOnDeserializeConfiguration);
                var message = contentReader.Read(new SimpleXmlReader(xmlReader, true));
                message.Should().BeEquivalentTo(new Message<SpecificTransaction>
                    {
                        SpecificTrash = "1",
                        Transaction = new SpecificTransaction {SpecificTransactionTrash = "2"},
                    });
            }
        }

        [Test]
        public void TestGenericArray()
        {
            using (var xmlReader = ReportReaderHelpers.CreateXmlReader(@"<Message><specificTrash>st</specificTrash><SpecificTransaction><specifictransactiontrash>stt1</specifictransactiontrash></SpecificTransaction><SpecificTransaction><specifictransactiontrash>stt2</specifictransactiontrash></SpecificTransaction></Message>"))
            {
                var interpreter = new XmlAttributeInterpreter();
                var contentReader = new ClassContentReader<ArrayMessage<SpecificTransaction>>(new ContentReaderCollection(interpreter, StandardConfigurations.EmptyOnDeserializeConfiguration), interpreter, StandardConfigurations.EmptyOnDeserializeConfiguration);
                var message = contentReader.Read(new SimpleXmlReader(xmlReader, true));
                message.Should().BeEquivalentTo(new ArrayMessage<SpecificTransaction>
                    {
                        SpecificTrash = "st",
                        Transactions = new[]
                            {
                                new SpecificTransaction {SpecificTransactionTrash = "stt1"},
                                new SpecificTransaction {SpecificTransactionTrash = "stt2"}
                            }
                    });
            }
        }
    }
}