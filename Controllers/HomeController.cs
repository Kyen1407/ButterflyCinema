using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ButterflyCinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ButterflycinemaContext _context;

        public HomeController(ILogger<HomeController> logger, ButterflycinemaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.ToList();
            var rooms = _context.Rooms.ToList();
            var movies = _context.Movies.ToList();
            var showtime = _context.Showtimes.Include(s => s.Movie).ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Rooms = rooms;
            ViewBag.Movies = movies;
            ViewBag.Showtime = showtime;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Lấy danh sách phim theo rạp
        public JsonResult GetMoviesByCinema(string cinemaId)
        {
            var movies = _context.Showtimes
                .Where(s => s.Room.CinemaId == cinemaId)
                .Select(s => s.Movie)
                .Distinct()
                .ToList();

            return Json(movies);
        }

        public JsonResult GetShowDatesByMovie(string movieId)
        {
            var dates = _context.Showtimes
                .Where(s => s.MovieId == movieId)
                .Select(s => s.ShowDate ?? DateOnly.MinValue)
                .Distinct()
                .OrderBy(date => date)
                .Take(3) // Lấy tối đa 3 ngày
                .ToList();

            return Json(dates);
        }

        [HttpGet]

        [HttpGet]
        public JsonResult GetShowtimes(string date, string movieId) // Thay đổi kiểu tham số
        {
            if (DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                var showtimes = _context.Showtimes
                    .Where(s => s.ShowDate.HasValue && s.ShowDate.Value == parsedDate && s.MovieId == movieId)
                    .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                    .Select(s => new
                    {
                        s.ShowDate,
                        StartTime = $"{s.StartTime:HH:mm}",
                        EndTime = $"{s.EndTime:HH:mm}",
                        s.Room.RoomId,
                        s.ShowtimeId,
                        CinemaName = s.Room.Cinema.CinemaName // Lấy tên rạp từ thông tin phòng
                    })
                    .ToList();

                return Json(showtimes);
            }

            return Json(new List<object>());
        }

        public IActionResult Detail(string id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            var showtimes = _context.Showtimes.Where(s => s.MovieId == id).ToList();
            var movies = _context.Movies.ToList();
            var cinemas = _context.Cinemas.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Movies = movies;
            ViewBag.Showtimes = showtimes;

            return View("~/Views/Movies/Detail.cshtml", movie);
        }
    }
}