using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PracticalAssignment.ApiContracts;
using PracticalAssignment.Services.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace PracticalAssignment.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<JsonOptions> _jsonOptions;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            IOptions<JsonOptions> jsonOptions)
        {
            _next = next;
            _jsonOptions = jsonOptions;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainServiceException ex)
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var json = JsonSerializer.Serialize(new BadRequestResponse
                {
                    Message = ex.Message
                }, _jsonOptions.Value.JsonSerializerOptions);
                await context.Response.WriteAsync(json);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // TODO: Maybe implement logging
            }
        }
    }
}
