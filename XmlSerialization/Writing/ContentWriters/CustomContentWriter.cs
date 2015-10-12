namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class CustomContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            ((ICustomWrite)obj).Write(writer);
        }
    }
}