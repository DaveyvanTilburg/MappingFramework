﻿using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Configuration
{
    [ContentType(ContentType.Json)]
    public sealed class JTokenToStringResultObjectCreator : ResultObjectCreator, ResolvableByTypeId
    {
        public const string _typeId = "111821e4-70dd-43b4-9c5d-3738aa4a102c";
        public string TypeId => _typeId;

        public JTokenToStringResultObjectCreator() { }

        public bool UseIndentation { get; set; } = true;

        public object Convert(object source)
            => ((JToken)source).ToString(UseIndentation ? Formatting.Indented : Formatting.None);
    }
}