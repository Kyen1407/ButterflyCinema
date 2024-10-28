namespace ButterflyCinema.Models
{
    public class ConfirmDelete
    {
        public string ObjectType { get; set; } // Ví dụ: "Rạp", "Phim", "Suất Chiếu"
        public string ObjectName { get; set; } // Tên của đối tượng cần xóa
        public string Id { get; set; } // ID của đối tượng
        public string Action { get; set; } // Tên action để xử lý xóa
        public string TabName { get; set; } // Tên tab để quay về
    }
}
