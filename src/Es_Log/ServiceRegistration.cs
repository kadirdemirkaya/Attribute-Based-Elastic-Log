using Es_Log.Extensions;
using Es_Log.Models.Entities;
using Es_Log.Options;
using Es_Log.Providers;
using Es_Log.Services;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Es_Log
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Es_LogCoreServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));

            services.AddSingleton<ElasticClientProvider>();

            var sqlServerOptions = services.GetOptions<SqlServerOptions>("SqlServerOptions");

            services.AddDbContext<ESLogDbContext>(options =>
            {
                options.UseSqlServer(
                    sqlServerOptions.SqlConnection,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: int.Parse(sqlServerOptions.RetryCount),
                            maxRetryDelay: TimeSpan.FromSeconds(int.Parse(sqlServerOptions.RetryDelay)),
                            errorNumbersToAdd: null);
                    });
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            }).AddEntityFrameworkStores<ESLogDbContext>()
            .AddDefaultTokenProviders();

            var jwtOptions = services.GetOptions<JwtOptions>("JwtOptions");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidAudience = jwtOptions.Audience,
                       ValidIssuer = jwtOptions.Issuer,
                       NameClaimType = ClaimTypes.Email,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                   };
               });

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IEndpointService, EndpointService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUserServive, UserService>();

            services.AddScoped<IRoleService, RoleService>();

            var sp = services.BuildServiceProvider();
            var context = sp.GetRequiredService<ESLogDbContext>();
            ESLogSeedContext.SeedAsync(context).GetAwaiter().GetResult();

            return services;
        }
    }
}
