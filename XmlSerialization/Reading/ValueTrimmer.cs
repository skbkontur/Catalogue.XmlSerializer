using System.Linq;
using System.Text;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public static class ValueTrimmer
    {
        public static string Trim(string value)
        {
            if(string.IsNullOrEmpty(value)) return "";
            var arr = value.Trim(' ', '\n', '\r', '\t').Split('\n', '\r').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var result = new StringBuilder();
            for(var i = 0; i < arr.Length; i++)
            {
                if(i > 0) result.AppendLine();
                result.Append(arr[i].Trim(' ', '\t'));
            }
            return result.ToString();
        }
    }
}