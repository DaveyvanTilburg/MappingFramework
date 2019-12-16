﻿using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetGeneratedIdValueTraversal : SetMutableValueTraversal
    {
        public XmlSetGeneratedIdValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }
        public bool SetAsCData { get; set; }
        public int StartingNumber { get; set; }

        protected override void SetValueImplementation(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", "error", Path, context.Target?.GetType().Name);
                return;
            }

            string number = GetId(xElement.Parent, mappingCaches);

            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), number, SetAsCData);
        }

        private string GetId(XElement parent, MappingCaches mappingCaches)
        {
            var cache = mappingCaches.GetCache<GenerateIdCache>(nameof(GenerateIdCache));

            int id = cache.GenerateNewId(parent, Path, StartingNumber);
            return id.ToString();
        }
    }
}