namespace Catalogue.XmlSerializer.Attributes
{
    public class XmlElementAttribute : XmlFormNameAttribute
    {
        public XmlElementAttribute()
            : base(XmlFormNameRule.Default, "")
        {
        }

        public XmlElementAttribute(XmlFormNameRule formNameRule)
            : base(formNameRule, "")
        {
        }

        public XmlElementAttribute(string specificName)
            : base(XmlFormNameRule.Default, specificName)
        {
        }
    }
}