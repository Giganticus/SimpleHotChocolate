using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Language;
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
            nameof(Subscriptions.OnCompanyAdded),
            company.Name);
        
        return new CompanyPayload(company);
    }
}

public class AddCompanyInput
{
    [GraphQLType(typeof(StringLimitedToFiveCharactersType))]
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

public class StringLimitedToFiveCharactersType : ScalarType
{
    public StringLimitedToFiveCharactersType() : base("StringLimitedToFiveCharacters")
    {
        Description = "String that can only have maximum 5 characters";
    }

    public override Type RuntimeType { get; } = typeof(string);

    public override bool IsInstanceOfType(IValueNode valueSyntax)
    {
        if (valueSyntax == null)
        {
            throw new ArgumentNullException(nameof(valueSyntax));
        }

        return valueSyntax is StringValueNode stringValueNode && stringValueNode.Value.Length <= 5;

    }

    public override object? ParseLiteral(IValueNode valueSyntax)
    {
        if (valueSyntax is StringValueNode stringLiteral &&
            stringLiteral.Value.Length <= 5)
        {
            return stringLiteral.Value;
        }

        throw new SerializationException(
            "The string must be less than 5 characters long",
            this);
    }

    public override IValueNode ParseValue(object? runtimeValue)
    {
        if (runtimeValue is string s &&
            s.Length <= 5)
        {
            return new StringValueNode(null, s, false);
        }

        throw new SerializationException(
            "The string must be less than 5 characters long",
            this);
    }

    public override IValueNode ParseResult(object? resultValue)
    {
        if (resultValue is string s &&
            s.Length <= 5)
        {
            return new StringValueNode(null, s, false);
        }

        throw new SerializationException(
            "The string must be less than 5 characters long",
            this);
    }

    public override bool TrySerialize(object? runtimeValue, out object? resultValue)
    {
        resultValue = null;

        if (runtimeValue is string s &&
            s.Length <= 5)
        {
            resultValue = s;
            return true;
        }

        return false;
    }

    public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
    {
        runtimeValue = null;

        if (resultValue is string s &&
            s.Length <= 5)
        {
            runtimeValue = s;
            return true;
        }

        return false;
    }
}