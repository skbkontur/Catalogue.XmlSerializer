using System;
using System.Collections.Specialized;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ReportReaderBigTest
    {
        [Test]
        public void TestKill()
        {
            const string source = @"
<R A='1'   B='2'>
    <Z>
        <V>4 5</V>
    </Z>
</R>";
            Assert.AreEqual("<R A=\"1\" B=\"2\"><Z><V>4 5</V></Z></R>", ReportReaderHelpers.KillSpaces(source));
        }

        [Test]
        public void TestKill2()
        {
            const string source =
                @"<A>1</A>
<A>2</A>
<B>100</B>";
            Assert.AreEqual("<A>1</A><A>2</A><B>100</B>", ReportReaderHelpers.KillSpaces(source));
        }

        [Test]
        public void TestSimple()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Val>1</Val>
    <Z>
        <trash>232323</trash>
        <QZZ>12</QZZ>
        <Val2>465</Val2>
    </Z>
    <P>-1</P>
    <Q />
</Root>",
                new C1 {Val = 1, Z = new Z1 {Val2 = 465, Val3 = 12}, P = -1});
        }

        [Test]
        public void TestSimpleNvc()
        {
            var nvc = new NameValueCollection
                {
                    {"Val", "1"},
                    {"Z.trash", "232323"},
                    {"Z.QZZ", "12"},
                    {"Z.Val2", "465"},
                    {"P", "-1"},
                    {"Q", null}
                };
            ReportReaderHelpers.Check(
                nvc,
                new C1 {Val = 1, Z = new Z1 {Val2 = 465, Val3 = 12}, P = -1});
        }

        [Test]
        public void TestEmptyNvc()
        {
            var reportReader = ReportReaderHelpers.CreateReader();
            Assert.Throws<EmptyNameValueCollectionNotSupportedException>(() => reportReader.Read<C1>(new NameValueCollection()));
        }

        [Test]
        public void TestWithAttributesNvc()
        {
            var nvc = new NameValueCollection
                {
                    {"A1$Attr", "zxx"},
                    {"B1$Attr", "1223"},
                    {"Zzz.QZZ", "12"},
                    {"Zzz.Val2", "465"}
                };
            ReportReaderHelpers.Check(
                nvc,
                new CWithAttr {A1 = "zxx", Zzz = new Z1 {Val2 = 465, Val3 = 12}});
        }

        [Test]
        public void TestWithAttributes()
        {
            ReportReaderHelpers.Check(
                @"
<Root A1='zxx' B1='1223'>
    <Zzz>
        <QZZ>12</QZZ>
        <Val2>465</Val2>
    </Zzz>
</Root>",
                new CWithAttr {A1 = "zxx", Zzz = new Z1 {Val2 = 465, Val3 = 12}});
        }

        [Test]
        public void TestEnumCorrect()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Z>
        B
    </Z>
</Root>
", new Z2 {Z = ZEnum.B});
        }

        [Test]
        public void TestEnumIncorrect()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Z>
        Z
    </Z>
</Root>
", new Z2 {Z = ZEnum.A});
        }

        [Test]
        public void TestEnumIgnoreCase()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Z>
        b
    </Z>
</Root>
", new Z2 {Z = ZEnum.B});
        }

        [Test]
        public void TestGuid()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Id>
        e3637da2-2220-422e-85f8-5f8268f024ea
    </Id>
</Root>
", new Z3 {Id = new Guid("e3637da2-2220-422e-85f8-5f8268f024ea")});
        }

        [Test]
        public void TestPrivateConstructor()
        {
            ReportReaderHelpers.Check(
                @"
<Root>
    <Id>
        e3637da2-2220-422e-85f8-5f8268f024ea
    </Id>
</Root>
", PrivateContructor.Create(new Guid("e3637da2-2220-422e-85f8-5f8268f024ea")));
        }

        public enum ZEnum
        {
            A,
            B
        }

        public class PrivateContructor
        {
            private PrivateContructor()
            {
            }

            private PrivateContructor(int x)
            {
            }

            private PrivateContructor(string y)
            {
            }

            public static PrivateContructor Create(Guid id)
            {
                return new PrivateContructor(6) {Id = id};
            }

            public Guid Id { get; set; }
        }

        public class Z3
        {
            public Guid Id { get; set; }
        }

        public class Z1
        {
            public int? Val2 { get; set; }

            [XmlElement("QZZ")]
            public int Val3 { get; set; }
        }

        public class C1
        {
            public int Val { get; set; }
            public Z1 Z { get; set; }
            public int Q { get; set; }
            public int P { get; set; }
        }

        public class CWithAttr
        {
            [XmlAttribute]
            public string A1 { get; set; }

            public Z1 Zzz { get; set; }
        }

        public class Z2
        {
            public ZEnum Z { get; set; }
        }
    }
}