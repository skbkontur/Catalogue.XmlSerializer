using System;

namespace SKBKontur.Catalogue.XmlSerializer.Attributes
{
    public abstract class XmlFormNameAttribute : Attribute
    {
        protected XmlFormNameAttribute(XmlFormNameRule xmlFormNameRule, string specificName)
        {
            XmlFormNameRule = xmlFormNameRule;
            SpecificName = specificName;
        }

        public XmlFormNameRule XmlFormNameRule { get; private set; }
        public string SpecificName { get; private set; }
    }

    public enum XmlFormNameRule
    {
        LowerAllLetters,
        LowerFirstLetter,
        GetAttributeFromTypeDeclaration,
        Default
    }
}