using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKGame.WebAPI
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            var auth = new Bot.PublicAPI.Yarik.Auth();
            if (auth.checkToken(token))
            {
                await _next.Invoke(context);

            }else
            {
                await context.Response.WriteAsync(auth.NoAccess);
            }
        }
    }
}
