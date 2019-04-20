using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TrackerService.Api.Infrastructure;
using TrackerService.Api.Infrastructure.Authentication;
using TrackerService.Api.Infrastructure.Middleware;
using TrackerService.Common.Configuration;
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
            services.AddAutoMapper(typeof(Startup));
            var authConfig = Configuration.GetAuthenticationOptions();
            services.AddAuthentication(authConfig);

            var authPolicyBuilder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter(authPolicyBuilder)))
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var userManagementConfig = Configuration.GetUserManagementOptions();
            services.AddSingleton(authConfig);
            services.AddSingleton(userManagementConfig);
            services.AddHttpContextAccessor();
            services.AddTransient<IUserContext, ApiUserContext>();
            services.AddHttpClients(authConfig, userManagementConfig);

            var storageConn = new StorageConnectionInfo(Configuration.GetConnectionString("CloudStorage"), Configuration["StorageConnection:ContainerName"]);
            var serviceProvider = services.BuildServiceProvider();
            var userContext = serviceProvider.GetService<IUserContext>();
            services.AddTransient<IRepositoryFactory>(provider => new RepositoryFactory(Configuration.GetConnectionString("SimpleTaxDB"), storageConn, userContext));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserAuthenticator, UserAuthenticator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.UseMiddleware<SerilogMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ApplicationVersionMiddleware>();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
