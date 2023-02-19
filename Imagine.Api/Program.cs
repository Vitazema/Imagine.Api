using Imagine.Api.Configuration;
using Imagine.Api.Extensions;
using Imagine.Api.Helpers;
using Imagine.Api.Middleware;
using Imagine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ArtDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging());

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();

// To get application settings
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
builder.WebHost.UseUrls(settings.AppUrl);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
try
{
    var context = scope.ServiceProvider.GetRequiredService<ArtDbContext>();
    await context.Database.MigrateAsync();
    await ArtDbContextSeed.SeedAsync(context, loggerFactory);
}
catch (Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occured during migration");
}

app.UseSwaggerDocumentation();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// Add endpoints for error handling. Redirects to errors controller.
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();