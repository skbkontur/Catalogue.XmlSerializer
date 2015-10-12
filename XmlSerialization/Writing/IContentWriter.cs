namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public interface IContentWriter
    {
        void Write(object obj, IWriter writer);
    }
}