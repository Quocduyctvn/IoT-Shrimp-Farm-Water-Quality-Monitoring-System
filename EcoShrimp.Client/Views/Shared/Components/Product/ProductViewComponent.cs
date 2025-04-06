using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoShrimp.Client.Views.Shared.Components.Device
{
	public class ProductViewComponent : ViewComponent
	{

		protected readonly ApplicationDbContext _DbContext;
		public ProductViewComponent(ApplicationDbContext DbContext)
		{
			_DbContext = DbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var device = _DbContext.AppProducts
										.Include(x => x.appImges)
										.Include(x => x.appCategory)
										.Take(3)
										.ToList();
			return View(device);
		}
	}
}
