using Microsoft.EntityFrameworkCore;
using reciever;
using reciever.Core.Services;
using reciever.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseMySql(connString, ServerVersion.AutoDetect(connString))
);
builder.Services.AddTransient<ServiceMsg>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();

