using ABO.ToDoApp.Application.MappingProfile;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ABO.ToDoApp.DIC;

public static class ServiceExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ToDoAppContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        },
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;

            options.User.RequireUniqueEmail = true;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            options.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<ToDoAppContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserProfile));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining(typeof(UserProfile)));
    }

    public static void ConfigureAuthService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}
