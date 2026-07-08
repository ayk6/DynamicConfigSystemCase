using DynamicConfig.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamicConfig.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestConfigController : ControllerBase
{
	private readonly IConfigurationReader _configReader;

	public TestConfigController(IConfigurationReader configReader)
	{
		_configReader = configReader;
	}

	[HttpGet("get-live-settings")]
	public IActionResult GetLiveSettings()
	{
		try
		{
			var siteName = _configReader.GetValue("SiteName");
			var maxRetry = _configReader.GetValue("MaxRetryCount");
			var isCampaignActive = _configReader.GetValue("IsCampaignEnabled");

			return Ok(new
			{
				Data = new
				{
					SiteName = siteName,
					MaxRetryCount = maxRetry,
					IsCampaignEnabled = isCampaignActive
				}
			});
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { Error = ex.Message });
		}
	}
}