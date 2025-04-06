using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcoShrimp.Client.Areas.Client.Views.Shared.Components.Footer
{
	public class ClientFooterViewComponent : ViewComponent
	{
		protected readonly ApplicationDbContext _DbContext;

		public ClientFooterViewComponent(ApplicationDbContext DbContext)
		{
			_DbContext = DbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var shrimp = _DbContext.AppShrimps.FirstOrDefault();

			return View(shrimp);
		}
	}
}
