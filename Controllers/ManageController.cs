using ButterflyCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ButterflyCinema.Controllers
{
    public class ManageController : Controller
    {
        private readonly ButterflycinemaContext _context;

        public ManageController(ButterflycinemaContext context)
        {
            _context = context;
        }

        public IActionResult Manage(string tab)
        {
            var cinemas = _context.Cinemas.ToList();
            var rooms = _context.Rooms.ToList();
            var movies = _context.Movies.ToList();
            var showtime = _context.Showtimes.Include(s => s.Movie).ToList();
            var concessions = _context.Concessions.ToList();
            var combos = _context.Combos.ToList();
            var comboItems = _context.Comboitems.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Rooms = rooms;
            ViewBag.Movies = movies;
            ViewBag.Showtime = showtime;
            ViewBag.Concessions = concessions;
            ViewBag.Combos = combos;
            ViewBag.ComboItems = comboItems;
            ViewBag.ActiveTab = tab ?? "cinema";

            return View();
        }

        public IActionResult Error(string errorMessage, string tab)
        {
            ViewData["ErrorMessage"] = errorMessage;
            ViewData["Tab"] = tab;
            return View();
        }

        // Thêm rạp
        [HttpPost]
        [ValidateAntiForgeryToken]  // Bảo vệ khỏi tấn công CSRF
        public IActionResult CreateCinema(Cinema cinema)
        {
            // Kiểm tra xem mã rạp có tồn tại không
            var cinemaExists = _context.Cinemas.Any(c => c.CinemaId == cinema.CinemaId);

            if (cinemaExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Mã rạp đã tồn tại!", tab = "cinema" }); // Trả về view với thông báo lỗi
            }

            if (ModelState.IsValid)
            {
                //var newCinema = new Cinema
                //{
                //    CinemaId = cinema.CinemaId,
                //    CinemaName = cinema.CinemaName,
                //    CinemaAddress = cinema.CinemaAddress
                //};
                _context.Cinemas.Add(cinema); // Thêm dữ liệu vào context
                _context.SaveChanges(); // Lưu thay đổi vào database

                return RedirectToAction("Manage");  // Chuyển hướng về trang danh sách rạp chiếu
            }

            return View("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "cinema" });
        }

        // Hiển thị form chỉnh sửa rạp
        [HttpGet]
        public IActionResult EditCinema(string id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }

        // Lưu thông tin đã chỉnh sửa rạp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCinema(Cinema cinema)
        {
            var nameExists = _context.Cinemas.Any(c => c.CinemaName == cinema.CinemaName);
            var addressExists = _context.Cinemas.Any(c => c.CinemaAddress == cinema.CinemaAddress);

            if (nameExists || addressExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Thông tin không hợp lệ!", tab = "cinema" });
            }

            if (ModelState.IsValid)
            {
                _context.Cinemas.Update(cinema); // Cập nhật dữ liệu vào context
                _context.SaveChanges(); // Lưu thay đổi vào database

                return RedirectToAction("Manage"); // Chuyển hướng về trang danh sách rạp chiếu
            }
            return View("Manage"); // Nếu dữ liệu không hợp lệ, quay lại form
        }

        // Hiển thị confirm xóa rạp
        [HttpGet]
        public IActionResult ConfirmDeleteCinema(string id)
        {
            var cinema = _context.Cinemas.Find(id); // Lấy thông tin rạp từ database
            var viewModel = new ConfirmDelete
            {
                ObjectType = "rạp",
                ObjectName = cinema.CinemaName,
                Id = cinema.CinemaId,
                Action = "DeleteCinema",
                TabName = "cinema"
            };

            return View("ConfirmDelete", viewModel);
        }

        // Xác nhận xóa rạp
        [HttpPost, ActionName("DeleteCinema")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var cinema = _context.Cinemas.Include(c => c.Rooms).FirstOrDefault(c => c.CinemaId == id);

            if (cinema != null)
            {
                // Gọi phương thức xóa phòng cho từng phòng trong rạp
                foreach (var room in cinema.Rooms)
                {
                    DeleteRoom(room.RoomId); // Gọi phương thức xóa phòng
                }

                _context.Cinemas.Remove(cinema); // Xóa dữ liệu khỏi context
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
            return RedirectToAction("Manage"); // Chuyển hướng về trang danh sách rạp chiếu
        }

        //---------------------------------------------------------------------------------

        // Thêm phòng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRoom(Room room)
        {
            // Kiểm tra xem mã phòng có tồn tại không
            var roomExists = _context.Rooms.Any(c => c.RoomId == room.RoomId);

            if (roomExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Mã phòng đã tồn tại!", tab = "room" }); // Trả về view với thông báo lỗi
            }

            // Kiểm tra xem mã rạp có tồn tại không
            var cinemaExists = _context.Cinemas.Any(c => c.CinemaId == room.CinemaId);

            if (!cinemaExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Mã rạp không tồn tại!", tab = "room" });
            }

            // Kiểm tra xem tên phòng đã tồn tại trong rạp này chưa
            var existingRoom = _context.Rooms.Any(r => r.RoomName == room.RoomName && r.CinemaId == room.CinemaId);

            if (existingRoom)
            {
                return RedirectToAction("Error", new { errorMessage = "Tên phòng đã tồn tại!", tab = "room" });
            }

            if (ModelState.IsValid)
            {
                room.RoomStatus = new BitArray(1);
                room.RoomStatus[0] = (Request.Form["RoomStatus"] == "1");

                int rows = ((int)room.TotalSeats / 12) + ((int)room.TotalSeats % 12 > 0 ? 1 : 0);
                char rowLetter = 'A';

                _context.Rooms.Add(room);
                _context.SaveChanges();

                for (int i = 0; i < room.TotalSeats; i++)
                {
                    int seatNumber = (i % 12) + 1;

                    if (seatNumber == 1 && i > 0)
                    {
                        rowLetter++;
                    }

                    var seatName = $"{rowLetter}{seatNumber:D2}";
                    int sequentialSeatNumber = i + 1;
                    // Ghép RoomId với số thứ tự ghế
                    var seatId = $"{room.CinemaId}{room.RoomId}{sequentialSeatNumber}";

                    var seat = new Seat
                    {
                        // Sử dụng seatId mới tạo
                        SeatId = seatId,
                        RoomId = room.RoomId,
                        SeatName = seatName,
                        SeatType = new BitArray(new[] { false })
                    };

                    _context.Seats.Add(seat);
                }

                _context.SaveChanges();
                return RedirectToAction("Manage", new { tab = "room" });
            }

            return View("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "room" });
        }

        // Hiển thị form chỉnh sửa phòng
        [HttpGet]
        public IActionResult EditRoom(string id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // Lưu thông tin đã chỉnh sửa phòng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mã rạp có tồn tại không
                var cinemaExists = _context.Cinemas.Any(c => c.CinemaId == room.CinemaId);

                if (!cinemaExists)
                {
                    return RedirectToAction("Error", new { errorMessage = "Mã rạp không tồn tại!", tab = "room" });
                }

                // Kiểm tra xem tên phòng đã tồn tại trong rạp này chưa, ngoại trừ phòng hiện tại
                var existingRoom = _context.Rooms.Any(r => r.RoomName == room.RoomName && r.CinemaId == room.CinemaId && r.RoomId != room.RoomId);

                if (existingRoom)
                {
                    return RedirectToAction("Error", new { errorMessage = "Tên phòng đã tồn tại!", tab = "room" });
                }

                // Nếu không tồn tại, cập nhật phòng
                _context.Rooms.Update(room);
                _context.SaveChanges();
                return RedirectToAction("Manage", new { tab = "room" }); // Chuyển hướng đến trang quản lý
            }

            return View("Manage");
        }

        // Hiển thị confirm xóa phòng
        [HttpGet]
        public IActionResult DeleteRoom(string id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // Xác nhận xóa phòng
        [HttpPost, ActionName("DeleteRoom")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRoomConfirmed(string id)
        {
            var room = _context.Rooms
                        .Include(r => r.Seats)
                        .Include(r => r.Showtimes) // Bao gồm suất chiếu
                            .ThenInclude(s => s.Tickets) // Bao gồm vé liên quan đến suất chiếu
                        .FirstOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            // Xóa tất cả vé liên quan đến ghế
            foreach (var seat in room.Seats)
            {
                var tickets = _context.Tickets.Where(t => t.SeatId == seat.SeatId);
                _context.Tickets.RemoveRange(tickets);
            }

            // Xóa tất cả ghế liên quan
            _context.Seats.RemoveRange(room.Seats);

            // Xóa tất cả suất chiếu liên quan
            _context.Showtimes.RemoveRange(room.Showtimes);

            // Xóa phòng
            _context.Rooms.Remove(room);

            // Lưu thay đổi vào database
            _context.SaveChanges();

            return RedirectToAction("Manage", new { tab = "room" }); // Chuyển hướng về trang danh sách phòng chiếu
        }

        //---------------------------------------------------------------------------------

        // Thêm phim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mã phim đã tồn tại chưa
                var existingMovie = _context.Movies.FirstOrDefault(m => m.MovieId == movie.MovieId);
                if (existingMovie != null)
                {
                    return RedirectToAction("Error", new { errorMessage = "Mã phim đã tồn tại", tab = "film" });
                }

                if (!string.IsNullOrEmpty(Request.Form["ReleaseDate"]))
                {
                    movie.ReleaseDate = DateOnly.Parse(Request.Form["ReleaseDate"]);
                }

                //// Đường dẫn tới thư mục lưu ảnh
                //var webRootPath = Directory.GetCurrentDirectory(); // Lấy đường dẫn gốc của ứng dụng
                //var posterDirectory = Path.Combine(webRootPath, "wwwroot", "Content", "img", "poster");

                //// Tải lên poster
                //if (poster != null && poster.Length > 0)
                //{
                //    var posterFileName = Guid.NewGuid() + Path.GetExtension(poster.FileName);
                //    var posterPath = Path.Combine(posterDirectory, posterFileName);

                //    using (var stream = new FileStream(posterPath, FileMode.Create))
                //    {
                //        poster.CopyTo(stream);
                //    }

                //    movie.Poster = $"/Content/img/poster/{posterFileName}"; // Lưu URL vào thuộc tính ImageUrl
                //}
                
                // Thêm phim mới vào cơ sở dữ liệu
                _context.Movies.Add(movie);
                _context.SaveChanges(); // Lưu thay đổi
                return RedirectToAction("Manage", new { tab = "film" }); // Chuyển hướng đến trang danh sách phim
            }

            return RedirectToAction("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "film" });
        }

        // Hiển thị form chỉnh sửa phim
        [HttpGet]
        public IActionResult EditMovie(string id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // Lưu thông tin đã chỉnh sửa phim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mã phim có tồn tại không
                var movieExists = _context.Movies.Any(c => c.MovieId == movie.MovieId);

                if (!movieExists)
                {
                    return RedirectToAction("Error", new { errorMessage = "Mã phim không tồn tại!", tab = "film" });
                }

                if (!string.IsNullOrEmpty(Request.Form["ReleaseDate"]))
                {
                    movie.ReleaseDate = DateOnly.Parse(Request.Form["ReleaseDate"]);
                }

                _context.Movies.Update(movie);
                _context.SaveChanges();
                return RedirectToAction("Manage", new { tab = "film" }); // Chuyển hướng đến trang quản lý
            }

            return View("Manage");
        }

        // Hiển thị confirm xóa phim
        public IActionResult ConfirmDeleteMovie(string id)
        {
            var movie = _context.Movies.Find(id); // Lấy thông tin phim từ database
            var viewModel = new ConfirmDelete
            {
                ObjectType = "phim",
                ObjectName = movie.MovieName,
                Id = movie.MovieId,
                Action = "DeleteMovie",
                TabName = "film"
            };

            return View("ConfirmDelete", viewModel);
        }

        // Xác nhận xóa phim
        [HttpPost, ActionName("DeleteMovie")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMovieConfirmed(string id)
        {
            var movie = _context.Movies.Include(m => m.Showtimes).FirstOrDefault(m => m.MovieId == id); ;

            _context.Showtimes.RemoveRange(movie.Showtimes);
            if (movie != null)
            {
                _context.Movies.Remove(movie); // Xóa dữ liệu khỏi context
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
            return RedirectToAction("Manage", new { tab = "film" }); 
        }

        //---------------------------------------------------------------------------------

        // Thêm suất chiếu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateShowtime(Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Request.Form["ShowDate"]))
                {
                    showtime.ShowDate = DateOnly.Parse(Request.Form["ShowDate"]);
                }

                // Chuyển đổi thời gian bắt đầu
                if (!string.IsNullOrEmpty(Request.Form["StartTime"]))
                {
                    showtime.StartTime = TimeOnly.Parse(Request.Form["StartTime"]);
                }

                // Chuyển đổi thời gian kết thúc
                if (!string.IsNullOrEmpty(Request.Form["EndTime"]))
                {
                    showtime.EndTime = TimeOnly.Parse(Request.Form["EndTime"]);
                }

                // Lấy thông tin phim từ cơ sở dữ liệu
                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == showtime.MovieId);

                // Kiểm tra phim có tồn tại hay không
                if (movie == null)
                {
                    ViewBag.errorMessage = "Phim không tồn tại";
                    return View("Error", new { tab = "showtime" });
                }

                // Chuyển đổi thời gian bắt đầu và kết thúc sang TimeSpan để so sánh
                var startTimeSpan = new TimeSpan(showtime.StartTime?.Hour ?? 0, showtime.StartTime?.Minute ?? 0, 0);
                var endTimeSpan = new TimeSpan(showtime.EndTime?.Hour ?? 0, showtime.EndTime?.Minute ?? 0, 0);
                var movieDuration = TimeSpan.FromMinutes(movie.Duration.GetValueOrDefault());

                // Kiểm tra thời gian bắt đầu và kết thúc
                if (startTimeSpan >= endTimeSpan)
                {
                    ViewBag.errorMessage = "Thời gian không hợp lệ";
                    return View("Error", new {tab = "showtime"});
                }

                // Kiểm tra thời gian kết thúc có bằng thời lượng phim không
                if (endTimeSpan - startTimeSpan != movieDuration.Add(new TimeSpan(0, 20, 0)))
                {
                    ViewBag.errorMessage = "Thời gian kết thúc phải bằng thời lượng phim cộng thêm 20 phút để dọn dẹp rạp.";
                    return View("Error", new { tab = "showtime" });
                }

                // Thêm suất chiếu mới vào cơ sở dữ liệu
                _context.Showtimes.Add(showtime);
                _context.SaveChanges();

                // Thêm vé tự động dựa trên số ghế có trong phòng
                var seats = _context.Seats.Where(s => s.RoomId == showtime.RoomId).ToList();
                if (seats.Count == 0)
                {
                    ViewBag.errorMessage = "Không tìm thấy ghế nào trong phòng chiếu này.";
                    return View("Error", new { tab = "showtime" });
                }
                foreach (var seat in seats)
                {
                    var ticket = new Ticket
                    {
                        TicketId = showtime.ShowtimeId + seat.SeatId,
                        ShowtimeId = showtime.ShowtimeId,
                        SeatId = seat.SeatId,
                        TicketPrice = 70000,
                        IsBooked = false
                    };
                    _context.Tickets.Add(ticket);
                }

                _context.SaveChanges(); // Lưu thay đổi

                return RedirectToAction("Manage", new { tab = "showtime" });
            }

            return View("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "showtime" });
        }

        // Hiển thị form chỉnh sửa suất chiếu
        [HttpGet]
        public IActionResult EditShowtime(string id)
        {
            var cinemas = _context.Cinemas.ToList();
            var rooms = _context.Rooms.ToList();
            var movies = _context.Movies.ToList();

            ViewBag.Cinemas = cinemas;
            ViewBag.Rooms = rooms;
            ViewBag.Movies = movies;

            var showtime = _context.Showtimes.Find(id);
            if (showtime == null)
            {
                return NotFound();
            }
            return View(showtime);
        }

        // Lưu thông tin đã chỉnh sửa suất chiếu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditShowtime(Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Request.Form["ShowDate"]))
                {
                    showtime.ShowDate = DateOnly.Parse(Request.Form["ShowDate"]);
                }

                if (!string.IsNullOrEmpty(Request.Form["StartTime"]))
                {
                    showtime.StartTime = TimeOnly.Parse(Request.Form["StartTime"]);
                }

                if (!string.IsNullOrEmpty(Request.Form["EndTime"]))
                {
                    showtime.EndTime = TimeOnly.Parse(Request.Form["EndTime"]);
                }

                // Lấy thông tin phim từ cơ sở dữ liệu
                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == showtime.MovieId);

                // Kiểm tra phim có tồn tại hay không
                if (movie == null)
                {
                    ViewBag.errorMessage = "Phim không tồn tại";
                    return View("Error", new { tab = "showtime" });
                }

                // Chuyển đổi thời gian bắt đầu và kết thúc sang TimeSpan để so sánh
                var startTimeSpan = new TimeSpan(showtime.StartTime?.Hour ?? 0, showtime.StartTime?.Minute ?? 0, 0);
                var endTimeSpan = new TimeSpan(showtime.EndTime?.Hour ?? 0, showtime.EndTime?.Minute ?? 0, 0);
                var movieDuration = TimeSpan.FromMinutes(movie.Duration.GetValueOrDefault());

                // Kiểm tra thời gian bắt đầu và kết thúc
                if (startTimeSpan >= endTimeSpan)
                {
                    ViewBag.errorMessage = "Thời gian không hợp lệ";
                    return View("Error", new { tab = "showtime" });
                }

                // Kiểm tra thời gian kết thúc có bằng thời lượng phim không
                if (endTimeSpan - startTimeSpan != movieDuration)
                {
                    ViewBag.errorMessage = "Thời gian kết thúc phải bằng với thời lượng phim";
                    return View("Error", new { tab = "showtime" });
                }

                _context.Showtimes.Update(showtime);
                _context.SaveChanges();
                return RedirectToAction("Manage", new { tab = "showtime" }); // Chuyển hướng đến trang quản lý
            }

            return View("Manage", new { tab = "showtime" });
        }

        // Hiển thị confirm xóa suất chiếu
        public IActionResult ConfirmDeleteShowtime(string id)
        {
            var showtime = _context.Showtimes.Find(id); // Lấy thông tin phim từ database
            var viewModel = new ConfirmDelete
            {
                ObjectType = "suất chiếu",
                ObjectName = showtime.ShowtimeId,
                Id = showtime.ShowtimeId,
                Action = "DeleteShowtime",
                TabName = "showtime"
            };

            return View("ConfirmDelete", viewModel);
        }

        // Xác nhận xóa suất chiếu
        [HttpPost, ActionName("DeleteShowtime")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteShowtimeConfirmed(string id)
        {
            var showtime = _context.Showtimes.Include(s => s.Tickets).FirstOrDefault(s => s.ShowtimeId == id);

            if (showtime == null)
            {
                return NotFound();
            }

            // Xóa tất cả ghế liên quan
            _context.Tickets.RemoveRange(showtime.Tickets);

            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime); // Xóa dữ liệu khỏi context
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
            return RedirectToAction("Manage", new { tab = "showtime" });
        }

        // Chuyển hướng trang quản lý vé
        [HttpGet]
        public IActionResult ManageTicket(String id)
        {
            var tickets = _context.Tickets.Where(t => t.ShowtimeId == id).ToList();
            var movies = _context.Movies.ToList();
            var showtime = _context.Showtimes.ToList();
            var seats = _context.Seats.ToList();

            ViewBag.Movies = movies;
            ViewBag.Showtimes = showtime;
            ViewBag.Tickets = tickets;
            ViewBag.Seats = seats;
            return View();
        }

        //--------------------------------------------------------------------------------------------------------------------------------

        // Thêm món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateConcession(Concession concession)
        {
            var fooodExists = _context.Concessions.Any(c => c.ConcessionId == concession.ConcessionId);

            if (fooodExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Mã thức ăn đã tồn tại!", tab = "food" });
            }
            
            if (ModelState.IsValid)
            {
                concession.ConcessionStatus = new BitArray(1);
                concession.ConcessionStatus[0] = (Request.Form["ConcessionStatus"] == "1");

                _context.Concessions.Add(concession);
                _context.SaveChanges();

                return RedirectToAction("Manage", new { tab = "food" });
            }

            return View("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "food" });
        }

        // Hiển thị xác nhận xóa
        public IActionResult ConfirmDeleteConcession(string id)
        {
            var concession = _context.Concessions.Find(id); // Lấy thông tin phim từ database
            var viewModel = new ConfirmDelete
            {
                ObjectType = "món ăn",
                ObjectName = concession.ConcessionName,
                Id = concession.ConcessionId,
                Action = "DeleteConcession",
                TabName = "food"
            };

            return View("ConfirmDelete", viewModel);
        }

        // Xác nhận xóa món ăn
        [HttpPost, ActionName("DeleteConcession")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConcession(string id)
        {
            var concession = _context.Concessions.Find(id);

            if (concession != null)
            {
                _context.Concessions.Remove(concession); // Xóa dữ liệu khỏi context
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
            return RedirectToAction("Manage", new { tab = "food" });
        }

        //------------------------------------------------------------------------------------------------------------------

        // Thêm món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCombo(Combo combo)
        {
            var fooodExists = _context.Combos.Any(c => c.ComboId == combo.ComboId);

            if (fooodExists)
            {
                return RedirectToAction("Error", new { errorMessage = "Mã thức ăn đã tồn tại!", tab = "food" });
            }

            if (ModelState.IsValid)
            {
                combo.ComboStatus = new BitArray(1);
                combo.ComboStatus[0] = (Request.Form["ComboStatus"] == "1");

                _context.Combos.Add(combo);
                _context.SaveChanges();

                return RedirectToAction("Manage", new { tab = "food" });
            }

            return View("Error", new { errorMessage = "Dữ liệu không hợp lệ", tab = "food" });
        }

        // Hiển thị xác nhận xóa
        public IActionResult ConfirmDeleteCombo(string id)
        {
            var combo = _context.Combos.Find(id);
            var viewModel = new ConfirmDelete
            {
                ObjectType = "combo",
                ObjectName = combo.ComboName,
                Id = combo.ComboId,
                Action = "DeleteCombo",
                TabName = "food"
            };

            return View("ConfirmDelete", viewModel);
        }

        // Xác nhận xóa combo
        [HttpPost, ActionName("DeleteCombo")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCombo(string id)
        {
            var combo = _context.Combos.Include(c => c.Comboitems).FirstOrDefault(c => c.ComboId == id);

            if (combo != null)
            {
                _context.Comboitems.RemoveRange(combo.Comboitems);
                _context.Combos.Remove(combo); // Xóa dữ liệu khỏi context
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
            return RedirectToAction("Manage", new { tab = "food" });
        }
    }
}
