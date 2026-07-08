using DynamicConfig.Core.Models;

namespace DynamicConfig.API.Interfaces;

public interface IRedisService
{
	Task AddAsync(ConfigItem configItem);
	Task<List<ConfigItem>> GetAllAsync(string applicationName);
	Task<ConfigItem?> GetByKeyAsync(string applicationName, string name); 
	Task UpdateAsync(ConfigItem configItem);
}
