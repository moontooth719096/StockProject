using Microsoft.AspNetCore.Http;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        Stopwatch stopwatch;


        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();

            long ends = stopwatch.ElapsedMilliseconds;

        }
    }
}
