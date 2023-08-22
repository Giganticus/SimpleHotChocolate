using System;
using HotChocolate;

namespace GraphQL;

public class Person
{
    public Person(
        Guid id,
        string name, 
        int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    [GraphQLDescription("Id of person")]
    public Guid Id { get; }
    public string Name { get;  }
    
    public int Age { get;  }
}