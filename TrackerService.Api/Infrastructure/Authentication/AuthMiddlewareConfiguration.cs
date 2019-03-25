using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrackerService.Common;

namespace TrackerService.Api.Infrastructure.Authentication
{
    public static class AuthMiddlewareConfiguration
    {
        internal static void AddAuthentication(this IServiceCollection services, AuthenticationConfig config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.Authority = config.Authority;
                option.Audience = config.Audience;
                option.IncludeErrorDetails = false;
                option.SaveToken = true;

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = config.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = config.Authority,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
