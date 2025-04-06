using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.Areas.Client.ViewModels.Season
{
	public class SeasonVM
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string Name { get; set; }
		public string? Desc { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		// Mật độ tôm nuôi trong ao (số lượng tôm/m²)
		public int? Density { get; set; }
	}
}
