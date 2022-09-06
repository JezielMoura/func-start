public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {        
        try
        {
            await next(context);
        }
        catch(UnauthenticatedException)
        {
            context.GetInvocationResult().Value = await CreateResponse(context, HttpStatusCode.Unauthorized);
        }
        catch(UnauthorizedAccessException)
        {
            context.GetInvocationResult().Value = await CreateResponse(context, HttpStatusCode.Forbidden);
        }
        catch (AggregateException exception)
        {
            context.GetInvocationResult().Value = await CreateResponse(context, HttpStatusCode.BadRequest, exception);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<HttpResponseData?> CreateResponse(FunctionContext context, HttpStatusCode statusCode, Exception? exception = null)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData?.CreateResponse(statusCode);

        if (exception?.InnerException is ValidationException validationException && response is not null)
            await response.WriteAsJsonAsync(validationException?.Errors, statusCode);

        return response;
    }
}