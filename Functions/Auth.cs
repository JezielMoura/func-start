using FluentValidation.Results;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

public class Auth
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;

    public Auth(ILoggerFactory loggerFactory, ITokenService tokenService)
    {
        _logger = loggerFactory.CreateLogger<Auth>();
        _tokenService = tokenService;
    }

    [Function("Auth")]
    [OpenApiRequestBody("application/json", typeof(UserInfo))]
    [OpenApiOperation(Summary = "E-mail and Password Authentication")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string))]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Forbidden")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString(_tokenService.Get());

        return response;
    }
}

public record UserInfo(string Email, string Password);