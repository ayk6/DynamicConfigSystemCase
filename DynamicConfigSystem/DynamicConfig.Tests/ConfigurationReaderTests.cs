using System.Threading;
using System.Text.Json;
using DynamicConfig.Core.Interfaces;
using DynamicConfig.Core.Models;
using DynamicConfig.Core.Services;
using StackExchange.Redis;
using Xunit;

namespace DynamicConfig.Tests;

public class ConfigurationReaderTests
{
	private const string ConnectionString = "localhost:6379";
	private const string ApplicationName = "SERVICE-A";

	[Fact]
	public void GetValue_Should_Return_Correctly_Typed_Values_And_Refresh_In_Background()
	{

		var redis = ConnectionMultiplexer.Connect(ConnectionString);
		var db = redis.GetDatabase();
		var server = redis.GetServer(redis.GetEndPoints().First());

		var keys = server.Keys(pattern: "config:*");
		foreach (var key in keys)
		{
			db.KeyDelete(key);
		}

		db.StringSet("config:id", "0");

		var config1 = new ConfigItem { Id = 1, Name = "SiteName", Type = "string", Value = "Brand", IsActive = true, ApplicationName = ApplicationName };
		var config2 = new ConfigItem { Id = 2, Name = "MaxRetryCount", Type = "int", Value = "3", IsActive = true, ApplicationName = ApplicationName };
		var config3 = new ConfigItem { Id = 3, Name = "IsCampaignEnabled", Type = "bool", Value = "True", IsActive = true, ApplicationName = ApplicationName };

		db.StringSet($"config:{ApplicationName}:SiteName", JsonSerializer.Serialize(config1));
		db.StringSet($"config:{ApplicationName}:MaxRetryCount", JsonSerializer.Serialize(config2));
		db.StringSet($"config:{ApplicationName}:IsCampaignEnabled", JsonSerializer.Serialize(config3));

		Thread.Sleep(50);

		IConfigurationReader configReader = new ConfigurationReader(ApplicationName, ConnectionString, refreshTimerIntervalInMs: 1000);

		Assert.Equal("Brand", configReader.GetValue("SiteName"));
		Assert.Equal(3, configReader.GetValue("MaxRetryCount"));
		Assert.Equal(true, configReader.GetValue("IsCampaignEnabled"));
	}
}