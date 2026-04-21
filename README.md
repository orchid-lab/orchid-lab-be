# 🌿 OrchidLab Backend API - Hệ thống Quản lý Phòng Thí Nghiệm Hoa Lan

## 📖 Giới thiệu Dự án
**OrchidLab Backend** là "trái tim" của hệ sinh thái **Orchid Research & Lab Management System**. Hệ thống API này đóng vai trò xử lý toàn bộ logic nghiệp vụ, quản lý dữ liệu tập trung và cung cấp giao tiếp mượt mà cho các nền tảng người dùng (Mobile App & Web App).

Được xây dựng trên nền tảng vi kiến trúc hiện đại với độ khả dụng cao, hệ thống đảm bảo luồng dữ liệu liên tục cho các Nhà nghiên cứu (Researcher), Kỹ thuật viên (Technician) và Ban quản trị (Admin) trong công tác nghiên cứu sinh học, quản lý mẫu vật và tích hợp AI nhận diện bệnh hại.

## ✨ Tính năng Nổi bật (Core Features)
* 🔐 **Authentication & Authorization (JWT):** Phân quyền nghiêm ngặt với 3 Role chính: Admin, Researcher, Technician.
* 🚀 **Tối ưu Hóa với Redis Caching:** Bộ nhớ đệm Redis giúp tăng tốc độ truy xuất các dữ liệu thường xuyên (như danh sách bệnh, danh mục phòng Lab) và quản lý session.
* 📸 **Lưu trữ Đám mây (Cloudinary):** Tích hợp Cloudinary API để tự động upload, tối ưu hóa và quản lý hình ảnh mẫu vật, nhật ký thí nghiệm.
* 🧪 **Quản lý Vòng đời Sinh học:** API xử lý CRUD phức tạp cho các Lô cấy mô (Batches), Cây giống (Seedlings) và Mẫu vật (Samples).
* 📋 **Quản lý Task & Tiến độ:** Luồng giao việc theo thời gian thực từ Researcher xuống Technician.
* 🤖 **AI Model Gateway:** Đóng vai trò là cầu nối (Proxy) tiếp nhận hình ảnh từ Mobile App, chuyển tiếp sang Server Python Flask (YOLOv8) để phân tích bệnh và trả về kết quả.

## 🛠️ Công nghệ Sử dụng (Tech Stack)

* **Ngôn ngữ & Framework:** C# 12, .NET 8 (ASP.NET Core Web API)
* **Hệ quản trị CSDL:** PostgreSQL (Relational Database)
* **ORM:** Entity Framework Core (Code-First Approach)
* **Caching & Queue:** Redis
* **Cloud Media:** Cloudinary
* **Deployment & DevOps:** Docker, Docker Compose, triển khai trên Digital Ocean Droplet.

## 🚀 Hướng dẫn Cài đặt & Khởi chạy (Sử dụng Docker)

### 1. Yêu cầu hệ thống (Prerequisites)
* **Docker & Docker Compose:** Bắt buộc phải có để chạy môi trường container.
* **.NET 8 SDK:** (Nếu muốn chạy hoặc debug trực tiếp trên máy không qua Docker).
* Các tài khoản dịch vụ Cloudinary để cấu hình khóa bảo mật.

### 2. Cài đặt mã nguồn & Môi trường
Clone dự án về máy tính:
```bash
git clone https://github.com/your-org/orchid-lab-backend.git
cd orchid-lab-backend
```

### 3. Cấu hình AppSettings / Biến môi trường
Tạo file `.env` hoặc chỉnh sửa `appsettings.Development.json` tại thư mục API với các thông số:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=OrchidLabDb;Username=postgres;Password=yourpassword"
},
"Redis": {
  "ConnectionString": "localhost:6379"
},
"Cloudinary": {
  "CloudName": "your_cloud_name",
  "ApiKey": "your_api_key",
  "ApiSecret": "your_api_secret"
},
"Jwt": {
  "Key": "your_super_secret_key_here",
  "Issuer": "OrchidLab",
  "Audience": "OrchidLabUsers"
}
```

### 4. Build và Chạy dự án (Chỉ với 1 lệnh)
Hệ thống đã được cấu hình sẵn `docker-compose.yml` để dựng cả CSDL PostgreSQL, Redis và Backend API cùng lúc. Mở terminal và chạy:
```bash
docker-compose up -d --build
```
*API sẽ tự động khởi chạy và Swagger UI sẽ có mặt tại: `http://localhost:5000/swagger/index.html`*

### 5. (Tùy chọn) Chạy Migrations thủ công
Nếu bạn chạy trên máy host không qua Docker:
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

## 📁 Cấu trúc Thư mục (Clean Architecture)

```text
OrchidLabBackend/
├── src/
│   ├── API/                  # Controllers, Middlewares, Program.cs (Entry point)
│   ├── Application/          # Business logic, Services, Interfaces, DTOs
│   ├── Domain/               # Các Entities (Models lõi), Enums, Exceptions
│   └── Infrastructure/       # ApplicationDbContext, Repositories, CloudinaryService, RedisCache
├── tests/                    # Unit Tests & Integration Tests (xUnit / NUnit)
├── Dockerfile                # Config build Docker cho .NET App
└── docker-compose.yml        # Config dựng toàn bộ cụm Postgres, Redis, .NET
```

## 📏 Quy chuẩn Code (Coding Conventions)
Dự án tuân thủ nghiêm ngặt các quy tắc quản lý chất lượng (PMP) dành cho .NET:
* **Tên Biến (Variables):** Sử dụng `camelCase` (VD: `routeType`, `accountService`).
* **Tên Hàm & Lớp (Functions & Classes):** Sử dụng `PascalCase` (VD: `UpdateToPaid`, `Register`, `LabRoom`).
* **Định dạng code (Layout):** * Viết đúng 1 câu lệnh/khai báo trên một dòng.
  * Thụt lề đúng 4 khoảng trắng (4 spaces/1 tab).
  * Thêm ít nhất 1 dòng trống (blank line) giữa các định nghĩa hàm (method) hoặc thuộc tính (property).
* Khuyến khích sử dụng ngoặc đơn `()` để làm rõ các điều kiện trong biểu thức phức tạp.


