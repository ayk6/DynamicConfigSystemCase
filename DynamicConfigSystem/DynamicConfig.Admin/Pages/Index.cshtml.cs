using DynamicConfig.Admin.Services;
using DynamicConfig.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicConfig.Admin.Pages;

public class IndexModel : PageModel
{
	private readonly ApiService _apiService;

	public List<ConfigItem> Configurations { get; set; } = new();

	[BindProperty(SupportsGet = true)]
	public string ApplicationName { get; set; } = string.Empty;

	public IndexModel(ApiService apiService)
	{
		_apiService = apiService;
	}

	public async Task OnGetAsync()
	{
		if (!string.IsNullOrWhiteSpace(ApplicationName))
		{
			Configurations = await _apiService.GetAllAsync(ApplicationName);
		}
	}
}
