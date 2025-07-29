using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ButterflyCinema.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public CheckoutController(ButterflycinemaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> MomoPayment([FromBody] MomoRequestModel model)
        {
            // Thông tin MoMo (lấy từ MoMo Developer)
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO";
            string accessKey = "F8BBA842ECF85";
            string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
            string orderId = Guid.NewGuid().ToString();
            string redirectUrl = "https://localhost:7116/";
            string ipnUrl = "https://localhost:7116/";
            string requestId = Guid.NewGuid().ToString();
            string orderInfo = model.orderInfo;
            string amount = model.amount;
            string extraData = "";

            // Tạo signature
            string rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType=captureWallet";
            string signature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToLower();
            }

            var requestBody = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount,
                orderId,
                orderInfo,
                redirectUrl,
                ipnUrl,
                extraData,
                requestType = "captureWallet",
                signature,
                lang = "vi"
            };

            using var client = new HttpClient();
            var response = await client.PostAsync(endpoint, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);

            // Lưu thông tin orderId, showtimeId, selectedSeats vào DB nếu cần để đối chiếu khi nhận callback

            return Json(new { payUrl = json["payUrl"] });
        }

        // Xử lý callback từ MoMo (redirectUrl, ipnUrl)
        [HttpGet]
        public IActionResult PaymentResult(string orderId, string resultCode, string showtimeId, string selectedSeats)
        {
            if (resultCode == "0")
            {
                // Thanh toán thành công
                // Cập nhật trạng thái vé (ticket) thành đã đặt
                var seatIds = selectedSeats.Split(',');
                foreach (var seatId in seatIds)
                {
                    var ticket = _context.Tickets.FirstOrDefault(t => t.ShowtimeId == showtimeId && t.SeatId == seatId);
                    if (ticket != null)
                    {
                        ticket.IsBooked = true;
                    }
                }
                _context.SaveChanges();
                ViewBag.Message = "Thanh toán thành công! Vé đã được đặt.";
            }
            else
            {
                ViewBag.Message = "Thanh toán thất bại hoặc bị hủy.";
            }
            return View();
        }

        public IActionResult Checkout(string id)
        {
            var cinemas = _context.Cinemas.ToList();
            var movies = _context.Movies.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Movies = movies;
            ViewBag.MovieId = id;

            return View();
        }
    }
}

public class MomoRequestModel
{
    public string amount { get; set; }
    public string orderInfo { get; set; }
    public string selectedSeats { get; set; }
    public string showtimeId { get; set; }
}
