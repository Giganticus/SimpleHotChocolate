using System.IO;
using GraphQL;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddTypeExtension<PersonQueries>()
    .AddTypeExtension<CountryQueries>()
    .AddTypeExtension<CompanyQueries>()
    .AddMutationType<PersonMutations>(b =>
    {
        b.Name("Mutation");
        b.Description("You can add notation to the schema using code");
    })
    .AddTypeExtension<CompanyMutation>()
    .AddTypeExtension<QueryOrMutation>()
    .AddSubscriptionType<Subscriptions>()
    .AddType<CompanyType>()
    .AddType<PersonType>()
    .AddFiltering()
    .AddSorting()
    .AddGlobalObjectIdentification()    
    .AddInMemorySubscriptions();

var app = builder.Build();

var executor = await app.Services.GetRequestExecutorAsync();
await File.WriteAllTextAsync("schema.graphql", executor.Schema.ToString());

app.UseWebSockets();
app.UseRouting();
app.MapGraphQL();
app.MapGraphQLVoyager();

app.UseDeveloperExceptionPage();
app.MapGet("/", () => "Hello World!");


app.Run();