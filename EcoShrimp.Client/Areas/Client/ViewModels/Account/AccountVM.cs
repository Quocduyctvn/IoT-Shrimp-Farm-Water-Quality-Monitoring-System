using EcoShrimp.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.Areas.Client.ViewModels.Account
{
	public class AccountVM
	{
		public AccountVM() { }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có chữ hoa, chữ thường, số, ký tự đặc biệt và ≥ 6 ký tự.")]
		public string Password { get; set; }
		[Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string ConfirmPassword { get; set; }
	}
}
