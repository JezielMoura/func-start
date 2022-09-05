using Microsoft.Extensions.Hosting;
using FluentValidation;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseWhen<AuthenticationMiddleware>(AllowAuthFunction);
        builder.UseWhen<AuthorizationMiddleware>(AllowAuthFunction);
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    })
    .Build();

await host.RunAsync();

bool AllowAuthFunction(FunctionContext context) =>
    !context.FunctionDefinition.Name.Equals("auth", StringComparison.OrdinalIgnoreCase);