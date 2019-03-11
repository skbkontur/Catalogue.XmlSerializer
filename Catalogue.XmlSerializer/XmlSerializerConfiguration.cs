using Catalogue.XmlSerializer.Reading.Configuration;

namespace Catalogue.XmlSerializer
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