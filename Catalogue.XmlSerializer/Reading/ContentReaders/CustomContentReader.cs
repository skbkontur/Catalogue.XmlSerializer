﻿using System;

namespace Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class CustomContentReader<T> : IContentReader<T> where T : ICustomRead
    {
        public CustomContentReader()
        {
            emitConstruction = ReadHelpers.EmitConstruction<T>();
        }

        #region IContentReader<T> Members

        public T Read(IReader reader)
        {
            var result = emitConstruction();
            result.Read(reader);
            //TODO check XmlReader not currupted
            return result;
        }

        #endregion

        private readonly Func<T> emitConstruction;
    }
}