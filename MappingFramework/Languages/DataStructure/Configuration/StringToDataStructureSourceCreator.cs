﻿using System;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Json;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class StringToDataStructureSourceCreator : SourceCreator, ResolvableByTypeId
    {
        public const string _typeId = "42dec10d-abb3-4c96-8c9a-dcc6798caa5a";
        public string TypeId => _typeId;

        public StringToDataStructureSourceCreator() { }

        public StringToDataStructureSourceCreator(DataStructureTargetCreatorSource dataStructureTargetInstantiatorSource)
        {
            DataStructureTargetInstantiatorSource = dataStructureTargetInstantiatorSource;
        }

        public DataStructureTargetCreatorSource DataStructureTargetInstantiatorSource { get; set; }

        public object Convert(Context context, object source)
        {
            if (source is not string input)
            {
                context.InvalidInput(source, typeof(string));
                return new NullDataStructure();
            }

            Type sourceType;
            try
            {
                sourceType = Activator.CreateInstance(
                    DataStructureTargetInstantiatorSource.AssemblyFullName,
                    DataStructureTargetInstantiatorSource.TypeFullName
                ).Unwrap().GetType();
            }
            catch
            {
                context.AddInformation($"Could not instantiate sourceType from {nameof(DataStructureTargetInstantiatorSource)}", InformationType.Error);
                return new NullDataStructure();
            }

            object result;
            try
            {
                result = JsonSerializer.Deserialize(sourceType, input);
            }
            catch
            {
                context.AddInformation("Could not deserialize source", InformationType.Error);
                return new NullDataStructure();
            }

            if (result is not TraversableDataStructure)
            {
                context.InvalidType(result, typeof(TraversableDataStructure));
                return new NullDataStructure();
            }

            return result;
        }
    }
}