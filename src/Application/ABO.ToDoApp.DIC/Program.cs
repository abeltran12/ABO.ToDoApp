using ABO.ToDoApp.DIC;
using ABO.ToDoApp.DIC.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//codigo viejo que funciona
//builder.Host.UseSerilog((context, loggerConfig) => 
//    loggerConfig.ReadFrom.Configuration(context.Configuration));

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.AzureApp()
            .CreateLogger();

builder.Host.UseSerilog();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(); 
builder.Logging.AddAzureWebAppDiagnostics();

//Hasta aca la configuracion

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