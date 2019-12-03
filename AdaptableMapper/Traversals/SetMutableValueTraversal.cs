﻿using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations;

namespace AdaptableMapper.Traversals
{
    public abstract class SetMutableValueTraversal : SetValueTraversal
    {
        public ValueMutation ValueMutation { get; set; }

        protected abstract void SetValueImplementation(Context context, string value);

        public void SetValue(Context context, string value)
        {
            string formattedValue = ValueMutation?.Mutate(value) ?? value;

            SetValueImplementation(context, formattedValue);
        }
    }
}