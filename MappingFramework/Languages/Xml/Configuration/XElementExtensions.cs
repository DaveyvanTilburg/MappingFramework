﻿using System.Xml.Linq;

namespace MappingFramework.Languages.Xml.Configuration
{
    public static class XElementExtensions
    {
        public static void RemoveAllNamespaces(this XElement element)
        {
            element.Name = element.Name.LocalName;

            foreach (var node in element.DescendantNodes())
                if (node is XElement xElement)
                    RemoveAllNamespaces(xElement);
        }
    }
}