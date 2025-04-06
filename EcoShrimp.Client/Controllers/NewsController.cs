using AutoMapper;
using EcoShrimp.Client.Controllers.Base;
using EcoShrimp.Data;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace EcoShrimp.Client.Controllers
{
	public class NewsController : ShrimpControllerBase
	{
		public NewsController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			var news = _DbContext.AppNews.Include(x => x.appCateNew).Where(x => x.Status != Status.Deleted)
										.OrderBy(x => x.SortOrder).AsQueryable();

			TempData["Products"] = _DbContext.AppProducts.Include(x => x.appCategory)
											.Take(8)
											.Include(x => x.appImges).ToList();

			TempData["News"] = _DbContext.AppNews.Include(x => x.appCateNew).Where(x => x.Status != Status.Deleted)
											.Take(8).ToList();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				news = news.Where(x => x.Title.ToUpper().Contains(keyword) || x.Summary.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}

			return View(news.ToPagedList(page, size));
		}

		public IActionResult Detail(int id)
		{
			var news = _DbContext.AppNews.Include(x => x.appCateNew).FirstOrDefault(x => x.Id == id);

			TempData["News"] = _DbContext.AppNews.Include(x => x.appCateNew)
											.Take(8).ToList();

			TempData["Products"] = _DbContext.AppProducts.Include(x => x.appCategory)
											.Include(x => x.appImges)
											.Take(8).ToList();
			return View(news);
		}
	}
}
