using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace EcoShrimp.Share.Extension
{
	public class AppUnitViewModel
	{
		public Status Status { get; set; }
		public IEnumerable<SelectListItem> StatusList { get; set; }

		public AppUnitViewModel()
		{
			StatusList = new List<SelectListItem>
		{
			new SelectListItem { Value = ((int)Status.Active).ToString(), Text = "Hoạt động" },
			new SelectListItem { Value = ((int)Status.Inactive).ToString(), Text = "Tạm ngưng" },
			new SelectListItem { Value = ((int)Status.Deleted).ToString(), Text = "Đã xóa" }
		};
		}
	}
}
