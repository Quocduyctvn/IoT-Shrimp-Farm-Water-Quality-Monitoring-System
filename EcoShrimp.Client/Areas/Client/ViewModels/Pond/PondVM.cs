using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.Areas.Client.ViewModels.Pond
{
	public class PondVM
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string Name { get; set; }
		public string? Address { get; set; }
		// diện tích ao
		[Range(0.01, double.MaxValue, ErrorMessage = "Diện tích ao phải lớn hơn 0.")]
		public decimal? Area { get; set; }
		public string? Desc { get; set; }
	}
}
