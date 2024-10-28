using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace ButterflyCinema.Controllers
{
    public class PricingController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public PricingController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Pricing()
        {
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;

            return View();
        }
    }
}
