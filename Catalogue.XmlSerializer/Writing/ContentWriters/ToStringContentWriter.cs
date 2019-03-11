namespace Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ToStringContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            writer.WriteValue(obj.ToString());
        }
    }
}