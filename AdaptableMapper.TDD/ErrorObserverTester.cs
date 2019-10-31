﻿using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ErrorObserverTester
    {
        [Fact]
        public void ErrorObserverTestMessages()
        {
            var errorObserver = new TestErrorObserver();
            var errorObserverVerbose = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);
            Errors.ErrorObservable.GetInstance().RegisterVerbose(errorObserverVerbose);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            mappingConfiguration.Map("");

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);
            Errors.ErrorObservable.GetInstance().UnregisterVerbose(errorObserverVerbose);

            errorObserver.GetErrors().Last().Should().Be("XML#1; Path could not be traversed;");
            errorObserverVerbose.GetErrors().Last().Should().Be("XML#1; Path could not be traversed; objects:[\"./army/platoon\",{\"nullObject\":null}]");
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var platoonCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Xml.XmlSetValue("./@code")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    platoonCode
                },
                new Xml.XmlGetScope("./army/platoon"),
                new Xml.XmlTraversal("./platoons"),
                new Xml.XmlTraversalTemplate("./platoon"),
                new Xml.XmlChildCreator()
            );

            var stolenIntelScope = new MappingScopeRoot(
                new List<MappingScopeComposite>()
                {
                    platoonScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(stolenIntelScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}