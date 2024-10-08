using Send.Service;
using Send.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SendService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPost("/send", async (MessageRequest message, SendService service) =>
{

    try
    {

        Console.WriteLine($"recipient:{message.recipient}");
        await service.PublishMessage(message);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);

    }
});

app.Run();


