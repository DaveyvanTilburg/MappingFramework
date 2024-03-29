﻿using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.Languages.DataStructure.Traversals;
using MappingFramework.Languages.Dictionary.Configuration;
using MappingFramework.Languages.Dictionary.Traversals;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework
{
    public static class OptionLists
    {
        private static List<Type> ComposedGetValueTraversals => new()
        {
            typeof(GetAdditionalSourceValue),
            typeof(GetConcatenatedByListValueTraversal),
            typeof(GetConcatenatedValueTraversal),
            typeof(GetMutatedValueTraversal),
            typeof(GetNumberOfHits),
            typeof(GetSearchValueTraversal),
            typeof(GetStaticValue),
            typeof(GetValueTraversalDaysBetweenDates),
            typeof(IfConditionThenAElseBGetValueTraversal)
        };

        private static List<Type> NewXmlGetValueTraversals => new()
        {
            typeof(XmlGetValueTraversal),
            typeof(XmlGetThisValueTraversal)
        };

        private static List<Type> NewJsonGetValueTraversals => new()
        {
            typeof(JsonGetValueTraversal)
        };

        private static List<Type> NewDataStructureGetValueTraversals => new()
        {
            typeof(DataStructureGetValueTraversal)
        };

        private static List<Type> XmlGetValueTraversals() => CreateList(NewXmlGetValueTraversals, ComposedGetValueTraversals);
        private static List<Type> JsonGetValueTraversals() => CreateList(NewJsonGetValueTraversals, ComposedGetValueTraversals);
        private static List<Type> DataStructureGetValueTraversals() => CreateList(NewDataStructureGetValueTraversals, ComposedGetValueTraversals);

        private static List<Type> GetValueTraversals(ContentType contentType)
        {
            switch(contentType)
            {
                case ContentType.Xml:
                    return XmlGetValueTraversals();
                case ContentType.Json:
                    return JsonGetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(GetValueTraversal)}");
            }
        }

        private static List<Type> ComposedSetValueTraversals => new()
        {
            typeof(SetMutatedValueTraversal)
        };

        private static List<Type> NewXmlSetValueTraversals => new()
        {
            typeof(XmlSetValueTraversal),
            typeof(XmlSetThisValueTraversal),
            typeof(XmlSetGeneratedIdValueTraversal)
        };

        private static List<Type> NewJsonSetValueTraversals => new()
        {
            typeof(JsonSetValueTraversal)
        };

        private static List<Type> NewDataStructureSetValueTraversals => new()
        {
            typeof(DataStructureSetValueOnPathTraversal),
            typeof(DataStructureSetValueOnPropertyTraversal)
        };

        private static List<Type> NewDictionarySetValueTraversals => new()
        {
            typeof(DictionarySetValueTraversal)
        };

        private static List<Type> XmlSetValueTraversals() => CreateList(NewXmlSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> JsonSetValueTraversals() => CreateList(NewJsonSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> DataStructureSetValueTraversals() => CreateList(NewDataStructureSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> DictionaryGetValueTraversals() => CreateList(NewDictionarySetValueTraversals, ComposedSetValueTraversals);

        private static List<Type> SetValueTraversals(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Xml:
                    return XmlSetValueTraversals();
                case ContentType.Json:
                    return JsonSetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureSetValueTraversals();
                case ContentType.Dictionary:
                    return DictionaryGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(SetValueTraversal)}");
            }
        }

        private static Type ObjectConverter(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlSourceCreator) :
            contentType == ContentType.Json ? typeof(JsonSourceCreator) :
            contentType == ContentType.DataStructure ? typeof(DataStructureSourceCreator) :
            contentType == ContentType.String ? typeof(StringToDataStructureSourceCreator) : null;

        private static Type TargetInstantiator(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlTargetCreator) :
            contentType == ContentType.Json ? typeof(JsonTargetCreator) :
            contentType == ContentType.DataStructure ? typeof(DataStructureTargetCreator) : 
            contentType == ContentType.Dictionary ? typeof(DictionaryTargetCreator) : null;

        private static Type ResultObjectConverter(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XElementToStringResultObjectCreator) :
            contentType == ContentType.Json ? typeof(JTokenToStringResultObjectCreator) :
            contentType == ContentType.DataStructure ? typeof(ObjectToJsonResultObjectCreator):
            contentType == ContentType.Dictionary ? typeof(ObjectToJsonResultObjectCreator) : null;

        private static Type GetListValueTraversal(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlGetListValueTraversal) :
            contentType == ContentType.Json ? typeof(JsonGetListValueTraversal) :
            contentType == ContentType.DataStructure ? typeof(DataStructureGetListValueTraversal) : null;

        private static Type GetTemplateTraversal(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlGetTemplateTraversal) :
            contentType == ContentType.Json ? typeof(JsonGetTemplateTraversal) :
            contentType == ContentType.DataStructure ? typeof(DataStructureGetTemplateTraversal) :
            contentType == ContentType.Dictionary ? typeof(DictionaryGetTemplateTraversal) : null;

        private static IEnumerable<Type> ChildCreator(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Xml:
                    yield return typeof(XmlChildCreator);
                    break;
                case ContentType.Json:
                    yield return typeof(JsonChildCreator);
                    break;
                case ContentType.DataStructure:
                    yield return typeof(DataStructureChildCreator);
                    break;
                case ContentType.Dictionary:
                    yield return typeof(DictionaryChildCreator);
                    yield return typeof(DictionaryChildToParent);
                    break;
            }
        }

        private static Type GetListSearchPathValueTraversal(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlGetListValueTraversal) :
            contentType == ContentType.Json ? typeof(JsonGetListValueTraversal) :
            contentType == ContentType.DataStructure ? typeof(DataStructureGetListValueTraversal) : null;

        private static Type GetSearchPathValueTraversal(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlGetValueTraversal) :
            contentType == ContentType.Json ? typeof(JsonGetValueTraversal) :
            contentType == ContentType.DataStructure ? typeof(DataStructureGetValueTraversal) : null;

        public static List<Type> List(Type type, ContentType contentType)
        {
            if (!type.IsInterface)
                throw new Exception("List is meant to be called with an interface type as parameter");

            if (type == typeof(SourceCreator))
                return new List<Type> { ObjectConverter(contentType) };
            if (type == typeof(TargetCreator))
                return new List<Type> { TargetInstantiator(contentType) };
            if (type == typeof(ResultObjectCreator))
                return new List<Type> { ResultObjectConverter(contentType) };

            if (type == typeof(Condition))
                return new List<Type>
                {
                    typeof(NullObject),
                    typeof(CompareCondition),
                    typeof(ListOfConditions),
                    typeof(NotEmptyCondition)
                };
            if (type == typeof(ValueMutation))
                return new List<Type>
                {
                    typeof(CreateSeparatedRangeFromNumberValueMutation),
                    typeof(DateValueMutation),
                    typeof(DictionaryReplaceValueMutation),
                    typeof(ListOfValueMutations),
                    typeof(NumberValueMutation),
                    typeof(PlaceholderValueMutation),
                    typeof(ReplaceValueMutation),
                    typeof(SubstringValueMutation),
                    typeof(ToLowerValueMutation),
                    typeof(ToUpperValueMutation),
                    typeof(TrimValueMutation)
                };
            if (type == typeof(GetValueStringTraversal))
                return new List<Type>
                {
                    typeof(SplitByCharTakePositionStringTraversal),
                    typeof(GetThisValueStringTraversal)
                };
            if (type == typeof(GetValueTraversal))
                return GetValueTraversals(contentType);
            if (type == typeof(SetValueTraversal))
                return SetValueTraversals(contentType);
            if (type == typeof(GetListValueTraversal))
                return new List<Type> { GetListValueTraversal(contentType), typeof(GetConditionedListValueTraversal), typeof(GetListSearchValueTraversal) };
            if (type == typeof(GetListSearchPathValueTraversal))
                return new List<Type> { GetListSearchPathValueTraversal(contentType) };
            if (type == typeof(GetSearchPathValueTraversal))
                return new List<Type> { GetSearchPathValueTraversal(contentType) };
            if (type == typeof(GetTemplateTraversal))
                return new List<Type> { GetTemplateTraversal(contentType) };
            if (type == typeof(ChildCreator))
                return new List<Type>(ChildCreator(contentType));

            throw new Exception($"{type.Name} is not supported");
        }

        private static List<T> CreateList<T>(params IEnumerable<T>[] itemsList)
        {
            var result = new List<T>();
            
            foreach (IEnumerable<T> items in itemsList)
                result.AddRange(items);

            return result;
        }
    }
}