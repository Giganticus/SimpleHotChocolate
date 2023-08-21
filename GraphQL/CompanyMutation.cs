using System.Collections.Generic;
using HotChocolate.Types;

namespace GraphQL;

[ExtendObjectType("Mutation")]
public class CompanyMutation
{
    private readonly List<Company> _companies = new ();
    
    
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

public class CompanyPayloadBase