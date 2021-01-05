﻿using System.Collections.Generic;

namespace MappingFramework.DataStructure
{
    public sealed class ChildList<T> : List<T> where T : TraversableDataStructure
    {
        private readonly TraversableDataStructure _parent;

        public ChildList(TraversableDataStructure parent)
        {
            _parent = parent;
        }

        public new void Add(T model)
        {
            model.Parent = _parent;

            base.Add(model);
        }
    }
}