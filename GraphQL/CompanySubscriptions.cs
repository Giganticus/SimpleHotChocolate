using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL;

[ExtendObjectType(Name = "Subscription")]
public class CompanySubscriptions
{
    private readonly Company[] _companies =
    {
        new Company(1, "Company1", 10000), 
        new Company(2, "Company2", 2000), 
        new Company(3, "Company3", 30000)
    };

    [Subscribe]
    [Topic]
    public Task<Company>  OnCompanyAdded(
        [EventMessage] string companyName) => Task.FromResult(_companies.Single(x => x.Name.Equals(companyName)));
}