using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using NUnit.Framework;

namespace SKBKontur.Catalogue.XmlSerialization.TestExtensions
{
    public static class XmlComparer
    {
        public static void AssertEqualsXml(this string actual, string expected, bool useAraxisMerge = false, string araxisComparePath = null)
        {
            var reformatExpected = ReformatXml(expected);
            var reformatActual = ReformatXml(actual);
            if(reformatExpected != reformatActual)
            {
                if(!useAraxisMerge)
                {
                    Console.WriteLine("expected: " + Environment.NewLine + reformatExpected);
                    Console.WriteLine("actual: " + Environment.NewLine + reformatActual);
                }
                else
                {
                    var expectedFilePath = "expected_" + Guid.NewGuid() + ".txt";
                    File.WriteAllText(expectedFilePath, reformatExpected);
                    var actualFilePath = "actual_" + Guid.NewGuid() + ".txt";
                    File.WriteAllText(actualFilePath, reformatActual);
                    Process.Start(new ProcessStartInfo
                        {
                            Arguments = string.Format("{0} {1}", actualFilePath, expectedFilePath),
                            FileName = araxisComparePath ?? @"c:\Program Files\Araxis\Araxis Merge\compare.exe",
                            WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                        });
                }
            }
            Assert.AreEqual(reformatExpected, reformatActual);
        }

        private static string ReformatXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            var result = new StringBuilder();
            var writer = XmlWriter.Create(result, new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = !HasXmlDeclaration(document)
                });
            document.WriteTo(writer);
            writer.Flush();
            return result.ToString();
        }

        private static bool HasXmlDeclaration(XmlDocument document)
        {
            return document.ChildNodes.OfType<XmlDeclaration>().Any();
        }
    }
}