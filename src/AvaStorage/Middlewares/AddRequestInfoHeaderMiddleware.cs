namespace AvaStorage.Middlewares
{
    public class AddRequestInfoHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public AddRequestInfoHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string url = new UriBuilder
                (
                    httpContext.Request.Scheme,
                    httpContext.Request.Host.Host,
                    httpContext.Request.Host.Port.GetValueOrDefault(80),
                    httpContext.Request.Path
                ).Uri.ToString();

            string call = $"{httpContext.Request.Method} {url}";

            httpContext.Response.Headers.Append("X-Request", call);
            httpContext.Response.Headers.Append("X-Request-Length", httpContext.Request.ContentLength.ToString());

            await _next(httpContext);
        }
    }
}
