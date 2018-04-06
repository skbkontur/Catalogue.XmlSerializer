using System;
using System.Collections.Generic;
using System.Reflection;

using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Reading.Configuration;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class ClassContentReader<T> : IContentReader<T>
    {
        public ClassContentReader(
            IContentReaderCollection contentReaderCollection,
            IXmlAttributeInterpretator xmlAttributeInterpretator,
            OnDeserializeConfiguration onDeserializeConfiguration)
        {
            this.onDeserializeConfiguration = onDeserializeConfiguration;
            emitConstruction = ReadHelpers.EmitConstruction<T>();
            var infos =
                typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var propertyInfo in infos)
            {
                if (PropertyIsBad(propertyInfo)) continue;
                if (propertyInfo.IsDefined(typeof(XmlAttributeAttribute), false))
                {
                    var name = xmlAttributeInterpretator.GetXmlNodeName(propertyInfo);
                    attributeMap.Add(name, ReadHelpers.BuildSetter<T>(propertyInfo, contentReaderCollection));
                }
                else
                {
                    var name = xmlAttributeInterpretator.GetXmlNodeName(propertyInfo);
                    propertiesMap.Add(name, ReadHelpers.BuildSetter<T>(propertyInfo, contentReaderCollection));
                }
            }
        }

        public T Read(IReader reader)
        {
            var result = emitConstruction();
            var workingDepth = reader.Depth;
            var usedProperties = new HashSet<string>();
            bool notRead;
            do
            {
                notRead = false;
                switch (reader.Depth - workingDepth)
                {
                case 1:
                    if (reader.NodeType == NodeType.Element)
                    {
                        IContentPropertySetter<T> setter;
                        var propertyName = reader.LocalName;
                        if (propertiesMap.TryGetValue(propertyName, out setter))
                        {
                            if (!usedProperties.Add(propertyName))
                                onDeserializeConfiguration.RaiseOnDuplicateElement(new DeserializationContext(propertyName));
                            setter.SetProperty(result, reader);
                            notRead = true;
                            //TODO check depth is correct
                        }
                        else
                            onDeserializeConfiguration.RaiseOnUnexpectedElement(new DeserializationContext(propertyName));
                    }
                    break;
                case 0:
                    if (reader.NodeType == NodeType.Element)
                    {
                        if (reader.HasAttributes)
                            SetAttributes(reader, result);
                        if (reader.IsEmptyElement)
                        {
                            reader.Read();
                            return result;
                        }
                    }
                    if (reader.NodeType == NodeType.EndElement)
                        return result;
                    break;
                }
            } while (notRead || reader.Read());
            return result;
        }

        private static bool PropertyIsBad(PropertyInfo propertyInfo)
        {
            var methodInfo = propertyInfo.GetSetMethod();
            if (methodInfo == null) return true;
            return propertyInfo.GetIndexParameters().Length > 0;
        }

        private void SetAttributes(IReader xmlReader, T result)
        {
            if (xmlReader.MoveToFirstAttribute())
            {
                do
                {
                    IContentPropertySetter<T> setter;
                    if (attributeMap.TryGetValue(xmlReader.LocalName, out setter))
                        setter.SetProperty(result, xmlReader);
                    else
                        onDeserializeConfiguration.RaiseOnUnexpectedAttribute(new DeserializationContext(xmlReader.LocalName));
                } while (xmlReader.MoveToNextAttribute());
            }
            xmlReader.MoveToElement();
        }

        private readonly OnDeserializeConfiguration onDeserializeConfiguration;

        private readonly Dictionary<string, IContentPropertySetter<T>>
            attributeMap = new Dictionary<string, IContentPropertySetter<T>>();

        private readonly Func<T> emitConstruction;

        private readonly Dictionary<string, IContentPropertySetter<T>>
            propertiesMap = new Dictionary<string, IContentPropertySetter<T>>();
    }
}