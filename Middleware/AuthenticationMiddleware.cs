public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ITokenService _tokenService;

    public AuthenticationMiddleware(ITokenService tokenService) =>
        _tokenService = tokenService;

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        if (!requestData?.Headers.Contains("Authorization") ?? false)
            throw new UnauthenticatedException();

        var authorization = requestData?.Headers.GetValues("Authorization").First();

        if (authorization is null || !authorization.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            throw new UnauthenticatedException();

        var token = authorization.Substring("Bearer ".Length).Trim();

        try
        {
            context.Features.Set<CurrentUserFeature>(_tokenService.GetCurrentUser(token));
        }
        catch (Exception)
        {
            throw new UnauthenticatedException();
        }

        await next(context);
    }
}