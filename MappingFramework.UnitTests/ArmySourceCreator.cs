﻿using MappingFramework.UnitTests.DataStructureExamples.Armies;

namespace MappingFramework.UnitTests
{
    internal static class ArmySourceCreator
    {
        public static Root CreateArmy()
        {
            var leader1 = new Leader { Reference = "alpha-bravo-tango-delta" , LeaderPerson = new LeaderPerson { Person = new Person { Name = "Christopher columbus" } } };
            var leader2 = new Leader { Reference = "Ween", LeaderPerson = new LeaderPerson { Person = new Person { Name = "Ocean man" } } };
            var leader3 = new Leader { Reference = "Pershing", LeaderPerson = new LeaderPerson { Person = new Person { Name = "John J. Pershing" } } };

            Army army1 = CreateArmy1();
            Army army2 = CreateArmy2();

            var root = new Root();
            root.Armies.Add(army1);
            root.Armies.Add(army2);
            root.Organization.Leaders.Add(leader1);
            root.Organization.Leaders.Add(leader2);
            root.Organization.Leaders.Add(leader3);

            return root;
        }

        private static Army CreateArmy1()
        {
            var army1platoon2member1crewMember1 = new CrewMember { Name = "Natasha" };
            var army1platoon2member1crewMember2 = new CrewMember { Name = "Yuri" };

            var army1platoon2member1 = new Member { Name = "Sub-Zero" };
            army1platoon2member1.CrewMembers.Add(army1platoon2member1crewMember1);
            army1platoon2member1.CrewMembers.Add(army1platoon2member1crewMember2);
            var army1platoon2 = new Platoon
            {
                Code = "clean-floors",
                LeaderReference = "Ween",
                Deployed = "True"
            };
            army1platoon2.Members.Add(army1platoon2member1);

            var army1platoon1member1crewMember1 = new CrewMember { Name = "John" };
            var army1platoon1member1crewMember2 = new CrewMember { Name = "Jane" };
            var army1platoon1member1 = new Member
            {
                Name = "FlagShip-Alpha"
            };
            army1platoon1member1.CrewMembers.Add(army1platoon1member1crewMember1);
            army1platoon1member1.CrewMembers.Add(army1platoon1member1crewMember2);

            var army1platoon1 = new Platoon
            {
                Code = "black-sky",
                LeaderReference = "alpha-bravo-tango-delta",
                Deployed = "True"
            };
            army1platoon1.Members.Add(army1platoon1member1);

            var army1 = new Army { Code = "navel" };
            army1.Platoons.Add(army1platoon1);
            army1.Platoons.Add(army1platoon2);
            return army1;
        }

        private static Army CreateArmy2()
        {
            var army1platoon2member1 = new Member{ Name = "Pharah" };
            var army2platoon2 = new Platoon
            {
                Code = "air-soldier",
                LeaderReference = "",
                Deployed = "False"
            };
            army2platoon2.Members.Add(army1platoon2member1);

            var army2platoon1member2crewMmeber1 = new CrewMember { Name = "John" };
            var army2platoon1member2 = new Member { Name = "Boeing B-17" };
            army2platoon1member2.CrewMembers.Add(army2platoon1member2crewMmeber1);

            var army2platoon1member1crewMmeber1 = new CrewMember { Name = "Hans" };
            var army2platoon1member1 = new Member { Name = "Messerschmitt Bf 109" };
            army2platoon1member1.CrewMembers.Add(army2platoon1member1crewMmeber1);

            var army2platoon1 = new Platoon
            {
                Code = "death-rains-from-above",
                LeaderReference = "Pershing",
                Deployed = "True"
            };
            army2platoon1.Members.Add(army2platoon1member1);
            army2platoon1.Members.Add(army2platoon1member2);

            var army2 = new Army { Code = "thunder-struck" };
            army2.Platoons.Add(army2platoon1);
            army2.Platoons.Add(army2platoon2);
            return army2;
        }
    }
}