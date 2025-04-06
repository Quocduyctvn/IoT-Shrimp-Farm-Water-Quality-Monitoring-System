using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcoShrimp.Client.Views.Shared.Components.Footer
{
	public class FooterViewComponent : ViewComponent
	{

		protected readonly ApplicationDbContext _DbContext;
		public FooterViewComponent(ApplicationDbContext DbContext)
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
