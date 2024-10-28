using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ButterflyCinema.Controllers
{
    public class ContactController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public ContactController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Contact()
        {
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;

            return View();
        }
    }
}
