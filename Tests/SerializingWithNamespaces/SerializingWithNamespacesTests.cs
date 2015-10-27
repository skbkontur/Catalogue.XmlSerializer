using System;
using System.IO;
using System.Text;
using System.Xml;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.TestExtensions;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces
{
    [TestFixture]
    public class SerializingWithNamespacesTests
    {
        [SetUp]
        public void SetUp()
        {
            xmlSerializer = new XmlSerialization.XmlSerializer();
        }

        [Test]
        public void SerializeWithoutNamespacePrefixes()
        {
            var res = xmlSerializer.SerializeToUtfString(new Root1 {A = "1", B = new BClass {C = "2"}}, true);
            res.AssertEqualsXml(@"<root xmlns=""http://alko.kontur.ru/napespace1"">
  <A>1</A>
  <B xmlns=""urn:2"">
    <C xmlns=""http://alko.kontur.ru/napespace1"">2</C>
  </B>
</root>
");
        }

        [Test]
        public void SerializeWithDeclaringNamespacePrefixes()
        {
            var res = xmlSerializer.SerializeToUtfString(new Root2 {A = "1", B = new BClass2 {C = new CClass {D = "2"}}}, true);
            res.AssertEqualsXml(@"<ns1:root xmlns:ns1=""http://alko.kontur.ru/napespace1"">
  <ns1:A>1</ns1:A>
  <B xmlns:ns3=""namespace:3"" xmlns=""urn:2"">
    <ns1:C>
      <ns3:D>2</ns3:D>
    </ns1:C>
  </B>
</ns1:root>");
        }

        [Test]
        public void SerializeWithNamespacesAndAttributesPrefixes()
        {
            var res = xmlSerializer.SerializeToUtfString(new Root3 {A = "1", B = "2"}, true);
            res.AssertEqualsXml(@"<ns1:root xmlns:ns1=""http://alko.kontur.ru/napespace1"" qxx=""2"">
  <ns1:zzz>1</ns1:zzz>
</ns1:root>");
        }

        private XmlSerialization.XmlSerializer xmlSerializer;
    }

    [XmlNamespace(Namespaces.Namespace1, true)]
    public class Root1
    {
        public string A { get; set; }

        [XmlNamespace(Namespaces.Namespace2)]
        public BClass B { get; set; }
    }

    [XmlNamespace(Namespaces.Namespace1, true)]
    public class BClass
    {
        public string C { get; set; }
    }

    [XmlNamespace(Namespaces.Namespace1, true)]
    [DeclareXmlNamespace("ns1", Namespaces.Namespace1)]
    public class Root2
    {
        public string A { get; set; }

        [XmlNamespace(Namespaces.Namespace2)]
        public BClass2 B { get; set; }
    }

    [XmlNamespace(Namespaces.Namespace1, true)]
    [DeclareXmlNamespace("ns3", Namespaces.Namespace3)]
    public class BClass2
    {
        public CClass C { get; set; }
    }

    public class CClass
    {
        [XmlNamespace(Namespaces.Namespace3)]
        public string D { get; set; }
    }

    [XmlNamespace(Namespaces.Namespace1)]
    [DeclareXmlNamespace("ns1", Namespaces.Namespace1)]
    public class Root3
    {
        [XmlElement("zzz")]
        public string A { get; set; }

        [XmlAttribute("qxx")]
        public string B { get; set; }
    }

    public class Namespaces
    {
        public const string Namespace1 = "http://alko.kontur.ru/napespace1";
        public const string Namespace2 = "urn:2";
        public const string Namespace3 = "namespace:3";
    }
}