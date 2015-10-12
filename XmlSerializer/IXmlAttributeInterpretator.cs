using System.Reflection;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer
{
    public interface IXmlAttributeInterpretator
    {
        string GetXmlNamespace(MemberInfo memberInfo);
        string GetXmlNodeName(MemberInfo memberInfo);
        XmlElementInfo GetXmlNodeInfo(MemberInfo memberInfo);
        XmlElementInfo GetRootNodeInfo(MemberInfo memberInfo);
    }
}