using System.Security.Claims;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        // if (!"/api/auth".Equals(requestData?.Url.LocalPath, StringComparison.OrdinalIgnoreCase))
        // {
        if (!requestData?.Headers.Contains("Authorization") ?? false)
            throw new UnauthenticatedException();

        var authorization = requestData?.Headers.GetValues("Authorization").First();

        if (authorization is null || !authorization.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            throw new UnauthenticatedException();

        var token = authorization.Substring("Bearer ".Length).Trim();
        
        #pragma warning disable 0618

        var claims = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret("hbfskbghisbgsbgdsijgerginergikxbghkbvkfbdrfibgrgierhgeruogr")
            .MustVerifySignature().Decode<IDictionary<string, string>>(token).Select(s => new Claim(s.Key, s.Value));

        var id = claims.FirstOrDefault(c => c.Type == "id")?.Value ?? string.Empty;
        var account = claims.FirstOrDefault(c => c.Type == "account")?.Value ?? string.Empty;
        var name = claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;
        var email = claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
        var permissions = claims.FirstOrDefault(c => c.Type == "permissions")?.Value.Split(";") ?? Enumerable.Empty<string>();
        var currentUser = new CurrentUserFeature(Guid.Parse(id), int.Parse(account), name, email, permissions);

        context.Features.Set<CurrentUserFeature>(currentUser);
        // }
        
        await next(context);
    }
}