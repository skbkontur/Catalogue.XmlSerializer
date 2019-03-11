using System.IO;

using NUnit.Framework;

namespace Catalogue.XmlSerializer.Tests.Reading
{
    [TestFixture]
    public class CassandraTypesTest
    {
        [Test]
        public void DeserializeTest()
        {
            var serializer = new XmlSerializer();
            var actual = serializer.Deserialize<Qxx>(File.ReadAllText(GetFilePath("CassandraColumn.xml"))).col;
            var expected = new[]
                {
                    new Column {Name = "123623", TTL = 52246, Timestamp = 77834, Value = new byte[] {1, 2, 3, 4}},
                    new Column {Name = "123623", TTL = 52246, Timestamp = 77834},
                    new Column {Name = "123623", TTL = 52246, Value = new byte[] {5, 6, 7, 8}},
                    new Column {Name = "123623", Timestamp = 77834, Value = new byte[] {9, 10, 11, 12}},
                    new Column {TTL = 52246, Timestamp = 77834, Value = new byte[] {13, 14, 15, 16}},
                    new Column {Name = "123623", TTL = 52246, Timestamp = 77834, Value = new byte[] {17, 18, 19, 20}},
                    new Column {Name = "123623", TTL = 52246, Timestamp = 77835, Value = new byte[] {21, 22, 23, 24}},
                    new Column {Name = "123623", TTL = 52245, Timestamp = 77834, Value = new byte[] {25, 26, 27, 28}},
                    new Column {Name = "123632", TTL = 52246, Timestamp = 77834, Value = new byte[] {29, 30, 31, 32}}
                };
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                var expected1 = expected[i];
                var actual1 = actual[i];
                Assert.AreEqual(expected1.Name, actual1.Name);
                Assert.AreEqual(expected1.TTL, actual1.TTL);
                Assert.AreEqual(expected1.Timestamp, actual1.Timestamp);
                CollectionAssert.AreEqual(expected1.Value, actual1.Value);
            }
        }

        private static string GetFilePath(string filename)
        {
            return $"{TestContext.CurrentContext.TestDirectory}/Reading/Files/{filename}";
        }

        public class Column
        {
            public string Name { get; set; }
            public byte[] Value { get; set; }
            public int? TTL { get; set; }
            public long? Timestamp { get; set; }
        }

        public class Qxx
        {
            public Column[] col { get; set; }
        }
    }
}