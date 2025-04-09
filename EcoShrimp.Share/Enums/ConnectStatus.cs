using System.ComponentModel.DataAnnotations;

namespace EcoShrimp.Share.Enums
{
	/// <summary>
	/// Represents the status of a connection.
	/// </summary>
	public enum ConnectStatus
	{
		[Display(Name = "Chưa kết nối")]
		NotConnected = 1,

		[Display(Name = "Kết nối thành công")]
		Connected = 2,

		[Display(Name = "Kết nối bị gián đoạn")]
		Disconnected = 3
	}
}
