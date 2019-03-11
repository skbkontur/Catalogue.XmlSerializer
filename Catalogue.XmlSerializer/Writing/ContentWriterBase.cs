namespace Catalogue.XmlSerializer.Writing
{
    public abstract class ContentWriterBase : IContentWriter
    {
        public void Write(object obj, IWriter writer)
        {
            if (obj != null)
                WriteNonNullableObject(obj, writer);
        }

        protected abstract void WriteNonNullableObject(object obj, IWriter writer);
    }
}