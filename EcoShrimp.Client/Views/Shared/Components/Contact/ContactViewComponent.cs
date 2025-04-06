using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcoShrimp.Client.Views.Shared.Components.Contact
{
	public class ContactViewComponent : ViewComponent
	{

		protected readonly ApplicationDbContext _DbContext;
		public ContactViewComponent(ApplicationDbContext DbContext)
		{
			_DbContext = DbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			TempData["shrimp"] = _DbContext.AppShrimps.FirstOrDefault();
			return View();
		}
	}
}
