using System;

namespace Catalogue.XmlSerializer.Reading.Configuration
{
    public class DeserializationContext : EventArgs
    {
        public DeserializationContext(string currentElementLocalName)
        {
            CurrentElementLocalName = currentElementLocalName;
        }

        public string CurrentElementLocalName { get; }
    }
}