using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.Application.MappingProfile;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Infrastructure.Data.Repositories;
using ABO.ToDoApp.Infrastructure.Identity.Handlers;
using ABO.ToDoApp.Infrastructure.Identity.Models;
using ABO.ToDoApp.Infrastructure.Identity.Services;
using ABO.ToDoApp.Infrastructure.Logging;
using ABO.ToDoApp.Infrastructure.Services;
using ABO.ToDoApp.Shared.Identity.Models;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;


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

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(UserProfile).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void ConfigureAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUnitofwork, Unitofwork>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (configuration["JwtSettings:Key"]!))
            };
        });

        services.AddScoped(sp =>
        {
            var httpContext = sp.GetService<IHttpContextAccessor>()!.HttpContext;

            var identityOptions = new IdentityConfig();

            if (httpContext!.User.Identity!.IsAuthenticated)
            {
                identityOptions.UserId = httpContext.User.FindFirstValue("uid") ?? "";
            }

            return identityOptions;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("OwnerOfTodoListPolicy", policy =>
            {
                policy.Requirements.Add(new OwnerOfTodoListRequirement());
            });
        });

        services.AddTransient<IAuthorizationHandler, OwnerOfTodoListHandler>();
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });
    }

    public static void ConfigureLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }

    public static void ConfigureTimeProvider(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TodoApp API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }});
        });
    }
}