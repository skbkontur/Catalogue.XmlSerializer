using SKBKontur.Catalogue.XmlSerialization.Reading.Configuration;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public class XmlSerializerConfiguration
    {
        public XmlSerializerConfiguration()
        {
            OnDeserialize = new OnDeserializeConfiguration();
        }

        public OnDeserializeConfiguration OnDeserialize { get; private set; }
    }
}