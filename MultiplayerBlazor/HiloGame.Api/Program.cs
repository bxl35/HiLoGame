using HiloGame.Data;
using HiloGame.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateLogger();




var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IDateProvider, DateProvider>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();



builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("HiloGame.Api"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
            options.SetDbStatementForStoredProcedure = true;
        })
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5000";
        options.Audience = "hilogame.api";
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            NameClaimType = "name"  
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasm", policy =>
    {
        policy.WithOrigins("https://localhost:7045")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));





var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hilo Game Multiplayer API v1");
        options.RoutePrefix = "swagger";

        
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Hilo Game Multiplayer API v1")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors("AllowWasm");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    SetupDevelopment(app);
}


app.MapControllers();

app.Run();



static void SetupDevelopment(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.MigrateAndCreateData();
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("hilo-swaggerui");
        options.OAuthAppName("HiloGame API");
        options.OAuthUsePkce();
    });
}