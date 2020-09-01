﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base {

    /// <summary>
    /// Creates a claims principal with claims specified in Configuration.
    /// NOTE: multiple claims principals can be specified in Configuration.
    /// The middleware looks for the presence of a key: "mcp" to select
    /// a specific mock claims principal.  Typically, this configuration 
    /// would be passed in through command-line arguments (e.g., mcp=Maria)
    /// </summary>
    public class MockClaimsPrincipalMiddleware {

        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<MockClaimsPrincipalOptions> _options;
        private readonly string _mcp;

        public MockClaimsPrincipalMiddleware(RequestDelegate next,
            IOptionsMonitor<MockClaimsPrincipalOptions> options,
            IConfiguration config) {
            _next = next;
            _options = options;
            _mcp = config.GetValueOrThrow<string>(MockClaimsPrincipalOptions.SELECTED_MOCK_CLAIMS_PRINCIPAL_KEY);
        }

        public async Task InvokeAsync(HttpContext context) {

            //bypass if _mcp == null or X-Claims header is present.  
            //The latter indicates that regular authentication should proceed 
            //  and X-Claims will be added after authentication
            if (_mcp == null || context.Request.Headers.ContainsKey(HeaderToClaimsOptions.HEADER_KEY))
                await _next(context);
            else {
                var claims = _options.CurrentValue[_mcp].ToClaimEnumerable();
                context.User = new ClaimsPrincipal(new ClaimsIdentity(claims,"mockAuth"));
                 
                await _next(context); 
            }

        }

    }

    public static class IServiceCollectionExtensions_MockClaimsPrincipalMiddleware {
        public static IServiceCollection AddMockClaimsPrincipal(this IServiceCollection services, IConfiguration config) {
            services.Configure<MockClaimsPrincipalOptions>(config.GetSection("Security:MockClaimsPrincipals"));
            return services;
        }
    }

    public static class IApplicationBuilderExtensions_MockClaimsPrincipalMiddleware {
        public static IApplicationBuilder UseMockClaimsPrincipal(this IApplicationBuilder app) {
            app.UseMiddleware<MockClaimsPrincipalMiddleware>();
            return app;
        }


        public static IApplicationBuilder UseMockClaimsPrincipalFor(this IApplicationBuilder app,
            params string[] startsWithSegments) {
            app.UseWhen(context =>
            {
                foreach (var partialPath in startsWithSegments)
                    if (context.Request.Path.StartsWithSegments(partialPath))
                        return true;
                return false;
            },
                app => app.UseMockClaimsPrincipal()
            );
            return app;
        }

        public static IApplicationBuilder UseMockClaimsPrincipalWhen(this IApplicationBuilder app,
            Func<HttpContext, bool> predicate) {
            app.UseWhen(predicate, app => app.UseMockClaimsPrincipal());
            return app;
        }


    }

}


