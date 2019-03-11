using System;
using System.Collections.Specialized;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests
{
    public static class NameValueCollectionExtentions
    {
        public static void AssertAreEqual(this NameValueCollection expected, NameValueCollection actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected.AllKeys[i], actual.AllKeys[i]);
                Assert.AreEqual(expected[expected.AllKeys[i]], actual[actual.AllKeys[i]]);
            }
        }

        public static void WriteNameValueCollection(this NameValueCollection nvc)
        {
            for (var i = 0; i < nvc.Count; i++)
                Console.WriteLine("{0}_{1}", nvc.AllKeys[i], nvc[nvc.AllKeys[i]]);
        }
    }
}