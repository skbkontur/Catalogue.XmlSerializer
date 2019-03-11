using System.Text;
using System.Xml;

using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer;
using SkbKontur.Catalogue.XmlSerializer.Writing;
using SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.Tests.Writing
{
    [TestFixture]
    public class ContentWriterCollectionTest
    {
        [SetUp]
        public void SetUp()
        {
            collection = new ContentWriterCollection(new XmlAttributeInterpreter());
        }

        [Test]
        public void TestCacheNonSimpleTypes()
        {
            var writer = collection.Get(typeof(C1));
            Assert.AreSame(writer, collection.Get(typeof(C1)));
        }

        [Test]
        public void TestCacheSimpleTypes()
        {
            var writer = collection.Get(typeof(string));
            //Assert.IsInstanceOfType(typeof (StringContentWriter), writer);
            Assert.AreSame(writer, collection.Get(typeof(string)));
            writer = collection.Get(typeof(int));
            //Assert.IsInstanceOfType(typeof (ValueContentWriter), writer);
            Assert.AreSame(writer, collection.Get(typeof(int)));
        }

        [Test]
        public void TestClassWriter()
        {
            Check(new C1 {S = "zzz", Z = ""}, "<S>zzz</S><Z />");
        }

        [Test]
        public void TestClassWriterHard()
        {
            Check(new Cyclic {S = new Cyclic(), I = new Inner {c = new Cyclic {Q = "q"}}},
                  "<Q /><S><Q /><S /><I /></S><I><c><Q>q</Q><S /><I /></c></I>");
        }

        [Test]
        public void TestCustomContent()
        {
            Check(new CustomWrite("z"), "z");
        }

        [Test]
        public void TestPrimitiveTypes()
        {
            Check(1, "1");
            Check(22332L, "22332");
            Check(22333u, "22333");
        }

        [Test]
        public void TestString()
        {
            Check("zZz", "zZz");
        }

        [Test]
        public void TestWithNullable()
        {
            Check(new WithNullable {Int = 37843}, "<Int>37843</Int>");
        }

        private void Check<T>(T value, string result)
        {
            var writer = collection.Get(typeof(T));
            var builder = new StringBuilder();
            using (
                var xmlWriter =
                    new SimpleXmlWriter(XmlWriter.Create(builder,
                                                         new XmlWriterSettings
                                                             {
                                                                 OmitXmlDeclaration = true,
                                                                 ConformanceLevel = ConformanceLevel.Fragment
                                                             })))
                writer.Write(value, xmlWriter);
            Assert.AreEqual(result, builder.ToString());
        }

        private ContentWriterCollection collection;

        private class C1
        {
            public string S { get; set; }
            public string Z { get; set; }
        }

        private class Cyclic
        {
            public string Q { get; set; }
            public Cyclic S { get; set; }
            public Inner I { get; set; }
        }

        private class Inner
        {
            public Cyclic c { get; set; }
        }

        private class WithNullable
        {
            public int? Int { get; set; }
        }

        private class CustomWrite : ICustomWrite
        {
            public CustomWrite(string s)
            {
                this.s = s;
            }

            public void Write(IWriter writer)
            {
                writer.WriteValue(s);
            }

            private readonly string s;
        }
    }
}