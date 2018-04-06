using System;

using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests
{
    [TestFixture]
    public class StrictXmlSerializerTest
    {
        [SetUp]
        public void SetUp()
        {
            strictXmlSerializer = new StrictXmlSerializer();
        }

        [Test]
        public void TestCorrect()
        {
            var obj1 = strictXmlSerializer.Deserialize<A>("<A B=\"zzz\"><C>qxx</C></A>");
            Assert.AreEqual("zzz", obj1.B);
            Assert.AreEqual("qxx", obj1.C);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestUnexpectedAttribute()
        {
            strictXmlSerializer.Deserialize<A>("<A B=\"zzz\" Z=\"p\"><C>qxx</C></A>");
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestUnexpectedElement()
        {
            strictXmlSerializer.Deserialize<A>("<A B=\"zzz\"><D>qxx</D></A>");
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestUnexpectedElementWithArray()
        {
            strictXmlSerializer.Deserialize<P>("<P><A><C>q</C></A><A D=\"z\"></A></P>");
        }

        [Test]
        public void TestDeserializeWithoutTrimming()
        {
            strictXmlSerializer.Deserialize<A>(@"<A B="" valueB ""><C> valueC </C></A>", false).ShouldBeEquivalentTo(new A {B = " valueB ", C = " valueC "});
            strictXmlSerializer.Deserialize<C>(@"<C><Z> 2 </Z></C>").ShouldBeEquivalentTo(new C {Z = 2});
        }

        private StrictXmlSerializer strictXmlSerializer;

        public class A
        {
            [XmlAttribute]
            public string B { get; set; }

            public string C { get; set; }
        }

        public class P
        {
            public A[] A { get; set; }
        }

        public class C
        {
            public int Z { get; set; }
        }
    }
}