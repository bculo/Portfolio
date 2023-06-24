using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Interfaces;
using User.Application.Persistence;
using User.Application.Services;

namespace User.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("UserDb"));
                options.UseLowerCaseNamingConvention();
            });

            services.AddScoped<IRegisterUserService, RegisterUserService>();
        }
    }
}
