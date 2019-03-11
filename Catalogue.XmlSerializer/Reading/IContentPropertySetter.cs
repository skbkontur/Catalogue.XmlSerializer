namespace Catalogue.XmlSerializer.Reading
{
    public interface IContentPropertySetter<in T>
    {
        void SetProperty(T target, IReader reader);
    }
}