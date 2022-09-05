using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {        
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception.InnerException is ValidationException validation)
            {
                var httpReqData = await context.GetHttpRequestDataAsync();

                if (httpReqData != null)
                {
                    var newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.BadRequest);
                    var jsonResult = JsonSerializer.SerializeToUtf8Bytes(validation.Errors, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                    await newHttpResponse.WriteBytesAsync(jsonResult);
                    newHttpResponse.Headers.Add("Content-type", "application/json; charset=utf-8");
                    context.GetInvocationResult().Value = newHttpResponse;
                }
            }
            else
            {
                var httpReqData = await context.GetHttpRequestDataAsync();
                var response = httpReqData.CreateResponse();

                await response.WriteStringAsync(exception.Message);

                context.GetInvocationResult().Value = response;
            }
        }
    }
}