﻿using System;
using AdaptableMapper.Process;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AdaptableMapper.TDD.ATDD
{
    [Binding]
    public class ConfigurationValidationsSteps
    {
        private MappingConfigurationBuilder _builder;
        private IReadOnlyCollection<Information> _information;
        private object _result;

        [Given(@"I create a mappingConfiguration")]
        public void GivenICreateAMappingConfiguration()
        {
            _builder = new MappingConfigurationBuilder();
            _builder.StartNew();
        }

        [Given(@"I add a contextFactory")]
        public void GivenIAddAContextFactory()
        {
            _builder.AddContextFactory();
        }

        [Given(@"I add a MappingScopeRoot with an empty list")]
        public void GivenIAddAMappingScopeRootWithAnEmptyList()
        {
            _builder.AddMappingScopeRoot();
        }

        [Given(@"I add a '(.*)' ObjectConverter for mappingConfiguration")]
        public void GivenIAddAObjectConverterForMappingConfiguration(string type)
        {
            _builder.AddObjectConverter(type);
        }

        [Given(@"I add a '(.*)' ObjectConverter to the contextFactory")]
        public void GivenIAddAObjectConverterToTheContextFactory(string type)
        {
            _builder.AddObjectConverterToContextFactory(type);
        }

        [Given(@"I add a '(.*)' TargetInitiator to the contextFactory")]
        public void GivenIAddATargetInitiatorToTheContextFactory(string type)
        {
            _builder.AddTargetInitiatorToContextFactory(type);
        }

        [Given(@"I add a Scope to the root")]
        public void GivenIAddAScopeToTheRoot(Table table)
        {
            var scopeCompositeModel = table.CreateInstance<ScopeCompositeModel>();

            _builder.AddScopeToRoot(scopeCompositeModel);
        }

        [Given(@"I add a mapping to the scope")]
        public void GivenIAddAMappingToTheScope(Table table)
        {
            var mapping = table.CreateInstance<MappingModel>();

            _builder.AddMappingToLastScope(mapping);
        }

        [When(@"I run Map with a null parameter")]
        public void WhenIRunMapWithANullParameter()
        {
            Map(null, null);
        }


        [When(@"I run Map with a string parameter '(.*)'")]
        public void WhenIRunMapWithAStringParameter(string p0)
        {
            Map(p0, null);
        }

        private void Map(object input, object targetSource)
        {
            MappingConfiguration mappingConfiguration = _builder.GetResult();
            _information = new Action(() => { _result = mappingConfiguration.Map(input, targetSource); }).Observe();
        }

        [Then(@"the result should contain the following errors '(.*)'")]
        public void ThenTheResultShouldContainTheFollowingErrors(string codes)
        {
            IReadOnlyCollection<string> expectedInformationCodes = Regex.Split(codes, @"(?<=[;])").Where(c => !string.IsNullOrEmpty(c)).ToList();

            _information.ValidateResult(expectedInformationCodes);
        }

        [Then(@"result should be null")]
        public void ThenResultShouldBeNull()
        {
            _result.Should().BeNull();
        }

        [Then(@"result should be '(.*)'")]
        public void ThenResultShouldBe(string p0)
        {
            _result.Should().Be(p0);
        }
    }
}