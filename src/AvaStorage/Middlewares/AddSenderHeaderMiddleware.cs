using System.Reflection;

namespace AvaStorage.Middlewares
{
    public class AddSenderHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _senderName;

        public AddSenderHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
            _senderName = 
                Assembly.GetAssembly(typeof(AddSenderHeaderMiddleware))?.GetName().Name
                ?? "undefined";
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Append("X-Sender-App", _senderName);

            await _next(httpContext);
        }
    }
}
