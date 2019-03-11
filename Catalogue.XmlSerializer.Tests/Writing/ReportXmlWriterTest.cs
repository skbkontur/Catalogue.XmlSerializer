using System;
using System.Text;
using System.Xml;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ReportXmlWriterTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            builder = new StringBuilder();
            writer = new CollapseWriter(new SimpleXmlWriter(XmlWriter.Create(builder, new XmlWriterSettings {OmitXmlDeclaration = true})));
        }

        #endregion

        [Test]
        public void TestAttrAfterValueBug()
        {
            writer.WriteStartElement("root");
            writer.WriteStartElement("A");
            writer.WriteValue("q");
            Assert.Throws<InvalidOperationException>(() => writer.WriteStartAttribute("z"));
        }

        [Test]
        public void TestAttrBeforeRootBug()
        {
            Assert.Throws<InvalidOperationException>(() => writer.WriteStartAttribute("z"));
        }

        [Test]
        public void TestCollapse()
        {
            writer.WriteStartElement("root");
            writer.WriteStartElement("A");

            writer.WriteStartElement("z");
            writer.WriteEndElement();

            writer.WriteStartElement("x");
            writer.WriteValue("y");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml(
                @"<root>
    <A>
        <x>y</x>
    </A>
</root>
");
        }

        [Test]
        public void TestCollapseHard()
        {
            writer.WriteStartElement("root");
            writer.WriteStartElement("A");
            writer.WriteValue("a");
            writer.WriteEndElement();

            writer.WriteStartElement("B");
            writer.WriteValue("b");
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml(
                @"<root>
    <A>a</A>
    <B>b</B>
</root>
");
        }

        [Test]
        public void TestRoot()
        {
            writer.WriteStartElement("root");
            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml("<root />");
        }

        [Test]
        public void TestRootWithAttr()
        {
            writer.WriteStartElement("root");
            writer.WriteStartAttribute("p");
            writer.WriteValue("q");
            writer.WriteEndAttribute();
            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml("<root p='q' />");
        }

        [Test]
        public void TestSimple()
        {
            writer.WriteStartElement("root");
            writer.WriteStartElement("a");
            writer.WriteStartAttribute("p");
            writer.WriteValue("q");
            writer.WriteEndAttribute();
            writer.WriteValue("zzz");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml(
                @"<root>
    <a p='q'>zzz</a>
</root>
");
        }

        [Test]
        public void TestNullableBool()
        {
            writer.WriteStartElement("root");
            writer.WriteStartElement("a");
            writer.WriteValue(null);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Dispose();
            builder.ToString().AssertEqualsXml(
                @"<root/>
");
        }

        private StringBuilder builder;
        private CollapseWriter writer;
    }
}