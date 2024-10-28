using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace ButterflyCinema.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public CheckoutController(ButterflycinemaContext context)
        {
            _context = context;
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
