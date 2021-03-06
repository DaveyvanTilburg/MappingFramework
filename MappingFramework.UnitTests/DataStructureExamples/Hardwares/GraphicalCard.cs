﻿using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Hardwares
{
    public class GraphicalCard : TraversableDataStructure
    {
        public GraphicalCard()
        {
            CPUs = new ChildList<CPU>(this);
            MemoryChips = new ChildList<MemoryChip>(this);
        }

        public ChildList<CPU> CPUs { get; set; }
        public ChildList<MemoryChip> MemoryChips { get; set; }
        public string Brand { get; set; }
    }
}