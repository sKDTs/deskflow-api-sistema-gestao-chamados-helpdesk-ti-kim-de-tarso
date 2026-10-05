using System.Text.Json;

namespace DeskFlow.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status404NotFound,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro interno no servidor.");
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = new
        {
            mensagem = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
