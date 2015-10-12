using System;
using System.Reflection;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public interface IXmlAttributeInterpretator
    {
        string GetXmlNodeName(MemberInfo memberInfo);
        XmlElementInfo GetPropertyNodeInfo(PropertyInfo propertyInfo, Type parentType = null);
        XmlElementInfo GetRootNodeInfo(Type type);
    }
}