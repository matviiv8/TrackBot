using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using TrackBot.Infrastructure.Context;
using TrackBot.Domain.Repositories;
using TrackBot.Infrastructure.Repositories;
using TrackBot.Domain.Interfaces;
using TrackBot.Infrastructure.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TrackContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var client = new TelegramBotClient(builder.Configuration["TelegramBot:Token"]);
await client.SetWebhookAsync(builder.Configuration["TelegramBot:Webhook"] + "/bot");

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ITelegramBotClient>(client);

builder.Services.AddScoped<IWalkService, WalkService>();
builder.Services.AddScoped<ITrackLocationService, TrackLocationService>();
builder.Services.AddScoped<ITrackLocationRepository, TrackLocationRepository>();
builder.Services.AddScoped<ITelegramMessageHandler, TelegramMessageHandler>();
builder.Services.AddScoped<ITelegramMessageSender, TelegramMessageSender>();

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