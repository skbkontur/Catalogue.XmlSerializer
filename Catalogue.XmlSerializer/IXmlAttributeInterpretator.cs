using System;
using System.Reflection;

using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer
{
    public interface IXmlAttributeInterpretator
    {
        string GetXmlNodeName(MemberInfo memberInfo);
        XmlElementInfo GetPropertyNodeInfo(PropertyInfo propertyInfo, Type parentType = null);
        XmlElementInfo GetRootNodeInfo(Type type);
    }
}