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
builder.Services.AddCors(opt => opt.AddDefaultPolicy(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true) // allow any origin
            .AllowCredentials()
));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

if (app.Environment.IsDevelopment())
{
    await ArtDbContextSeed.SeedAsync(services);
}
else
{
    await using var context = services.GetRequiredService<ArtDbContext>();
    await context.Database.MigrateAsync();
}

app.UseSwaggerDocumentation();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// Add endpoints for error handling. Redirects to errors controller.
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseCors();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
