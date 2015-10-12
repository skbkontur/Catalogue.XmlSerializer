using System;

using SKBKontur.Catalogue.XmlSerialization;
using SKBKontur.Catalogue.XmlSerialization.Writing;
using SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Writing
{
    public class TestContentWriterCollection : IContentWriterCollection
    {
        public IContentWriter Get(Type type)
        {
            if(type == typeof(string))
                return new StringContentWriter();
            if(type == typeof(int))
                return new ValueContentWriter();
            if(type == typeof(int?))
                return new NullableContentWriter(type, this);
            if(typeof(ICustomWrite).IsAssignableFrom(type))
                return new CustomContentWriter();

            return new ClassContentWriter(type, this, new XmlAttributeInterpretator());
        }

        public IContentWriter GetRootWriter(Type type)
        {
            return new RootContentWriter(type, this, new XmlAttributeInterpretator());
        }
    }
}