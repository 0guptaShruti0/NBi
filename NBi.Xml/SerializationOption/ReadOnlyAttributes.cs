﻿using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Constraints;

namespace NBi.Xml.SerializationOption
{
    internal class ReadOnlyAttributes : XmlAttributeOverrides
    {

        public ReadOnlyAttributes()
            : base()
        {
        }

        public void Build()
        {

            XmlAttributes attrs = null;

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("description");
            Add(typeof(TestXml), "Description", attrs);

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("ignore");
            Add(typeof(TestXml), "Ignore", attrs);

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("caption");
            Add(typeof(ContainXml), "Caption", attrs);

            var property = typeof(TestXml).GetField("Constraints");
            var arrayAttr = (XmlArrayAttribute)property.GetCustomAttributes(typeof(XmlArrayAttribute), false)[0];
            var arrayItemAttrs = property.GetCustomAttributes(typeof(XmlArrayItemAttribute), false).Cast<XmlArrayItemAttribute>().ToList();
            attrs = new XmlAttributes();
            attrs.XmlArray = arrayAttr;
            arrayItemAttrs.ForEach(i => attrs.XmlArrayItems.Add(i));
            attrs.XmlArrayItems.Add(new XmlArrayItemAttribute("subsetOf", typeof(SubsetOfXml)));
            Add(typeof(TestXml), "Constraints", attrs);
        }
    }
}