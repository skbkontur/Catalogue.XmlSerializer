namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public interface IValueWriter
    {
        void Write(object value, IWriter writer);
    }
}