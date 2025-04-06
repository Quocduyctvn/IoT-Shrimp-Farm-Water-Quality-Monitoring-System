using AutoMapper;
using EcoShrimp.Client.Areas.Client.Controllers.Base;
using EcoShrimp.Client.Areas.Client.ViewModels.Season;
using EcoShrimp.Data;
using EcoShrimp.Data.Entities;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoShrimp.Client.Areas.Client.Controllers
{
	public class ClientSeasonController : ClientControllerBase
	{
		public ClientSeasonController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(SeasonVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("Index", "ClientHome");
			}
			var pond = _DbContext.AppPonds.FirstOrDefault(x => x.Id == model.Id);
			if (pond == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index", "ClientHome");
			}
			var season = new AppSeasons();
			season.IdPond = pond.Id;
			season.Name = model.Name;
			season.Density = model.Density;
			season.StartDate = model.StartDate;
			season.EndDate = model.EndDate;
			season.Desc = model.Desc;
			season.CreatedDate = DateTime.Now;
			season.Status = Status.Active;
			_DbContext.Add(season);
			_DbContext.SaveChanges();
			SetSuccessMesg("Thêm đợt nuôi thành công!!");
			return RedirectToAction("Index", "ClientHome");
		}

		[HttpPost]
		public IActionResult Update(SeasonVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("Index", "ClientHome");
			}
			var season = _DbContext.AppSeasons.FirstOrDefault(x => x.Id == model.Id);
			if (season == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index", "ClientHome");
			}

			season.Name = model.Name;
			season.Density = model.Density;
			season.EndDate = model.EndDate;
			season.Desc = model.Desc;
			season.UpdatedDate = DateTime.Now;
			_DbContext.Update(season);
			_DbContext.SaveChanges();
			SetSuccessMesg("Cập nhật đợt nuôi thành công!!");
			return RedirectToAction("Index", "ClientHome");
		}

		[HttpPost]
		public IActionResult EndSeason(int id)
		{
			if (id <= 0)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index", "ClientHome");
			}

			var season = _DbContext.AppSeasons.Include(x => x.appConnects).FirstOrDefault(x => x.Id == id);
			season.Status = Status.Inactive;
			_DbContext.Update(season);

			if (season.appConnects?.Any() == true)
			{
				foreach (var connect in season.appConnects)
				{
					connect.Status = ConnectStatus.NotConnected;
				}
			}

			_DbContext.SaveChanges();
			SetSuccessMesg("Hoàn tất kết thúc đợt nuôi thành công!");
			return RedirectToAction("Index", "ClientHome");
		}
	}
}
