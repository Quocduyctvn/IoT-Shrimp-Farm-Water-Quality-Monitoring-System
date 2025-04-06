using AutoMapper;
using EcoShrimp.Client.Areas.Client.Controllers.Base;
using EcoShrimp.Client.Areas.Client.ViewModels.Connect;
using EcoShrimp.Data;
using EcoShrimp.Data.Entities;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoShrimp.Client.Areas.Client.Controllers
{
	public class ClientHomeController : ClientControllerBase
	{
		private readonly HttpClient _httpClient;
		public ClientHomeController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
			_httpClient = new HttpClient();
		}

		public IActionResult Index()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.Include(x => x.appProInstances)
											.ThenInclude(x => x.appProducts)
											.ThenInclude(x => x.appCategory)
										.Include(x => x.appPonds)
										.ThenInclude(x => x.appSeasons)
										.ThenInclude(x => x.appConnects)
										.ThenInclude(x => x.appProInstances)
										.FirstOrDefault(x => x.Id == IdFarm);
			farm.appProInstances = farm.appProInstances ?? new List<AppProInstances>();
			return View(farm);
		}

		[HttpGet]
		public async Task<IActionResult> GetDevice(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			var proInstans = _DbContext.AppProInstances.Where(x => x.IdFarm == farm.Id).ToList();

			var devices = new List<AppProInstances>();
			// lấy danh sach thiết bị trống 
			foreach (var item in proInstans)
			{
				var connect = _DbContext.AppConnects.Where(x => x.IdProInstances == item.Id
														&& x.Status == ConnectStatus.Connected).FirstOrDefault();
				if (connect == null)
				{
					devices.Add(item);
				}
			}

			// set mô tả các thiết bị 
			foreach (var item in devices)
			{
				if (string.IsNullOrEmpty(item.IP))
				{
					item.Desc = "Not-Config";
				}
				else
				{
					string url = $"http://{item.IP}:{item.Port}";
					bool isOnline = await IsWebsiteAvailable(url);
					item.Desc = isOnline ? "Connect" : "NotConnect";
				}
			}
			return Ok(devices.Select(d => new
			{
				d.Id,
				d.Name,
				d.SeriNumber,
				d.Desc
			}));
		}

		public async Task<IActionResult> GetConnect(int id)
		{
			var device = _DbContext.AppProInstances.FirstOrDefault(x => x.Id == id);
			if (device == null)
			{
				return NotFound(new { message = "Thiết bị không tồn tại." });
			}

			string url = $"http://{device.IP}:{device.Port}";
			bool isOnline = await IsWebsiteAvailable(url);

			return Ok(new
			{
				DeviceId = id,
				IP = device.IP,
				Port = device.Port,
				IsOnline = isOnline
			});
		}


		static async Task<bool> IsWebsiteAvailable(string url)
		{
			using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) }) // Timeout 3s
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(url);
					return response.IsSuccessStatusCode;
				}

				catch (TaskCanceledException ex)
				{
					Console.WriteLine($"⏳ Timeout: {url} - {ex.Message}");
					return false;
				}
				catch (HttpRequestException ex)
				{
					Console.WriteLine($"🚫 Lỗi kết nối: {url} - {ex.Message}");
					return false;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"❌ Lỗi không xác định: {url} - {ex.Message}");
					return false;
				}
			}
		}


		[HttpPost]
		public IActionResult Connect(ConnectVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Kết nối không hợp lệ!!");
				return RedirectToAction("Index");
			}

			var connect = _DbContext.AppConnects.Where(x => x.IdProInstances == model.DeviceId
									&& x.IdSeason == model.SeasonId && x.Status == ConnectStatus.NotConnected).FirstOrDefault();
			// nếu đã kết nôi trước đó rồi 
			if (connect != null)
			{
				connect.Status = ConnectStatus.Connected;
				connect.StartDate = DateTime.Now;
				connect.EndDate = null;
				_DbContext.Update(connect);
			}
			else
			{
				connect = new AppConnects();
				connect.IdProInstances = model.DeviceId;
				connect.IdSeason = model.SeasonId;
				connect.StartDate = DateTime.Now;
				connect.Status = ConnectStatus.Connected;
				_DbContext.Add(connect);
			}
			_DbContext.SaveChanges();
			SetSuccessMesg("Kết nối thành công!!");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult CancelConect(int id)
		{
			var connect = _DbContext.AppConnects.Where(x => x.Id == id && x.Status == ConnectStatus.Connected).FirstOrDefault();
			// nếu đã kết nôi trước đó rồi 
			if (connect == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí!!");
				return RedirectToAction("Index");
			}

			connect.Status = ConnectStatus.NotConnected;
			connect.EndDate = DateTime.Now;
			_DbContext.Update(connect);
			_DbContext.SaveChanges();
			SetSuccessMesg("Hủy kết nối thành công!!");
			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
			if (id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			var connect = _DbContext.AppConnects.Include(x => x.appDataSensors)
												.Include(x => x.appProInstances)
												.Include(x => x.appSeasons)
												.ThenInclude(x => x.appPond)
												.FirstOrDefault(x => x.Id == id);


			if (connect == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}

			connect.UpdatedDate = DateTime.Now;
			_DbContext.Update(connect);
			_DbContext.SaveChanges();

			string url = $"http://{connect.appProInstances.IP}:{connect.appProInstances.Port}";
			bool isOnline = await IsWebsiteAvailable(url);
			ViewBag.IsOnline = isOnline;

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.Include(x => x.appProInstances).FirstOrDefault(x => x.Id == IdFarm);

			ViewData["farm"] = _DbContext.AppFarms.Include(x => x.appProInstances)
											.ThenInclude(x => x.appProducts)
											.ThenInclude(x => x.appCategory)
										.Include(x => x.appPonds)
										.ThenInclude(x => x.appSeasons)
										.ThenInclude(x => x.appConnects)
										.FirstOrDefault(x => x.Id == IdFarm);

			//TempData["ValueTime"] = _DbContext.AppTimeIntervals.FirstOrDefault(x => x.Id == connect.appProInstances.IdTime);

			return View(connect);
		}

		public async Task<JsonResult> GetSensorData(int idConnect, DateTime? dateSelected)
		{
			try
			{
				TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
				DateTime todayVN = TimeZoneInfo.ConvertTime(DateTime.UtcNow, vnTimeZone).Date;

				DateTime startDate = dateSelected?.Date ?? todayVN;
				DateTime endDate;

				if (dateSelected?.Date == todayVN)
				{
					endDate = dateSelected.Value.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
				}
				else
				{
					endDate = dateSelected?.Date.AddHours(23).AddMinutes(59).AddSeconds(59) ?? DateTime.Now;
				}

				var sensorData = await _DbContext.AppDataSensor
					.Where(x => x.IdConnect == idConnect &&
								x.CreatedDate >= startDate &&
								x.CreatedDate <= endDate)
					.OrderBy(x => x.CreatedDate)
					.Select(x => new
					{
						x.CreatedDate,
						x.PH,
						x.Temp,
						x.DO,
						x.Nh4,
						x.Sal,
						x.Tur
					})
					.ToListAsync();

				var connect = _DbContext.AppConnects.FirstOrDefault(x => x.Id == idConnect);
				connect.UpdatedDate = DateTime.Now;
				_DbContext.Update(connect);
				_DbContext.SaveChanges();

				return Json(sensorData);
			}
			catch (Exception ex)
			{
				return Json(new { error = "Lỗi khi lấy dữ liệu: " + ex.Message });
			}
		}
	}
}
