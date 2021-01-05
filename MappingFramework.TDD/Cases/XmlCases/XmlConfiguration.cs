﻿using MappingFramework.Process;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals;
using MappingFramework.Xml;
using Xunit;
using FluentAssertions;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlConfiguration
    {
        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "e-XmlChildCreator#1;")]
        [InlineData("InvalidTemplateType", ContextType.EmptyObject, "e-XmlChildCreator#2;")]
        public void XmlChildCreatorCreateChild(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlChildCreator();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = context, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "e-XmlChildCreator#3;")]
        [InlineData("InvalidTemplateType", ContextType.EmptyObject, "e-XmlChildCreator#4;")]
        public void XmlChildCreatorAddToParent(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlChildCreator();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.AddToParent(new Template { Parent = context, Child = string.Empty }, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", "e-XML#18;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", "e-XML#19;")]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml")]
        public void XmlObjectConverter(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, params string[] expectedErrors)
        {
            var subject = new XmlObjectConverter { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            object value = null;
            List<Information> result = new Action(() => { value = subject.Convert(context); }).Observe();

            value.Should().NotBeNull();
            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringObjectConverter();
                var convertedResult = converter.Convert(xElementValue);
                convertedResult.Should().Be(expectedResult);
            }
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", "e-XML#24;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", "e-XML#6;")]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml")]
        public void XmlTargetInstantiator(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, params string[] expectedErrors)
        {
            var subject = new XmlTargetInstantiator { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            object value = null;
            List<Information> result = new Action(() => { value = subject.Create(context); }).Observe();

            value.Should().NotBeNull();
            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringObjectConverter();
                var convertedResult = converter.Convert(xElementValue);
                convertedResult.Should().Be(expectedResult);
            }
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, true, true, 0, "e-XML#9;")]
        [InlineData("LengthCheckUseIndentation", ContextType.AlternativeTestObject, true, true, 13)]
        [InlineData("LengthCheckDoNotUseIndentation", ContextType.AlternativeTestObject, false, true, 1)]
        [InlineData("LengthCheckUseIndentationWithoutDeclaration", ContextType.AlternativeTestObject, true, false, 12)]
        [InlineData("LengthCheckDoNotUseIndentationWithoutDeclaration", ContextType.AlternativeTestObject, false, false, 1)]
        public void XElementToStringObjectConverter(string because, ContextType contextType, bool useIndentation, bool includeDeclaration, int expectedLines, params string[] expectedErrors)
        {
            var subject = new XElementToStringObjectConverter { UseIndentation = useIndentation, IncludeDeclaration = includeDeclaration };
            object context = Xml.CreateTarget(contextType);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Convert(context) as string; }).Observe();

            information.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                lines.Length.Should().Be(expectedLines);
            }
        }
    }
}