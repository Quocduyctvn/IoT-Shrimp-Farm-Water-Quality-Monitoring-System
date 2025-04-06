using AutoMapper;
using EcoShrimp.Client.Areas.Client.Controllers.Base;
using EcoShrimp.Client.Areas.Client.ViewModels.Pond;
using EcoShrimp.Data;
using EcoShrimp.Data.Entities;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace EcoShrimp.Client.Areas.Client.Controllers
{
	public class ClientPondController : ClientControllerBase
	{
		protected new List<SelectListItem> status;
		public ClientPondController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
			this.status = new List<SelectListItem>
					{
						new SelectListItem { Value = ((int)Status.Active).ToString(), Text = "Hoạt động" },
						new SelectListItem { Value = ((int)Status.Inactive).ToString(), Text = "Ngưng hoạt động" }
					};
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			var ponds = _DbContext.AppPonds.Where(x => x.Status != Status.Deleted && x.IdFarm == farm.Id)
										.OrderBy(x => x.SortOrder).AsQueryable();
			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				ponds = ponds.Where(x => x.Name.ToUpper().Contains(keyword) || x.Desc.ToUpper().Contains(keyword) || x.Address.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}

			return View(ponds.ToPagedList());
		}

		[HttpPost]
		public IActionResult Create(PondVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ!!");
				return RedirectToAction("Index");
			}
			if (model.Area.HasValue && model.Area.Value <= 0)
			{
				SetErrorMesg("Diện tích ao phải lớn hơn 0.");
				return RedirectToAction("Index");
			}
			bool isCheckName = _DbContext.AppPonds.Where(x => x.Status == Status.Active).Any(x => x.Name == model.Name);
			if (isCheckName)
			{
				SetErrorMesg("Ao đã tồn tại");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);
			var maxSortOrder = _DbContext.AppPonds.Where(x => x.IdFarm == farm.Id)
												.DefaultIfEmpty()
												.Max(x => x == null ? 0 : x.SortOrder);


			var pond = new AppPonds();
			pond.Status = Status.Active;
			pond.Name = model.Name;
			pond.Address = model.Address != null ? model.Address.ToString() : "";
			pond.Area = model.Area != null ? (decimal)model.Area : 0;
			pond.Desc = model.Desc != null ? model.Desc.ToString() : string.Empty;
			pond.IdFarm = IdFarm;
			pond.SortOrder = maxSortOrder + 1;
			pond.CreatedDate = DateTime.Now;

			_DbContext.Add(pond);
			_DbContext.SaveChanges();
			SetSuccessMesg("Thêm vuông nuôi thành công!!");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Update(PondVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("Index");
			}
			if (model.Area.HasValue && model.Area.Value <= 0)
			{
				SetErrorMesg("Diện tích ao phải lớn hơn 0.");
				return RedirectToAction("Index");
			}
			var pond = _DbContext.AppPonds.FirstOrDefault(x => x.Id == model.Id);
			if (pond == null)
			{
				SetErrorMesg("Đã xảy ro lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}

			pond.Name = model.Name;
			pond.Address = model.Address;
			pond.Area = model.Area;
			pond.Desc = model.Desc;
			pond.UpdatedDate = DateTime.Now;
			_DbContext.Update(pond);
			_DbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			if (id == null || id < 0)
			{
				SetErrorMesg("Đã xảy ro lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			var pond = _DbContext.AppPonds.FirstOrDefault(x => x.Id == id);
			if (pond == null)
			{
				SetErrorMesg("Đã xảy ro lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			pond.Status = Status.Deleted;
			_DbContext.Update(pond);
			_DbContext.SaveChanges();
			SetSuccessMesg("Xóa thành công");
			return RedirectToAction("Index");
		}



		public IActionResult Plus(int id)
		{
			var ponds = _DbContext.AppPonds.OrderBy(x => x.SortOrder).ToList();

			var currentItem = ponds.FirstOrDefault(x => x.Id == id);

			int currentIndex = ponds.IndexOf(currentItem);
			if (currentIndex == ponds.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = ponds[currentIndex + 1];

			(currentItem.SortOrder, nextItem.SortOrder) = (nextItem.SortOrder, currentItem.SortOrder);

			_DbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			var ponds = _DbContext.AppPonds.OrderBy(x => x.SortOrder).ToList();

			var currentItem = ponds.FirstOrDefault(x => x.Id == id);
			int currentIndex = ponds.IndexOf(currentItem);

			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			var previousItem = ponds[currentIndex - 1];

			(currentItem.SortOrder, previousItem.SortOrder) = (previousItem.SortOrder, currentItem.SortOrder);

			_DbContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
