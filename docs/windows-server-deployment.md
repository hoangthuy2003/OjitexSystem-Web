# Hướng dẫn Phân tích Kiến trúc và Triển khai OjitexSystem lên Windows Server (IIS)

Tài liệu này phân tích chi tiết bản kế hoạch triển khai tổng quát (tài liệu PDF đính kèm), đối chiếu trực tiếp với mã nguồn thực tế của dự án **OjitexSystem** (`.NET 10.0` + `Angular 22.2` + `2 CSDL SQL Server`), từ đó đưa ra quy trình đóng gói và cấu hình chuẩn xác nhất trên máy chủ Windows Server nội bộ (LAN).

---

## Phần 1: Phân tích Đối chiếu Tài liệu Đính kèm với Mã nguồn `OjitexSystem`

Tài liệu PDF đính kèm đã mô tả chính xác nền tảng lý thuyết về **IIS**, **ASP.NET Core Module (ANCM - In-Process Hosting)**, **Application Pool Identity** và **Windows Firewall**. Tuy nhiên, khi áp dụng vào mã nguồn thực tế của **OjitexSystem**, có **6 điểm đặc thù sống còn** trong code mà người triển khai bắt buộc phải nắm rõ để tránh lỗi `HTTP 500.30` hoặc lỗi `404 API`:

### 1.1. Lựa chọn Kiến trúc: `OjitexSystem` đã được thiết kế sẵn cho Phương án Hợp nhất (Unified Hosting)

Trong tài liệu PDF (trang 4–5) nêu ra 2 phương án tổ chức giữa Angular và ASP.NET Core trên IIS:
- **Phương án 1 (Tách rời 2 cổng hoặc Sub-Application)**: Tạo 2 Website trên IIS (ví dụ cổng 80 cho Frontend, cổng 5000 cho Backend).
- **Phương án 2 (Hợp nhất trong `wwwroot` của Backend)**: Đặt bản build của Angular vào thư mục `wwwroot` của ASP.NET Core và chạy trên **một Website IIS duy nhất**.

**Đối chiếu mã nguồn thực tế của `OjitexSystem`**:
1. **Ở phía Frontend**: Toàn bộ các service gọi API (`AuthService`, `AdminDashboardService`, `StockReportService`, `CorrugatorOnlineService`, `ConvertingOnlineService`) và `authInterceptor` (`OjitexSystem_Frontend/src/app/core/auth/auth.interceptor.ts`) đều đang sử dụng **đường dẫn tương đối `/api/...`**:
   ```typescript
   if (!request.url.startsWith('/api/') || request.url.split('?')[0] === '/api/auth/login') {
     return next(request);
   }
   ```
   Khi chạy ở máy cá nhân (`ng serve`), Angular dùng `proxy.conf.json` để chuyển tiếp `/api` sang `https://localhost:7013`. Khi biên dịch ra bản Production (`ng build`), `proxy.conf.json` **không còn tồn tại**. Nếu bạn triển khai tách làm 2 cổng độc lập (ví dụ Frontend cổng 80, Backend cổng 5000) mà không sửa code hoặc không cài thêm ARR Reverse Proxy trên IIS, trình duyệt sẽ gọi `http://<IP_Server>:80/api/auth/login` vào trang tĩnh Angular và nhận lỗi `404 Not Found` / `405 Method Not Allowed`.
2. **Ở phía Backend (`OjitexSystem_Backend/OjitexSystem-Backend/Program.cs`)**: Mã nguồn **đã được cấu hình hoàn chỉnh cho Phương án Hợp nhất**:
   ```csharp
   app.UseDefaultFiles();
   app.UseStaticFiles();
   app.UseCors("Frontend");
   app.UseAuthentication();
   app.UseAuthorization();

   app.MapControllers();
   // API không tồn tại trả 404.
   app.MapFallback("/api/{**path}", () => Results.NotFound());

   // Các URL giao diện được Angular xử lý.
   app.MapFallbackToFile("index.html");
   ```

