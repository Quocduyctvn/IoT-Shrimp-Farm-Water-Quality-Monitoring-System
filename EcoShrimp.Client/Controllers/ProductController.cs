using AutoMapper;
using EcoShrimp.Client.Controllers.Base;
using EcoShrimp.Client.ViewModels.Request;
using EcoShrimp.Data;
using EcoShrimp.Data.Entities;
using EcoShrimp.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using X.PagedList.Extensions;

namespace EcoShrimp.Client.Controllers
{
	public class ProductController : ShrimpControllerBase
	{
		public ProductController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			var products = _DbContext.AppProducts.Include(x => x.appImges).Where(x => x.Status != Status.Deleted)
										.OrderBy(x => x.SortOrder).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				products = products.Where(x => x.Name.ToUpper().Contains(keyword) || x.Desc.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}

			TempData["Products"] = _DbContext.AppProducts.Include(x => x.appCategory)
											.Take(3)
											.Include(x => x.appImges).ToList();

			// Trả về view với dữ liệu phân trang
			return View(products.ToPagedList(page, size));
		}


		public IActionResult Detail(int id)
		{
			if (id <= 0)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí!!");
				return RedirectToAction("Index", "Home");
			}

			var product = _DbContext.AppProducts.Include(x => x.appCategory)
											.Include(x => x.appImges).FirstOrDefault(x => x.Id == id);
			if (product == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí!!");
				return RedirectToAction("Index", "Home");
			}

			TempData["Products"] = _DbContext.AppProducts.Include(x => x.appCategory)
											.Where(x => x.Id != product.Id)
											.Take(8)
											.Include(x => x.appImges).ToList();

			return View(product);
		}

		[HttpPost]
		public IActionResult Requests(RequestVM model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				SetErrorMesg(string.Join("\n", errors)); // Gộp tất cả lỗi thành một chuỗi

				return Redirect(Request.Headers["Referer"].ToString() ?? "/");
			}

			// Tạo đối tượng yêu cầu
			var request = new AppRequests
			{
				CompanyName = model.CompanyName,
				Email = model.Email,
				Phone = model.Phone,
				Address = model.Address,
				Content = model.Content,
				CreatedDate = DateTime.Now,
				Status = RequestStatus.Pending
			};

			// Lưu vào Cơ sở dữ liệu
			_DbContext.Add(request);
			_DbContext.SaveChanges();

			// Nội dung email
			string content = $@"
							<div class=""container bg-secondary d-flex justify-content-center"">
								<div class=""bg-light mt-1 rounded-2 p-3"" style=""width: 1200px"">
									<div class=""row px-4"">
										<div class=""col p-0"">
											<div class=""row"">Thân gửi {model.CompanyName},</div>
											<div class=""row"">
												<span class=""p-0 col-auto fw-bold"" style=""color:#c88321"">Booking.com</span>
												<div class=""col-auto ps-1"">vừa nhận được một yêu cầu từ khách hàng liên quan đến dịch vụ của bạn trên <span style=""color:#c88321"">EcoShrimp.com</span></div>
											</div>
											<div class=""row mt-2""><i class=""p-0"">Thông tin yêu cầu:</i></div>
											<div class=""row ms-3"">Tên công ty: {model.CompanyName}</div>
											<div class=""row ms-3"">Email: {model.Email}</div>
											<div class=""row ms-3"">Số điện thoại: {model.Phone}</div>
											<div class=""row ms-3"">Địa chỉ: {model.Address}</div>
											<div class=""row ms-3"">Nội dung yêu cầu: {model.Content}</div>
											<div class=""row mt-2""><i class=""p-0""><span class=""text-danger"">Ghi chú:</span> Vui lòng liên hệ qua thông tin trên để phản hồi yêu cầu từ khách hàng.</i></div>
											<div class=""row mt-2"">
												Chúng tôi đã nhận được thông tin yêu cầu của bạn và sẽ xử lý nhanh chóng. Đội ngũ của chúng tôi sẽ liên hệ lại với bạn trong thời gian sớm nhất để hỗ trợ và giải đáp mọi thắc mắc. Chúng tôi rất trân trọng sự tin tưởng và hợp tác của bạn. Cảm ơn bạn đã lựa chọn dịch vụ của chúng tôi!
											</div>
											<div class=""row mt-2"">Trân trọng cảm ơn,</div>
											<div class=""row"">EcoShrimp.com</div>
										</div>
									</div>
								</div>
							</div>
							";

			// Thông tin người gửi email
			var username = "quocduyctvn@gmail.com";
			var password = "ylya rfag tclg nhae";
			var host = "smtp.gmail.com";
			var port = 587;
			var fromEmail = "quocduyctvn@gmail.com";

			// Tạo đối tượng MailMessage để gửi mail
			MailMessage message = new MailMessage();
			message.From = new MailAddress(fromEmail); // Người gửi email
			message.To.Add(model.Email); // Người nhận mail (từ model, có thể là khách hàng)
			message.Subject = "Yêu cầu liên hệ từ khách hàng trên EcoShrimp.com";
			message.IsBodyHtml = true;
			message.Body = content;

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
				SetSuccessMesg("Yêu cầu của bạn đã được gửi đi thành công!");
			}
			catch (Exception ex)
			{
				SetErrorMesg($"Có lỗi xảy ra trong quá trình gửi email: {ex.Message}");
			}

			return Redirect(Request.Headers["Referer"].ToString() ?? "/");
		}



	}
}
