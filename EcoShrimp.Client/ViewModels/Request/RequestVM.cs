using EcoShrimp.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.ViewModels.Request
{
	public class RequestVM
	{
		public RequestVM() { }
		[Required(ErrorMessage = "Tên công ty là bắt buột!!")]
		public string CompanyName { get; set; }
		public string? Address { get; set; }

		[RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Số điện thoại không hợp lệ")]
		public string? Phone { get; set; }
		[Required(ErrorMessage = "Email là bắt buột!!")]
		[RegularExpression(RegexConst.EMAIL, ErrorMessage = "Email không đúng định dạng")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Nội dung tư vấn là bắt buột!!")]
		public string Content { get; set; }
	}
}
