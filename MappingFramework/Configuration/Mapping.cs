﻿using MappingFramework.Traversals;

namespace MappingFramework.Configuration
{
    public sealed class Mapping
    {
        public GetValueTraversal GetValueTraversal { get; set; }
        public SetValueTraversal SetValueTraversal { get; set; }

        public Mapping(
            GetValueTraversal getValueTraversal, 
            SetValueTraversal setValueTraversal)
        {
            GetValueTraversal = getValueTraversal;
            SetValueTraversal = setValueTraversal;
        }

        public void Map(Context context, MappingCaches mappingCaches)
        {
            if (!Validate())
                return;

            string value = GetValueTraversal.GetValue(context);

            SetValueTraversal.SetValue(context, mappingCaches, value);
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#11; GetValueTraversal cannot be null", "error");
                result = false;
            }

            if (SetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#12; SetValueTraversal cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}