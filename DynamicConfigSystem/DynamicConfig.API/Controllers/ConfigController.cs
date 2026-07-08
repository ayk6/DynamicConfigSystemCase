using DynamicConfig.API.Services;
using DynamicConfig.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicConfig.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigController : ControllerBase
{
	private readonly RedisService _redisService;

	public ConfigController(RedisService redisService)
	{
		_redisService = redisService;
	}

	[HttpPost]
	public async Task<IActionResult> Add(ConfigItem configItem)
	{
		await _redisService.AddAsync(configItem);

		return Ok(configItem);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll(string applicationName)
	{
		var configurations = await _redisService.GetAllAsync(applicationName);

		return Ok(configurations);
	}

	[HttpPut]
	public async Task<IActionResult> Update(ConfigItem configItem)
	{
		await _redisService.UpdateAsync(configItem);

		return Ok();
	}

	[HttpGet("{applicationName}/{name}")]
	public async Task<IActionResult> GetByKey(string applicationName, string name)
	{
		var configuration = await _redisService.GetByKeyAsync(applicationName, name);

		if (configuration == null)
			return NotFound();

		return Ok(configuration);
	}
}
