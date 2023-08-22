using System.Collections.Generic;
using HotChocolate.Data;
using HotChocolate.Types;

namespace GraphQL;

public class Company
{
    public int Id { get; }
    public string Name { get; }
    public int Employees { get; }

    public Company(
        int id,
        string name,
        int employees)
    {
        Id = id;
        Name = name;
        Employees = employees;
    }
}

public class CompanyType : ObjectType<Company>
{
    protected override void Configure(IObjectTypeDescriptor<Company> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(x => x.Id);
    }
}

[ExtendObjectType("Query")]
public class CompanyQueries
{
    private readonly Company[] _companies =
        { 
            new Company(1, "Company1", 10000), 
            new Company(2, "Company2", 2000), 
            new Company(3, "Company3", 30000),
            new Company(22, "Company22", 220000)
        };


    //We need to think about dangers of exposing this IEnumerable to the outside world - limit execution time, result size, response size
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Company> GetCompanies => _companies;
}