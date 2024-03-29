﻿using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Hardwares
{
    public class Motherboard : TraversableDataStructure
    {
        public Motherboard()
        {
            GraphicalCards = new ChildList<GraphicalCard>(this);
            Memories = new ChildList<Memory>(this);
            HardDrives = new ChildList<HardDrive>(this);
        }

        public CPU CPU { get; set; } = new();
        public ChildList<GraphicalCard> GraphicalCards { get; set; }
        public ChildList<Memory> Memories { get; set; }
        public ChildList<HardDrive> HardDrives { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}