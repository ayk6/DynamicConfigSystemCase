using DynamicConfig.Admin.Services;
using DynamicConfig.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicConfig.Admin.Pages;

public class CreateModel : PageModel
{
	private readonly ApiService _apiService;

	[BindProperty]
	public ConfigItem ConfigItem { get; set; } = new();

	public CreateModel(ApiService apiService)
	{
		_apiService = apiService;
	}

	public async Task<IActionResult> OnPostAsync()
	{
		await _apiService.AddAsync(ConfigItem);

		return RedirectToPage("Index");
	}
}
