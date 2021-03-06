﻿using System;

namespace SkbKontur.Catalogue.XmlSerializer.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DeclareXmlNamespaceAttribute : Attribute
    {
        public DeclareXmlNamespaceAttribute(string prefix, string uri)
        {
            Prefix = prefix;
            Uri = uri;
        }

        public string Prefix { get; set; }
        public string Uri { get; set; }
    }
}