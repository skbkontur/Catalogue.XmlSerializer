using SKBKontur.Catalogue.XmlSerializer.Reading.Configuration;

namespace SKBKontur.Catalogue.XmlSerializer
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