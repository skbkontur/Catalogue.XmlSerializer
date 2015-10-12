﻿using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer
{
    public class XmlAttributeInterpretator : IXmlAttributeInterpretator
    {
        public string GetXmlNodeName(MemberInfo memberInfo)
        {
            var name = GetXmlNodeInternal(memberInfo);
            CheckName(memberInfo, name);
            return name;
        }

        public XmlElementInfo GetPropertyNodeInfo(PropertyInfo propertyInfo, Type parentType = null)
        {
            return new XmlElementInfo { Name = GetXmlNodeName(propertyInfo), NamespaceUri = GetXmlNamespace(propertyInfo, parentType), Optional = IsOptional(propertyInfo), NamespaceDescriptions = GetXmlNamespaceDescriptions(GetPropertyType(propertyInfo)) };
        }

        public XmlElementInfo GetRootNodeInfo(Type type)
        {
            return new XmlElementInfo { Name = GetRootName(type), NamespaceUri = GetXmlNamespace(type), Optional = IsOptional(type), NamespaceDescriptions = GetXmlNamespaceDescriptions(type) };
        }

        private bool IsOptional(MemberInfo memberInfo)
        {
            return GetAttribute<XmlOptionalAttribute>(memberInfo, true) != null;
        }

        private XmlNamespaceDescription[] GetXmlNamespaceDescriptions(MemberInfo memberInfo)
        {
            var attributes = GetAttributes<DeclareXmlNamespaceAttribute>(memberInfo, false);
            return attributes.Select(x => new XmlNamespaceDescription(x.Prefix, x.Uri)).ToArray();
        }

        private string GetXmlNamespace(MemberInfo memberInfo, Type parentType = null)
        {
            var namespaceAttribute = GetAttribute<XmlNamespaceAttribute>(memberInfo, false);
            if(namespaceAttribute != null)
                return namespaceAttribute.NamespaceUri;

            if(parentType != null)
            {
                namespaceAttribute = GetAttribute<XmlNamespaceAttribute>(parentType, false);
                if(namespaceAttribute != null)
                    return namespaceAttribute.NamespaceUri;
            }
            return null;
        }

        private string GetXmlNodeInternal(MemberInfo memberInfo)
        {
            var formNameAttribute = GetAttribute<XmlFormNameAttribute>(memberInfo, true) ?? new XmlElementAttribute();
            if(memberInfo.MemberType != MemberTypes.Property)
            {
                if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.GetAttributeFromTypeDeclaration)
                    throw new NotSupportedException(string.Format("У элемента {0} не может быть выставлено правило {1}. Это правило можно выставлять только для свойств.", memberInfo, XmlFormNameRule.GetAttributeFromTypeDeclaration));
            }

            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.GetAttributeFromTypeDeclaration)
                return GetXmlNodeName(GetPropertyType((PropertyInfo)memberInfo));
            if(!string.IsNullOrEmpty(formNameAttribute.SpecificName))
                return formNameAttribute.SpecificName;
            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.LowerFirstLetter)
                return LowerFirstChar(memberInfo.Name);
            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.LowerAllLetters)
                return memberInfo.Name.ToLowerInvariant();
            return memberInfo.Name;
        }

        private string GetRootNameInternal(MemberInfo memberInfo)
        {
            var formNameAttribute = GetAttribute<XmlFormNameAttribute>(memberInfo, true) ?? new XmlElementAttribute();
            if(memberInfo.MemberType != MemberTypes.Property)
            {
                if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.GetAttributeFromTypeDeclaration)
                    throw new NotSupportedException(string.Format("У элемента {0} не может быть выставлено правило {1}. Это правило можно выставлять только для свойств.", memberInfo, XmlFormNameRule.GetAttributeFromTypeDeclaration));
            }

            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.GetAttributeFromTypeDeclaration)
                return GetXmlNodeName(GetPropertyType((PropertyInfo)memberInfo));
            if(!string.IsNullOrEmpty(formNameAttribute.SpecificName))
                return formNameAttribute.SpecificName;
            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.LowerFirstLetter)
                return LowerFirstChar(memberInfo.Name);
            if(formNameAttribute.XmlFormNameRule == XmlFormNameRule.LowerAllLetters)
                return memberInfo.Name.ToLowerInvariant();
            return "root";
        }

        private string GetRootName(MemberInfo memberInfo)
        {
            var name = GetRootNameInternal(memberInfo);
            CheckName(memberInfo, name);
            return name;
        }

        private static void CheckName(MemberInfo memberInfo, string name)
        {
            if(!PossibleXmlName(name))
                throw new NotSupportedException(string.Format("У элемента {0} не может отсутствовать конкретное имя. Его имя '{1}' не является допустимым для xml.", memberInfo, name));
        }

        private static T GetAttribute<T>(MemberInfo memberInfo, bool inherit)
            where T : Attribute
        {
            var attributes = memberInfo.GetAttributes<T>(inherit);
            if(attributes.Length > 1)
                throw new Exception(string.Format("У элемента '{0}' не может быть больше одного атрибута типа '{1}'.", memberInfo, typeof(T).Name));
            if(attributes.Length == 1) return attributes[0];
            return null;
        }

        private static T[] GetAttributes<T>(MemberInfo memberInfo, bool inherit)
            where T : Attribute
        {
            return memberInfo.GetAttributes<T>(inherit);
        }

        private static bool PossibleXmlName(string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(c => (c != '\'' && c != '<' && c != '>' && c != '?' && c != '`'));
        }

        private static string LowerFirstChar(string str)
        {
            return str.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) +
                   str.Substring(1, str.Length - 1);
        }

        private static Type GetPropertyType(PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            if(propertyType.HasElementType) return propertyType.GetElementType();
            return propertyType;
        }
    }
}