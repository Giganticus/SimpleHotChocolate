using System;

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

    public Guid Id { get; }
    public string Name { get;  }
    
    public int Age { get;  }
}