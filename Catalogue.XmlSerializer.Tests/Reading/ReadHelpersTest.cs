using NUnit.Framework;

using SkbKontur.Catalogue.XmlSerializer.Reading;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class ReadHelpersTest
    {
        [Test]
        public void TestHasPublicConstructor()
        {
            var emitConstruction = ReadHelpers.EmitConstruction<CWithPublic>();
            Assert.AreEqual(0, CWithPublic.count);
            var cWithPublic = emitConstruction();
            Assert.AreEqual(1, cWithPublic.localCount);
            Assert.AreEqual(1, CWithPublic.count);
        }

        [Test]
        public void TestNoPublicConstructor()
        {
            var emitConstruction = ReadHelpers.EmitConstruction<CNoPublic>();
            Assert.AreEqual(0, CNoPublic.count);
            var result = emitConstruction();
            Assert.AreEqual(0, result.localCount);
            Assert.AreEqual(0, CNoPublic.count);
        }

        private class CWithPublic
        {
            public CWithPublic()
            {
                localCount = ++count;
            }

            public static int count;
            public readonly int localCount;
        }

        private class CNoPublic
        {
            private CNoPublic()
            {
                localCount = ++count;
            }

            public static int count;
            public readonly int localCount;
        }
    }
}