> **Kết luận Kiến trúc cho `OjitexSystem`**:
> Bắt buộc ưu tiên sử dụng **Phương án Hợp nhất (Unified Hosting)** — đưa kết quả build của Angular vào thư mục `wwwroot` trong gói publish của Backend và chỉ tạo **1 Website duy nhất trên IIS**.
> - **Không cần sửa bất kỳ dòng code nào** ở cả Frontend lẫn Backend.
> - **Không gặp lỗi CORS** vì giao diện và API chạy cùng một Origin (`http://<IP_Server>:<Port>`).
> - **Không lo lỗi 404 khi nhấn F5 (Refresh)** trên các đường dẫn con (`/logistic/...`, `/factory/...`, `/admin/...`), vì `app.MapFallbackToFile("index.html")` của ASP.NET Core đã tự động trả về `index.html` cho các route giao diện và trả chuẩn `404` cho các `/api/...` không tồn tại.
> - Chỉ cần mở **1 cổng mạng duy nhất** trên Windows Firewall.

---

### 1.2. Bảng Đối chiếu Chi tiết giữa Tài liệu PDF và Thực tế Dự án `OjitexSystem`

| Hạng mục | Nội dung trong Tài liệu PDF | Thực tế trong Mã nguồn `OjitexSystem` | Hành động Chính xác Cần làm |
| :--- | :--- | :--- | :--- |
| **Phiên bản .NET Hosting Bundle** | Nêu ví dụ `.NET 8.0, 9.0 hoặc 10.0`. | File `OjitexSystem-Backend.csproj` chỉ định `<TargetFramework>net10.0</TargetFramework>`. | Bắt buộc cài đặt **.NET 10.0 Hosting Bundle** trên Windows Server. Cài bản .NET 8 hoặc 9 sẽ bị lỗi **HTTP 500.31**. |
| **Đường dẫn xuất bản Angular (`dist`)** | Nêu chung là lấy các tệp trong thư mục `dist`. | `angular.json` dùng builder mới `@angular/build:application` của Angular 22. Đầu ra thực tế nằm tại `dist/OjitexSystem_Frontend/browser`. | Phải copy **phần ruột bên trong thư mục `browser`** (nơi có `index.html`) vào `wwwroot`. Không copy cả thư mục vỏ `browser`. |
| **Khóa ký JWT (`SigningKey`) khi lên Production** | Không đề cập (chỉ nhắc chung về User Secrets và `appsettings.Production.json`). | Trong `ServiceCollectionExtensions.cs`, khi `ASPNETCORE_ENVIRONMENT` khác `Development`, nếu thiếu `Authentication:SigningKey` (tối thiểu 32 byte), ứng dụng **chủ động ném ngoại lệ `InvalidOperationException` và sập ngay khi khởi động**. | Bắt buộc khai báo `Authentication:SigningKey` (chuỗi tối thiểu 32 ký tự) trong `appsettings.Production.json` trên máy chủ để không bị lỗi **HTTP 500.30**. |
| **Chuỗi kết nối SQL Server (`ConnectionStrings`)** | Ví dụ 1 chuỗi kết nối cơ sở dữ liệu duy nhất. | `ServiceCollectionExtensions.cs` đăng ký **2 DbContext tách biệt** kết nối tới **2 cơ sở dữ liệu**:<br>1. `EVOLIO` (`AuthContext` - Tài khoản & Phân quyền)<br>2. `OJITEXHP` (`ProductionContext` - Sản xuất & Tồn kho) | Phải cấu hình đầy đủ cả 2 chuỗi kết nối `EVOLIO` và `OJITEXHP` trong `appsettings.Production.json`, kèm theo `TrustServerCertificate=True;` (do dùng `Microsoft.Data.SqlClient` 7.1.1 trong mạng LAN). |
| **Định tuyến SPA (URL Rewrite)** | Bắt buộc dùng `web.config` với `<rewrite>` cho trang Angular độc lập. | Khi gộp vào `wwwroot` của Backend (In-Process), mọi request đi qua ANCM vào pipeline ASP.NET Core và được xử lý bởi `app.UseStaticFiles()` + `app.MapFallbackToFile("index.html")`. | Vẫn nên cài **IIS URL Rewrite Module** trên Server theo chuẩn hạ tầng, nhưng **không được ghi đè `web.config` của Angular đè lên `web.config` tự sinh của ASP.NET Core** trong thư mục gốc của Backend. |
| **Thời gian nghiệp vụ (Timezone)** | Không đề cập. | Dự án dùng `TimeProvider` và `BusinessTime` cố định múi giờ nghiệp vụ **UTC+7**, thời hạn JWT tính theo **UTC**. | Đảm bảo đồng hồ hệ thống (System Clock) của máy chủ Windows Server đồng bộ chính xác thời gian thực (NTP). |

