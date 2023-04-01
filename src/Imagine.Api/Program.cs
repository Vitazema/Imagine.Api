using Imagine.Api.Extensions;
using Imagine.Api.Helpers;
using Imagine.Api.Middleware;
using Imagine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ArtDbContext>(x => x
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging());

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddHealthChecks();
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true) // allow any origin
    .AllowCredentials()
));

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    await ArtDbContextSeed.SeedAsync(services);
}
else
{
    await using var context = services.GetRequiredService<ArtDbContext>();
    await context.Database.MigrateAsync();
    // app.UseExceptionHandler("/Error");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

// # If the app calls UseStaticFiles, place UseStaticFiles before UseRouting.
app.UseStaticFiles();

// If the app uses CORS scenarios, such as [EnableCors], place the call to UseCors before any other middleware that use CORS (for example, place UseCors before UseAuthentication, UseAuthorization, and UseEndpoints).
app.UseCors();

// Calling UseAuthentication and UseAuthorization adds the authentication and authorization middleware. These middleware are placed between UseRouting and UseEndpoints so that they can:
//
// See which endpoint was selected by UseRouting.
//     Apply an authorization policy before UseEndpoints dispatches to the endpoint.
app.UseAuthorization();
// app.UseAuthentication();

app.UseSwaggerDocumentation();


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// Add endpoints for error handling. Redirects to errors controller.
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseStaticFiles();

app.UseAuthorization();

app.MapHealthChecks("/healthz");
app.MapControllers();

app.Run();
