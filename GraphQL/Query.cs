using HotChocolate.Types;

namespace GraphQL;

public class Query
{
    public string GetHelloWorld(string sayHelloTo)
    {
        return $"Hello {sayHelloTo}";
    }
}

[ExtendObjectType("Mutation")]
public class QueryOrMutation
{
    public string  GetAnotherHelloWorld(string sayHelloTo)
    {
        return $"Hello {sayHelloTo}";
    }
}