using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace ButterflyCinema.Controllers
{
    public class EventController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public EventController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Event()
        {
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;

            return View();
        }
    }
}
