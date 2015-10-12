namespace SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ValueContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            writer.WriteValue(obj);
        }
    }
}