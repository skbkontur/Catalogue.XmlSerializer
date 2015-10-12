namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public interface IValueWriter
    {
        void Write(object value, IWriter writer);
    }
}