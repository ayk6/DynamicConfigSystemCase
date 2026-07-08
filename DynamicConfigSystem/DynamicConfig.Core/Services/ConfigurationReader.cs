using DynamicConfig.Core.Interfaces;
using DynamicConfig.Core.Models;

namespace DynamicConfig.Core.Services
{
	public class ConfigurationReader : IConfigurationReader
	{
		private readonly ConfigurationProvider _provider;

		private readonly string _applicationName;

		private readonly Timer _timer;

		private Dictionary<string, ConfigItem> _configurations = new();


		public ConfigurationReader(
			string applicationName,
			string connectionString,
			int refreshTimerIntervalInMs)
		{
			_provider = new ConfigurationProvider(connectionString);
			_applicationName = applicationName;

			var configurations = _provider
		   .GetConfigItemsAsync(applicationName)
		   .GetAwaiter()
		   .GetResult();

			_configurations = configurations.ToDictionary(x => x.Name);

			_timer = new Timer(
				RefreshConfigurations,
				null,
				refreshTimerIntervalInMs,
				refreshTimerIntervalInMs);
		}

		private void RefreshConfigurations(object? state)
		{
			try
			{
				var configurations = _provider
					.GetConfigItemsAsync(_applicationName)
					.GetAwaiter()
					.GetResult();

				_configurations = configurations.ToDictionary(x => x.Name);
			}
			catch
			{
				Console.WriteLine("Failed to refresh configurations from Redis.");
			}
		}

		public object GetValue(string key)
		{
			if (!_configurations.TryGetValue(key, out var configuration))
			{
				throw new KeyNotFoundException($"'{key}' configuration not found.");
			}

			Type targetType = configuration.Type.ToLower() switch
			{
				"int" or "integer" => typeof(int),
				"bool" or "boolean" => typeof(bool),
				"double" => typeof(double),
				_ => typeof(string)
			};

			return Convert.ChangeType(configuration.Value, targetType);
		}
	}
}
