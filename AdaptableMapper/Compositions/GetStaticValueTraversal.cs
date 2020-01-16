﻿using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.Compositions
{
    public class GetStaticValueTraversal : GetValueTraversal, GetValueStringTraversal, ResolvableByTypeId
    {
        public const string _typeId = "136fe331-e3c2-496d-a7fc-e317b7eb80aa";
        public string TypeId => _typeId;

        public string Value { get; set; }

        public GetStaticValueTraversal() { }
        public GetStaticValueTraversal(string value)
            => Value = value;

        public string GetValue(string value)
        {
            return GetValue(new Context(value, string.Empty));
        }

        public string GetValue(Context context)
        {
            if (string.IsNullOrWhiteSpace(Value))
                Process.ProcessObservable.GetInstance().Raise("GetStaticValueTraversal#1; Value is set to an empty string", "error");

            return Value;
        }
    }
}