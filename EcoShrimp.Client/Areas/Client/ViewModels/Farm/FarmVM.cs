using EcoShrimp.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.Areas.Client.ViewModels.Farm
{
	public class FarmVM
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string FarmName { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string OwnerName { get; set; }
		public string? NumberHouse { get; set; }
		public string? Ward { get; set; }
		public string? District { get; set; }
		public string? City { get; set; }
		public int? IdTime { get; set; }
		public string? Location { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Số điện thoại không hợp lệ!")]
		public string Phone { get; set; }
		[RegularExpression(RegexConst.EMAIL, ErrorMessage = "Email không đúng định dạng")]
		public string? Email { get; set; }
		public string? Avatar { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có chữ hoa, chữ thường, số, ký tự đặc biệt và ≥ 6 ký tự.")]
		public string PassWord { get; set; }
		public string? Desc { get; set; }
		public IFormFile? FormFile { get; set; }
	}
}
