namespace Catalogue.XmlSerializer.Attributes
{
    public class XmlAttributeAttribute : XmlFormNameAttribute
    {
        public XmlAttributeAttribute()
            : base(XmlFormNameRule.Default, "")
        {
        }

        public XmlAttributeAttribute(XmlFormNameRule formNameRule)
            : base(formNameRule, "")
        {
        }

        public XmlAttributeAttribute(string specificName)
            : base(XmlFormNameRule.Default, specificName)
        {
        }
    }
}