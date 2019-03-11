using System;

namespace SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class CustomContentReader<T> : IContentReader<T> where T : ICustomRead
    {
        public CustomContentReader()
        {
            emitConstruction = ReadHelpers.EmitConstruction<T>();
        }

        public T Read(IReader reader)
        {
            var result = emitConstruction();
            result.Read(reader);
            //TODO check XmlReader not corrupted
            return result;
        }

        private readonly Func<T> emitConstruction;
    }
}