public static class HttpRequestDataExtensions
{
    public static async Task<HttpResponseData> JsonResult<T>(this HttpRequestData req, T result)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync<T>(result);

        return response;
    }
}