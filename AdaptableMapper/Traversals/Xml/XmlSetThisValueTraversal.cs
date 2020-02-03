﻿using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetThisValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0e33e050-8da6-4d87-ad49-ff9bde9bf953";
        public string TypeId => _typeId;

        public XmlSetThisValueTraversal() { }

        public bool SetAsCData { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#20; target is not of expected type XElement", "error", context.Target?.GetType().Name);
                return;
            }

            if (SetAsCData)
            {
                xElement.Add(new XCData(value));
            }
            else
            {
                xElement.Value = value;
            }
        }
    }
}