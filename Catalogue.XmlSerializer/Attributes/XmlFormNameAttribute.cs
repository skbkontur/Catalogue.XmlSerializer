using System;

namespace Catalogue.XmlSerializer.Attributes
{
    public abstract class XmlFormNameAttribute : Attribute
    {
        protected XmlFormNameAttribute(XmlFormNameRule xmlFormNameRule, string specificName)
        {
            XmlFormNameRule = xmlFormNameRule;
            SpecificName = specificName;
        }

        public XmlFormNameRule XmlFormNameRule { get; }
        public string SpecificName { get; }
    }

    public enum XmlFormNameRule
    {
        LowerAllLetters,
        LowerFirstLetter,
        GetAttributeFromTypeDeclaration,
        Default
    }
}