using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //  Checks that the token has been signed by the issuer. VERY IMPORTANT
                    //  Without it, anyone can make up a random JWT token.
                    ValidateIssuerSigningKey = true,
                    //  Assigns the IssuerSigningKey the ["TokenKey"] we have in appsettings.json
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding
                    .UTF8.GetBytes(config["TokenKey"])),
                    //  Not yet implemented, so they're false.
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

    }
}