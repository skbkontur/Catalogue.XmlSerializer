using System;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.Configuration
{
    public class OnDeserializeConfiguration
    {
        public event EventHandler<DeserializationContext> OnDuplicateElement;

        public event EventHandler<DeserializationContext> OnUnexpectedElement;

        public event EventHandler<DeserializationContext> OnUnexpectedAttribute;

        internal void RaiseOnDuplicateElement(DeserializationContext e)
        {
            var handler = OnDuplicateElement;
            if (handler != null) handler(this, e);
        }

        internal void RaiseOnUnexpectedElement(DeserializationContext e)
        {
            var handler = OnUnexpectedElement;
            if (handler != null) handler(this, e);
        }

        internal void RaiseOnUnexpectedAttribute(DeserializationContext e)
        {
            var handler = OnUnexpectedAttribute;
            if (handler != null) handler(this, e);
        }
    }
}