using AutoMapper;
using EcoShrimp.Client.Controllers.Base;
using EcoShrimp.Client.ViewModels.Account;
using EcoShrimp.Data;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoShrimp.Client.Controllers
{
	public class AccountController : ShrimpControllerBase
	{
		public AccountController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Login(string returnUrl = null)
		{
			if (returnUrl != null)
			{
				SetErrorMesg("Truy cập không hợp lệ!!");
			}
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Thông tin đăng nhập không hợp lệ");
				return View(model);
			}
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Phone == model.Phone);
			if (farm == null)
			{
				SetErrorMesg("Thông tin đăng nhập không hợp lệ");
				return View(model);
			}
			if (farm.Status == Status.Deleted)
			{
				SetErrorMesg("tài khoản đã bị khóa - vui lòng liên hệ admin");
				return View(model);
			}
			var checkPass = BCrypt.Net.BCrypt.Verify(model?.Pass, farm.PassWord);
			if (!checkPass)
			{
				SetErrorMesg("Thông tin đăng nhập không hợp lệ!!!");
				return View(model);
			}

			var claims = new List<Claim>
							{
								new Claim(ClaimTypes.MobilePhone, farm.Phone),
								new Claim(ClaimTypes.Name, farm.OwnerName),
								new Claim(("ID"), farm.Id.ToString()),
								new Claim(ClaimTypes.Role , "Client"),
							};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			HttpContext.SignInAsync(claimsPrincipal);
			return RedirectToAction("Index", "ClientHome", new { area = "Client" });
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home", new { area = "" });
		}
	}
}
