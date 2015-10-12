using System;
using System.Reflection;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer
{
    public interface IXmlAttributeInterpretator
    {
        string GetXmlNodeName(MemberInfo memberInfo);
        XmlElementInfo GetPropertyNodeInfo(PropertyInfo propertyInfo, Type parentType = null);
        XmlElementInfo GetRootNodeInfo(Type type);
    }
}