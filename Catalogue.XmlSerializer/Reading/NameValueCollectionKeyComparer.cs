using System;
using System.Collections.Generic;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public class NameValueCollectionKeyComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var xArray = x.Split('$', '.');
            var yArray = y.Split('$', '.');

            var minLength = Math.Min(xArray.Length, yArray.Length);

            for (var i = 0; i < minLength; i++)
            {
                var compareResult = CompareToken(xArray[i], yArray[i]);
                if (compareResult != 0)
                    return compareResult;
            }

            if (xArray.Length == yArray.Length)
                return 0;

            return xArray.Length == minLength ? -1 : 1;
        }

        private int CompareToken(string token1, string token2)
        {
            ulong token1LongValue;
            ulong token2LongValue;
            if (ulong.TryParse(token1, out token1LongValue) && ulong.TryParse(token2, out token2LongValue))
                return token1LongValue.CompareTo(token2LongValue);
            return token1.CompareTo(token2);
        }
    }
}