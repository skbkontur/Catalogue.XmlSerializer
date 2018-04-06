using System.Threading;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public class ContentWriterAdaper : ContentWriterBase
    {
        public void SetWorker(IContentWriter writer)
        {
            worker = writer;
        }

        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            while (worker == null) Sleep();
            worker.Write(obj, writer);
        }

        private void Sleep()
        {
            Thread.Sleep(100);
        }

        private volatile IContentWriter worker;
    }
}