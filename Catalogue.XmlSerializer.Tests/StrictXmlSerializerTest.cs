using System;

using Catalogue.XmlSerializer.Attributes;

using FluentAssertions;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests
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

        [Test]
        public void TestUnexpectedAttribute()
        {
            Assert.Throws<InvalidOperationException>(() => strictXmlSerializer.Deserialize<A>("<A B=\"zzz\" Z=\"p\"><C>qxx</C></A>"));
        }

        [Test]
        public void TestUnexpectedElement()
        {
            Assert.Throws<InvalidOperationException>(() => strictXmlSerializer.Deserialize<A>("<A B=\"zzz\"><D>qxx</D></A>"));
        }

        [Test]
        public void TestUnexpectedElementWithArray()
        {
            Assert.Throws<InvalidOperationException>(() => strictXmlSerializer.Deserialize<P>("<P><A><C>q</C></A><A D=\"z\"></A></P>"));
        }

        [Test]
        public void TestDeserializeWithoutTrimming()
        {
            strictXmlSerializer.Deserialize<A>(@"<A B="" valueB ""><C> valueC </C></A>", false).Should().BeEquivalentTo(new A {B = " valueB ", C = " valueC "});
            strictXmlSerializer.Deserialize<C>(@"<C><Z> 2 </Z></C>").Should().BeEquivalentTo(new C {Z = 2});
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