using System.Collections.Generic;

namespace GraphQL;

public abstract class Payload
{
    protected Payload(IReadOnlyList<string>? errors = null)
    {
        Errors = errors;
    }

    public IReadOnlyList<string>? Errors { get; }
}