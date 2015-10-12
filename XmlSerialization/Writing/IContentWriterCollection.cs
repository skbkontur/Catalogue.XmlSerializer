using System;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public interface IContentWriterCollection
    {
        IContentWriter Get(Type type);
        IContentWriter GetRootWriter(Type type);
    }
}