---

## Phần 2: Quy trình Triển khai Chuẩn xác Từng bước lên Windows Server

### Giai đoạn 1: Chuẩn bị Hạ tầng trên Máy chủ Windows Server

Thực hiện trực tiếp trên máy chủ **Windows Server** với quyền **Administrator** theo đúng thứ tự sau:

#### Bước 1.1: Bật vai trò Web Server (IIS)
1. Mở **Server Manager** → **Manage** → **Add Roles and Features**.
2. Tại mục **Server Roles**, tích chọn **Web Server (IIS)**.
3. Tại mục **Role Services**, giữ nguyên các mục mặc định (`Static Content`, `Default Document`, `HTTP Errors`,...) và kiểm tra thêm:
   - Mở rộng **Web Management Tools** → đảm bảo đã chọn **IIS Management Console**.
   - Mở rộng **Web Server** → **Health and Diagnostics** → chọn **HTTP Logging**.
4. Nhấn **Install** và chờ hoàn tất.

#### Bước 1.2: Cài đặt .NET 10.0 Hosting Bundle (Bắt buộc sau Bước 1.1)
1. Tải bộ cài đặt **ASP.NET Core 10.0 Runtime - Windows Hosting Bundle Installer** (`dotnet-hosting-10.0.x-win.exe`) từ trang chính thức của Microsoft.
2. Chạy file cài đặt với quyền **Administrator**.
3. Sau khi cài xong, mở **Command Prompt (Admin)** hoặc **PowerShell (Admin)** và chạy lệnh khởi động lại dịch vụ IIS để nạp module `ANCM` (`AspNetCoreModuleV2`):
   ```powershell
   net stop was /y
   net start w3svc
   ```
4. Kiểm tra lại bằng lệnh:
   ```powershell
   dotnet --list-runtimes
   ```
   **Yêu cầu kết quả**: Phải xuất hiện cả `Microsoft.AspNetCore.App 10.0.x` và `Microsoft.NETCore.App 10.0.x`.

> **Lưu ý**: Nếu lỡ cài `.NET 10.0 Hosting Bundle` trước khi bật IIS ở Bước 1.1, hãy chạy lại bộ cài Hosting Bundle và chọn **Repair**.

---

### Giai đoạn 2: Biên dịch và Đóng gói (Build & Publish) trên Máy tính Cá nhân

Theo quy chuẩn của dự án (`AGENTS.md`), chúng ta **không sửa đổi cấu trúc source code hay đưa file cấu hình chứa mật khẩu vào Git**, mà thực hiện đóng gói sạch ra một thư mục phát hành riêng (ví dụ `C:\Publish\OjitexSystem`).

Mở **PowerShell** tại thư mục gốc của dự án (`c:\Code\OjitexSystem`) và thực hiện tuần tự:

#### Bước 2.1: Biên dịch và Publish Backend ASP.NET Core (.NET 10)
```powershell
# 1. Xóa thư mục publish cũ (nếu có) để đảm bảo gói sạch
if (Test-Path "C:\Publish\OjitexSystem") { Remove-Item "C:\Publish\OjitexSystem" -Recurse -Force }

# 2. Publish Backend ở chế độ Release (Framework-Dependent, x64/Portable)
dotnet publish OjitexSystem_Backend/OjitexSystem-Backend/OjitexSystem-Backend.csproj -c Release -o C:\Publish\OjitexSystem
```
Sau bước này, thư mục `C:\Publish\OjitexSystem` sẽ chứa `OjitexSystem-Backend.dll`, `OjitexSystem-Backend.exe`, các thư viện `.dll` và tệp `web.config` tự sinh cho IIS In-Process.

