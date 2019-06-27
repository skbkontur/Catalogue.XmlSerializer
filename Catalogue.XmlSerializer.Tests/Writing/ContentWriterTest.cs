using System;
using System.Text;
using System.Xml;

using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer;
using SkbKontur.Catalogue.XmlSerializer.Attributes;
using SkbKontur.Catalogue.XmlSerializer.Writing;
using SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ContentWriterTest
    {
        [SetUp]
        public void SetUp()
        {
            collection = new TestContentWriterCollection();
            xmlAttributeInterpreter = new XmlAttributeInterpreter();
        }

        [Test]
        public void TestArrayRank()
        {
            Assert.Throws<NotSupportedException>(() => new ClassContentWriter(typeof(CWithBadArray), collection, xmlAttributeInterpreter));
        }

        [Test]
        public void TestClass()
        {
            var writer = new ClassContentWriter(typeof(MyClass), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new MyClass {Value = "zzz"}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root>
    <Value>zzz</Value>
</root>");
        }

        [Test]
        public void NullArrayTest()
        {
            var writer = new ClassContentWriter(typeof(Temp), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new Temp {Val = "zzz"}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root>
    <Val>zzz</Val>
</root>");
        }

        [Test]
        public void TestCustomContent()
        {
            var writer = new ClassContentWriter(typeof(WithCustomContent), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(
                    new WithCustomContent {Arr = new[] {new CustomWrite("a"), null}, Item = new CustomWrite("b")},
                    xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(
                @"
<root>
    <Arr>custom-a</Arr>
    <Arr/>
    <Item2 />
    <Item>custom-b</Item>
</root>
");
        }

        [Test]
        public void TestNullable()
        {
            var writer = new ClassContentWriter(typeof(C1), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new C1 {Int = 134, Value = 567}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root>
    <Value>567</Value>
    <Int>134</Int>
</root>");
        }

        [Test]
        public void TestNullableWithNullValue()
        {
            var writer = new ClassContentWriter(typeof(C1), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new C1 {Int = 134}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root><Value /><Int>134</Int></root>");
        }

        [Test]
        public void TestNullValue()
        {
            var writer = new ClassContentWriter(typeof(MyClass), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new MyClass {Value = null}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml("<root><Value /></root>");
        }

        [Test]
        public void TestStringContentWriter()
        {
            var writer = new StringContentWriter();
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write("zzz", xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml("<root>zzz</root>");
        }

        [Test]
        public void TestStringContentWriterEmptyString()
        {
            var writer = new StringContentWriter();
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write("", xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml("<root />");
        }

        [Test]
        public void TestValueContentWriter()
        {
            var writer = new ValueContentWriter();
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(1, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml("<root>1</root>");
        }

        [Test]
        public void TestWithArray()
        {
            var writer = new ClassContentWriter(typeof(CWithArray), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new CWithArray {Values = new int?[] {1, null, 2, 3}}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(
                @"
<root>
    <Values>1</Values>
    <Values/>
    <Values>2</Values>
    <Values>3</Values>
</root>
");
        }

        [Test]
        public void TestWithArrayEmpty()
        {
            var writer = new ClassContentWriter(typeof(CWithArray), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new CWithArray {Values = new int?[0]}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root />");
        }

        [Test]
        public void TestWithAttributes()
        {
            var writer = new ClassContentWriter(typeof(ClassWithAttributes), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new ClassWithAttributes {Attr1 = "a1", attr2 = 2, Value = "zzz", Attr3 = "a3", SuperValue = "superZZZ"}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root Attr3='a3' Attr1='a1' attr2='2'>
    <SuperValue>superZZZ</SuperValue>
    <Value>zzz</Value>
</root>");
        }

        [Test]
        public void TestWithElementNameAttr()
        {
            var writer = new ClassContentWriter(typeof(ClassWithName), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new ClassWithName {Value = "Qqq"}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root>
    <ZzZ>Qqq</ZzZ>
</root>");
        }

        [Test]
        public void TestWithNullAttributes()
        {
            var writer = new ClassContentWriter(typeof(ClassWithAttributes), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new ClassWithAttributes {Attr1 = null, attr2 = 2, Value = "zzz"}, xmlWriter);
                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(@"
<root Attr3="""" Attr1="""" attr2='2'>
    <SuperValue/>
    <Value>zzz</Value>
</root>");
        }

        [Test]
        public void TestWithSublcass()
        {
            var writer = new ClassContentWriter(typeof(MyClassWithSubclass), collection, xmlAttributeInterpreter);
            var builder = new StringBuilder();
            using (var xmlWriter = CreateWriter(builder))
            {
                xmlWriter.WriteStartElement("root");
                writer.Write(new MyClassWithSubclass {MyClassProp = new MyClass {Value = "zzz"}}, xmlWriter);

                xmlWriter.WriteEndElement();
            }
            builder.ToString().AssertEqualsXml(
                @"
<root>
    <MyClassProp>
        <Value>zzz</Value>
    </MyClassProp>
</root>");
        }

        private static IWriter CreateWriter(StringBuilder builder)
        {
            return new SimpleXmlWriter(XmlWriter.Create(builder, new XmlWriterSettings {OmitXmlDeclaration = true}));
        }

        private IContentWriterCollection collection;
        private IXmlAttributeInterpreter xmlAttributeInterpreter;

        private class Temp
        {
            public string Val { get; set; }
            public Temp2[] Temp2 { get; set; }
        }

        private class Temp2
        {
            public string Val { get; set; }
        }

        private class MyClass
        {
            public string Value { get; set; }
        }

        private class ClassWithName
        {
            [XmlElement("ZzZ")]
            public string Value { get; set; }
        }

        private class SuperclassWithAttributes
        {
            [XmlAttribute]
            public string Attr3 { get; set; }

            public string SuperValue { get; set; }
        }

        private class ClassWithAttributes : SuperclassWithAttributes
        {
            [XmlAttribute]
            public string Attr1 { get; set; }

            public string Value { get; set; }

            [XmlAttribute]
            public int attr2 { get; set; }
        }

        private class MyClassWithSubclass
        {
            public MyClass MyClassProp { get; set; }
        }

        private class C1
        {
            public int? Value { get; set; }
            public int Int { get; set; }
        }

        private class CWithArray
        {
            public int?[] Values { get; set; }
        }

        private class CWithBadArray
        {
            public int[,] Values { get; set; }
        }

        private class CustomWrite : ICustomWrite
        {
            public CustomWrite(string value)
            {
                this.value = value;
            }

            public void Write(IWriter writer)
            {
                writer.WriteValue("custom-" + value);
            }

            private readonly string value;
        }

        private class WithCustomContent
        {
            public CustomWrite[] Arr { get; set; }
            public CustomWrite Item2 { get; set; }
            public CustomWrite Item { get; set; }
        }
    }
}