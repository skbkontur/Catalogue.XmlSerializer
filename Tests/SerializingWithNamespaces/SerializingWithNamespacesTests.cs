using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.TestExtensions;
using SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents;
using SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleClassWithManyNamespaces;
using SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots;

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
        public void TestSerializeWithoutNamespacePrefixes()
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
        public void TestSerializeWithDeclaringNamespacePrefixes()
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
        public void TestSerializeWithNamespacesAndAttributesPrefixes()
        {
            var res = xmlSerializer.SerializeToUtfString(new Root3 {A = "1", B = "2", C = "3"}, true);
            res.AssertEqualsXml(@"<ns1:root xmlns:ns1=""http://alko.kontur.ru/napespace1"" qxx=""2"" ns1:qxz=""3"">
  <ns1:zzz>1</ns1:zzz>
</ns1:root>");
        }

        [Test]
        public void TestSerializeWithNamespaceInheriting()
        {
            xmlSerializer
                .SerializeToUtfString(
                    new Document<Body1>
                        {
                            Body = new Body1
                                {
                                    A = "A"
                                }
                        }, true).
                 AssertEqualsXml(@"<m:root xmlns:m=""main"" xmlns:one=""one"" xmlns:three=""three"" xmlns:two=""two"" ><m:Body><one:A>A</one:A></m:Body></m:root>");

            xmlSerializer
                .SerializeToUtfString(
                    new Document<Body2>
                        {
                            Body = new Body2
                                {
                                    A = "A",
                                    B = "B"
                                }
                        }, true).
                 AssertEqualsXml(@"<m:root xmlns:m=""main"" xmlns:one=""one"" xmlns:three=""three"" xmlns:two=""two"" ><m:Body><two:A>A</two:A><two:B>B</two:B></m:Body></m:root>");

            xmlSerializer
                .SerializeToUtfString(
                    new Document<Body3>
                        {
                            Body = new Body3
                                {
                                    A = "A",
                                    B = "B",
                                    C = "C"
                                }
                        }, true).
                 AssertEqualsXml(@"<m:root xmlns:m=""main"" xmlns:one=""one"" xmlns:three=""three"" xmlns:two=""two"" ><m:Body> <three:A>A</three:A> <three:B>B</three:B> <three:C>C</three:C> </m:Body></m:root>");
        }

        [Test]
        public void TestCheckPrefixDeclaringOrder()
        {
            xmlSerializer.SerializeToUtfString(new RootWithManyNamespaces
                {
                    A = "A",
                    B = "B",
                    C = "C",
                    D = "D",
                }, true)
                         .AssertEqualsXml(@"<root xmlns:d=""urn:a"" xmlns:c=""urn:b"" xmlns:b=""urn:c"" xmlns:a=""urn:d""> <d:A>A</d:A> <c:B>B</c:B> <b:C>C</b:C> <a:D>D</a:D> </root>");
        }

        private XmlSerialization.XmlSerializer xmlSerializer;
    }
}