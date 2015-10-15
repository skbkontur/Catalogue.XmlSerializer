//using System;

//using NUnit.Framework;

//namespace SKBKontur.Catalogue.XmlSerializer.Tests.Reading
//{
//    [TestFixture]
//    public class ReadHelpersTest
//    {
//        #region Setup/Teardown

//        [SetUp]
//        public void SetUp()
//        {
//            contentWriterCollection = GetMock<IContentReaderCollection>();
//            contentReader = GetMock<IContentReader<int>>();
//        }

//        #endregion

//        [Test]
//        public void TestBuildSetter()
//        {
//            contentWriterCollection.Expect(collection => collection.Get<int>()).Return(contentReader);

//            IContentPropertySetter<CWithProp> contentPropertySetter =
//                ReadHelpers.BuildSetter<CWithProp>(typeof(CWithProp).GetProperty("A"),
//                                                   contentWriterCollection);

//            var target = new CWithProp {A = 1};

//            contentReader.Expect(reader => reader.Read(null)).Return(100);
//            contentPropertySetter.SetProperty(target, null);
//            Assert.AreEqual(100, target.A);

//            contentReader.Expect(reader => reader.Read(null)).Return(200);
//            contentPropertySetter.SetProperty(target, null);
//            Assert.AreEqual(200, target.A);
//        }

//        [Test]
//        public void TestHasPublicConstructor()
//        {
//            Func<CWithPublic> emitConstruction = ReadHelpers.EmitConstruction<CWithPublic>();
//            Assert.AreEqual(0, CWithPublic.count);
//            CWithPublic cWithPublic = emitConstruction();
//            Assert.AreEqual(1, cWithPublic.localCount);
//            Assert.AreEqual(1, CWithPublic.count);
//        }

//        [Test]
//        public void TestNoPublicConstructor()
//        {
//            Func<CNoPublic> emitConstruction = ReadHelpers.EmitConstruction<CNoPublic>();
//            Assert.AreEqual(0, CNoPublic.count);
//            CNoPublic result = emitConstruction();
//            Assert.AreEqual(0, result.localCount);
//            Assert.AreEqual(0, CNoPublic.count);
//        }

//        public class CWithProp
//        {
//            public int A { get; set; }
//        }

//        private IContentReaderCollection contentWriterCollection;
//        private IContentReader<int> contentReader;

//        private class CWithPublic
//        {
//            public CWithPublic()
//            {
//                localCount = ++count;
//            }

//            public static int count;
//            public readonly int localCount;
//        }

//        private class CNoPublic
//        {
//            private CNoPublic()
//            {
//                localCount = ++count;
//            }

//            public static int count;
//            public readonly int localCount;
//        }
//    }
//}

