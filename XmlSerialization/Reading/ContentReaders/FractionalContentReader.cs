using System.Globalization;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class FractionalContentReader<T> : IContentReader<T>
    {
        public FractionalContentReader(TryParseDelegate<T> tryParseDelegate)
        {
            this.tryParseDelegate = tryParseDelegate;
            worker = new SimpleContentReader<T>(ParseDelegateDouble);
        }

        public T Read(IReader reader)
        {
            return worker.Read(reader);
        }

        private bool ParseDelegateDouble(string s, out T result)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Contains(","))
                    s = s.Replace(",", ".");
            }

            return tryParseDelegate(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        private readonly TryParseDelegate<T> tryParseDelegate;
        private readonly SimpleContentReader<T> worker;
    }
}