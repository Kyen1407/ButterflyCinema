using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ButterflyCinema.Controllers
{
    public class AdsController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public AdsController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Ads()
        {
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;

            return View();
        }
    }
}
