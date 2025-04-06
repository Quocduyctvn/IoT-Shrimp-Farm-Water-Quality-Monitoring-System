using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoShrimp.Client.Views.Shared.Components.News
{
	public class NewsViewComponent : ViewComponent
	{

		protected readonly ApplicationDbContext _DbContext;
		public NewsViewComponent(ApplicationDbContext DbContext)
		{
			_DbContext = DbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var device = _DbContext.AppNews
										.Include(x => x.appCateNew)
										.Take(3)
										.ToList();
			return View(device);
		}
	}
}
