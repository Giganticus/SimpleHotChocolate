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
        b.Description("People Mutations");
    })
    .AddTypeExtension<CompanyMutation>()
    .AddSubscriptionType(x => x.Name("Subscription"))
    .AddTypeExtension<CompanySubscriptions>()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();

var app = builder.Build();
app.UseWebSockets();
app.UseRouting();
app.MapGraphQL();
app.UseDeveloperExceptionPage();
app.MapGet("/", () => "Hello World!");

app.Run();