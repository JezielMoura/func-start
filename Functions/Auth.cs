using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace func_start
{
    public class Auth
    {
        private readonly ILogger _logger;

        public Auth(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Auth>();
        }

        [Function("Auth")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var token = new TokenService().GetAsync();

            response.WriteString(token);

            return response;
        }
    }
}
