using AutoMapper;
using EcoShrimp.Client.Areas.Client.Controllers.Base;
using EcoShrimp.Client.Areas.Client.ViewModels.Account;
using EcoShrimp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Mail;
using System.Security.Claims;

namespace EcoShrimp.Client.Areas.Client.Controllers
{
	public class ClientAccountController : ClientControllerBase
	{
		private readonly IDistributedCache _cache;

		public ClientAccountController(ApplicationDbContext DbContext, IMapper mapper, IDistributedCache cache) : base(DbContext, mapper)
		{
			_cache = cache;
		}

		public IActionResult SendCode()
		{
			return View();
		}

		public async Task<IActionResult> SendMail()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			// Tạo mã xác nhận ngẫu nhiên
			var code = new Random().Next(100000, 999999).ToString();

			// Lưu mã xác nhận vào database hoặc cache (ví dụ: Redis) kèm thời gian hết hạn
			var expirationTime = DateTime.Now.AddMinutes(1); // Mã hết hạn sau 1 phút
			await _cache.SetStringAsync($"PasswordResetCode-{farm.Email}", code, new DistributedCacheEntryOptions
			{
				AbsoluteExpiration = expirationTime
			});

			await SendPasswordResetCodeEmail(farm.Email, code);
			return RedirectToAction("VerifyCode");
		}

		[HttpGet]
		public async Task<IActionResult> VerifyCode()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> VerifyCode(string code)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			var cachedCode = await _cache.GetStringAsync($"PasswordResetCode-{farm.Email}");

			if (cachedCode == null)
			{
				SetErrorMesg("Mã xác nhận đã hết hạn!!");
				return View();
			}

			if (cachedCode != code)
			{
				SetErrorMesg("Mã xác nhận không chính xác");
				return View();
			}

			// Mã chính xác, tiếp tục cho phép người dùng thay đổi mật khẩu
			return RedirectToAction("ChangePass");
		}


		[HttpGet]
		public async Task<IActionResult> ChangePass()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ChangePassword(AccountVM model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Mật khẩu không đúng định dạng!!");
				return View(model);
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdClaim = identity.FindFirst("ID")?.Value;
			int IdFarm = int.Parse(IdClaim);
			var farm = _DbContext.AppFarms.FirstOrDefault(x => x.Id == IdFarm);

			farm.PassWord = BCrypt.Net.BCrypt.HashPassword(model.Password);
			_DbContext.Update(farm);
			_DbContext.SaveChanges();
			SetSuccessMesg("Thay đổi mật khẩu thành công!!");
			return RedirectToAction("Index", "ClientHome");
		}


		public async Task SendPasswordResetCodeEmail(string email, string code)
		{
			var username = "quocduyctvn@gmail.com";
			var password = "ylya rfag tclg nhae";
			var host = "smtp.gmail.com";
			var port = 587;
			var fromEmail = "quocduyctvn@gmail.com";

			// Tạo đối tượng MailMessage để gửi mail
			MailMessage message = new MailMessage();
			message.From = new MailAddress(fromEmail);
			message.To.Add(email);
			message.Subject = "Mã xác nhận đổi mật khẩu trên EcoShrimp.com";
			message.IsBodyHtml = true;
			message.Body = $@"
							<html>
								<body style='font-family: Arial, sans-serif;'>
									<div style='background-color: #f8f8f8; padding: 20px; border-radius: 8px;'>
										<h2 style='color: #333;'>Hãy giúp chúng tôi bảo vệ tài khoản của bạn</h2>
										<p style='font-size: 16px; color: #555;'>Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu cho tài khoản của bạn. Để tiếp tục, vui lòng nhập mã xác nhận sau vào trang đăng nhập.</p>
										<div style='text-align: center; margin: 20px 0;'>
											<div style='display: inline-block; background-color: #e5e5e5; padding: 20px 40px; font-size: 32px; font-weight: bold; color: #333; border-radius: 8px;'>
												{code}
											</div>
										</div>
										<p style='font-size: 16px; color: #555;'>Mã xác nhận này sẽ hết hạn sau 1 phút, vì vậy hãy nhập mã ngay khi có thể. Nếu bạn không yêu cầu thay đổi mật khẩu, vui lòng bỏ qua email này.</p>
										<p style='font-size: 14px; color: #999;'>Nếu bạn gặp bất kỳ vấn đề nào hoặc cần thêm sự trợ giúp, vui lòng liên hệ với bộ phận hỗ trợ của chúng tôi.</p>
										<p style='font-size: 14px; color: #999;'>Chúng tôi luôn ở đây để giúp bạn!</p>
										<br>
										<p style='font-size: 12px; color: #aaa;'>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>
									</div>
								</body>
							</html>";

			// Tạo đối tượng SmtpClient để cấu hình gửi mail
			SmtpClient mailClient = new SmtpClient
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new System.Net.NetworkCredential(username, password),
				Host = host,
				Port = port
			};

			// Gửi email
			try
			{
				mailClient.Send(message);
			}
			catch (Exception ex)
			{
				SetErrorMesg($"Có lỗi xảy ra trong quá trình gửi: {ex.Message}");
			}
		}

	}
}
