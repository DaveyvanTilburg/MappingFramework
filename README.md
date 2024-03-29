# MappingFramework

### How to use

Mock up of structure and flow:

![UML](Diagram.jpg)

See unit tests for examples

### How to use searches
 * SearchPath : This path should have use for the placeholder {{searchValue}}, so that traversal is possible outside of this scope with values of inside this scope
 * SearchPathValue : {{searchValue}} will be replaced with the result of this path

Example target Json
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionName":""
            }
         }
      ]
   }
}
```

* Xml as Source
```
<root>
    <people>
        <person>
            <professionId>123</professionId>
        </person>
        <person>
            <professionId>123</professionId>
        </person>
    </people>
    <professions>
        <profession id="123">
            <name>Programmer</name>
        </profession>
    </professions>
</root>
```
 * Example Scope : //people/person
 * Example JsonTraversal : $.root.people
 * SearchPath : ../../professions/profession[@id='{{searchValue}}']/name
 * SearchPathValue : ./professionId
 * Example JsonSetValue : .person.professionName
    
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionId":"123"
            }
         },
         { 
            "person":{ 
               "professionId":"123"
            }
         }
      ],
      "profession":[ 
         { 
            "id":"123",
            "name":"Programmer"
         }
      ]
   }
}
```
* Json as source
    * Example Scope : $.root.people[*]
    * Example JsonTraversal : $.root.people
    * SearchPath : ../../../.profession[?(@.id=='{{searchValue}}')].name
    * SearchPathValue : .person.professionId
    * Example JsonSetValue : .person.professionName
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionId":"123"
            }
         },
         { 
            "person":{ 
               "professionId":"123"
            }
         }
      ],
      "profession":[ 
         { 
            "id":"123",
            "name":"Programmer"
         }
      ]
   }
}
```
* Model as source
    * Example Scope : /root/people
    * Example JsonTraversal : $.root.people
    * SearchPath : ../../profession{'PropertyName':'id','Value':'{{searchValue}}'}/name
    * SearchPathValue : professionId
    * Example JsonSetValue : .person.professionName

How to use DataStructureTargetCreatorSource
```
{
  "AssemblyFullName": "MappingFramework.MappingInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
  "TypeFullName": "MappingFramework.MappingInterface.Examples.ExampleRootDataStructure"
}
```

### Authors

* **Davey van Tilburg**

### License

Do what ever you want! (But please respect each other in the process)
