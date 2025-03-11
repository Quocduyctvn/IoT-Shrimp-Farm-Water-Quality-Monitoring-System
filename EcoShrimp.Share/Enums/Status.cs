using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Share.Enums
{
	public enum Status
	{
		[Display(Name = "Hoạt động")]
		Active = 1,

		[Display(Name = "Không hoạt động")]
		Inactive = 2,

		[Display(Name = "Đã xóa")]
		Deleted = 3,

		[Display(Name = "Đã bán")]
		Bought = 4
	}
}
