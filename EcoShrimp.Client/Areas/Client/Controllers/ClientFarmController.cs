using AutoMapper;
using EcoShrimp.Client.Areas.Client.Controllers.Base;
using EcoShrimp.Client.Areas.Client.ViewModels.Device;
using EcoShrimp.Client.Areas.Client.ViewModels.Farm;
using EcoShrimp.Client.Areas.Client.ViewModels.Time;
using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoShrimp.Client.Areas.Client.Controllers
{
	public class ClientFarmController : ClientControllerBase
	{
		public ClientFarmController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			if (farm == null)
			{
				SetErrorMesg("Không tìm thấy trang trại");
				return RedirectToAction("Index");
			}
			var farmVM = new FarmVM();
			farmVM.Id = farm.Id;
			farmVM.OwnerName = farm.OwnerName;
			farmVM.FarmName = farm.FarmName;
			farmVM.Phone = farm.Phone;
			farmVM.Email = farm.Email;
			farmVM.City = farm.City;
			farmVM.District = farm.District;
			farmVM.Ward = farm.Ward;
			farmVM.NumberHouse = farm.NumberHouse;
			farmVM.Desc = farm.Desc;
			farmVM.Avatar = farm.Avatar;
			farmVM.IdTime = farm.IdTime;

			TempData["proInstans"] = _DbContext.AppProInstances.Where(x => x.IdFarm == farm.Id).ToList();
			TempData["times"] = _DbContext.AppTimeIntervals.ToList();
			return View(farmVM);
		}

		[HttpPost]
		public IActionResult UpdateDevice(DeviceVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("index");
			}
			var device = _DbContext.AppProInstances.FirstOrDefault(x => x.Id == model.Id);
			if (device == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("index");
			}
			device.Name = model.Name;
			_DbContext.Update(device);
			_DbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công!!");
			return RedirectToAction("index");
		}

		[HttpPost]
		public IActionResult Index(FarmVM model, [FromServices] IWebHostEnvironment envi)
		{
			ModelState.Remove("PassWord");
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ!!");
				return View(model);
			}
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);
			if (farm == null)
			{
				SetErrorMesg("Không tìm thấy trang trại");
				return RedirectToAction("Index");
			}

			farm.OwnerName = model.OwnerName;
			farm.FarmName = model.FarmName;
			farm.Phone = model.Phone;
			farm.Email = model.Email != null ? model.Email : "";
			farm.City = model.City != "0" ? model.City : "";
			farm.District = model.District != "0" ? model.District : "";
			farm.Ward = model.Ward != "0" ? model.Ward : "";
			farm.NumberHouse = model.NumberHouse != null ? model.NumberHouse : "";
			farm.Desc = model.Desc != null ? model.Desc : "";
			farm.UpdatedDate = DateTime.Now;
			if (model.FormFile != null)
			{
				if (farm.Avatar != null)
				{
					string filePath = Path.Combine(envi.WebRootPath, farm.Avatar);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
				}
				string adminRootPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EcoShrimp.Admin", "wwwroot");
				string clientRootPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EcoShrimp.Client", "wwwroot");

				farm.Avatar = UploadFileToBothProjects(model.FormFile, adminRootPath, clientRootPath);
			}
			_DbContext.Update(farm);
			_DbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công.");
			return RedirectToAction("Index");
		}

		private string UploadFileToBothProjects(IFormFile file, string adminWebRoot, string clientWebRoot)
		{
			var fName = Path.GetFileNameWithoutExtension(file.FileName)
						+ DateTime.Now.Ticks
						+ Path.GetExtension(file.FileName);

			var relativePath = "/images/farm/" + fName; // Đường dẫn như cũ

			// Lưu vào thư mục của Admin
			var adminPath = Path.Combine(adminWebRoot, "images", "farm", fName);
			Directory.CreateDirectory(Path.GetDirectoryName(adminPath));
			using (var stream = new FileStream(adminPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			// Lưu vào thư mục của Client
			var clientPath = Path.Combine(clientWebRoot, "images", "farm", fName);
			Directory.CreateDirectory(Path.GetDirectoryName(clientPath));
			using (var stream = new FileStream(clientPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return relativePath; // Trả về đường dẫn tương đối
		}

		[HttpPost]
		public async Task<IActionResult> UpdateIdTime([FromBody] UpdateTimeVM dto)
		{
			try
			{
				if (dto == null || dto.IdFarm <= 0)
				{
					return BadRequest(new { message = "Dữ liệu không hợp lệ!" });
				}

				var appFarm = await _DbContext.AppFarms.FindAsync(dto.IdFarm);
				if (appFarm == null)
				{
					return NotFound(new { message = "AppFarm không tồn tại!" });
				}

				appFarm.IdTime = dto.IdTime;
				await _DbContext.SaveChangesAsync();

				return Ok(new { message = "Cập nhật thành công!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Lỗi server!", error = ex.Message });
			}
		}

	}
}
