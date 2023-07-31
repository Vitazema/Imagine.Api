using HealthChecks.UI.Client;
using Imagine.Api.Extensions;
using Imagine.Api.Infrastructure.AutoMapper;
using Imagine.Api.Middleware;
using Imagine.Api.Mvc;
using Imagine.Core.Configurations;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.Configure<WorkersSettings>(builder.Configuration.GetSection(nameof(WorkersSettings)));
builder.Services.AddControllers(
    // Todo: cause duplication controller calls
    // c => c.Filters.Add<PermissionsCheckServiceFilter>()
);
builder.Services.AddDbContext<ArtDbContext>(x => x
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging());
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("RedisConnection") ?? string.Empty,
        true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCustomAutoMapper();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException());

builder.Services.AddCors(opt => opt.AddDefaultPolicy(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true) // allow any origin
    .AllowCredentials()
));

// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddDirectoryBrowser();
// }

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.DbInitialize();

var enableDefaultBrowsing = true;
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    // app.UseDeveloperExceptionPage();
    enableDefaultBrowsing = true;
}
else
{
// app.UseExceptionHandler("/Error");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
// app.UseHsts();
}

// app.UseHttpsRedirection();

// # If the app calls UseStaticFiles, place UseStaticFiles before UseRouting.
var storageDir = app.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>().StorageDir;

if (storageDir == null) throw new Exception("Cannot get storage directory"); 
if (!Path.Exists(storageDir))
{
    Directory.CreateDirectory(storageDir);
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(storageDir),
    RequestPath = "/storage",
    EnableDirectoryBrowsing = enableDefaultBrowsing
});

// If the app uses CORS scenarios, such as [EnableCors], place the call to UseCors before any other middleware that use CORS (for example, place UseCors before UseAuthentication, UseAuthorization, and UseEndpoints).
app.UseCors();

// Calling UseAuthentication and UseAuthorization adds the authentication and authorization
// middleware. These middleware are placed between UseRouting and UseEndpoints so that they can:
// See which endpoint was selected by UseRouting.
//     Apply an authorization policy before UseEndpoints dispatches to the endpoint.
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// Add endpoints for error handling. Redirects to errors controller.
// app.UseStatusCodePagesWithReExecute("/errors/{0}");


app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();

app.Run();
