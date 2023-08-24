using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL;

public class Country
{
    public int Id { get; }
    public string Name { get; }
    public decimal Gdp { get; }

    public IEnumerable<City> Cities { get; } = new List<City>();

    public Country(
        int id,
        string name, 
        decimal gdp)
    {
        Id = id;
        Name = name;
        Gdp = gdp;
        
    }

    public Country(
        int id,
        string name, 
        decimal gdp,
        IEnumerable<City> cities): this(id, name, gdp)
    {
        Cities = cities;
    }
}

public class City
{
    public int Id { get; }
    public string Name { get; }

    public City(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }
}

[ExtendObjectType("Query")]
public class CountryQueries
{
    private readonly Country[] _countries = new[]
        { new Country(1, "Country1", 1000), new Country(2, "Country2", 20000), new Country(3, "Country3", 30000) };

    [UsePaging]
    public IQueryable<Country> GetCountries() => _countries.AsQueryable();
}

public class CountryType : ObjectType<Country>
{
    protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
    {
        descriptor
            .Field(t => t.Cities)
            .ResolveWith<CountryResolvers>(r => r.GetCities(default!));
    }
}

public class CountryResolvers
{
    public IEnumerable<City> GetCities(
        [Parent] Country country)
    {
        return new[] { new City(1, "City1"), new City(2, "City2"), new City(3, "City3") };
    }
}