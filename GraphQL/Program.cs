using GraphQL;
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
    .AddSubscriptionType(x => x.Name("Subscription"))
    .AddTypeExtension<CompanySubscriptions>()
    .AddType<CompanyType>()
    .AddType<CountryType>()
    .AddFiltering()
    .AddSorting()
    .AddGlobalObjectIdentification()    
    .AddInMemorySubscriptions();

var app = builder.Build();
app.UseWebSockets();
app.UseRouting();
app.MapGraphQL();
app.MapGraphQLVoyager();

app.UseDeveloperExceptionPage();
app.MapGet("/", () => "Hello World!");


app.Run();