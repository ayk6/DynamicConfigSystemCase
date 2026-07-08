using System.Text.Json;
using DynamicConfig.Core.Models;
using StackExchange.Redis;

namespace DynamicConfig.Core.Services
{
	public class ConfigurationProvider
	{
		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _database;

		public ConfigurationProvider(string connectionString)
		{
			_redis = ConnectionMultiplexer.Connect(connectionString);
			_database = _redis.GetDatabase();
		}

		public async Task<List<ConfigItem>> GetConfigItemsAsync(string applicationName)
		{
			var server = _redis.GetServer(_redis.GetEndPoints().First());

			var keys = server.Keys(pattern: $"config:{applicationName}:*");

			var configItems = new List<ConfigItem>();

			foreach (var key in keys)
			{
				var value = await _database.StringGetAsync(key);

				if (!value.HasValue) continue;

				var configItem = JsonSerializer.Deserialize<ConfigItem>(value!);

				if (configItem != null && configItem.IsActive)
				{
					{
						configItems.Add(configItem);
					}
				}
			}

			return configItems;
		}
	}
}
