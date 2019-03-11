namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public interface IContentPropertySetter<in T>
    {
        void SetProperty(T target, IReader reader);
    }
}