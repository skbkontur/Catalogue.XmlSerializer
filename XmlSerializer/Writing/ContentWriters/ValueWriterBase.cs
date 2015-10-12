namespace SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public abstract class ValueWriterBase : IValueWriter
    {
        public void Write(object value, IWriter writer)
        {
            if(value != null)
                WriteNonNullableValue(value, writer);
        }

        protected abstract void WriteNonNullableValue(object value, IWriter writer);
    }
}