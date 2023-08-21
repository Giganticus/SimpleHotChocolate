using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Types;

namespace GraphQL;

[ExtendObjectType("Mutation")]
public class PersonMutations
{
    private readonly List<Person> _people = new();
    
    public AddPersonPayload AddAPerson(AddPersonInput input)
    {
        if (_people.Select(x => x.Name).Contains(input.Name))
        {
            return new AddPersonPayload($"Person with name {input.Name} is already in the collection");
        }
        
        var person = new Person(
            Guid.NewGuid(),
            input.Name,
            input.Age);
        
        _people.Add(person);

        return new AddPersonPayload(person);    
    }
}

public class AddPersonPayload : PersonPayloadBase
{
    public AddPersonPayload(string error) : base(new[] { error })
    {
    }
    
    public AddPersonPayload(Person person) : base(person)
    {
    }

    public AddPersonPayload(IReadOnlyList<string> errors) : base(errors)
    {
    }
}

public abstract class PersonPayloadBase : Payload
{
    protected PersonPayloadBase(Person person)
    {
        Person = person;
    }

    protected PersonPayloadBase(IReadOnlyList<string> errors) : base(errors)
    {
        
    }

    public Person? Person { get; }
}