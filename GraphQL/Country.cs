using System.Linq;
using HotChocolate.Types;

namespace GraphQL;

public class Country
{
    public int Id { get; }
    public string Name { get; }
    public decimal Gdp { get; }

    public Country(
        int id,
        string name, 
        decimal gdp)
    {
        Id = id;
        Name = name;
        Gdp = gdp;
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