using DynamicConfig.API.Services;
using DynamicConfig.Core.Interfaces;
using DynamicConfig.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
	new RedisService("localhost:6379"));

string appName = builder.Configuration["DynamicConfig:ApplicationName"] ?? "SERVICE-A";
string redisConn = builder.Configuration["DynamicConfig:ConnectionString"] ?? "localhost:6379";
int interval = int.Parse(builder.Configuration["DynamicConfig:RefreshTimerIntervalInMs"] ?? "5000");

builder.Services.AddSingleton<IConfigurationReader>(sp =>
	new ConfigurationReader(appName, redisConn, interval));

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
