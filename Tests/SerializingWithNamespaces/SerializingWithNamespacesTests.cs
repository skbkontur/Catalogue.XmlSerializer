using System;
using System.IO;
using System.Text;
using System.Xml;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;

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
            Console.WriteLine(res);
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
        public void Test()
        {
            var memoryStream = new MemoryStream();
            using(var z = XmlWriter.Create(memoryStream, new XmlWriterSettings
                {
                    NamespaceHandling = NamespaceHandling.OmitDuplicates
                }))
            {
                z.WriteStartElement("a", "urn1");
                z.WriteAttributeString("xmlns", "z", null, "urn1");
                z.WriteStartElement("b", "urn1");

                z.WriteAttributeString("c", "urn1", "2");
                z.WriteEndElement();

                z.WriteEndElement();
            }
            Console.WriteLine(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        [Test]
        public void Test2()
        {
            //var xml = @"<?xml version=""1.0"" encoding=""utf-8""?><a xmlns:z=""urn1"" xmlns=""urn1""><z:b a:c=""2"" z:c=""z"" xmlns:a=""urn2"" /></a>";
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?><a xmlns=""urn"" xmlns:n=""urn2"" ><n:b><c></c></n:b></a>";
            var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
            while(reader.Read())
            {
                switch(reader.NodeType)
                {
                case XmlNodeType.Element:
                    Console.Write("<" + reader.Name + "(" + reader.NamespaceURI + ")");
                    if(reader.HasAttributes)
                    {
                        while(reader.MoveToNextAttribute())
                            Console.Write(" " + reader.Name + "(" + reader.NamespaceURI + ")=" + reader.Value);
                        reader.MoveToElement();
                    }
                        Console.WriteLine(">");

                    break;

                case XmlNodeType.Text:
                    Console.WriteLine(reader.Value);
                    break;
                case XmlNodeType.EndElement:
                    Console.WriteLine("<" + reader.Name + "(" + reader.NamespaceURI + ")" + ">");
                    break;
                }
            }
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

    [XmlNamespace(Namespaces.Namespace1, true)]
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