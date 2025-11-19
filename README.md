📦 Getting Started – Agora

Dự án Agora được xây dựng theo kiến trúc N-Layers / Clean Architecture gồm các tầng:

Agora.Domain

Agora.Application

Agora.Infrastructure

Agora.Auth

Agora.Payment

Agora.API (Web API)

Hướng dẫn dưới đây giúp bạn clone, khôi phục packages, và chạy API sau khi tải dự án về.

🚀 1. Clone Project
git clone https://github.com/<username>/Agora.git
cd Agora

📥 2. Restore Dependencies

Tải toàn bộ NuGet packages cho solution:

dotnet restore

▶️ 3. Run the API

Đi vào project API và chạy:

cd Agora.API
dotnet run


Sau khi chạy thành công, API sẽ khởi động tại:

http://localhost:5000
https://localhost:5001

🛢 4. Database (nếu dùng EF Core)

Nếu dự án sử dụng Entity Framework Core và có migrations:

Cài dotnet-ef (nếu chưa có)
dotnet tool install --global dotnet-ef

Update database
dotnet ef database update

⚙️ 5. App Settings (Nếu không được commit)

Nếu appsettings.json không nằm trong repo, bạn cần tự tạo:

File: Agora.API/appsettings.json

Thêm các keys cần thiết (JWT, ConnectionStrings, v.v.)
Hoặc lưu secrets bằng lệnh:

cd Agora.API
dotnet user-secrets set "Jwt:Key" "your_jwt_key_here"

🧩 6. Mở Project
Visual Studio

Mở file:

Agora.sln

VS Code
code .

📝 7. Project Structure
Agora/
 ├── Agora.sln
 ├── Agora.Domain/
 ├── Agora.Application/
 ├── Agora.Infrastructure/
 ├── Agora.Auth/
 ├── Agora.Payment/
 ├── Agora.API/
 └── .gitignore