#### Bước 2.2: Biên dịch Frontend Angular 22 và Hợp nhất vào `wwwroot`
```powershell
# 1. Di chuyển vào thư mục Frontend và chạy build Production
Push-Location OjitexSystem_Frontend
npm.cmd run build
Pop-Location

# 2. Tạo thư mục wwwroot bên trong gói Publish của Backend
New-Item -ItemType Directory -Force -Path "C:\Publish\OjitexSystem\wwwroot" | Out-Null

# 3. Sao chép toàn bộ RUỘT bên trong dist/OjitexSystem_Frontend/browser vào wwwroot
Copy-Item -Path "OjitexSystem_Frontend\dist\OjitexSystem_Frontend\browser\*" -Destination "C:\Publish\OjitexSystem\wwwroot" -Recurse -Force
```

#### Bước 2.3: Kiểm tra Cấu trúc Gói Xuất bản (`C:\Publish\OjitexSystem`)
Trước khi chép lên máy chủ, hãy đảm bảo thư mục `C:\Publish\OjitexSystem` có đúng cấu trúc sau:
```text
C:\Publish\OjitexSystem\
├── OjitexSystem-Backend.dll
├── OjitexSystem-Backend.exe
├── web.config                        <-- Tệp tự sinh của .NET (chứa AspNetCoreModuleV2)
├── appsettings.json                  <-- (Nếu có, không chứa secret thật khi lưu source)
├── appsettings.Production.json       <-- Sẽ tạo/cấu hình ở Giai đoạn 3 trên Server
└── wwwroot\                          <-- Chứa bản build của Angular 22
    ├── index.html                    <-- Phải nằm ngay dưới wwwroot\index.html
    ├── favicon.ico
    ├── main-XXXXXXXX.js
    ├── polyfills-XXXXXXXX.js
    ├── chunk-XXXXXXXX.js
    └── styles-XXXXXXXX.css
```
> **Cảnh báo quan trọng**:
> - Kiểm tra chắc chắn `index.html` nằm tại `C:\Publish\OjitexSystem\wwwroot\index.html` (chứ không phải `wwwroot\browser\index.html`).
> - Tệp `web.config` ở thư mục gốc `C:\Publish\OjitexSystem\web.config` phải là tệp do `dotnet publish` sinh ra (có thẻ `<aspNetCore processPath="dotnet" arguments=".\OjitexSystem-Backend.dll" ... hostingModel="inprocess" />`). **Không** chép đè `web.config` của Angular vào đây vì chính `OjitexSystem-Backend.dll` đã xử lý định tuyến SPA qua `app.MapFallbackToFile("index.html")`.

---

### Giai đoạn 3: Đưa lên Máy chủ Windows Server, Cấu hình IIS, CSDL & Tường lửa

#### Bước 3.1: Sao chép Gói Xuất bản lên Máy chủ Windows Server
1. Trên máy chủ Windows Server, tạo thư mục vật lý lưu trữ ứng dụng, ví dụ:
   ```text
   C:\inetpub\wwwroot\OjitexSystem
   ```
2. Sao chép toàn bộ nội dung bên trong `C:\Publish\OjitexSystem` từ máy cá nhân sang `C:\inetpub\wwwroot\OjitexSystem` trên máy chủ.

#### Bước 3.2: Tạo tệp `appsettings.Production.json` trên Máy chủ
Tạo tệp `C:\inetpub\wwwroot\OjitexSystem\appsettings.Production.json` trực tiếp trên máy chủ Windows Server (lưu với bảng mã **UTF-8**).

