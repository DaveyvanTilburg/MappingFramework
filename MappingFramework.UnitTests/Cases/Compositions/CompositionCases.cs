﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Languages.DataStructure.Traversals;
using MappingFramework.Languages.Xml.Interpretation;
using MappingFramework.Languages.Xml.Traversals;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using Xunit;

namespace MappingFramework.UnitTests.Cases.Compositions
{
    public class CompositionCases
    {
        [Theory]
        [InlineData("", "B", "C", "C")]
        [InlineData("A", "B", "C", "B")]
        public void IfConditionThenAElseBGetValueTraversal(string valueA, string valueB, string valueC, string expectedValue)
        {
            var subject = new IfConditionThenAElseBGetValueTraversal(
                new NotEmptyCondition(new GetStaticValue(valueA)),
                new GetStaticValue(valueB),
                new GetStaticValue(valueC));

            string result = subject.GetValue(new Context(null, null, null));
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void GetSearchValueTraversalWithXml()
        {
            var subject = new GetSearchValueTraversal(
                new XmlGetValueTraversal("./SimpleItems/SimpleItem[@Id='{{searchValue}}']/Name"),
                new GetStaticValue("2"));

            object source = XDocument.Load("./Resources/Simple.xml").Root;
            var context = new Context(source, null, null);

            string result = subject.GetValue(context);

            context.Information().Should().BeEmpty();
            result.Should().BeEquivalentTo("Joey");
        }

        [Theory]
        [InlineData("Items{'PropertyName':'Code','Value':'1'/Code", "", 1)]
        [InlineData("Items{'PropertyName':'Code','Value':'3'}/Code", "", 1)]
        [InlineData("Items{'PropertyName':'Code','Value':'1'}/Code", "1", 0)]
        public void GetSearchValueTraversalWithDataStructure(string path, string expectedResult, int informationCount)
        {
            var subject = new GetSearchValueTraversal(
                new DataStructureGetValueTraversal(path),
                new GetStaticValue("2"));

            object source = DataStructureCases.DataStructure.Stub(ContextType.TestObject, "item");
            var context = new Context(source, null, null);

            string result = subject.GetValue(context);

            if (informationCount == 0)
            {
                context.Information().Should().BeEmpty();
                result.Should().BeEquivalentTo(expectedResult);
            }
            else
            {
                context.Information().Count.Should().Be(informationCount);
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetListSearchValueTraversalXml()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetListSearchValueTraversal(
                new XmlGetListValueTraversal("./SimpleItems/SimpleItem[@Type='{{searchValue}}']"),
                new GetStaticValue("Person")
            );

            MethodResult<IEnumerable<object>> result = subject.GetValues(context);

            context.Information().Should().BeEmpty();
            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(2);
        }

        [Fact]
        public void GetConditionedListValueTraversalBadPath()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem***$%#$") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValue("AI")
                )
            );

            MethodResult<IEnumerable<object>> result = subject.GetValues(context);

            context.Information().Count.Should().Be(2);

            result.IsValid.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void GetConditionedListValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValue("AI")
                )
            );

            MethodResult<IEnumerable<object>> result = subject.GetValues(context);

            context.Information().Count.Should().Be(0);

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(3);

            result.Value.First().ToString().Should().Contain("Easy");
            result.Value.ToList()[1].ToString().Should().Contain("Medium");
            result.Value.Last().ToString().Should().Contain("Hard");
        }

        [Fact]
        public void GetConditionedListValueTraversalDistinctBy()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new XmlGetValueTraversal("./@Type")
            );

            MethodResult<IEnumerable<object>> result = subject.GetValues(context);

            context.Information().Count.Should().Be(0);

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(2);

            result.Value.First().ToString().Should().Contain("Davey");
            result.Value.Last().ToString().Should().Contain("Easy");
        }

        [Fact]
        public void GetConditionedListValueTraversalDistinctByConditioned()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValue("AI")
                ),
                new XmlGetValueTraversal("./@Type")
            );

            MethodResult<IEnumerable<object>> result = subject.GetValues(context);

            context.Information().Count.Should().Be(0);

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(1);

            result.Value.Last().ToString().Should().Contain("Easy");
        }

        [Fact]
        public void GetConcatenatedByListValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConcatenatedByListValueTraversal(
                new GetConditionedListValueTraversal(
                    new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                    new CompareCondition(
                        new XmlGetValueTraversal("./@Type"),
                        CompareOperator.Equals,
                        new GetStaticValue("Person")
                    )
                ),
                new XmlGetValueTraversal("./Name"),
                "-"
            );

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(0);

            result.Should().BeEquivalentTo("Davey-Joey");
        }

        [Fact]
        public void GetConcatenatedByListValueTraversalNullSeparator()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConcatenatedByListValueTraversal(
                new GetConditionedListValueTraversal(
                    new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                    new CompareCondition(
                        new XmlGetValueTraversal("./@Type"),
                        CompareOperator.Equals,
                        new GetStaticValue("Person")
                    )
                ),
                new XmlGetValueTraversal("./Name")
            );

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(0);

            result.Should().BeEquivalentTo("DaveyJoey");
        }

        [Fact]
        public void GetConcatenatedValueTraversalNullSeparator()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConcatenatedValueTraversal(
                new List<GetValueTraversal>
                {
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[1]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[4]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[6]/Name")
                }
            );

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(1);

            result.Should().BeEquivalentTo("DaveyMedium");
        }

        [Fact]
        public void GetConcatenatedValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null, null);

            var subject = new GetConcatenatedValueTraversal(
                new List<GetValueTraversal>
                {
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[1]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[4]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[6]/Name")
                },
                "-"
            );

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(1);

            result.Should().BeEquivalentTo("Davey-Medium");
        }

        [Fact]
        public void GetMutatedValueTraversal()
        {
            var context = new Context();
            
            var subject = new GetMutatedValueTraversal(
                new GetStaticValue("test"),
                new PlaceholderValueMutation("({0})")
            );

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(0);

            result.Should().BeEquivalentTo("(test)");
        }

        [Fact]
        public void SetMutatedValueTraversal()
        {
            var context = new Context();
            var subject = new SetMutatedValueTraversal(
                new XmlSetThisValueTraversal(),
                new PlaceholderValueMutation("({0})")
            );
            object source = XmlCases.Xml.Stub(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            Template name = traversal.GetTemplate(context, source);

            var setContext = new Context(null, name.Child, null);

            subject.SetValue(setContext, "Test");
            context.Information().Count.Should().Be(0);

            string value = new XmlGetThisValueTraversal().GetValue(new Context(setContext.Target, null, null));
            value.Should().BeEquivalentTo("(Test)");
        }

        [Theory]
        [InlineData("RangeWithLastDate", "2019/01/01", "2019/01/04", true, "4", 0)]
        [InlineData("RangeWithoutLastDate", "2019/01/01", "2019/01/04", false, "3", 0)]
        [InlineData("invalid first path", "a", "2019/01/04", false, "", 1)]
        [InlineData("invalid first path", "2019/01/01", "a", false, "", 1)]
        public void GetValueTraversalDaysBetweenDates(string because, string firstDate, string lastDate, bool includeLastDay, string expectedResult, int informationCount)
        {
            var subject = new GetValueTraversalDaysBetweenDates(new GetStaticValue(firstDate), new GetStaticValue(lastDate))
            {
                IncludeLastDay = includeLastDay
            };
            var context = new Context();
            
            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(informationCount, because);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }
    }
}