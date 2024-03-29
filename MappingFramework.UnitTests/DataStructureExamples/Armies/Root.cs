﻿using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Armies
{
    public class Root : TraversableDataStructure
    {
        public Root()
        {
            Armies = new ChildList<Army>(this);
        }

        public ChildList<Army> Armies { get; set; }
        public Organization Organization { get; set; } = new();
    }
}