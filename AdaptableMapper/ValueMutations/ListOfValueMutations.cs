﻿using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class ListOfValueMutations : ValueMutation
    {
        private List<ValueMutation> ValueMutations { get; set; }

        public string Mutate(Context context, string source)
        {
            var result = source;

            if (!Validate())
                return result;

            foreach(ValueMutation valueMutation in ValueMutations)
                result = valueMutation.Mutate(context, result);

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if ((ValueMutations?.Any() ?? false) == false)
            {
                Process.ProcessObservable.GetInstance().Raise("ListOfValueMutations#1; ValueMutations is empty", "error");
                result = false;
            }

            return result;
        }
    }
}