using System;
using System.Collections;
using System.Collections.Generic;

using SkbKontur.Catalogue.XmlSerializer.Reading.Configuration;
using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public class ContentReaderCollection : IContentReaderCollection
    {
        public ContentReaderCollection(IXmlAttributeInterpreter xmlAttributeInterpreter, OnDeserializeConfiguration onDeserializeConfiguration)
        {
            var leafContentReaders = new Dictionary<object, object>
                {
                    {typeof(string), new StringContentReader()},
                    {typeof(int), new SimpleContentReader<int>(int.TryParse)},
                    {typeof(byte), new SimpleContentReader<byte>(byte.TryParse)},
                    {typeof(long), new SimpleContentReader<long>(long.TryParse)},
                    {typeof(bool), new SimpleContentReader<bool>(bool.TryParse)},
                    {typeof(double), new FractionalContentReader<double>(double.TryParse)},
                    {typeof(float), new FractionalContentReader<float>(float.TryParse)},
                    {typeof(Guid), new SimpleContentReader<Guid>(Guid.TryParse)},
                    {typeof(DateTime), new DateTimeContentReader()},
                    {typeof(decimal), new FractionalContentReader<decimal>(decimal.TryParse)},
                };
            this.xmlAttributeInterpreter = xmlAttributeInterpreter;
            this.onDeserializeConfiguration = onDeserializeConfiguration;
            foreach (var leafContentReader in leafContentReaders)
                readers.Add(leafContentReader.Key, leafContentReader.Value);
        }

        public IContentReader<T> Get<T>()
        {
            CheckGanGet<T>();
            IContentReader<T> contentReader;
            if ((contentReader = TryGet<T>()) == null)
            {
                lock (readersLock)
                {
                    if ((contentReader = TryGet<T>()) == null)
                    {
                        var adapter = new ContentReaderAdapter<T>();
                        readers[typeof(T)] = adapter;
                        var newReader = CreateNewReader<T>();
                        adapter.SetReader(newReader);
                        contentReader = adapter;
                    }
                }
            }
            return contentReader;
        }

        private void CheckGanGet<T>()
        {
            var type = typeof(T);
            if (!type.IsVisible)
                throw new NotSupportedException($"Type '{type}' should be visible outside assembly");
            if (badTypes.Contains(type))
                throw new NotSupportedException($"Type '{type}' cannot be deserialized");
        }

        private IContentReader<T> TryGet<T>()
        {
            return (IContentReader<T>)readers[typeof(T)];
        }

        private IContentReader<T> CreateNewReader<T>()
        {
            var customRead = TryCreateForCustomRead<T>();
            if (customRead != null)
                return customRead;
            var listContentReader = TryCreateForList<T>(this);
            if (listContentReader != null)
                return listContentReader;
            var dictContentReader = TryCreateForDict<T>(this);
            if (dictContentReader != null)
                return dictContentReader;
            var byteArrayContentReader = TryCreateForByteArray<T>(this);
            if (byteArrayContentReader != null)
                return byteArrayContentReader;
            var arrayContentReader = TryCreateForArray<T>(this);
            if (arrayContentReader != null)
                return arrayContentReader;
            var nullableReader = TryCreateForNullable<T>();
            if (nullableReader != null)
                return nullableReader;
            var enumReader = TryCreateForEnum<T>();
            if (enumReader != null)
                return enumReader;

            return new ClassContentReader<T>(this, xmlAttributeInterpreter, onDeserializeConfiguration);
        }

        private static IContentReader<T> TryCreateForEnum<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum) return null;
            var genericType = typeof(EnumContentReader<>).MakeGenericType(type);
            var publicConstructor = genericType.GetPublicConstructor(Type.EmptyTypes);
            return (IContentReader<T>)publicConstructor.Invoke(new object[0]);
        }

        //HACK из-за where T: ... в NullableContentReader<T> и CustomContentReader<T>
        private IContentReader<T> TryCreateForNullable<T>()
        {
            var type = typeof(T);
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>)) return null;
            var nullableType = type.GetGenericArguments()[0];

            var genericType = typeof(NullableContentReader<>).MakeGenericType(nullableType);
            var publicConstructor = genericType.GetPublicConstructor(typeof(IContentReaderCollection));
            var invoke = publicConstructor.Invoke(new[] {this});
            return (IContentReader<T>)invoke;
        }

        private static IContentReader<T> TryCreateForDict<T>(IContentReaderCollection contentReaderCollection)
        {
            var type = typeof(T);
            if (!type.IsDictionary()) return null;
            var keyType = type.GetDictionaryKeyType();
            var valueType = type.GetDictionaryValueType();
            var genericType = typeof(DictionaryContentReader<,>).MakeGenericType(keyType, valueType);
            var publicConstructor = genericType.GetPublicConstructor(typeof(IContentReaderCollection));
            var invoke = publicConstructor.Invoke(new[] {contentReaderCollection});
            return (IContentReader<T>)invoke;
        }

        private static IContentReader<T> TryCreateForList<T>(IContentReaderCollection contentReaderCollection)
        {
            var type = typeof(T);
            if (!type.IsList()) return null;
            var elementType = type.GetListType();
            var genericType = typeof(ListContentReader<>).MakeGenericType(elementType);
            var publicConstructor = genericType.GetPublicConstructor(typeof(IContentReaderCollection));
            var invoke = publicConstructor.Invoke(new[] {contentReaderCollection});
            return (IContentReader<T>)invoke;
        }

        private static IContentReader<T> TryCreateForByteArray<T>(IContentReaderCollection contentReaderCollection)
        {
            var type = typeof(T);
            if (type != typeof(byte[])) return null;
            return (IContentReader<T>)new ByteArrayContentReader();
        }

        private static IContentReader<T> TryCreateForArray<T>(IContentReaderCollection contentReaderCollection)
        {
            var type = typeof(T);
            if (!type.IsArray) return null;
            var elementType = type.GetElementType();
            if (elementType != null)
            {
                if (elementType.IsArray)
                    throw new NotSupportedException("массив массивов");
                var arrayRank = type.GetArrayRank();
                if (arrayRank > 1)
                    throw new NotSupportedException("многомерный массив");
            }
            var genericType = typeof(ArrayContentReader<>).MakeGenericType(elementType);
            var publicConstructor = genericType.GetPublicConstructor(typeof(IContentReaderCollection));
            var invoke = publicConstructor.Invoke(new[] {contentReaderCollection});
            return (IContentReader<T>)invoke;
        }

        private static IContentReader<T> TryCreateForCustomRead<T>()
        {
            var type = typeof(T);
            if (type.GetInterface(typeof(ICustomRead).Name) == null) return null;

            var genericType = typeof(CustomContentReader<>).MakeGenericType(type);

            var publicConstructor = genericType.GetPublicConstructor();
            var invoke = publicConstructor.Invoke(new object[0]);
            return (IContentReader<T>)invoke;
        }

        private readonly HashSet<Type> badTypes = new HashSet<Type>
            {
                typeof(object)
            };

        private readonly Hashtable readers = new Hashtable();
        private readonly object readersLock = new object();
        private readonly IXmlAttributeInterpreter xmlAttributeInterpreter;
        private readonly OnDeserializeConfiguration onDeserializeConfiguration;
    }
}