using DynamicConfig.Core.Models;

namespace DynamicConfig.Admin.Services;

public class ApiService(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<List<ConfigItem>> GetAllAsync(string applicationName)
	{
		return await _httpClient.GetFromJsonAsync<List<ConfigItem>>
		($"api/config?applicationName={applicationName}") ?? [];
	}

	public async Task AddAsync(ConfigItem configItem)
	{
		await _httpClient.PostAsJsonAsync("api/config", configItem);
	}

	public async Task<ConfigItem?> GetByKeyAsync(string applicationName, string name)
	{
		return await _httpClient.GetFromJsonAsync<ConfigItem>(
			$"api/config/{applicationName}/{name}");
	}

	public async Task UpdateAsync(ConfigItem configItem)
	{
		await _httpClient.PutAsJsonAsync("api/config", configItem);
	}
}
