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
        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage = "массив массивов")]
        public void TestArrayOfArrays()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<string[][]>("<xxx />");
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

        [Test, ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Duplicate element 'A'")]
        public void TestDuplicateItemsWithExceptionWhenDuplicateElement()
        {
            var configuration = new OnDeserializeConfiguration();
            configuration.OnDuplicateElement += delegate(object sender, DeserializationContext context) { throw new InvalidOperationException(string.Format("Duplicate element '{0}'", context.CurrentElementLocalName)); };

            ReportReaderHelpers.CreateReader(configuration).ReadFromString<CForDuplicateTest>(
                @"<root>
    <A>1</A>
    <B>x</B>
    <B>y</B>
    <A>2</A>
</root>
");
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
").ShouldBeEquivalentTo(new CForDuplicateTest {A = 2, B = new[] {"x", "y"}});
        }

        [Test, ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Duplicate element 'B'")]
        public void TestDuplicateItemsInArrayWithExceptionWhenDuplicateElement()
        {
            var configuration = new OnDeserializeConfiguration();
            configuration.OnDuplicateElement += delegate(object sender, DeserializationContext context) { throw new InvalidOperationException(string.Format("Duplicate element '{0}'", context.CurrentElementLocalName)); };

            ReportReaderHelpers.CreateReader(configuration).ReadFromString<CForDuplicateTest>(
                @"<root>
    <B>x</B>
    <B>y</B>
    <A>2</A>
    <B>z</B>
</root>
");
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
").ShouldBeEquivalentTo(new CForDuplicateTest {A = 2, B = new[] {"z"}});
        }

        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage = "многомерный массив")]
        public void TestMultidimensionalArray()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<string[,]>("<xxx />");
        }

        [Test]
        public void TestNonStringAttr()
        {
            ReportReaderHelpers.CreateReader().ReadFromString
                <CWithNonStringProp>(
                    @"<root A='1'></root>
").ShouldBeEquivalentTo(new CWithNonStringProp {A = 1});
        }

        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Type '.*CInvisible' should be visible outside assembly", MatchType = MessageMatch.Regex)]
        public void TestNonVisibleClassesNotSupported()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<CInvisible>("<xxx />");
        }

        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Type 'System.Object' cannot be deserialized")]
        public void TestObjectDisabled()
        {
            ReportReaderHelpers.CreateReader().ReadFromString<object>("<xxx />");
        }

        public class CWithBadProps
        {
            public int this[int i] { get { return this.i + i; } set { this.i = value + i; } }

            public string this[string s] { get { return s; } }

            public long this[long l] { set { this.l = value + l; } }

            public long SetOnlyL { set { fl = value; } }

            public long SetOnlyLPrivate { private set { throw new NotSupportedException(); } get { return 200; } }

            public long GetOnlyL { get { return 23445; } }

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