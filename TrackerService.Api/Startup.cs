using System.Diagnostics;
using AutoMapper;
using Microsoft.ApplicationInsights.Extensibility;
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
using TrackerService.Api.Infrastructure.HealthChecks;
using TrackerService.Api.Infrastructure.Middleware;
using TrackerService.Common.Configuration;
using TrackerService.Core;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.Repositories;
using TrackerService.Core.Tasks.TaskCreation;
using TrackerService.Core.Tasks.TaskRetrieval;
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
            services.AddApplicationInsights(Configuration, environment);
            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter(authPolicyBuilder)))
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDistributedCache(environment, Configuration);
            services.AddApiHealthCheck();

            var userManagementConfig = Configuration.GetUserManagementOptions();
            services.AddSingleton(authConfig);
            services.AddSingleton(userManagementConfig);
            services.AddHttpContextAccessor();
            services.AddTransient<IUserContext, ApiUserContext>();
            services.AddTransient<IServiceContext, ApiServiceContext>();
            services.AddHttpClients(authConfig, userManagementConfig);

            services.AddRepositoryFactory(Configuration);
            services.AddTransient<IDataAccessConfiguration, DataAccessConfiguration>();
            services.AddTransient<ITaskRepository, SqlTaskRepository>();
            services.AddTransient<ITaskCreator, TaskCreator>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserAuthenticator, UserAuthenticator>();
            services.AddTransient<ITaskRetrievalRepository, SqlTaskRetrievalRepository>();
            services.AddTransient<ITaskRetriever, TaskRetriever>();
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

            app.UseApiHealthCheck();

            DisableApplicationInsightsOnDebug();

            app.UseMiddleware<SerilogMiddleware>();
            app.UseMiddleware<RequestIdGenerationMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ApplicationVersionMiddleware>();
            app.UseAuthentication();
            app.UseMvc();
        }

        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}
