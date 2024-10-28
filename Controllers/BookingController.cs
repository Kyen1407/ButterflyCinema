using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ButterflyCinema.Controllers
{
    public class BookingController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public BookingController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Booking(string roomId, string showTimeId)
        {
            var cinemas = _context.Cinemas.ToList();
            var rooms = _context.Rooms.ToList();
            var movies = _context.Movies.ToList();
            var showtime = _context.Showtimes.FirstOrDefault(s => s.ShowtimeId == showTimeId);
            var seats = GetSeatsByRoomAndShowTime(roomId, showTimeId);
            var tickets = GetTicketsByShowTime(showTimeId);
            var concessions = _context.Concessions.ToList();
            var combos = _context.Combos.ToList();
            var comboItems = _context.Comboitems.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Rooms = rooms;
            ViewBag.Movies = movies;
            ViewBag.Showtime = showtime;
            ViewBag.Seats = seats;
            ViewBag.Tickets = tickets;
            ViewBag.Concessions = concessions;
            ViewBag.Combos = combos;
            ViewBag.ComboItems = comboItems;

            return View();
        }

        public List<Seat> GetSeatsByRoomAndShowTime(string roomId, string showTimeId)
        {
            // Giả sử có một bảng Showtimes với RoomId và có thể xác định ghế phù hợp
            var showtime = _context.Showtimes
                .FirstOrDefault(st => st.ShowtimeId == showTimeId && st.RoomId == roomId);

            if (showtime == null)
            {
                return new List<Seat>(); // Hoặc xử lý lỗi nếu không tìm thấy suất chiếu
            }

            // Giả sử có một bảng Seats chứa RoomId và có thể sử dụng bảng Showtimes để xác định ghế
            var seats = _context.Seats
                .Where(seat => seat.RoomId == roomId) // Chỉ lọc theo RoomId
                .ToList();

            return seats;
        }

        private List<Ticket> GetTicketsByShowTime(string showTimeId)
        {
            return _context.Tickets
                .Where(ticket => ticket.ShowtimeId == showTimeId)
                .ToList();
        }
    }
}
