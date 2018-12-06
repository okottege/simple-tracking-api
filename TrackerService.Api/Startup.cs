using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using TrackerService.Api.Configuration;
using TrackerService.Api.Infrastructure;
using TrackerService.Api.Infrastructure.Authentication;
using TrackerService.Api.Infrastructure.Authentication.Models;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Api.Infrastructure.Filters;
using TrackerService.Api.Infrastructure.Middleware;
using TrackerService.Common.Contracts;
using TrackerService.Data;
using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (environment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("DevPolicy", builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
                });
            }

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(option =>
                {
                    option.Authority = Configuration["Authentication:Authority"];
                    option.Audience = Configuration["Authentication:Audience"];
                    option.SaveToken = true;
                });
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserContext, ApiUserContext>();
            services.AddAutoMapper();

            var storageConn = new StorageConnectionInfo(Configuration.GetConnectionString("CloudStorage"), Configuration["StorageConnection:ContainerName"]);
            var serviceProvider = services.BuildServiceProvider();
            var userContext = serviceProvider.GetService<IUserContext>();
            services.AddTransient<IRepositoryFactory>(provider => new RepositoryFactory(Configuration.GetConnectionString("SimpleTaxDB"), storageConn, userContext));

            var userManagementConfig = new ServiceAuthenticationConfiguration
            {
                ClientId = Configuration["UserManagement:ClientID"],
                ClientSecret = Configuration["UserManagement:ClientSecret"],
                GrantType = Configuration["UserManagement:GrantType"],
                Audience = Configuration["UserManagement:Audience"],
                AuthBaseUrl = Configuration["Authentication:Authority"]
            };
            services.AddSingleton(userManagementConfig);
            services.AddTransient<IServiceAuthenticator, ServiceToServiceAuthenticator>();

            var userRepoConfig = new UserRepositoryConfig
            {
                ConnectionName = Configuration["UserManagement:ConnectionID"],
                UserManagementBaseUrl = Configuration["UserManagement:BaseUrl"]
            };
            services.AddTransient<IUserRepository>(sp => new UserRepository(sp.GetService<IHttpClientFactory>(), userRepoConfig));
            services.AddScoped<RequireServiceToken>();

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseApplicationVersionMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevPolicy");
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
