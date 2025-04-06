using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Client.Areas.Client.ViewModels.Device
{
	public class DeviceVM
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên không được để trống.")]
		[MaxLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự.")]
		public string Name { get; set; }
	}
}
