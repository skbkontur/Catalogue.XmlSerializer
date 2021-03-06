﻿using System.Collections.Generic;
using System.Text;

using Catalogue.XmlSerializer.Tests.Writing;

using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer;
using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests
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
            xmlSerializer = new SkbKontur.Catalogue.XmlSerializer.XmlSerializer();
        }

        [Test]
        public void Test()
        {
            var data = xmlSerializer.Deserialize<Q>(Encoding.UTF8.GetBytes(xmlText));
            Assert.AreEqual(xmlText, xmlSerializer.SerializeToUtfString(data, true));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestNull(bool collapseArrayElements)
        {
            var data = xmlSerializer.Deserialize<Q>(Encoding.UTF8.GetBytes(xmlNullText));
            Assert.AreEqual(xmlNullText, xmlSerializer.SerializeToUtfString(data, true, collapseArrayElements));
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

        private const string xmlNullText = @"<root />";

        private SkbKontur.Catalogue.XmlSerializer.XmlSerializer xmlSerializer;
    }
}