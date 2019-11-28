﻿using System;
using System.Xml.Linq;

namespace AdaptableMapper.Configuration.Xml
{
    public sealed class XmlObjectConverterRemovesNamespace : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#30; source is not of expected type String", "error", source?.GetType().Name);
                return string.Empty;
            }

            XElement root;
            try
            {
                root = XElement.Parse(input);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#31; input could not be parsed to XElement", "error", input, exception.GetType().Name, exception.Message);
                root = new XElement("nullObject");
            }

            RemoveAllNamespaces(root);

            return root;
        }

        private static void RemoveAllNamespaces(XElement element)
        {
            element.Name = element.Name.LocalName;

            foreach (var node in element.DescendantNodes())
                if (node is XElement xElement)
                    RemoveAllNamespaces(xElement);
        }
    }
}