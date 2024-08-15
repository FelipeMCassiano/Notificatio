using Microsoft.EntityFrameworkCore;
using reciever;
using reciever.Db;

var builder = Host.CreateApplicationBuilder(args);
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connString, ServerVersion.AutoDetect(connString))
);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();

