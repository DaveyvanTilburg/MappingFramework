﻿using System.Xml.Linq;
using MappingFramework.Caches;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Languages.Xml.Interpretation;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Traversals
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlSetGeneratedIdValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "907c1a97-cee0-4616-b986-c8a00fdec422";
        public string TypeId => _typeId;

        public XmlSetGeneratedIdValueTraversal() { }
        public XmlSetGeneratedIdValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }
        public bool SetAsCData { get; set; }
        public int StartingNumber { get; set; }

        public void SetValue(Context context, string value)
        {
            XElement xElement = (XElement)context.Target;
            string number = GetId(xElement.Parent, context);

            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), number, SetAsCData, context);
        }

        private string GetId(XElement parent, Context context)
        {
            var cache = context.MappingCaches.GetCache<GenerateIdCache>(nameof(GenerateIdCache));

            int id = cache.GenerateNewId(parent, Path, StartingNumber);
            return id.ToString();
        }
    }
}