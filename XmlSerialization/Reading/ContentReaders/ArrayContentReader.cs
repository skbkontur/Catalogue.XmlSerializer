using System;
using System.Collections.Generic;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class ArrayContentReader<T> : IContentReader<T[]>
    {
        public ArrayContentReader(IContentReaderCollection contentReaderCollection)
        {
            itemReader = contentReaderCollection.Get<T>();
        }

        public T[] Read(IReader reader)
        {
            var list = new List<T>();
            var workingDepth = reader.Depth;
            string elementName = null;
            bool dontRead;
            do
            {
                dontRead = false;
                switch (reader.Depth - workingDepth)
                {
                case 0:
                    if (reader.NodeType == NodeType.Element)
                    {
                        if (elementName == null) elementName = reader.LocalName;
                        if (reader.LocalName == elementName)
                        {
                            var item = itemReader.Read(reader);
                            dontRead = true;
                            list.Add(item);
                        }
                        else return list.ToArray();
                    }
                    break;
                case -1:
                    return list.ToArray();
                default:
                    throw new InvalidOperationException("TRASH IN ARRAY");
                }
            } while (dontRead || reader.Read());
            return list.ToArray();
        }

        private readonly IContentReader<T> itemReader;
    }
}