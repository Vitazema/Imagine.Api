using HealthChecks.UI.Client;
using Imagine.Api.Extensions;
using Imagine.Api.Middleware;
using Imagine.Infrastructure.Data;
using Imagine.Infrastructure.Data.AutoMapper;
using Imagine.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ArtDbContext>(x => x
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging());

builder.Services.AddCustomAutoMapper();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException());
builder.Services.AddApplicationServices();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true) // allow any origin
    .AllowCredentials()
));

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.DbInitialize();

if (app.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerDocumentation();
    // app.UseDeveloperExceptionPage();
}
// else
// {
// app.UseExceptionHandler("/Error");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
// app.UseHsts();
// }

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

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();

app.Run();
