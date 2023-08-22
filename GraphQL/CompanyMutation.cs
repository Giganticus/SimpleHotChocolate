using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace GraphQL;

[ExtendObjectType("Mutation")]
public class CompanyMutation
{
    private readonly List<Company> _companies = new ();
    private int _count = 0;
    public async Task<CompanyPayload> AddCompany(
        AddCompanyInput input,
        [Service] ITopicEventSender eventSender)
    {
        if (_companies.Select(x => x.Name).Contains(input.Name))
        {
            return new CompanyPayload($"Company with name {input.Name} already in collection.");
        }
        
        _count++;
        var company = new Company(_count, input.Name, input.Employees);
        _companies.Add(company);

        await eventSender.SendAsync(
            nameof(CompanySubscriptions.OnCompanyAdded),
            company.Name);
        
        return new CompanyPayload(company);
    }
}

public class AddCompanyInput
{
    public string Name { get; }
    public int Employees { get; }

    public AddCompanyInput(string name, int employees)
    {
        Name = name;
        Employees = employees;
    }
}

public class CompanyPayload: CompanyPayloadBase
{
    public CompanyPayload(Company company) : base(company)
    {
    }

    public CompanyPayload(string error) : base(new []{error})
    {
    }
}

public abstract class CompanyPayloadBase : Payload
{
    protected CompanyPayloadBase(Company company)
    {
        Company = company;
    }
    
    protected CompanyPayloadBase(IReadOnlyList<string> errors) : base(errors)
    {}

    public Company? Company { get; }
}
