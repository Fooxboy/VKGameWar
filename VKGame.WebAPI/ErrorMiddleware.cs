using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKGame.WebAPI
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            var code = context.Response.StatusCode;
            if(code == 404)
            {
                await context.Response.WriteAsync(Bot.PublicAPI.Yarik.HttpErrors.E404);
            }else if(code == 500)
            {
                await context.Response.WriteAsync(Bot.PublicAPI.Yarik.HttpErrors.E500);

            }
        }
    }
}
