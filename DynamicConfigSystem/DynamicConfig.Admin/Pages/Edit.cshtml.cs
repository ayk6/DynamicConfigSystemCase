using DynamicConfig.Admin.Services;
using DynamicConfig.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicConfig.Admin.Pages;

public class EditModel : PageModel
{
	private readonly ApiService _apiService;

	public EditModel(ApiService apiService)
	{
		_apiService = apiService;
	}

	[BindProperty]
	public ConfigItem ConfigItem { get; set; } = new();

	public async Task OnGetAsync(string applicationName, string name)
	{
		ConfigItem = await _apiService.GetByKeyAsync(applicationName, name);
	}

	public async Task<IActionResult> OnPostAsync()
	{
		await _apiService.UpdateAsync(ConfigItem);

		return RedirectToPage("Index");
	}
}
