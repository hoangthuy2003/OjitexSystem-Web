# OjitexSystem

Ứng dụng ASP.NET Core / EF Core (.NET 10) và Angular standalone (22.2), dùng hai cơ sở dữ liệu SQL Server: EVOLIO cho tài khoản và phân quyền, OJITEXHP cho dữ liệu sản xuất và tồn kho.

## Phạm vi tính năng hiện có

| Nhóm | Tính năng |
| --- | --- |
| Xác thực | Đăng nhập JWT, lưu phiên, đăng xuất, đổi mật khẩu |
| Quản trị | Danh sách tài khoản, tạo/sửa, role, quyền category, đặt lại mật khẩu, xóa mềm |
| Logistics | Current / Last / Monthly Stock, tìm theo tiền tố mã sản phẩm, lịch sử theo sản phẩm, in và sao chép bảng |
| Factory | Corrugator Online: kết quả chạy máy trong ngày UTC+7, Refresh lấy lại dữ liệu, Clear xóa bảng hiển thị |
| Điều hướng | Trang chủ, danh mục phòng ban, danh mục báo cáo |

Các mục Work Progress, Management Loss, FFG và những phòng ban chưa có chức năng vẫn là mục dự kiến trong danh mục điều hướng. Không tạo module nghiệp vụ rỗng cho các mục này.

Xem [kiến trúc và kết quả rà soát](docs/architecture.md) để biết ranh giới trách nhiệm, cấu trúc thư mục và phạm vi kiểm thử.

Trước khi phát triển hoặc sửa code, đọc [quy chuẩn kiến trúc và phong cách code](docs/development-standards.md). [AGENTS.md](AGENTS.md) đặt quy tắc làm việc cho coding agent trong toàn repository, để các yêu cầu sau tiếp tục theo cùng kiến trúc.

## Backend

Chạy từ thư mục gốc repository:

```powershell
dotnet restore OjitexSystem_Backend/OjitexSystem_Backend.slnx
dotnet build OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
dotnet test OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
dotnet run --project OjitexSystem_Backend/OjitexSystem-Backend --launch-profile https
```

Cấu hình `ConnectionStrings:EVOLIO`, `ConnectionStrings:OJITEXHP` bằng cấu hình local hoặc biến môi trường. `appsettings.json` local được gitignore. Ngoài môi trường Development, phải cung cấp `Authentication:SigningKey` có ít nhất 32 byte UTF-8; có thể cấu hình thêm issuer, audience và `Frontend:Origin`.

Profile HTTPS phục vụ API tại `https://localhost:7013`; tài liệu Scalar nằm tại `/scalar` trong Development. Nếu tiến trình đang chạy giữ khóa file build, có thể kiểm tra ở thư mục output riêng:

```powershell
dotnet test OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore -p:OutputPath=bin/Verification/
```

## Frontend

```powershell
cd OjitexSystem_Frontend
npm.cmd ci
npm.cmd start
npm.cmd run build
npm.cmd run test:ci
npm.cmd run format:check
```

Frontend chạy tại `http://localhost:4200`; proxy `/api` đến backend HTTPS. `npm.cmd` dùng được trên Windows khi execution policy không cho chạy `npm.ps1`; trên hệ điều hành khác dùng `npm`.
