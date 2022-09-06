using Microsoft.Extensions.Hosting;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        builder.UseWhen<AuthenticationMiddleware>(AllowAuthFunction);
        builder.UseWhen<AuthorizationMiddleware>(AllowAuthFunction);
    })
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ITokenService, TokenService>();
    })
    .Build();

await host.RunAsync();

bool AllowAuthFunction(FunctionContext context)
{
    if (context.FunctionDefinition.Name.Equals("auth", StringComparison.OrdinalIgnoreCase))
        return false;
    
    if (context.FunctionDefinition.Name.Contains("swagger", StringComparison.OrdinalIgnoreCase))
        return false;

    return true;
}
        