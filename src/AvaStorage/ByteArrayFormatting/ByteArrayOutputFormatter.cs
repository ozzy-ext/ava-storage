using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace AvaStorage.ByteArrayFormatting;

public class ByteArrayOutputFormatter : OutputFormatter
{
    public ByteArrayOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ByteArrayFormatters.JpegMediaType));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ByteArrayFormatters.PngMediaType));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ByteArrayFormatters.GifMediaType));
    }

    protected override bool CanWriteType(Type type)
    {
        return type == typeof(byte[]);
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var array = (byte[])context.Object;
        return context.HttpContext.Response.Body.WriteAsync(array, 0, array.Length);
    }
}