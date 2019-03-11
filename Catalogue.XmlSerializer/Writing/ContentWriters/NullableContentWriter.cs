using System;

namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class NullableContentWriter : ContentWriterBase
    {
        public NullableContentWriter(Type type, IContentWriterCollection contentWriterCollection)
        {
            callHasValue = ReportEmitHelpers.EmitReadPropertyFunc(type.GetProperty("HasValue"), type);
            callGetValue = ReportEmitHelpers.EmitReadPropertyFunc(type.GetProperty("Value"), type);
            var valueType = type.GetGenericArguments()[0];
            contentWriter = contentWriterCollection.Get(valueType);
        }

        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            if ((bool)callHasValue(obj))
            {
                var nullableValue = callGetValue(obj);
                contentWriter.Write(nullableValue, writer);
            }
        }

        private readonly Func<object, object> callGetValue;
        private readonly Func<object, object> callHasValue;
        private readonly IContentWriter contentWriter;
    }
}