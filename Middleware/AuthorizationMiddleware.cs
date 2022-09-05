using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        // if (!"/api/auth".Equals(requestData?.Url.LocalPath, StringComparison.OrdinalIgnoreCase))
        // {
        var currentUser = context.Features.Get<CurrentUserFeature>();

        if (currentUser is null)
            throw new UnauthorizedAccessException();

        var functionName = context.FunctionDefinition.Name;

        if (!currentUser.Permissions.Contains(functionName))
            throw new UnauthorizedAccessException();
        // }

        await next(context);
    }
}