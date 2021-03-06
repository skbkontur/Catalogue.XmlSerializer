﻿using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

using SkbKontur.Catalogue.XmlSerializer.Writing;

namespace SkbKontur.Catalogue.XmlSerializer
{
    public interface IXmlSerializer
    {
        byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding, bool collapseArrayElements = true);
        NameValueCollection SerializeToNameValueCollection<T>(T data, bool skipEmpty = true);
        void Serialize<T>(T data, IWriter writer);

        T Deserialize<T>(XmlReader reader, bool needTrimValues = true);
        T Deserialize<T>(NameValueCollection collection);
        T Deserialize<T>(byte[] source, bool needTrimValues = true);
        T Deserialize<T>(Stream stream, bool needTrimValues = true);

        XmlSerializerConfiguration Configuration { get; }
    }
}