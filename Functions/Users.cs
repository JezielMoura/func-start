using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

 namespace Mobnet.Trip.Application.Functions;

public class UserFunctions
{
    private readonly ILogger _logger;
    private readonly IMediator _mediatr;

    public UserFunctions(ILoggerFactory loggerFactory, IMediator mediatr)
    {
        _logger = loggerFactory.CreateLogger<UserFunctions>();
        _mediatr = mediatr;
    }

    [Function("user")]
    public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        var addUserCommand = await req.ReadFromJsonAsync<AddUserCommand>();
        var commandResult = await _mediatr.Send(addUserCommand);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString(commandResult.ToString());

        return response;
    }
}