using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.Types;

namespace GraphQL;

[ExtendObjectType("Query")] 
public class PersonQueries
{
    private readonly Person[] _people = {
        new Person(Guid.NewGuid(), "Person1", 25),
        new Person(Guid.NewGuid(), "Person2", 30),
        new Person(Guid.NewGuid(), "Person3", 40)
    };

    public IEnumerable<Person> GetPeople()
    {
        return _people;
    }
    
    public Person? GetSinglePersonByName(string personName)
    {
        return _people.SingleOrDefault(x => x.Name.Equals(personName));
    }

    public async Task<Person> GetSinglePersonByNameUsingDataLoader(
        string personName,
        PersonByNameDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(personName, CancellationToken.None);
    }

    public async Task<IEnumerable<Person>> GetPeopleByName(
        IReadOnlyCollection<string> names,
        PersonByNameDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(names);
    }
}

public class PersonByNameDataLoader : BatchDataLoader<string, Person>
{
    private readonly Person[] _people = {
        new Person(Guid.NewGuid(), "Person1", 25),
        new Person(Guid.NewGuid(), "Person2", 30),
        new Person(Guid.NewGuid(), "Person3", 40)
    };
    
    public PersonByNameDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
    }

    //This method is abstract in base class and so needs to be implemented.
    protected override Task<IReadOnlyDictionary<string, Person>> LoadBatchAsync(
        IReadOnlyList<string> keys, 
        CancellationToken cancellationToken)
    {
        var peopleToReturn = _people.Where(p => keys.Contains(p.Name)).ToList();
        var dictionary = peopleToReturn.ToDictionary(p => p.Name);
        var readonlyDictionary = new ReadOnlyDictionary<string, Person>(dictionary);
        return Task.FromResult((IReadOnlyDictionary<string, Person>)readonlyDictionary); 
    }
}  