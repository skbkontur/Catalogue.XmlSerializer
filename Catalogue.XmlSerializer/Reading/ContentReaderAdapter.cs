using System.Threading;

using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public class ContentReaderAdapter<T> : IContentReader<T>
    {
        public T Read(IReader xmlReader)
        {
            while (worker == null) Sleep();
            return worker.Read(xmlReader);
        }

        public void SetReader(IContentReader<T> reader)
        {
            worker = reader;
        }

        private void Sleep()
        {
            Thread.Sleep(100);
        }

        private volatile IContentReader<T> worker;
    }
}