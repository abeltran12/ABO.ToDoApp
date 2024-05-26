using ABO.ToDoApp.DIC;
using ABO.ToDoApp.DIC.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureLogging();
builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddApplicationDependencies();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureTimeProvider();
builder.Services.ConfigureAuthService(builder.Configuration);



builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(opt => { });
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
});

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();