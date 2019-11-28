﻿using FluentAssertions;
using System.Collections.Generic;
using AdaptableMapper.Configuration;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToModel
    {
        [Fact]
        public void XmlToModelTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            string modelTargetInstantiatorSource = CreateModelTargetInstantiatorSource();

            object resultObject = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), modelTargetInstantiatorSource);

            var result = resultObject as ModelObjects.Armies.Root;
            result.Should().NotBeNull();

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            errorObserver.GetRaisedWarnings().Count.Should().Be(2);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Organization.Leaders.Count.Should().Be(3);
            result.Organization.Leaders[0].Reference.Should().Be("alpha-bravo-tango-delta");
            result.Organization.Leaders[2].LeaderPerson.Person.Name.Should().Be("John J. Pershing");

            result.Armies.Count.Should().Be(2);
            result.Armies[0].Code.Should().Be("naval");
            result.Armies[1].Platoons.Count.Should().Be(2);
            result.Armies[0].Platoons[0].Members.Count.Should().Be(1);
            result.Armies[0].Platoons[0].Members[0].Name.Should().Be("FlagShip-Alpha");
            result.Armies[1].Platoons[0].Members[1].CrewMembers[0].Name.Should().Be("John");
            result.Armies[1].Platoons[1].LeaderReference.Should().Be("");
            result.Armies[0].Platoons[1].LeaderReference.Should().Be("Ween");
        }

        [Fact]
        public void XmlToModelToString()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Configuration.Model.ModelToStringObjectConverter();
            string modelTargetInstantiatorSource = CreateModelTargetInstantiatorSource();

            object resultObject = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), modelTargetInstantiatorSource);

            var result = resultObject as string;
            result.Should().NotBeNull();

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ModelTarget_ArmyExpected.txt");
            result.Should().BeEquivalentTo(expectedResult);
        }

        private string CreateModelTargetInstantiatorSource()
        {
            var rootType = typeof(ModelObjects.Armies.Root);
            var result = new Configuration.Model.ModelTargetInstantiatorSource
            {
                AssemblyFullName = rootType.Assembly.FullName,
                TypeFullName = rootType.FullName
            };

            return JsonSerializer.Serialize(result);
        }

        public static MappingConfiguration GetMappingConfiguration()
        {
            var crewMember = new Mapping(
                new Xml.XmlGetThisValue(),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("Name")
            );

            var crewMemberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMember
                },
                new Xml.XmlGetScope("./crew/crewMember"),
                new Traversals.Model.ModelGetTemplateTraversal("CrewMembers"),
                new Configuration.Model.ModelChildCreator()
            );

            var memberName = new Mapping(
                new Xml.XmlGetValue("./@name"),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("Name")
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    crewMemberScope
                },
                new List<Mapping>
                {
                    memberName
                },
                new Xml.XmlGetScope("./members/member"),
                new Traversals.Model.ModelGetTemplateTraversal("Members"),
                new Configuration.Model.ModelChildCreator()
            );

            var platoonCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("Code")
            );

            var leaderReference = new Mapping(
                new Xml.XmlGetValue("./leaderReference"),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("LeaderReference")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    memberScope
                },
                new List<Mapping>
                {
                    platoonCode,
                    leaderReference
                },
                new Xml.XmlGetScope("./platoon"),
                new Traversals.Model.ModelGetTemplateTraversal("Platoons"),
                new Configuration.Model.ModelChildCreator()
            );

            var armyCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("Code")
            );

            var armyScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    platoonScope
                },
                new List<Mapping>
                {
                    armyCode
                },
                new Xml.XmlGetScope("./army"),
                new Traversals.Model.ModelGetTemplateTraversal("Armies"),
                new Configuration.Model.ModelChildCreator()
            );

            var reference = new Mapping(
                new Xml.XmlGetValue("./@reference"),
                new Traversals.Model.ModelSetValueOnPropertyTraversal("Reference")
            );

            var leaderName = new Mapping(
                new Xml.XmlGetThisValue(),
                new Traversals.Model.ModelSetValueOnPathTraversal("LeaderPerson/Person/Name")
            );

            var leadersScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    reference,
                    leaderName
                },
                new Xml.XmlGetScope("./leaders/leader"),
                new Traversals.Model.ModelGetTemplateTraversal("Organization/Leaders"),
                new Configuration.Model.ModelChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                leadersScope,
                armyScope
            };

            var contextFactory = new ContextFactory(
                new Xml.XmlObjectConverterRemovesNamespace(),
                new Configuration.Model.ModelTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}