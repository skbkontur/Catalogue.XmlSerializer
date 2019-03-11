using System;
using System.Reflection;

using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer
{
    public interface IXmlAttributeInterpreter
    {
        string GetXmlNodeName(MemberInfo memberInfo);
        XmlElementInfo GetPropertyNodeInfo(PropertyInfo propertyInfo, Type parentType = null);
        XmlElementInfo GetRootNodeInfo(Type type);
    }
}