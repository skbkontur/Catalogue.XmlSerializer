using System;
using System.Collections;

using Catalogue.XmlSerializer.Writing.ContentWriters;

namespace Catalogue.XmlSerializer.Writing
{
    public class ContentWriterCollection : IContentWriterCollection
    {
        public ContentWriterCollection(IXmlAttributeInterpreter xmlAttributeInterpreter)
        {
            this.xmlAttributeInterpreter = xmlAttributeInterpreter;
        }

        public IContentWriter Get(Type type)
        {
            IContentWriter writer;
            if ((writer = Read(type)) == null)
            {
                lock (writersLock)
                    if ((writer = Read(type)) == null)
                    {
                        //для циклических зависимостей
                        var adaper = new ContentWriterAdaper();
                        writers[type] = adaper;
                        var worker = Create(type);
                        adaper.SetWorker(worker);
                        writer = adaper;
                    }
            }
            return writer;
        }

        public IContentWriter GetRootWriter(Type type)
        {
            return new RootContentWriter(type, this, xmlAttributeInterpreter);
        }

        private IContentWriter CreateLeafWriter(Type type)
        {
            if (type.IsPrimitive || type == typeof(decimal))
                return valueContentWriter;
            if (type == typeof(string))
                return stringContentWriter;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return new NullableContentWriter(type, this);
            if (type.IsEnum || type == typeof(Guid))
                return toStringContentWriter;
            if (type == typeof(DateTime))
                return new DateTimeContentWriter();
            return null;
        }

        private IContentWriter Create(Type type)
        {
            if (typeof(ICustomWrite).IsAssignableFrom(type))
                return customContentWriter;
            return CreateLeafWriter(type) ?? new ClassContentWriter(type, this, xmlAttributeInterpreter);
        }

        private IContentWriter Read(Type type)
        {
            return (IContentWriter)writers[type];
        }

        private static readonly ValueContentWriter valueContentWriter = new ValueContentWriter();
        private static readonly StringContentWriter stringContentWriter = new StringContentWriter();
        private static readonly ToStringContentWriter toStringContentWriter = new ToStringContentWriter();

        private readonly IXmlAttributeInterpreter xmlAttributeInterpreter;
        private readonly CustomContentWriter customContentWriter = new CustomContentWriter();
        private readonly Hashtable writers = new Hashtable();
        private readonly object writersLock = new object();
    }
}