using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ButterflyCinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public MoviesController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Movies()
        {
            var cinemas = _context.Cinemas.ToList();
            var movies = _context.Movies.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Movies = movies;

            return View();
        }
    }
}
