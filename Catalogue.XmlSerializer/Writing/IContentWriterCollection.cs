using System;

namespace SkbKontur.Catalogue.XmlSerializer.Writing
{
    public interface IContentWriterCollection
    {
        IContentWriter Get(Type type);
        IContentWriter GetRootWriter(Type type);
    }
}