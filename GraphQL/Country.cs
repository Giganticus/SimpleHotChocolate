using System.Linq;
using System.Threading.Tasks;
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

public class CountryType : ObjectType<Country>
{
    private readonly Country[] _countries = new[]
        { new Country(1, "Country1", 1000), new Country(2, "Country2", 20000), new Country(3, "Country3", 30000) };
    
    protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(x => x.Id)
            .ResolveNode((ctx, id) => Task.FromResult(_countries.SingleOrDefault(x => x.Id.Equals(id))));

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