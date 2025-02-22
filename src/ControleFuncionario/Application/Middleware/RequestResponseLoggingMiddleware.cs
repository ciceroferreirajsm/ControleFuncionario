using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext contextoHttp)
    {
        await _next(contextoHttp);
    }
}
