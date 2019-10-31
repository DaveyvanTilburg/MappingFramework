﻿using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if(!(parent is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("nullObject");
            }

            if (!(template is XElement xTemplate))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("nullObject");
            }

            var xTemplateCopy = new XElement(xTemplate);
            xElement.Add(xTemplateCopy);

            return xTemplateCopy;
        }
    }
}