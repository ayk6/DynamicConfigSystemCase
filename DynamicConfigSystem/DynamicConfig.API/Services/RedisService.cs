using System.Text.Json;
using DynamicConfig.API.Interfaces;
using DynamicConfig.Core.Models;
using StackExchange.Redis;

namespace DynamicConfig.API.Services;

public class RedisService : IRedisService
{
	private readonly ConnectionMultiplexer _redis;
	private readonly IDatabase _database;

	public RedisService(string connectionString)
	{
		_redis = ConnectionMultiplexer.Connect(connectionString);
		_database = _redis.GetDatabase();
	}

	public async Task AddAsync(ConfigItem configItem)
	{
		var key = $"config:{configItem.ApplicationName}:{configItem.Name}";

		configItem.Id = Convert.ToInt32(await _database.StringIncrementAsync("config:id"));

		var exists = (await GetAllAsync(configItem.ApplicationName))
		.Any(x => x.Name == configItem.Name);

		if (exists)
			throw new Exception("Configuration already exists.");

		var json = JsonSerializer.Serialize(configItem);

		await _database.StringSetAsync(key, json);
	}

	public async Task<List<ConfigItem>> GetAllAsync(string applicationName)
	{
		var server = _redis.GetServer(_redis.GetEndPoints().First());

		var keys = server.Keys(pattern: "config:*");

		var configurations = new List<ConfigItem>();

		foreach (var key in keys)
		{
			if (key == "config:id")
				continue;

			var json = (await _database.StringGetAsync(key)).ToString();

			if (string.IsNullOrWhiteSpace(json))
				continue;

			var configuration = JsonSerializer.Deserialize<ConfigItem>(json!);

			if (configuration != null &&
				configuration.IsActive &&
				configuration.ApplicationName == applicationName)
			{
				configurations.Add(configuration);
			}
		}

		return configurations;
	}

	public async Task<ConfigItem?> GetByKeyAsync(string applicationName, string name)
	{
		var json = await _database.StringGetAsync($"config:{applicationName}:{name}");

		if (json.IsNullOrEmpty)
			return null;

		return JsonSerializer.Deserialize<ConfigItem>(json!);
	}

	public async Task UpdateAsync(ConfigItem configItem)
	{
		var json = JsonSerializer.Serialize(configItem);

		await _database.StringSetAsync($"config:{configItem.Id}", json);
	}
}
