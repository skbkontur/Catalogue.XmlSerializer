﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class ClassContentWriter : ContentWriterBase
    {
        public ClassContentWriter(Type type, IContentWriterCollection contentWriterCollection, IXmlAttributeInterpretator xmlAttributeInterpretator)
        {
            this.xmlAttributeInterpretator = xmlAttributeInterpretator;
            var list = new List<IValueWriter>();
            var funcList = new List<Func<object, object>>();
            var elementInfoList = new List<XmlElementInfo>();
            ProcessAttributes(type, funcList, list, elementInfoList, contentWriterCollection);
            attributeCount = elementInfoList.Count;
            ProcessOtherProps(type, funcList, list, elementInfoList, contentWriterCollection);
            writers = list.ToArray();
            readPropertyFuncs = funcList.ToArray();
            elementInfos = elementInfoList.ToArray();
        }

        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            for (var i = 0; i < readPropertyFuncs.Length; i++)
            {
                var value = readPropertyFuncs[i](obj);
                writers[i].Write(value, writer);
            }
        }

        private static PropertyInfo[] GetOrderedProperties(Type type)
        {
            return GetProperties(type).OrderBy(property => property.MetadataToken).ToArray();
        }

        private static PropertyInfo[] GetProperties(Type type)
        {
            var result = new List<PropertyInfo>();
            if (type.BaseType != null)
                result.AddRange(GetProperties(type.BaseType));
            result.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.DeclaredOnly));
            return result.ToArray();
        }

        private void ProcessOtherProps(Type type, ICollection<Func<object, object>> funcList,
                                       ICollection<IValueWriter> list, ICollection<XmlElementInfo> xmlElementInfoList, IContentWriterCollection collection)
        {
            foreach (var propertyInfo in GetOrderedProperties(type))
            {
                if (!IsAttr(propertyInfo) && propertyInfo.GetIndexParameters().Length == 0)
                {
                    var elementInfo = xmlAttributeInterpretator.GetPropertyNodeInfo(propertyInfo, type);
                    xmlElementInfoList.Add(elementInfo);
                    var propertyType = propertyInfo.PropertyType;
                    var emitReadPropertyFunc = ReportEmitHelpers.EmitReadPropertyFunc(propertyInfo, type);
                    funcList.Add(emitReadPropertyFunc);

                    if (propertyType == typeof(byte[]))
                        list.Add(new ByteArrayValueWriter(elementInfo));
                    else if (propertyType.IsArray)
                    {
                        if (propertyType.GetArrayRank() > 1)
                            throw new NotSupportedException(string.Format("array with rank {0}", propertyType.GetArrayRank()));
                        list.Add(new ArrayValueWriter(elementInfo, collection.Get(propertyType.GetElementType())));
                    }
                    else if (propertyType.IsList())
                        list.Add(new ListValueWriter(elementInfo, collection.Get(propertyType.GetListType())));
                    else if (propertyType.IsDictionary())
                    {
                        var keyWriter = collection.Get(propertyType.GetDictionaryKeyType());
                        var valueWriter = collection.Get(propertyType.GetDictionaryValueType());
                        list.Add(new DictionaryValueWriter(elementInfo, keyWriter, valueWriter, xmlAttributeInterpretator));
                    }
                    else list.Add(new ItemValueWriter(elementInfo, collection.Get(propertyType)));
                }
            }
        }

        private void ProcessAttributes(Type type, ICollection<Func<object, object>> funcList,
                                       ICollection<IValueWriter> list, ICollection<XmlElementInfo> xmlElementInfoList, IContentWriterCollection collection)
        {
            foreach (var propertyInfo in GetOrderedProperties(type))
            {
                if (IsAttr(propertyInfo))
                {
                    funcList.Add(ReportEmitHelpers.EmitReadPropertyFunc(propertyInfo, type));
                    var xmlElementInfo = xmlAttributeInterpretator.GetPropertyNodeInfo(propertyInfo, type);
                    xmlElementInfoList.Add(xmlElementInfo);
                    list.Add(new AttributeValueWriter(xmlElementInfo, collection.Get(propertyInfo.PropertyType)));
                }
            }
        }

        private static bool IsAttr(ICustomAttributeProvider propertyInfo)
        {
            return propertyInfo.IsDefined(typeof(XmlAttributeAttribute), false);
        }

        private readonly IXmlAttributeInterpretator xmlAttributeInterpretator;

        private readonly Func<object, object>[] readPropertyFuncs;
        private readonly IValueWriter[] writers;
        private readonly XmlElementInfo[] elementInfos;
        private readonly int attributeCount;
    }
}