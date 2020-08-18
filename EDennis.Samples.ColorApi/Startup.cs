using EDennis.NetStandard.Base;
using EDennis.Samples.ColorApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EDennis.Samples.ColorApi {
    public class Startup {
        public Startup(IConfiguration configuration, IHostEnvironment env) {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment {get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithDefaultPolicies(Configuration,HostEnvironment,"Security:DefaultPolicies:ClaimTypes");
            
            //System.Diagnostics.Debugger.Launch();

            var cxnString = Configuration["ConnectionStrings:ColorContext"];
            services.AddScoped<DbContextProvider<ColorContext>>();
            services.AddDbContext<ColorContext>(options => {
                options.UseSqlServer(cxnString);
                options.EnableSensitiveDataLogging();
            });

            services.AddSingleton(new TransactionCache<ColorContext>());

            services.AddMockUser(Configuration);
            services.AddHeaderToClaims(Configuration);
            services.AddCachedTransaction(Configuration);
            services.AddHttpLogging(Configuration);
            services.AddScopedRequestMessage(Configuration);


            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ColorApi", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMockUserFor("/api/Rgb");
            app.UseHeaderToClaimsFor("/api/Rgb");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCachedTransactionFor<ColorContext>("/api/Rgb");
            app.UseHttpLoggingFor("/api/Rgb");
            app.UseScopedRequestMessageFor("/api/Rgb");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
            });


        }
    }
}
