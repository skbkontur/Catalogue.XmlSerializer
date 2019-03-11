using SkbKontur.Catalogue.XmlSerializer.Reading.Configuration;

namespace SkbKontur.Catalogue.XmlSerializer
{
    public class XmlSerializerConfiguration
    {
        public XmlSerializerConfiguration()
        {
            OnDeserialize = new OnDeserializeConfiguration();
        }

        public OnDeserializeConfiguration OnDeserialize { get; }
    }
}