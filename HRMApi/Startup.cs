using DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using Utility.StaticData;

namespace HRMApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var origins = Configuration[StaticData.ORIGINS].Split(';').ToArray();
            //add cors policy
            services.AddCors(options => options.AddPolicy(StaticData.CORS_POLICY,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(origins);
                }));
            //dependency injection
            services.RegisterHRMServiceInstance(Configuration);

            services.AddControllers();
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
            //register JWT authentication scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = Configuration[StaticData.SERVICE_BASE],
                      ValidAudience = Configuration[StaticData.ORIGINS],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[StaticData.JWT_KEY])),
                      RequireExpirationTime = true,
                      RequireSignedTokens = true,
                      ClockSkew = TimeSpan.Zero

                  };
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Migrations at runtime
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<HRMContext>().Database.Migrate();
                
            }
            if (env.IsDevelopment())
            {
                //Developer Exception Page Middleware reports app runtime errors.
                app.UseDeveloperExceptionPage();
                //Database Error Page Middleware reports database runtime errors.
                app.UseDatabaseErrorPage();
            }
            else
            {
                //Exception Handler Middleware catches exceptions thrown in the following middlewares.
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                    {
                        var error = context.Features;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { success = false, error = error }));
                    });
                });
                //HTTP Strict Transport Security Protocol (HSTS) Middleware adds the Strict-Transport-Security header.
                app.UseHsts();
            }
           // HTTPS Redirection Middleware to redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();
            //Static File Middleware returns static files and short-circuits further request processing. 
            app.UseStaticFiles();
            app.UseRouting();
            //Apply cors policy
            app.UseCors(StaticData.CORS_POLICY);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async context =>
            {
                if (context.Request.Path.Value == "/")
                {
                    string ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
                    await context.Response.WriteAsync($"{{status :'HRM API is Running.'}} Version: {ver}. Environment: {env.EnvironmentName}");
                }
                else
                {
                    await context.Response.WriteAsync("{ status :'404 Method Not Found'}");
                }
            });
        }
    }
}
