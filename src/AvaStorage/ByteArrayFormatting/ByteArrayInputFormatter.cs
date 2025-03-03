﻿using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace AvaStorage.ByteArrayFormatting;

public class ByteArrayInputFormatter : InputFormatter
{
    public ByteArrayInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ByteArrayFormatters.MediaType));
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(byte[]);
    }

    public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var stream = new MemoryStream();
        context.HttpContext.Request.Body.CopyToAsync(stream);
        return InputFormatterResult.SuccessAsync(stream.ToArray());
    }
}