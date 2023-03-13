using API.Services;
using API.Interfaces;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServiceExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();


            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("SqlConnection"));
            });

            return services;
        }
    }
}
