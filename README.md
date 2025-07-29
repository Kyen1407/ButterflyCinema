# 🎮 Butterfly Cinema - Cinema Management System

## 📋 Giới thiệu

**Butterfly Cinema** là hệ thống quản lý rạp chiếu phim hiện đại, xây dựng trên nền tảng **ASP.NET Core Razor Pages (.NET 6)** với cơ sở dữ liệu **PostgreSQL**.
Dự án hỗ trợ quản lý toàn diện các nghiệp vụ rạp chiếu phim như: rạp, phòng chiếu, ghế, phim, suất chiếu, vé, món ăn, combo, đặt vé, thanh toán, người dùng, nhân viên, phân quyền, quảng cáo...

---

### 🏠 Home Page
![Home Page](./Screenshots/Home1.png)

## 🚀 Tính năng nổi bật

* **Quản lý rạp (Cinema):** Thêm, sửa, xóa, xem danh sách rạp.
* **Phòng chiếu và ghế (Room & Seat):** Tạo phòng và sinh ghế tự động.
* **Phim (Movie):** Quản lý phim, poster, ngày phát hành, thời lượng,...
* **Suất chiếu (Showtime):** Quản lý lịch chiếu, kiểm tra trùng suất.
* **Vé (Ticket):** Sinh vé theo ghế và suất chiếu, đặt vé, kiểm tra trạng thái.
* **Món ăn & Combo (Concession, Combo, ComboItem):** Quản lý sản phẩm, tình trạng bán.
* **Hóa đơn & Thanh toán (Invoice, Payment):** Lưu thông tin thanh toán, hình thức thanh toán.
* **Người dùng & Phân quyền (User, Staff, Role):** Đăng nhập, phân quyền.
* **Quảng cáo (Ads):** Quản lý quảng cáo hiển thị trên trang chủ.
* **Đặt vé online:** Chọn phim, suất chiếu, ghế và thanh toán trực tuyến.

---

## 🏧 Kiến trúc & Công nghệ

| Thành phần            | Công nghệ sử dụng                 |
| --------------------- | --------------------------------- |
| Backend               | ASP.NET Core Razor Pages (.NET 6) |
| ORM                   | Entity Framework Core             |
| Cơ sở dữ liệu         | PostgreSQL                        |
| Frontend              | Razor Pages, Bootstrap, jQuery    |
| Xác thực & phân quyền | Role-based Authorization          |
| Migration             | EF Core Migration                 |

---

## 📁 Cấu trúc thư mục

```
ButterflyCinema/
│
├── Controllers/           # Xử lý logic cho các Razor Pages
├── Models/                # Chứa các Entity, ViewModel, và DbContext
├── Migrations/            # Các file migration của Entity Framework
├── Views/                 # Razor Pages (.cshtml)
│   ├── Shared/            # Layouts, partial views dùng chung
│   └── [ChứcNăng]/		   # Các trang chức năng cụ thể
├── wwwroot/               # Static files (JS, CSS, images, fonts)
│   └── Content/
│       └── js/            # JavaScript cho từng chức năng
│       └── css/           # Css cho từng chức năng
│       └── img/           # Ảnh của dự án
│       └── font/          # Font chữ cho dự án
├── appsettings.json       # File cấu hình (kết nối DB, logging, ...)
├── Program.cs             # Điểm khởi tạo ứng dụng
└── README.md              # Tài liệu mô tả dự án
```

---

## 🧰 Mô hình dữ liệu (16 bảng chính)

* `Cinema` – Rạp chiếu phim
* `Room` – Phòng chiếu
* `Seat` – Ghế trong phòng
* `Movie` – Phim
* `Showtime` – Suất chiếu
* `Ticket` – Vé
* `Concession` – Món ăn
* `Combo` – Gói combo
* `ComboItem` – Chi tiết combo
* `Invoice` – Hóa đơn
* `Payment` – Thanh toán
* `User` – Người dùng
* `Role` – Vai trò người dùng
* `Staff` – Nhân viên
* `Booking` – Đặt vé
* `Ads` – Quảng cáo

---

## ⚙️ Hướng dẫn cài đặt & sử dụng

### 🔧 1. Yêu cầu hệ thống

* [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* [PostgreSQL](https://www.postgresql.org/download/)

### 📦 2. Cài đặt

**Clone dự án:**

```bash
git clone <repository-url>
cd ButterflyCinema
```

**Cấu hình chuỗi kết nối:**

* Mở file `appsettings.json`
* Cập nhật phần `"ConnectionStrings": { "CinemaDb": "..." }` với thông tin PostgreSQL của bạn.

**Khởi tạo cơ sở dữ liệu:**

```bash
dotnet ef database update
```

> 💡 Nếu chưa có migration, dùng `dotnet ef migrations add InitialCreate` trước.

### ▶️ 3. Chạy dự án

```bash
dotnet run
```

* Truy cập: `https://localhost:7116` hoặc theo cổng bạn đã cấu hình.

