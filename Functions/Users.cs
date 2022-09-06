using FluentValidation.Results;
using MediatR;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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

    [Function("ReadUser")]
    [OpenApiOperation(Summary = "Get user informations")]
    [OpenApiParameter("GUID", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "User Id")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(UserInfo))]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Forbidden")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
    public async Task<HttpResponseData> ReadUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user")] HttpRequestData req)
    {
        var addUserCommand = await req.ReadFromJsonAsync<AddUserCommand?>();
        var commandResult = await _mediatr.Send(addUserCommand ?? throw new ArgumentNullException());

        return await req.JsonResult<bool>(commandResult);
    }

    [Function("AddUser")]
    [OpenApiOperation(Summary = "Add new user")]
    [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "User added")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Forbidden")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
    public async Task<HttpResponseData> AddUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequestData req)
    {
        var addUserCommand = await req.ReadFromJsonAsync<AddUserCommand?>();
        var commandResult = await _mediatr.Send(addUserCommand ?? throw new ArgumentNullException());

        return await req.JsonResult<bool>(commandResult);
    }

    [Function("EditUser")]
    [OpenApiOperation(Summary = "Edit user informations")]
    [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "User edited")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Forbidden")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
    public async Task<HttpResponseData> EditUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user")] HttpRequestData req)
    {
        var addUserCommand = await req.ReadFromJsonAsync<AddUserCommand>();
        var commandResult = await _mediatr.Send(addUserCommand ?? throw new ArgumentNullException());
        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync<bool>(commandResult);

        return response;
    }

    [Function("TestDeleteUser")]
    [OpenApiOperation(Summary = "Delete user")]
    [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "User deleted")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Forbidden")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
    public async Task<HttpResponseData> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user")] HttpRequestData req)
    {
        var addUserCommand = await req.ReadFromJsonAsync<AddUserCommand?>();
        var commandResult = await _mediatr.Send(addUserCommand ?? throw new ArgumentNullException());

        return await req.JsonResult<bool>(commandResult);
    }
}
