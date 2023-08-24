using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

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

    public Person(
        Guid id,
        string name,
        int age,
        IEnumerable<Interest> interests):this(id, name, age)
    {
        Interests = interests;
    }

    [GraphQLDescription("Id of person")]
    public Guid Id { get; }
    public string Name { get;  }
    
    public int Age { get;  }

    public IEnumerable<Interest> Interests { get; } = new List<Interest>();
}

public class Interest
{
    public int Id { get; }
    public string Description { get; }

    public Interest(
        int id,
        string description)
    {
        Id = id;
        Description = description;
    }
}

public class PersonType : ObjectType<Person>
{
    protected override void Configure(IObjectTypeDescriptor<Person> descriptor)
    {
        descriptor
            .Field(p => p.Interests)
            .ResolveWith<PersonFieldResolvers>(p => p.GetInterests(default!));
    }
}

public class PersonFieldResolvers
{
    public IEnumerable<Interest> GetInterests(
    [Parent] Person person)
    {
        return new[] { new Interest(1, "Interest1"), new Interest(2, "Interest2"), new Interest(3, "Interest 3") };
    }
}