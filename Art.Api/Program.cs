
// .net 6/7 version:
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// // .net 3.x/5 version:
// var builder = Host.CreateDefaultBuilder(args)
//     .ConfigureServices(service =>
//     {
//         service.AddControllers();
//         service.AddSwaggerGen();
//         service.AddEndpointsApiExplorer();
//     })
//     .ConfigureWebHostDefaults(builder =>
//     {
//         builder.Configure((ctx, app) =>
//         {
//             // Configure the HTTP request pipeline.
//             if (ctx.HostingEnvironment.IsDevelopment())
//             {
//                 app.UseSwagger();
//                 app.UseSwaggerUI();
//             }
//
//             app.UseStaticFiles();
//             app.UseHttpsRedirection();
//             app.UseRouting();
//             app.UseAuthorization();
//             app.UseEndpoints(endpoint =>
//             {
//                 endpoint.MapControllers();
//                 endpoint.MapGet("/", () => "Hello world");
//             });
//         });
//     });
//
// builder.Build().Run();
