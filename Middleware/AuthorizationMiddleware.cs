public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        var currentUser = context.Features.Get<CurrentUserFeature>();

        if (currentUser is null)
            throw new UnauthorizedAccessException();

        var functionName = context.FunctionDefinition.Name;

        if (!currentUser.Permissions.Contains(functionName))
            throw new UnauthorizedAccessException();

        await next(context);
    }
}