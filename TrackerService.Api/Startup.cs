using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrackerService.Data;
using TrackerService.Data.Contracts;

namespace TrackerService.Api
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("default", policy => policy.RequireAuthenticatedUser());
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(option =>
                {
                    option.Authority = string.Format(Configuration["ADSettings:ADInstance"], Configuration["ADSettings:TenantId"]);
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            Configuration["ADSettings:AppIdUri"],
                            Configuration["ADSettings:ClientId"]
                        }
                    };

                });
            services.AddTransient<IRepositoryFactory>(provider => new RepositoryFactory(Configuration.GetConnectionString("SimpleTaxDB")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
