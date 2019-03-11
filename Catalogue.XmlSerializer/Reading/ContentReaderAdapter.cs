using System.Threading;

using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
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