Như đã phân tích ở Phần 1, tệp này **bắt buộc** phải có đủ 3 nhóm thông tin:
1. Chuỗi kết nối `EVOLIO` (CSDL tài khoản & phân quyền).
2. Chuỗi kết nối `OJITEXHP` (CSDL sản xuất & báo cáo tồn kho).
3. Khóa `Authentication:SigningKey` dài tối thiểu **32 ký tự**.

**Mẫu `appsettings.Production.json` chuẩn cho `OjitexSystem` (Sử dụng SQL Server Authentication - Khuyến nghị)**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "EVOLIO": "Server=IP_HOAC_TEN_MAY_CHU_SQL;Database=EVOLIO;User Id=TAI_KHOAN_SQL;Password=MAT_KHAU_SQL;TrustServerCertificate=True;MultipleActiveResultSets=True;",
    "OJITEXHP": "Server=IP_HOAC_TEN_MAY_CHU_SQL;Database=OJITEXHP;User Id=TAI_KHOAN_SQL;Password=MAT_KHAU_SQL;TrustServerCertificate=True;MultipleActiveResultSets=True;"
  },
  "Authentication": {
    "Issuer": "OjitexSystem",
    "Audience": "OjitexSystem",
    "SigningKey": "THAY_BANG_CHUOI_BI_MAT_NGau_Nhien_Dai_Toi_Thieu_32_Ky_Tu_Tro_Len_2026!"
  }
}
```

> **Mẹo tạo nhanh `SigningKey` ngẫu nhiên an toàn trên PowerShell của Server**:
> Bạn có thể chạy lệnh sau trong PowerShell để lấy một chuỗi ngẫu nhiên 44 ký tự (32 bytes Base64) dán vào `SigningKey`:
> ```powershell
> [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
> ```
> **Lưu ý**: Giữ nguyên giá trị `SigningKey` này trong các lần cập nhật code sau để người dùng đang đăng nhập không bị văng phiên khi bảo trì.

#### Bước 3.3: Khởi tạo Application Pool và Website trên IIS
1. Mở **IIS Manager** (`inetmgr`).
2. **Tạo Application Pool riêng cho `OjitexSystem`**:
   - Nhấp chuột phải vào **Application Pools** → **Add Application Pool...**
   - **Name**: `OjitexSystemPool`
   - **.NET CLR version**: Chọn **`No Managed Code`** (Bắt buộc).
   - **Managed pipeline mode**: `Integrated`
   - Nhấn **OK**.
   - Nhấp chuột phải vào `OjitexSystemPool` → **Advanced Settings...**:
     - Kiểm tra **Enable 32-Bit Applications**: Đặt là **`False`** (trừ khi bạn cố tình publish gói win-x86).
     - Kiểm tra **Identity** (dưới mục Process Model): Mặc định là `ApplicationPoolIdentity` (nếu dùng SQL Server Authentication như mẫu trên). Nếu chuỗi kết nối dùng `Integrated Security=True;` (Windows Auth), bạn phải đổi Identity này sang tài khoản Domain/Service Account có quyền truy cập cả 2 DB `EVOLIO` và `OJITEXHP`.
3. **Tạo Website trên IIS**:
   - Nhấp chuột phải vào **Sites** → **Add Website...**
   - **Site name**: `OjitexSystem`
   - **Application pool**: Nhấn **Select...** và chọn `OjitexSystemPool`.
   - **Physical path**: `C:\inetpub\wwwroot\OjitexSystem`
   - **Binding**:
     - **Type**: `http` (hoặc `https` nếu công ty có chứng chỉ SSL nội bộ).
     - **IP address**: `All Unassigned` (hoặc chọn IP tĩnh nội bộ của máy chủ).
     - **Port**: `80` (nếu dùng cổng 80, nhớ **Stop** trang *Default Web Site* mặc định của IIS để tránh đụng cổng; hoặc đặt cổng riêng như `8080`).
     - **Host name**: Để trống nếu nhân viên truy cập bằng địa chỉ IP (ví dụ `http://192.168.1.100`) hoặc tên máy chủ (`http://<ComputerName>`), hoặc điền tên miền nội bộ (ví dụ `ojitex.local`) nếu đã cấu hình DNS nội bộ.
   - Nhấn **OK**.
   - *(Xem giải thích chi tiết về IP tĩnh, `SigningKey`, sự khác nhau giữa cổng `80` vs `8080`, và cách cấu hình DNS nội bộ tại [iis-network-and-security-faq.md](iis-network-and-security-faq.md)).*

#### Bước 3.4: Phân quyền Thư mục Vật lý cho Application Pool
1. Mở File Explorer, nhấp chuột phải vào thư mục `C:\inetpub\wwwroot\OjitexSystem` → **Properties** → thẻ **Security** → **Edit...** → **Add...**
2. Nhấn **Locations...**, chọn tên máy chủ Windows Server hiện tại (nằm ở dòng trên cùng) rồi nhấn **OK**.
3. Trong ô *Enter the object names to select*, nhập chính xác:
   ```text
   IIS AppPool\OjitexSystemPool
   ```
   Nhấn **Check Names** (hệ thống sẽ gạch chân tên `OjitexSystemPool`) → nhấn **OK**.
4. Cấp quyền **Read & execute**, **List folder contents**, **Read** → nhấn **OK**.

*(Hoặc chạy nhanh 1 dòng lệnh trong **PowerShell Admin** trên Server)*:
```powershell
icacls "C:\inetpub\wwwroot\OjitexSystem" /grant "IIS AppPool\OjitexSystemPool:(OI)(CI)(RX)" /T
```

#### Bước 3.5: Kiểm tra Tại chỗ và Mở Cổng Tường lửa (Windows Firewall)
1. **Kiểm tra tại chỗ trên máy chủ**:
   Mở trình duyệt ngay trên Windows Server, truy cập `http://localhost` (hoặc `http://localhost:8080` tùy cổng bạn chọn).
   - Màn hình đăng nhập **Ojitex System** hiển thị đầy đủ giao diện.
   - Đăng nhập thử bằng tài khoản hợp lệ → kiểm tra trang **Logistics (Stock Report)** và **Factory (Corrugator / Converting Online & History)** để xác nhận cả 2 kết nối CSDL `EVOLIO` và `OJITEXHP` đều hoạt động tốt.
   - Nhấn phím **F5 (Refresh)** trực tiếp tại trang `/factory/corrugator-online` hoặc `/logistic/current-stock` → trang tải lại bình thường, không báo lỗi 404.
2. **Mở cổng trên Windows Firewall cho mạng LAN**:
   Mở **PowerShell (Admin)** trên Windows Server và chạy lệnh mở cổng tương ứng (thay `80` bằng cổng thực tế của bạn):
   ```powershell
   New-NetFirewallRule -DisplayName "OjitexSystem Web (HTTP-In)" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow -Profile Domain,Private
   ```
3. Từ một máy tính trạm bất kỳ trong mạng LAN công ty, mở trình duyệt và truy cập:
   `http://<IP_May_Chu_Windows_Server>` (hoặc kèm `:Port` nếu dùng cổng khác 80).

---

## Phần 3: Quy trình Cập nhật Phiên bản Mới (Release Update) An toàn

Khi bạn sửa code hoặc thêm tính năng mới trên máy cá nhân và muốn cập nhật lên Windows Server mà không làm mất cấu hình máy chủ:

1. **Trên máy cá nhân**: Chạy lại Bước 2.1 và Bước 2.2 để xuất bản gói mới ra `C:\Publish\OjitexSystem`.
2. **Trên máy chủ Windows Server**:
   - Mở IIS Manager → chọn **Application Pools** → nhấp chuột phải vào `OjitexSystemPool` → chọn **Stop** (để giải phóng khóa file `OjitexSystem-Backend.dll`).
   - **Sao lưu (Backup)** thư mục `C:\inetpub\wwwroot\OjitexSystem` hiện tại (đặc biệt giữ lại tệp `appsettings.Production.json` trên máy chủ).
   - Chép đè các tệp mới từ `C:\Publish\OjitexSystem` vào `C:\inetpub\wwwroot\OjitexSystem` (**Lưu ý**: Không ghi đè làm mất tệp `appsettings.Production.json` đang có trên máy chủ).
   - Quay lại IIS Manager → nhấp chuột phải vào `OjitexSystemPool` → chọn **Start**.

---

## Phần 4: Bảng Chẩn đoán Nhanh Các Lỗi Đặc thù của `OjitexSystem` trên IIS

| Hiện tượng / Mã lỗi | Nguyên nhân Phổ biến nhất trong `OjitexSystem` | Cách Kiểm tra và Khắc phục Triệt để |
| :--- | :--- | :--- |
| **HTTP Error 500.30** *(ASP.NET Core app failed to start)* | 1. Thiếu `Authentication:SigningKey` hoặc khóa ngắn hơn 32 ký tự trong `appsettings.Production.json`.<br>2. Lỗi cú pháp JSON (dư dấu phẩy, thiếu dấu ngoặc kép, hoặc dùng dấu `\` đơn trong tên instance SQL Server như `Server=192.168.1.10\SQLEXPRESS` mà không viết thành `\\`). | Mở **Event Viewer** → **Windows Logs** → **Application**, xem lỗi nguồn `.NET Runtime`, hoặc mở CMD tại `C:\inetpub\wwwroot\OjitexSystem` chạy thử `dotnet OjitexSystem-Backend.dll` để đọc thông báo lỗi trực tiếp trên màn hình. |
| **HTTP Error 500.31** *(Failed to load ASP.NET Core runtime)* | Máy chủ Windows Server chưa cài **.NET 10.0 Hosting Bundle** (chỉ mới cài .NET 8/9), hoặc `Enable 32-Bit Applications` trong Application Pool bị bật `True` lệch với gói 64-bit. | Chạy `dotnet --list-runtimes` trên Server để kiểm tra có bản `10.0.x` chưa; kiểm tra `Advanced Settings` của `OjitexSystemPool`. |
| **Trang đăng nhập hiện lên nhưng bấm Login báo lỗi máy chủ (500)** | Không kết nối được CSDL `EVOLIO` do: sai IP/Password SQL, tài khoản SQL chưa có quyền trên DB `EVOLIO`, tường lửa máy chủ SQL chặn cổng `1433`, hoặc thiếu `TrustServerCertificate=True;` trong chuỗi kết nối. | Kiểm tra chuỗi kết nối `ConnectionStrings:EVOLIO` trong `appsettings.Production.json` và xem chi tiết ngoại lệ `SqlException` trong **Event Viewer** của Windows Server. |
| **Đăng nhập thành công nhưng vào trang Stock Report / Factory báo lỗi** | Kết nối `EVOLIO` đúng nhưng chuỗi kết nối `ConnectionStrings:OJITEXHP` bị sai hoặc tài khoản SQL chưa được cấp quyền `db_datareader` trên CSDL `OJITEXHP`. | Kiểm tra chuỗi kết nối `ConnectionStrings:OJITEXHP` và quyền truy cập của tài khoản SQL đối với database `OJITEXHP`. |
| **Mở trang web chỉ thấy lỗi 404 hoặc trắng trang** | Chép nhầm cả thư mục vỏ `browser` vào `wwwroot` (thành `wwwroot\browser\index.html`) thay vì chép ruột bên trong `browser` vào `wwwroot\index.html`. | Kiểm tra đảm bảo tệp `index.html` nằm đúng tại đường dẫn `C:\inetpub\wwwroot\OjitexSystem\wwwroot\index.html`. |
| **Truy cập `http://localhost` trên Server được nhưng máy trạm LAN không vào được** | Chưa mở Inbound Rule cho cổng tương ứng trên **Windows Defender Firewall** của máy chủ Windows Server. | Chạy lệnh `New-NetFirewallRule` ở Bước 3.5 để mở cổng TCP cho mạng LAN. |
