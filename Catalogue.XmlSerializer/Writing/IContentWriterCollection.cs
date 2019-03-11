using System;

namespace Catalogue.XmlSerializer.Writing
{
    public interface IContentWriterCollection
    {
        IContentWriter Get(Type type);
        IContentWriter GetRootWriter(Type type);
    }
}