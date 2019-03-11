using System;
using System.Collections.Specialized;

using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Reading.Configuration;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ReportReaderHardCasesTest
    {
        [Test]
        public void TestArrayOfArrays()
        {
            Assert.That(() => ReportReaderHelpers.CreateReader().ReadFromString<string[][]>("<xxx />"), Throws.Exception.TypeOf<NotSupportedException>().With.Message.EqualTo("массив массивов"));
        }

        [Test]
        public void TestBadPropertiesIgnoredNvc()
        {
            var nvc = new NameValueCollection
                {
                    {"SetOnlyL", "100"},
                    {"l", "123"},
                    {"S", "qqq"},
                    {"set_Item_0", "3"},
                    {"set_Item_0", "4"}
                };
            ReportReaderHelpers.Check(
                nvc,
                new CWithBadProps {fl = 100});
        }

        [Test]
        public void TestBadPropertiesIgnored()
        {
            ReportReaderHelpers.Check(
                @"
<root>
    <SetOnlyL>100</SetOnlyL>
    <l>123</l>
    <S>qqq</S>
    <set_Item>3</set_Item>
    <get_Item>4</get_Item>
</root>
",
                new CWithBadProps {fl = 100});
        }

        [Test]
        public void TestDuplicateItemsWithExceptionWhenDuplicateElement()
        {
            var configuration = new OnDeserializeConfiguration();
            configuration.OnDuplicateElement += delegate(object sender, DeserializationContext context) { throw new InvalidOperationException($"Duplicate element '{context.CurrentElementLocalName}'"); };

            Assert.That(() => ReportReaderHelpers.CreateReader(configuration).ReadFromString<CForDuplicateTest>(
                @"<root>
    <A>1</A>
    <B>x</B>
    <B>y</B>
    <A>2</A>
</root>
"), Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("Duplicate element 'A'"));
        }

        [Test]
        public void TestDuplicateItemsWithNoExceptionWhenDuplicateElement()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<CForDuplicateTest>(
                @"<root>
    <A>1</A>
    <B>x</B>
    <B>y</B>
    <A>2</A>
</root>
").Should().BeEquivalentTo(new CForDuplicateTest {A = 2, B = new[] {"x", "y"}});
        }

        [Test]
        public void TestDuplicateItemsInArrayWithExceptionWhenDuplicateElement()
        {
            var configuration = new OnDeserializeConfiguration();
            configuration.OnDuplicateElement += delegate(object sender, DeserializationContext context) { throw new InvalidOperationException($"Duplicate element '{context.CurrentElementLocalName}'"); };

            Assert.That(() => ReportReaderHelpers.CreateReader(configuration).ReadFromString<CForDuplicateTest>(
                @"<root>
    <B>x</B>
    <B>y</B>
    <A>2</A>
    <B>z</B>
</root>
"), Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("Duplicate element 'B'"));
        }

        [Test]
        public void TestDuplicateItemsInArrayWithNoExceptionWhenDuplicateElement()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<CForDuplicateTest>(
                @"<root>
    <B>x</B>
    <B>y</B>
    <A>2</A>
    <B>z</B>
</root>
").Should().BeEquivalentTo(new CForDuplicateTest {A = 2, B = new[] {"z"}});
        }

        [Test]
        public void TestMultidimensionalArray()
        {
            Assert.That(() => ReportReaderHelpers.CreateReader().ReadFromString<string[,]>("<xxx />"), Throws.Exception.TypeOf<NotSupportedException>().With.Message.EqualTo("многомерный массив"));
        }

        [Test]
        public void TestNonStringAttr()
        {
            ReportReaderHelpers.CreateReader().ReadFromString
                <CWithNonStringProp>(
                    @"<root A='1'></root>
").Should().BeEquivalentTo(new CWithNonStringProp {A = 1});
        }

        [Test]
        public void TestNonVisibleClassesNotSupported()
        {
            Assert.That(() => ReportReaderHelpers.CreateReader().ReadFromString<CInvisible>("<xxx />"), Throws.Exception.TypeOf<NotSupportedException>().With.Message.Matches("Type '.*CInvisible' should be visible outside assembly"));
        }

        [Test]
        public void TestObjectDisabled()
        {
            Assert.That(() => ReportReaderHelpers.CreateReader().ReadFromString<object>("<xxx />"), Throws.Exception.TypeOf<NotSupportedException>().With.Message.EqualTo("Type 'System.Object' cannot be deserialized"));
        }

        public class CWithBadProps
        {
            public int this[int i] { get => this.i + i; set => this.i = value + i; }

            public string this[string s] => s;

            public long this[long l] { set => this.l = value + l; }

            public long SetOnlyL { set => fl = value; }

            public long SetOnlyLPrivate { private set => throw new NotSupportedException(); get => 200; }

            public long GetOnlyL => 23445;

            public static string S { get; set; }
            public long fl;
            public int i;
            public long l;
        }

        public class CForDuplicateTest
        {
            public int A { get; set; }
            public string[] B { get; set; }
        }

        public class CWithNonStringProp
        {
            [XmlAttribute]
            public int A { get; set; }
        }

        private class CInvisible
        {
        }
    }
}