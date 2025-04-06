using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoShrimp.Client.Areas.Client.Views.Shared.Components.MainNavBar
{
	public class MainNavBarViewComponent : ViewComponent
	{
		protected readonly ApplicationDbContext _DbContext;
		public MainNavBarViewComponent(ApplicationDbContext DbContext)
		{
			_DbContext = DbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			TempData["farm"] = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);


			var idGroupClaim = HttpContext.User.FindFirst("IdGroup")?.Value;

			var navBar = new NavBarViewModel();
			navBar.Items.AddRange(new MenuItem[]
			{
				new MenuItem
				{
					Action = "Index",
					Controller = "ClientHome",
					DisplayText = "Trang chủ",
					Icon = "fas fa-home",
					//Permission = AuthConst.AppAmenity.VIEW_LIST, 
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "ClientFarm",
					DisplayText = "Trang trại",
					Icon = "fas fa-suitcase",
					//Permission = AuthConst.AppAmenity.VIEW_LIST,
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "ClientPond",
					DisplayText = "Vuông nuôi",
					Icon = "fas fa-expand",
					//Permission = AuthConst.AppAmenity.VIEW_LIST,
				},
				new MenuItem
				{
					Action = "SendCode",
					Controller = "ClientAccount",
					DisplayText = "Đổi mật khẩu",
					Icon = "fas fa-lock",
					//Permission = AuthConst.AppAmenity.VIEW_LIST,
				},
				//new MenuItem
				//{
				//	Action = "Index",
				//	Controller = "AdminProduct",
				//	DisplayText = "Quản lý Sản phẫm",
				//	Icon = "fas fa-bath",
				//	//Permission = AuthConst.AppAmenity.VIEW_LIST,
				//},
				//new MenuItem
				//{
				//	Action = "Index",
				//	Controller = "AdminUnit",
				//	DisplayText = "Quản lý đơn vị đo",
				//	Icon = "fas fa-bath",
				//	//Permission = AuthConst.AppAmenity.VIEW_LIST,
				//}


			});
			//TempData["Hotel"] = _HotelDbContext.AppHotel.Where(x => x.IdGroup == int.Parse(idGroupClaim)).FirstOrDefault();
			//if (TempData["Hotel"] as AppHotels == null)
			//{
			//	return View("Index");
			//}


			return View(navBar);
		}
	}
}
