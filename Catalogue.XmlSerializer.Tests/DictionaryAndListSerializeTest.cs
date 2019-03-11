using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerializer.Tests.Writing;

namespace SKBKontur.Catalogue.XmlSerializer.Tests
{
    public class Q
    {
        [XmlElement("Qxx")]
        public List<Message<string>> List { get; set; }

        [XmlElement("Qzz")]
        public Message<string>[] Array { get; set; }

        [XmlElement("Qyy")]
        public Dictionary<string, Message<string>> Dict { get; set; }
    }

    [TestFixture]
    public class DictionaryAndListSerializeTest
    {
        [SetUp]
        public void SetUp()
        {
            xmlSerializer = new XmlSerialization.XmlSerializer();
        }

        [Test]
        public void Test()
        {
            var data = xmlSerializer.Deserialize<Q>(Encoding.UTF8.GetBytes(xmlText));
            Assert.AreEqual(xmlText, xmlSerializer.SerializeToUtfString(data, true));
        }

        private const string xmlText = @"<root>
  <Qxx>
    <specificTrash>qzz1</specificTrash>
    <String>qxx1</String>
  </Qxx>
  <Qxx>
    <specificTrash>qzz2</specificTrash>
    <String>qxx2</String>
  </Qxx>
  <Qzz>
    <specificTrash>qzz1</specificTrash>
    <String>qxx1</String>
  </Qzz>
  <Qzz>
    <specificTrash>qzz2</specificTrash>
    <String>qxx2</String>
  </Qzz>
  <Qyy>
    <Key>qzz1</Key>
    <Value>
      <specificTrash>qzz1</specificTrash>
      <String>qxx1</String>
    </Value>
  </Qyy>
  <Qyy>
    <Key>qzz2</Key>
    <Value>
      <specificTrash>qzz2</specificTrash>
      <String>qxx2</String>
    </Value>
  </Qyy>
</root>";

        private XmlSerialization.XmlSerializer xmlSerializer;
    }
}