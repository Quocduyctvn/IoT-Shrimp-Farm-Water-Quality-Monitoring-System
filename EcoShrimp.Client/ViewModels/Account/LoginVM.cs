using EcoShrimp.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.ViewModels.Account
{
	public class LoginVM
	{
		[Required(ErrorMessage = "Vui lòng nhập trường này!")]
		[RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Số điện thoại không hợp lệ")]
		public string Phone { get; set; }
		[RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có chữ hoa, chữ thường, số, ký tự đặc biệt và ≥ 6 ký tự.")]
		[Required(ErrorMessage = "Vui lòng nhập trường này!")]
		public string Pass { get; set; }
	}
}
