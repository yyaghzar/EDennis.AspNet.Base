﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDennis.NetStandard.Base {

    /// <summary>
    /// Configures "Default Policies" -- policies that require values of one or more 
    /// claim types to wildcard match one of the defined acceptable patterns.  
    /// <see cref="DefaultAuthorizationPolicyConvention"/>
    /// <see cref="ClaimPatternAuthorizationHandler"/>
    /// </summary>
    public static class IServiceCollectionExtensions_DefaultPolicies {

        /// <summary>
        /// Configures "Default Policies" -- policies that require values of one or more 
        /// claim types to wildcard match one of the defined acceptable patterns.  
        /// The defined acceptable patterns are generated by the 
        /// DefaultAuthorizationPolicyConvention. The matching algorithm (which caches 
        /// matching patterns for performance) is provided in the 
        /// ClamPatternAuthorizationHandler class.
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="config">IConfiguration instance</param>
        /// <param name="env">IHostEnvironment instance</param>
        /// <param name="defaultPoliciesKey">Configuration key that will hold the generated Default Policies</param>
        /// <param name="defaultPoliciesClaimTypesKey">Configuration key that holds the claim types 
        ///   (e.g., scope, user_scope, client_scope) used for policy evaluation.  Note: all listed claim types
        ///   must each match the policy pattern.  If you provide user_scope and client_scope, values from
        ///   each of these claim types must match the policy pattern.</param>
        /// <returns>An IMvcCoreBuilder, which can be used to add other conventions or perform other configurations.</returns>
        /// <see cref="DefaultAuthorizationPolicyConvention"/>
        /// <see cref="ClaimPatternAuthorizationHandler"/>
        public static IMvcCoreBuilder AddControllersWithDefaultPolicies(this IServiceCollection services,
            IConfiguration config, IHostEnvironment env,
            string defaultPoliciesClaimTypesKey = "Security:DefaultPolicies:ClaimTypes",
            string defaultPoliciesKey = "Security:DefaultPolicies:Policies"
            ) {
            var builder = services.AddMvcCore(options=> {
                options.Conventions.Add(new DefaultAuthorizationPolicyConvention(env.ApplicationName, config, defaultPoliciesKey));
            });

            services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    config, logger, defaultPoliciesKey, defaultPoliciesClaimTypesKey);
            });

            return builder;
        }
    }
}
