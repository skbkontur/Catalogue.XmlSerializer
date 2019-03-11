namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class SqlBoolWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            writer.WriteValue((bool)obj ? "1" : "0");
        }
    }
}