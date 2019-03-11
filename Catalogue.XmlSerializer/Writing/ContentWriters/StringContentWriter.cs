namespace Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class StringContentWriter : ContentWriterBase
    {
        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            var value = obj as string;
            if (!string.IsNullOrEmpty(value))
                writer.WriteValue(value);
        }
    }
}