// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using ButterflyCinema.Models;
using System.Linq;
using System.Security.Cryptography; // Để tạo hash mật khẩu
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Web; // Để mã hóa URL
using ButterflyCinema.Services; // Thêm dòng này
using System.Net.Mail; // Để sử dụng MailAddress

namespace ButterflyCinema.Controllers
{
    public class AccountController : Controller
    {
        private readonly ButterflycinemaContext _context;
        private readonly EmailService _emailService;

        public AccountController(ButterflycinemaContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Action cho trang đăng nhập (nếu bạn có một view riêng cho nó)
        public IActionResult Login()
        {
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;

            return View();
        }

        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new { success = false, message = "Vui lòng nhập email và mật khẩu." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            // Kiểm tra xác thực email
            if (!user.IsEmailVerified)
            {
                return Json(new { success = false, message = "Tài khoản của bạn chưa được xác thực email. Vui lòng kiểm tra email của bạn để hoàn tất đăng ký." });
            }

            // Hash mật khẩu được cung cấp và so sánh
            var hashedPassword = HashPassword(model.Password);
            if (user.Password != hashedPassword)
            {
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            // Đăng nhập thành công, bạn có thể thiết lập session hoặc cookie authentication ở đây
            // Ví dụ đơn giản: lưu UserID vào session
            HttpContext.Session.SetString("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserRole", user.RoleId);


            return Json(new { success = true, message = "Đăng nhập thành công!" });
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {       
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Phone) || string.IsNullOrEmpty(model.Gender) || model.DateOfBirth == default(DateOnly))
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            if (model.Password.Length < 6)
            {
                return Json(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự." });
            }

            // Kiểm tra định dạng email
            try
            {
                var addr = new MailAddress(model.Email);
                if (addr.Address != model.Email)
                {
                    return Json(new { success = false, message = "Email không đúng định dạng." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Email không đúng định dạng." });
            }

            // Kiểm tra định dạng số điện thoại (ví dụ: 10-11 chữ số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(model.Phone, @"^\d{10,11}$"))
            {
                return Json(new { success = false, message = "Số điện thoại không đúng định dạng." });
            }

            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại." });
            }

            // Tạo token xác thực email
            var emailVerificationToken = Guid.NewGuid().ToString();

            var newUser = new User
            {
                UserId = Guid.NewGuid().ToString(), // Tạo UUID cho UserId
                UserName = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Password = HashPassword(model.Password), // Hash mật khẩu trước khi lưu
                Gender = model.Gender,
                Birthday = model.DateOfBirth,
                RoleId = "1", // Role mặc định là "0" (string)
                CreationDate = DateOnly.FromDateTime(DateTime.Now),
                IsEmailVerified = false, // Ban đầu chưa xác thực
                EmailVerificationToken = emailVerificationToken
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Gửi email xác thực
            var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = newUser.UserId, token = emailVerificationToken }, Request.Scheme);
            var emailSubject = "Xác thực tài khoản BUTTERFLY CINEMA của bạn";
            var emailBody = $"Cảm ơn bạn đã đăng ký tài khoản tại BUTTERFLY CINEMA. Vui lòng click vào link sau để xác thực tài khoản của bạn: <a href='{verificationLink}'>Xác thực tài khoản</a>";

            try
            {
                await _emailService.SendEmailAsync(newUser.Email, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi để debug
                Console.WriteLine($"Error during email sending in AccountController: {ex.Message}");
                // Bạn có thể chọn cách xử lý:
                // 1. Trả về lỗi cho người dùng (ví dụ: "Không thể gửi email xác thực. Vui lòng thử lại.")
                //    return Json(new { success = false, message = "Đăng ký thành công nhưng không thể gửi email xác thực. Vui lòng liên hệ hỗ trợ." });
                // 2. Hoặc cho phép đăng ký thành công nhưng log lỗi và người dùng sẽ phải xác thực sau bằng cách khác (ví dụ: resend email).
                //    Trong trường hợp này, bạn vẫn trả về success = true nhưng có thể kèm theo cảnh báo.
                return Json(new { success = false, message = "Đăng ký thành công nhưng không thể gửi email xác thực. Vui lòng kiểm tra lại email hoặc liên hệ hỗ trợ." });
            }


            return Json(new { success = true, message = "Đăng ký thành công! Vui lòng kiểm tra email của bạn để xác thực tài khoản." });
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                // Hoặc trả về View lỗi
                return RedirectToAction("VerificationError", new { type = "register" });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.IsEmailVerified || user.EmailVerificationToken != token)
            {
                // Hoặc trả về View lỗi
                return RedirectToAction("VerificationError", new { type = "register" });
            }

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null; // Xóa token sau khi xác thực
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang thông báo xác thực thành công
            return RedirectToAction("VerificationSuccess");
        }

        public IActionResult VerificationSuccess()
        {
            return View();
        }

        public IActionResult VerificationError(string message)
        {
            return View();
        }


        [HttpPost]
        [Route("Account/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return Json(new { success = false, message = "Vui lòng nhập email." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                // Trả về thông báo chung để tránh tiết lộ thông tin tài khoản
                return Json(new { success = true, message = "Nếu email của bạn tồn tại trong hệ thống, một liên kết đặt lại mật khẩu đã được gửi đến bạn." });
            }

            // Tạo token đặt lại mật khẩu (có thể sử dụng một trường riêng hoặc tái sử dụng EmailVerificationToken)
            var resetToken = Guid.NewGuid().ToString();
            user.EmailVerificationToken = resetToken; // Tái sử dụng để tránh thêm trường mới
            await _context.SaveChangesAsync();

            var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.UserId, token = resetToken }, Request.Scheme);
            var emailSubject = "Đặt lại mật khẩu BUTTERFLY CINEMA của bạn";
            var emailBody = $"Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản BUTTERFLY CINEMA. Vui lòng click vào link sau để đặt lại mật khẩu của bạn: <a href='{resetLink}'>Đặt lại mật khẩu</a>";

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return Json(new { success = true, message = "Nếu email của bạn tồn tại trong hệ thống, một liên kết đặt lại mật khẩu đã được gửi đến bạn." });
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("VerificationError", new { message = "Liên kết đặt lại mật khẩu không hợp lệ." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId && u.EmailVerificationToken == token);

            if (user == null)
            {
                return RedirectToAction("VerificationError", new { message = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });
            }

            // Lưu userId và token vào TempData hoặc ViewBag để sử dụng trong form đặt lại mật khẩu
            ViewBag.UserId = userId;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        [Route("Account/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            if (model.NewPassword.Length < 6)
            {
                return Json(new { success = false, message = "Mật khẩu mới phải có ít nhất 6 ký tự." });
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Json(new { success = false, message = "Mật khẩu xác nhận không khớp." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == model.UserId && u.EmailVerificationToken == model.Token);

            if (user == null)
            {
                return Json(new { success = false, message = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });
            }

            user.Password = HashPassword(model.NewPassword);
            user.EmailVerificationToken = null; // Xóa token sau khi đặt lại mật khẩu
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Mật khẩu của bạn đã được đặt lại thành công! Bạn có thể đăng nhập ngay bây giờ." });
        }

        // Hàm để hash mật khẩu (nên sử dụng thư viện như BCrypt.NET cho mục đích bảo mật thực tế)
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public IActionResult Logout()
        {
            // Xóa tất cả các khóa trong session
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserRole");

            // Hoặc nếu bạn muốn xóa toàn bộ session:
            // HttpContext.Session.Clear();

            // Chuyển hướng người dùng về trang chủ
            return RedirectToAction("Index", "Home");
        }

        // Request DTOs
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public DateOnly DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ForgotPasswordRequest
        {
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            public string UserId { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }
    }
}