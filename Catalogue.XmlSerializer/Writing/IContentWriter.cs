namespace SkbKontur.Catalogue.XmlSerializer.Writing
{
    public interface IContentWriter
    {
        void Write(object obj, IWriter writer);
    }
}