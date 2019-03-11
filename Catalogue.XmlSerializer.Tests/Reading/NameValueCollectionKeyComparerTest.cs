using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer.Reading;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class NameValueCollectionKeyComparerTest
    {
        [SetUp]
        public void SetUp()
        {
            comparer = new NameValueCollectionKeyComparer();
        }

        [Test]
        public void TestNoDelimitersStrings()
        {
            Assert.AreEqual(-1, comparer.Compare("abracadabra", "abracadabrr"));
            Assert.AreEqual(1, comparer.Compare("abracadabrr", "abracadabra"));
        }

        [Test]
        public void TestNoDelimitersNumbers()
        {
            Assert.AreEqual(1, comparer.Compare("10", "2"));
            Assert.AreEqual(-1, comparer.Compare("1", "10"));
            Assert.AreEqual(-1, comparer.Compare("1", "9"));
            Assert.AreEqual(1, comparer.Compare("11", "10"));
            Assert.AreEqual(-1, comparer.Compare("1", "2"));
            Assert.AreEqual(1, comparer.Compare("100", "11"));

            Assert.AreEqual(0, comparer.Compare("1", "01"));
            Assert.AreEqual(0, comparer.Compare("0", "000"));
            Assert.AreEqual(0, comparer.Compare("-0", "-000"));
        }

        [Test]
        public void TestNegativeNumbersIsStrings()
        {
            Assert.AreEqual(-1, comparer.Compare("-10", "-2"));
            Assert.AreEqual(-1, comparer.Compare("-1", "-10"));
            Assert.AreEqual(-1, comparer.Compare("-1", "-9"));
            Assert.AreEqual(1, comparer.Compare("-11", "-10"));
            Assert.AreEqual(-1, comparer.Compare("-1", "-2"));
            Assert.AreEqual(-1, comparer.Compare("-100", "-11"));

            Assert.AreEqual(1, comparer.Compare("-1", "-01"));
        }

        [Test]
        public void TestEquals()
        {
            Assert.AreEqual(0, comparer.Compare("a", "a"));
            Assert.AreEqual(0, comparer.Compare("a$b$c", "a$b$c"));
            Assert.AreEqual(0, comparer.Compare("a$1$b", "a$01$b"));
            Assert.AreEqual(0, comparer.Compare("1", "01"));
        }

        [Test]
        public void TestEqualPrefix()
        {
            Assert.AreEqual(-1, comparer.Compare("a$b$c$d", "a$b$c$d$"));
            Assert.AreEqual(1, comparer.Compare("a$b$c$d$", "a$b$c$d"));
        }

        [Test]
        public void TestSimple()
        {
            Assert.AreEqual(-1, comparer.Compare("a$b$c", "a$d$e"));
            Assert.AreEqual(1, comparer.Compare("a$03$c", "a$2$e"));
        }

        private NameValueCollectionKeyComparer comparer;
    }
}