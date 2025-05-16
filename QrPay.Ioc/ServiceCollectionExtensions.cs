using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QrPay.Database;
using QrPay.Database.Repository;
using QrPay.Domain.Repository;
using QrPay.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Services;
using System.Reflection;
using QrPay.Application;

namespace QrPay.Ioc
{
    public static class ServiceCollectionExtensions
    {
        private static Assembly ApplicationAssembly => typeof(ApplicationAssembly).Assembly;

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediaTR();
            services.AddDatabase(configuration);
            services.AddAuthentication(configuration);
            services.AddQrCode();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbConfig>(config => configuration.GetRequiredSection(nameof(DbConfig)).Bind(config));

            services.AddDbContext<QrPayDbContext>((serviceProvider, options) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<DbConfig>>().Value;

                var connectionString = config.ConnectionString;

                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfig>(config => configuration.GetRequiredSection(nameof(TokenConfig)).Bind(config));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                var key = configuration["TokenConfig:Key"];
                var asciiKey = Encoding.ASCII.GetBytes(key);

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(asciiKey),
                    ValidateIssuer = false
                };
            });

            services.AddTransient<ITokenService, TokenService>();

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            return services;
        }

        public static IServiceCollection AddQrCode(this IServiceCollection services)
        {
            services.AddTransient<IQrCodeService, QrCodeService>();

            return services;
        }

        public static IServiceCollection AddMediaTR(this IServiceCollection services)
        {
            return services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly(), ApplicationAssembly]));
        }
    }
}
