# Triển khai OjitexSystem lên Windows Server trong mạng nội bộ

Hướng dẫn từ máy mới đến địa chỉ truy cập bằng DNS, dựa trên source hiện tại của repository. Phạm vi là **mạng LAN/VPN của công ty**, dùng IIS để chạy ứng dụng tự động sau khi máy chủ khởi động. Không cần mở port trên router ra Internet.

Backend hiện dùng .NET 10, frontend Angular 22.2 và hai database SQL Server: `EVOLIO` và `OJITEXHP`. Đọc thêm [README](../README.md) và [kiến trúc](architecture.md). Các IP, tên miền và đường dẫn bên dưới là **ví dụ cần thay bằng thông tin thực tế**.

## 1. Mô hình triển khai và thông tin cần chuẩn bị

```text
Máy nhân viên
  │ hỏi DNS: ojitex.corp.example.com là IP nào?
  ├──────────────────────────────> DNS nội bộ: 192.168.10.10
  │ nhận IP 192.168.10.20
  │ HTTPS, TCP 443
  ▼
Windows Server: 192.168.10.20
  IIS / site OjitexSystem
    ASP.NET Core phục vụ:
      /                  → Angular trong wwwroot
      /logistic/...      → Angular router
      /api/...           → API backend
          │ kết nối SQL từ máy chủ web
          ▼
  SQL Server: 192.168.10.30, port TCP thực tế của instance
    EVOLIO    → tài khoản, role, quyền
    OJITEXHP  → báo cáo sản xuất, tồn kho
```

Frontend hiện gọi API bằng đường dẫn tương đối `/api/...`. `Program.cs` đã có `UseDefaultFiles`, `UseStaticFiles`, fallback về `index.html` cho URL giao diện và trả 404 cho API không tồn tại. Vì vậy, ta đặt bản build Angular vào `wwwroot` của bản publish backend và chạy **một IIS site ở gốc tên miền**. Không cần IIS URL Rewrite, ARR, CORS giữa hai tên miền hoặc một tiến trình Node trên server.

| Thông tin | Ví dụ dùng trong hướng dẫn | Giá trị thực tế của bạn |
| --- | --- | --- |
| Máy build | PC đang có source và công cụ lập trình | Điền trước khi làm |
| Windows Server host web | Windows Server 2022/2025 x64, có Desktop Experience | Điền phiên bản |
| IP cố định máy web | `192.168.10.20` | Điền IP |
| Subnet nhân viên được truy cập | `192.168.10.0/24` | Điền tất cả VLAN/VPN cần dùng |
| DNS nội bộ hiện có | `192.168.10.10` | Hỏi IT/network admin |
| Zone DNS nội bộ | `corp.example.com` | Dùng zone thuộc công ty |
| Địa chỉ web cuối cùng | `https://ojitex.corp.example.com` | Chốt trước khi cấp chứng chỉ |
| SQL Server | `sql.corp.example.com,1433` | Điền host và port thật |
| IIS site / app pool | `OjitexSystem` / `OjitexSystemPool` | Có thể giữ tên ví dụ |
| Thư mục chứa các bản phát hành | `C:\Sites\OjitexSystem\releases` | Có thể giữ đường dẫn |
| Nơi nhận gói cài đặt | `C:\Deploy` | Có thể giữ đường dẫn |

`example.com` là tên dùng cho ví dụ; thay toàn bộ hậu tố bằng zone của công ty. Nếu công ty có zone AD như `company.local`, có thể thêm record vào zone hiện có và dùng CA nội bộ cho HTTPS; không cần đổi tên AD. Khi thiết kế mới, ưu tiên tên con thuộc tên miền công ty đang quản lý.

Chuẩn bị quyền Administrator trên máy web, quyền cấu hình DNS/DHCP hoặc người IT hỗ trợ, và người quản trị SQL để cấp kết nối. Việc host IIS không đòi hỏi biến máy web thành Domain Controller.

## 2. Chuẩn bị Windows Server và kết nối mạng

### 2.1. Đặt IP ổn định

1. Cắm server vào mạng có thể đi tới SQL Server và các VLAN nhân viên.
2. Mở `ncpa.cpl` → card mạng đang dùng → **Properties → Internet Protocol Version 4 (TCP/IPv4)**.
3. Đặt IP, subnet mask và gateway do IT cấp, hoặc dùng DHCP reservation để IP không đổi.
4. Đặt DNS của card mạng là DNS nội bộ của công ty. Máy thuộc AD phải dùng DNS hỗ trợ domain đó.
5. Kiểm tra giờ/ngày và đồng bộ thời gian của Windows. Sai giờ có thể gây lỗi chứng chỉ và JWT. Nghiệp vụ ứng dụng tự dùng UTC+7, JWT dùng UTC.

Nếu đang Remote Desktop, thay IP/gateway có thể ngắt phiên; thực hiện qua console hoặc có cách kết nối lại bằng IP mới.

Mở **Windows PowerShell → Run as administrator**, kiểm tra:

```powershell
ipconfig /all
Get-NetConnectionProfile
Test-NetConnection sql.corp.example.com -Port 1433
```

Thay host/port SQL cho đúng. `TcpTestSucceeded: True` chỉ chứng minh tới được port, chưa chứng minh đăng nhập SQL hoặc truy vấn database thành công. Nếu SQL dùng named instance với dynamic port, nhờ DBA xác định port; hướng dẫn này ưu tiên kết nối `host,port` cố định để không phụ thuộc SQL Browser.

### 2.2. Giữ máy chủ hoạt động

- Cập nhật Windows và đặt lịch reboot bảo trì.
- Tắt sleep/hibernate theo chính sách máy chủ; cân nhắc UPS và tự bật lại sau mất điện trong BIOS.
- Xác nhận các dịch vụ IIS khởi động tự động sau khi cài. Đăng xuất khỏi RDP không được làm web dừng.

## 3. Chuẩn bị SQL Server và dữ liệu

### 3.1. Phương án nên làm trước: giữ SQL Server hiện có

Chuyển nơi chạy web không bắt buộc chuyển database. Máy Windows Server mới chỉ cần kết nối tới SQL đang có `EVOLIO` và `OJITEXHP`. Đây cũng là cách giữ dữ liệu tồn kho tiếp tục được cập nhật bởi hệ thống hiện hữu.

Trên máy SQL, nhờ DBA:

1. Xác nhận hai database tồn tại, online và tương thích mapping hiện tại.
2. Bật TCP/IP cho instance trong **SQL Server Configuration Manager**, xác định port và restart dịch vụ SQL trong lịch bảo trì nếu thay đổi cấu hình.
3. Cho phép máy web đi vào port SQL qua Windows Firewall và firewall giữa VLAN. Chỉ máy web cần kết nối SQL; máy nhân viên chỉ truy cập HTTPS tới máy web.
4. Tạo tài khoản riêng cho ứng dụng và cấp quyền theo bảng thực sự sử dụng. Không dùng `sa`, `sysadmin` hay mặc định cấp `db_owner`.
5. Kiểm tra kết nối từ máy web bằng SSMS với đúng thông tin mà ứng dụng sẽ dùng.

Với SQL Authentication: instance phải cho phép chế độ này. Trong SSMS, vào **Security → Logins → New Login → SQL Server authentication**, đặt mật khẩu riêng, sau đó **User Mapping** để tạo user trong các database cần dùng. DBA cần quản lý thời hạn mật khẩu và cập nhật cấu hình ứng dụng khi đổi. Có thể dùng hai login riêng cho hai database.

Phạm vi quyền theo chức năng hiện tại để DBA đối chiếu, áp dụng đúng schema thực tế:

| Database | Quyền cần theo chức năng |
| --- | --- |
| `EVOLIO` | Đọc user, role, category, category grant; cập nhật mật khẩu/trạng thái user; thêm user; thêm/xóa liên kết role và grant trực tiếp theo chức năng quản trị. Các bảng chính: `IE_USER`, `IE_USER_ROLE`, `IE_ROLE`, `IE_CATEGORY`, `IE_CATEGORY_AUTH`. |
| `OJITEXHP` | Chỉ đọc các bảng snapshot/history Current, Last, Monthly và danh mục sản phẩm/khách hàng mà repository báo cáo sử dụng. Chức năng web hiện tại không cần quyền ghi database này. |

DBA đối chiếu `Repositories/Auth`, `Repositories/Admin`, `Repositories/Logistics` và mapping trong `Data` trước khi cấp quyền. Nếu chỉ cấp quyền đọc `EVOLIO`, đăng nhập có thể hoạt động nhưng đổi mật khẩu/quản trị sẽ lỗi. Phải kiểm tra cả các thao tác được phép ghi khi nghiệm thu.

### 3.2. Nếu muốn chuyển SQL sang máy mới

Đây là công việc chuyển dữ liệu riêng: backup nhất quán hai database, restore bằng SQL Server phiên bản tương thích, chuyển hoặc ánh xạ lại login/user, giữ collation/schema và kiểm tra các job/nguồn đang cập nhật tồn kho. Chốt thời điểm chuyển để không phát sinh hai bộ dữ liệu khác nhau. Bản restore đứng riêng sẽ không tự nhận dữ liệu sản xuất mới nếu các nguồn ghi vẫn trỏ về SQL cũ.

Repository không có quy trình tự khởi tạo đầy đủ hai database từ đầu. **Không tạo hai database rỗng rồi chờ ứng dụng tự tạo bảng.** Không chạy EF migration, sửa schema scaffold hoặc chế độ reset mật khẩu hàng loạt để cài web. Dùng tài khoản quản trị ứng dụng hiện có trong `EVOLIO`; nếu chưa có, thống nhất cách cấp với người quản lý hệ thống.

### 3.3. Mã hóa kết nối SQL

Mẫu cấu hình bên dưới dùng `Encrypt=True;TrustServerCertificate=False`. Chứng chỉ SQL phải được máy web tin cậy và khớp tên SQL trong connection string. Chứng chỉ HTTPS của IIS và chứng chỉ SQL là hai việc riêng.

Nếu SQL nội bộ đang dùng chứng chỉ tự ký, cách xử lý lâu dài là cài chứng chỉ phù hợp và chuỗi CA lên máy web. `TrustServerCertificate=True` vẫn mã hóa nhưng bỏ kiểm tra danh tính SQL; chỉ dùng tạm khi IT chấp nhận và có kế hoạch khắc phục. Không khắc phục bằng cách tắt mã hóa. Xem [cú pháp và xác thực chứng chỉ SQL](https://learn.microsoft.com/en-us/sql/connect/ado-net/connection-string-syntax?view=sql-server-ver17).

## 4. Cài IIS và .NET Hosting Bundle trên máy web

### 4.1. Cài IIS trước

Trong PowerShell Administrator trên Windows Server:

```powershell
Install-WindowsFeature -Name Web-Server -IncludeManagementTools
```

Hoặc dùng **Server Manager → Manage → Add Roles and Features → Role-based → Web Server (IIS)**, giữ các role service mặc định và IIS Management Console.

### 4.2. Cài Hosting Bundle đúng .NET 10

1. Mở [trang tải .NET 10 chính thức](https://dotnet.microsoft.com/en-us/download/dotnet/10.0).
2. Chọn **ASP.NET Core Runtime → Windows → Hosting Bundle**, dùng bản vá được hỗ trợ phù hợp bản publish.
3. Chạy bộ cài bằng Administrator. Hosting Bundle cung cấp runtime và module `AspNetCoreModuleV2` để IIS chạy ứng dụng.
4. Nếu đã cài bundle trước IIS, chạy lại installer và **Repair** sau khi cài IIS.
5. Reboot server trong lịch bảo trì để IIS nhận runtime/module mới. Nếu có site khác, tính cả ảnh hưởng của lần restart.

Máy web không cần SDK, Visual Studio, Node.js hoặc Angular CLI nếu chỉ nhận gói publish. Kiểm tra trong PowerShell mới:

```powershell
dotnet --list-runtimes
Get-Service WAS, W3SVC
```

Cần thấy `Microsoft.NETCore.App 10.0.x`, `Microsoft.AspNetCore.App 10.0.x` và dịch vụ IIS đang chạy. Đối chiếu yêu cầu framework trong `OjitexSystem-Backend.runtimeconfig.json` của gói mới; `dotnet --version` không phải kiểm tra phù hợp khi server chỉ có runtime. Tham khảo [quy trình IIS của Microsoft](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-10.0).

## 5. Build và đóng gói trên PC có source

Các lệnh mục này chạy trên **máy build**, không phải máy nhân viên. Có thể dùng chính server nếu đã cài công cụ build, nhưng phương án dưới giữ server chỉ chạy ứng dụng.

### 5.1. Chuẩn bị công cụ build

- .NET SDK 10 phù hợp project `net10.0`.
- Node.js phù hợp Angular 22.2. Package CLI đang cài trong repository khai báo `^22.22.3 || ^24.15.0 || >=26.0.0`; có thể chọn nhánh Node 24 LTS với phiên bản ít nhất `24.15.0`, đáp ứng lockfile hiện tại. Kiểm tra lại `engines` nếu dependency được cập nhật.
- npm phù hợp; `package.json` hiện khai báo `npm@11.19.0`.
- Source đầy đủ, `package-lock.json` và khả năng tải dependency nếu máy chưa có cache.

Xem [bảng tương thích Angular](https://angular.dev/reference/versions). Kiểm tra máy build:

```powershell
dotnet --list-sdks
node --version
npm.cmd --version
```

### 5.2. Kiểm tra bản source định đưa lên server

Mở PowerShell ở repository root, ví dụ `C:\Code\OjitexSystem`:

```powershell
Set-Location C:\Code\OjitexSystem
git status --short
```

Xác nhận các thay đổi cần triển khai đều đã có trên máy build. `appsettings.json` local đang được gitignore; clone source sang máy khác sẽ không mang cấu hình này theo. Ta sẽ tạo cấu hình production riêng trên server.

Chạy từng lệnh; nếu lỗi thì xử lý trước khi tiếp tục. Restore/install cần cho máy mới, có thể bỏ khi dependency đã sẵn có và không đổi:

```powershell
dotnet restore OjitexSystem_Backend/OjitexSystem_Backend.slnx
dotnet build OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
dotnet test OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore

Set-Location OjitexSystem_Frontend
npm.cmd ci
npm.cmd run test:ci
npm.cmd run format:check
Set-Location ..
```

Nếu file backend đang bị tiến trình local giữ khóa, dùng output riêng theo [README](../README.md). Không dùng `npm start` hoặc `dotnet run` làm dịch vụ production.

### 5.3. Tạo gói mới, ghép Angular vào backend

Chạy block này từ repository root trên máy build. Output nằm ngoài repository, có timestamp riêng để không trộn với gói cũ trong `artifacts/publish`.

```powershell
$ErrorActionPreference = 'Stop'
$repoRoot = (Get-Location).Path
$releaseId = Get-Date -Format 'yyyyMMdd-HHmmss'
$buildRoot = Join-Path 'C:\OjitexBuild' $releaseId
$publishPath = Join-Path $buildRoot 'publish'
$frontendOutput = Join-Path $buildRoot 'frontend'
New-Item -ItemType Directory -Path $buildRoot -Force | Out-Null

Push-Location (Join-Path $repoRoot 'OjitexSystem_Frontend')
try {
    npm.cmd run build -- --configuration production --output-path $frontendOutput
    if ($LASTEXITCODE -ne 0) { throw 'Angular build failed.' }
}
finally {
    Pop-Location
}

dotnet publish OjitexSystem_Backend/OjitexSystem-Backend/OjitexSystem-Backend.csproj -c Release --no-restore -p:UseAppHost=false -o $publishPath
if ($LASTEXITCODE -ne 0) { throw 'Backend publish failed.' }

$browserPath = Join-Path $frontendOutput 'browser'
if (-not (Test-Path -LiteralPath (Join-Path $browserPath 'index.html'))) {
    throw 'Missing Angular browser/index.html. Check build output.'
}
$webRoot = Join-Path $publishPath 'wwwroot'
New-Item -ItemType Directory -Path $webRoot -Force | Out-Null
Copy-Item -Path (Join-Path $browserPath '*') -Destination $webRoot -Recurse -Force

# Thay cấu hình từ máy lập trình bằng cấu hình nền không chứa secret.
@'
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
'@ | Set-Content -LiteralPath (Join-Path $publishPath 'appsettings.json') -Encoding UTF8

# Chỉ loại các file cấu hình môi trường ở output mới tạo, không sửa source.
Get-ChildItem -LiteralPath $publishPath -File -Filter 'appsettings.*.json' |
    ForEach-Object { Remove-Item -LiteralPath $_.FullName }

foreach ($relativePath in @('web.config', 'OjitexSystem-Backend.dll', 'wwwroot\index.html')) {
    if (-not (Test-Path -LiteralPath (Join-Path $publishPath $relativePath))) {
        throw "Missing deployment file: $relativePath"
    }
}

$zipPath = Join-Path $buildRoot "OjitexSystem-$releaseId.zip"
Compress-Archive -Path (Join-Path $publishPath '*') -DestinationPath $zipPath
Get-FileHash -LiteralPath $zipPath -Algorithm SHA256
```

`$ErrorActionPreference` không tự dừng mọi lỗi native command trong Windows PowerShell, nên block đã kiểm tra `$LASTEXITCODE` cho build/publish. Các lệnh kiểm tra ở mục trước cũng phải thành công.

Trước khi chuyển ZIP, rà soát output không có secret, cấu hình local khác, backup hay log. Giữ `web.config` do `dotnet publish` sinh; file đó phải có handler `AspNetCoreModuleV2`. Không đưa source, `.git`, `node_modules`, test output hay file `.bak` vào thư mục web. Frontend chỉ chứa thông tin công khai; không đặt mật khẩu hoặc SigningKey vào Angular.

## 6. Chuyển gói và tạo IIS site

### 6.1. Giải nén trên Windows Server

Chuyển file ZIP qua kênh nội bộ được công ty cho phép vào `C:\Deploy`. So sánh SHA256 với máy build. Trong PowerShell Administrator trên server:

```powershell
New-Item -ItemType Directory -Path C:\Deploy -Force | Out-Null
New-Item -ItemType Directory -Path C:\Sites\OjitexSystem\releases -Force | Out-Null
Get-FileHash -LiteralPath C:\Deploy\OjitexSystem-RELEASE_ID.zip -Algorithm SHA256
```

Thay `RELEASE_ID` bằng timestamp của ZIP. Giải nén vào một thư mục mới tương ứng, ví dụ:

```powershell
Expand-Archive -LiteralPath C:\Deploy\OjitexSystem-RELEASE_ID.zip -DestinationPath C:\Sites\OjitexSystem\releases\RELEASE_ID
```

Cấu trúc đúng là:

```text
C:\Sites\OjitexSystem\releases\RELEASE_ID\
  web.config
  OjitexSystem-Backend.dll
  OjitexSystem-Backend.deps.json
  OjitexSystem-Backend.runtimeconfig.json
  appsettings.json
  ... các DLL phụ thuộc ...
  wwwroot\
    index.html
    main-....js
    chunk-....js
    styles-....css
    ... ảnh và tài nguyên ...
```

IIS phải trỏ tới thư mục có **`web.config` và DLL**, không trỏ thẳng vào `wwwroot`, thư mục source hoặc ZIP. Trong các mục tiếp theo, thay mọi `RELEASE_ID` bằng cùng tên thư mục thật.

### 6.2. Tạo application pool và site

1. Mở **IIS Manager** (`inetmgr`).
2. **Application Pools → Add Application Pool**:
   - Name: `OjitexSystemPool`.
   - .NET CLR version: **No Managed Code**.
   - Managed pipeline: **Integrated**.
3. Trong **Advanced Settings** của pool: `Enable 32-Bit Applications = False`, Identity = `ApplicationPoolIdentity`. Giữ pool riêng cho ứng dụng.
4. **Sites → Add Website**:
   - Site name: `OjitexSystem`.
   - Application pool: chọn `OjitexSystemPool`.
   - Physical path: `C:\Sites\OjitexSystem\releases\RELEASE_ID`.
   - Binding tạm để tạo site: Type `http`, IP máy web `192.168.10.20`, Port `80`, Host name `ojitex.corp.example.com`.
   - Chưa dùng binding HTTP này để đăng nhập; sẽ thêm HTTPS ở mục 9.
5. **Stop** site và pool trong lúc hoàn thiện cấu hình.
6. Trong **Authentication** của site: bật **Anonymous Authentication**, chọn **Edit → Application pool identity**. Ứng dụng xác thực bằng JWT; không cần bật IIS Windows Authentication hoặc Basic Authentication cho mô hình này.

Không xóa site khác để giải phóng port. Kiểm tra binding trùng IP/port/host bằng **Bindings...** và chọn host riêng. Hướng dẫn dùng ứng dụng tại `/`; không thêm IIS application `/ojitex` hoặc `/api` vì sẽ đổi base path hiện tại.

### 6.3. Cấp quyền đọc thư mục

Sau khi đã tạo app pool, chạy:

```powershell
icacls 'C:\Sites\OjitexSystem' /grant 'IIS AppPool\OjitexSystemPool:(OI)(CI)RX'
```

Identity của pool cần đọc/thực thi release và đi qua thư mục cha. Kiểm tra ACL kế thừa không cấp đọc cho nhóm người dùng không cần thiết vì cấu hình sắp chứa secret. Không cấp `Everyone:Full Control`; quyền ghi chỉ cấp cho thư mục log khi cần ở mục 12. Người triển khai và Administrators giữ quyền quản lý file.

Để rà soát bằng GUI: chuột phải `C:\Sites\OjitexSystem` → **Properties → Security → Advanced**. Nếu thư mục nhận quyền đọc rộng từ thư mục cha, chọn **Disable inheritance → Convert inherited permissions into explicit permissions**, rồi gỡ các grant rộng không cần thiết theo chính sách IT. Giữ quyền của `SYSTEM`, `Administrators`, tài khoản triển khai và quyền đọc/thực thi của `IIS AppPool\OjitexSystemPool`; áp dụng kế thừa xuống các release. Kiểm tra **Effective Access** của pool sau khi chỉnh, không sửa quyền của toàn bộ ổ C hoặc site khác.

## 7. Tạo cấu hình Production trên server

### 7.1. Tạo file cấu hình

Tạo `C:\Sites\OjitexSystem\releases\RELEASE_ID\appsettings.Production.json` bằng editor chạy với quyền phù hợp, cùng cấp `web.config`, **ở ngoài `wwwroot`**:

```json
{
  "ConnectionStrings": {
    "EVOLIO": "Server=sql.corp.example.com,1433;Database=EVOLIO;User ID=REPLACE_AUTH_LOGIN;Password=REPLACE_AUTH_PASSWORD;Encrypt=True;TrustServerCertificate=False;",
    "OJITEXHP": "Server=sql.corp.example.com,1433;Database=OJITEXHP;User ID=REPLACE_REPORT_LOGIN;Password=REPLACE_REPORT_PASSWORD;Encrypt=True;TrustServerCertificate=False;"
  },
  "Authentication": {
    "Issuer": "OjitexSystem",
    "Audience": "OjitexSystem",
    "SigningKey": "REPLACE_WITH_GENERATED_KEY"
  },
  "Frontend": {
    "Origin": "https://ojitex.corp.example.com"
  },
  "AllowedHosts": "ojitex.corp.example.com",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

Thay host, database login và password thực tế. Nếu instance được ghi dưới dạng `SERVER\INSTANCE`, trong JSON phải ghi `SERVER\\INSTANCE`. Password có dấu nháy hoặc ký tự đặc biệt phải escape đúng cả JSON lẫn connection string; nhờ DBA tạo chuỗi bằng connection-string builder khi cần.

`Frontend:Origin` phải là origin đầy đủ gồm `https://` và host, không có path hoặc dấu `/` cuối. Với mô hình cùng origin, trình duyệt không cần CORS; giá trị này không cấu hình DNS, binding IIS hoặc địa chỉ API. Issuer/Audience là định danh JWT, không bắt buộc phải là tên miền.

### 7.2. Sinh SigningKey, ghi trực tiếp vào file

Chạy trên server sau khi tạo JSON, thay đường dẫn release. Block không in key ra console và chỉ chạy khi vẫn còn placeholder:

```powershell
$configPath = 'C:\Sites\OjitexSystem\releases\RELEASE_ID\appsettings.Production.json'
$settings = Get-Content -LiteralPath $configPath -Raw -Encoding UTF8 | ConvertFrom-Json
if ($settings.Authentication.SigningKey -ne 'REPLACE_WITH_GENERATED_KEY') {
    throw 'SigningKey is already set. Keep the existing key for this deployment.'
}
$randomBytes = New-Object byte[] 48
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
try {
    $rng.GetBytes($randomBytes)
    $settings.Authentication.SigningKey = [Convert]::ToBase64String($randomBytes)
    $settings | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $configPath -Encoding UTF8
}
finally {
    $rng.Dispose()
    [Array]::Clear($randomBytes, 0, $randomBytes.Length)
    Remove-Variable settings
}
```

Backend bắt buộc có SigningKey ít nhất 32 byte UTF-8 khi chạy ngoài Development. Giữ key ổn định khi cập nhật/reboot, lưu bản sao ở nơi quản lý secret của công ty. Đổi key, issuer hoặc audience sẽ làm JWT cũ không còn hợp lệ và người dùng cần đăng nhập lại.

File production chứa secret: chỉ tạo trên server, không commit/copy vào source, `wwwroot`, ZIP dùng chung hoặc log. Đây là cách cấu hình tại máy chủ phù hợp triển khai thủ công; ACL và backup của file cũng phải được bảo vệ.

Nếu dùng biến môi trường thay file, các tên tương ứng là `ConnectionStrings__EVOLIO`, `ConnectionStrings__OJITEXHP`, `Authentication__SigningKey`. Biến môi trường có thể ghi đè JSON. Kiểm tra các thiết lập cũ ở machine/app pool/`web.config` nếu ứng dụng kết nối nhầm SQL hoặc dùng sai key.

### 7.3. Cấu hình môi trường và chuyển hướng HTTPS trong web.config

Trong `web.config` đã publish, giữ handler và các thuộc tính hiện có. Mẫu gói tạo bằng lệnh mục 5 có thể cấu hình như sau:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet"
                  arguments=".\OjitexSystem-Backend.dll"
                  stdoutLogEnabled="false"
                  stdoutLogFile=".\logs\stdout"
                  hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
          <environmentVariable name="ASPNETCORE_HTTPS_PORT" value="443" />
        </environmentVariables>
      </aspNetCore>
    </system.webServer>
  </location>
</configuration>
```

Chỉ có một phần tử `aspNetCore` và một `environmentVariables` bên trong nó. Nếu gói sau thay cách publish, giữ `processPath`/`arguments` phù hợp gói đó. Tránh cấu hình `DOTNET_ENVIRONMENT=Development` ở nơi khác gây xung đột. Trong Production, `/scalar` và OpenAPI không được map như môi trường Development; không dùng chúng làm phép thử sức khỏe.

`UseHttpsRedirection()` đã có trong source; biến HTTPS port cho biết đích chuyển hướng là 443. Hoàn thiện binding HTTPS trước khi start site và luôn nghiệm thu việc HTTP chuyển đúng về HTTPS. Xem [cấu hình IIS nâng cao](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/advanced?view=aspnetcore-10.0) và [HTTPS redirection](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-10.0).

## 8. Cấu hình DNS để mọi người nhớ tên truy cập

DNS chỉ đổi **tên → IP**, không tự tạo website, mở firewall hoặc cấp chứng chỉ. Phải khớp ba nơi: **record DNS**, **Host name trong IIS binding**, **tên SAN của chứng chỉ HTTPS**.

### 8.1. Công ty đã có DNS nội bộ/Active Directory — ưu tiên cách này

Thao tác trên **máy quản lý DNS hiện có**, không cần cài DNS lên máy web:

1. Mở **DNS Manager** (`dnsmgmt.msc`).
2. Mở DNS server → **Forward Lookup Zones** → zone nội bộ đang dùng, ví dụ `corp.example.com`.
3. Chuột phải zone → **New Host (A or AAAA)**.
4. Name: `ojitex`; IP: `192.168.10.20`.
5. Chọn **Add Host**. PTR là tùy chọn cho reverse lookup, không bắt buộc để truy cập web.

Nếu dùng PowerShell trên DNS server, sau khi xác nhận record chưa tồn tại:

```powershell
Add-DnsServerResourceRecordA -ZoneName 'corp.example.com' -Name 'ojitex' -IPv4Address '192.168.10.20' -TimeToLive 00:05:00
```

Nếu đã có record, kiểm tra và sửa IP đúng thay vì thêm bản ghi trùng. Sau khi ổn định có thể tăng TTL. Xem [quản lý record DNS trên Windows Server](https://learn.microsoft.com/en-us/windows-server/networking/dns/manage-resource-records?tabs=powershell).

Không tự tạo zone mới trùng tên miền công ty đang dùng: DNS sẽ coi mình có thẩm quyền cho cả zone và có thể làm mất phân giải các host khác. Nếu chỉ cần một tên thuộc miền công khai mà DNS nội bộ chưa quản lý, thống nhất với IT cách split DNS hoặc zone riêng cho đúng tên host trước khi tạo.

### 8.2. Chưa có DNS nội bộ

Nếu router/firewall có local DNS và có thể phân phối resolver qua DHCP, có thể thêm record ở đó. Nếu cần Windows DNS, dùng một Windows Server IP cố định, có thể là máy web trong mạng nhỏ. Không cài thêm Domain Controller chỉ để có tên truy cập.

Trên máy được chọn làm DNS, PowerShell Administrator:

```powershell
Install-WindowsFeature -Name DNS -IncludeManagementTools
```

Trong **DNS Manager**:

1. **Forward Lookup Zones → New Zone → Primary zone**.
2. Nếu không có AD, tạo standard primary zone bằng zone file; không chọn AD-integrated.
3. Nhập namespace nội bộ đã thống nhất, ví dụ `corp.example.com`, thuộc tên miền công ty quản lý. Khi chưa cần máy tự đăng ký DNS, chọn **Do not allow dynamic updates**.
4. Thêm A record `ojitex → 192.168.10.20` như mục 8.1.
5. Chuột phải DNS server → **Properties → Forwarders → Edit**: thêm DNS upstream do IT/ISP chỉ định để các tên ngoài zone vẫn phân giải được. Không trỏ forwarder về chính máy này hoặc tạo vòng lặp với DNS khác.
6. Cho DNS lắng nghe trên card mạng nội bộ; xác nhận firewall cho **UDP 53 và TCP 53** từ các subnet máy trạm. Không công khai DNS resolver ra Internet.
7. Trong DHCP của router/Windows DHCP, đổi **DNS Servers / Option 006** để cấp IP DNS mới cho máy trạm. Có thể cấu hình DNS thủ công trên một máy để thử trước.

Máy trạm phải sử dụng resolver biết zone nội bộ. Không đặt DNS công khai như `8.8.8.8` làm DNS phụ cho zone riêng: nó không biết record nội bộ và có thể gây lỗi lúc được lúc không. Nếu có hai DNS nội bộ, cả hai phải phân giải được cùng zone. Máy đã thuộc AD phải tiếp tục dùng DNS của domain; tích hợp record/forwarding với IT thay vì đổi sang DNS mới không biết AD.

Nếu DNS chạy ngay trên máy web, IP DNS trong cấu hình máy trạm là `192.168.10.20`, thay cho ví dụ `192.168.10.10`. Xem [cài và cấu hình Windows DNS, gồm forwarder](https://learn.microsoft.com/en-us/windows-server/networking/dns/quickstart-install-configure-dns-server).

### 8.3. Kiểm tra từ máy nhân viên

Sau khi cấu hình DHCP hoặc DNS trên card mạng, lấy lease mới theo cách phù hợp; `ipconfig /renew` có thể ngắt kết nối tạm thời. Kiểm tra:

```powershell
ipconfig /all
ipconfig /flushdns
nslookup ojitex.corp.example.com
Resolve-DnsName ojitex.corp.example.com
```

Phải trả IP web `192.168.10.20`. Để chẩn đoán trực tiếp DNS dự kiến:

```powershell
nslookup ojitex.corp.example.com 192.168.10.10
```

Nếu hỏi trực tiếp được nhưng hỏi mặc định không được, kiểm tra DNS do DHCP cấp, VPN và DNS-over-HTTPS của trình duyệt có đang bỏ qua resolver nội bộ hay không.

Để gõ ngắn `https://ojitex`, cần thêm DNS suffix/search list phù hợp, ví dụ DHCP Option 015 là `corp.example.com`, đồng thời chứng chỉ và IIS binding phải hỗ trợ tên người dùng thực sự nhập. Hướng dẫn này chọn **FQDN `https://ojitex.corp.example.com`** làm địa chỉ phát cho mọi người để thống nhất chứng chỉ và phân giải. Có thể đặt bookmark/shortcut mang tên “Ojitex”.

File `hosts` chỉ hữu ích để thử trên từng máy; sửa `hosts` trên server không phân phối tên cho các máy khác. DNS nội bộ không yêu cầu mua thêm tên miền chỉ để thêm record vào zone công ty đã có; việc cấp chứng chỉ công khai phải dùng tên miền hợp lệ mà công ty kiểm soát.

## 9. Cấp chứng chỉ và bật HTTPS trên IIS

### 9.1. Dùng CA nội bộ hoặc chứng chỉ công ty cung cấp

Đây là phương án phù hợp khi có nhiều máy nhân viên:

1. Yêu cầu IT/CA cấp chứng chỉ **Server Authentication** có SAN chứa chính xác `ojitex.corp.example.com`, còn hạn và có private key tương ứng trên máy web.
2. Nếu nhận file `.pfx`, vào IIS Manager → chọn node server → **Server Certificates → Import**, chọn PFX và nhập mật khẩu. Có thể dùng `certlm.msc` để kiểm tra tại **Local Computer → Personal → Certificates**. Không chỉ import vào Current User của người quản trị.
3. Với chứng chỉ từ CA nội bộ, triển khai root CA và intermediate CA đúng kho tin cậy trên các máy nhân viên; máy domain thường có thể nhận qua Group Policy. Chỉ phân phối chứng chỉ CA công khai, giữ private key/PFX trên nơi được bảo vệ.
4. Làm binding ở mục 9.3.

Nếu công ty dùng chứng chỉ từ CA công khai cho tên miền thuộc quyền quản lý, IT có thể xác minh quyền DNS theo phương thức CA hỗ trợ; máy web vẫn có thể chỉ nằm trong LAN. DNS công khai và DNS nội bộ cần được thiết kế tương ứng, không mở Internet tới ứng dụng chỉ để mọi người trong LAN dùng tên.

### 9.2. Chưa có CA: tạo chứng chỉ tự ký cho đợt thử nội bộ

Chứng chỉ tự ký yêu cầu cài tin cậy trên **từng máy truy cập**, nên phải tính việc phân phối và gia hạn trước khi dùng rộng rãi. Trong PowerShell Administrator trên máy web:

```powershell
$siteHost = 'ojitex.corp.example.com'
$certificate = New-SelfSignedCertificate -DnsName $siteHost -CertStoreLocation 'Cert:\LocalMachine\My' -FriendlyName 'Ojitex intranet HTTPS' -NotAfter (Get-Date).AddYears(1) -KeyAlgorithm RSA -KeyLength 2048 -HashAlgorithm SHA256
Export-Certificate -Cert $certificate -FilePath C:\Deploy\ojitex-intranet.cer
```

Lưu thumbprint và hạn dùng trong hồ sơ triển khai. File `.cer` chỉ có phần công khai; đối chiếu thumbprint qua kênh tin cậy trước khi cài trên client. Không gửi PFX/private key cho người dùng.

Trên **máy Windows nhân viên**, PowerShell Administrator, import đúng CER do IT chuyển:

```powershell
Import-Certificate -FilePath C:\Deploy\ojitex-intranet.cer -CertStoreLocation Cert:\LocalMachine\Root
```

Đường dẫn CER trên client có thể khác. Có thể làm bằng GUI `certlm.msc` → **Trusted Root Certification Authorities → Certificates → Import**. Import tương tự trên máy web nếu cần kiểm tra HTTPS ngay tại đó. Với trình duyệt/thiết bị dùng kho chứng chỉ riêng, IT phải cài trust tương ứng. Không nghiệm thu bằng cách yêu cầu mọi người bỏ qua cảnh báo chứng chỉ.

### 9.3. Tạo binding HTTPS

Trong IIS Manager → site `OjitexSystem` → **Bindings... → Add**:

| Thuộc tính | Giá trị |
| --- | --- |
| Type | `https` |
| IP address | `192.168.10.20` |
| Port | `443` |
| Host name | `ojitex.corp.example.com` |
| Require Server Name Indication | Bật, đặc biệt khi chung IP/443 với site khác |
| SSL certificate | Chọn đúng chứng chỉ vừa import/tạo |

Giữ binding HTTP 80 cùng host để chuyển hướng về HTTPS. Không bật tùy chọn **Require SSL** chặn HTTP tại IIS nếu muốn request HTTP đi tới middleware chuyển hướng. Kiểm tra không có binding trùng và chứng chỉ có private key. Xem [thiết lập SSL binding của IIS](https://learn.microsoft.com/en-us/iis/manage/configuring-security/how-to-set-up-ssl-on-iis).

## 10. Mở firewall nội bộ và khởi động ứng dụng

Trên máy web, với subnet ví dụ và network profile Domain/Private:

```powershell
New-NetFirewallRule -DisplayName 'Ojitex HTTPS LAN' -Direction Inbound -Action Allow -Protocol TCP -LocalPort 443 -RemoteAddress 192.168.10.0/24 -Profile Domain,Private
New-NetFirewallRule -DisplayName 'Ojitex HTTP redirect LAN' -Direction Inbound -Action Allow -Protocol TCP -LocalPort 80 -RemoteAddress 192.168.10.0/24 -Profile Domain,Private
```

Thay hoặc bổ sung các subnet/VPN được phép truy cập. Dùng `Get-NetConnectionProfile` để xác nhận profile đang áp dụng; nếu đang là Public, nhờ IT xác định cấu hình mạng phù hợp. Rule Domain/Private không có hiệu lực trên profile Public.

IIS có thể đã tạo rule HTTP/HTTPS rộng hơn. Rà soát các rule hiện hữu vì thêm rule giới hạn subnet không thu hẹp một rule Allow khác đang mở rộng; điều chỉnh theo chính sách và tính cả các site chung server. Firewall giữa VLAN cũng phải cho phép TCP 443/80 tương ứng. Không cần mở `4200`, `7013` hoặc SQL cho máy nhân viên. Nếu máy web kiêm DNS, phần DNS còn cần TCP/UDP 53 như mục 8.

Trong IIS Manager, **Start** pool rồi **Start** site. Site đặt **Start Automatically = True**; pool bật tự khởi động. Mặc định IIS có thể khởi tạo ứng dụng khi có request đầu tiên và idle/recycle pool sau một thời gian, nên request đầu có thể chậm hơn. Kiểm tra web sau khi logout RDP và sau lần reboot bảo trì; không cần giữ cửa sổ terminal mở.

## 11. Nghiệm thu trước khi đưa link cho mọi người

### 11.1. Kiểm tra hạ tầng và HTTP

Từ một máy nhân viên đã tin cậy chứng chỉ:

```powershell
Resolve-DnsName ojitex.corp.example.com
Test-NetConnection ojitex.corp.example.com -Port 443
curl.exe -I http://ojitex.corp.example.com/
curl.exe -I https://ojitex.corp.example.com/
curl.exe -i https://ojitex.corp.example.com/api/auth/me
curl.exe -i https://ojitex.corp.example.com/api/not-a-real-endpoint
curl.exe -I https://ojitex.corp.example.com/appsettings.Production.json
```

Kết quả mong đợi:

- DNS trả đúng IP; TCP 443 thành công.
- HTTP trả redirect 307/308 với `Location` trỏ HTTPS cùng host, port 443.
- HTTPS `/` trả 200 HTML, không cảnh báo chứng chỉ.
- `/api/auth/me` khi chưa gửi token trả **401**: API nhận request và yêu cầu đăng nhập. Phép thử này chưa chứng minh truy cập SQL thành công.
- API không tồn tại trả **404**, không trả Angular HTML.
- URL file cấu hình trả **404**, không lộ cấu hình. Chỉ static asset trong `wwwroot` được phục vụ.

Nếu DNS chưa sẵn sàng, có thể thử từ máy đã tin chứng chỉ bằng `curl.exe --resolve ojitex.corp.example.com:443:192.168.10.20 -I https://ojitex.corp.example.com/`. Cách này giữ đúng host và tên chứng chỉ nhưng bỏ qua bước hỏi DNS. Truy cập IP trực tiếp có thể vào nhầm IIS site hoặc báo sai tên chứng chỉ; không dùng làm kết luận DNS/HTTPS hỏng.

### 11.2. Kiểm tra bằng trình duyệt và dữ liệu thật

1. Mở `https://ojitex.corp.example.com`, kiểm tra không có cảnh báo chứng chỉ.
2. Đăng nhập bằng tài khoản hiện có; trong **F12 → Network**, request phải đi tới `https://ojitex.corp.example.com/api/...`, không có `localhost:7013` hoặc `localhost:4200`.
3. Vào Current/Last/Monthly Stock bằng tài khoản có quyền, kiểm tra dữ liệu với nguồn nghiệp vụ, tìm mã, mở history, in/copy theo nhu cầu.
4. Đang ở `/logistic/current-stock`, nhấn **F5** và dán URL đó vào tab mới. Sau xử lý đăng nhập, trang phải mở được; không gặp IIS 404 do thiếu fallback.
5. Dùng tài khoản thường kiểm tra hạn chế quản trị/quyền logistics. **403** là thiếu quyền; không sửa firewall hoặc xóa phiên chỉ để né 403.
6. Với tài khoản thử nghiệm được cho phép, kiểm tra đổi mật khẩu và chức năng quản trị để xác minh quyền ghi `EVOLIO`. Không thử xóa/hạ quyền admin cuối cùng và không dùng dữ liệu người thật để kiểm tra phá hủy.
7. Thử từ ít nhất một máy khác/VLAN khác thuộc phạm vi triển khai; kiểm tra lại sau logout RDP và reboot bảo trì.

Đăng nhập truy cập `EVOLIO`; báo cáo truy cập `OJITEXHP`, nên một chức năng chạy được không chứng minh cả hai connection string đều đúng. Build/unit test không thay thế nghiệm thu với SQL Server thực tế.

Sau khi đạt các bước trên, phát địa chỉ **`https://ojitex.corp.example.com`** cho người dùng và hướng dẫn bookmark. Không kèm username/password quản trị trong thông báo link.

## 12. Xử lý các lỗi thường gặp

| Hiện tượng | Kiểm tra và hướng xử lý |
| --- | --- |
| `NXDOMAIN`/không phân giải tên | A record, đúng zone, DNS thực tế của client, DHCP/VPN/DoH và cache. So sánh truy vấn mặc định với truy vấn trực tiếp DNS nội bộ. |
| Có IP nhưng timeout 443 | IIS site đã start, binding/IP đúng, Windows Firewall/profile, route và firewall giữa VLAN. Ping thất bại không đủ kết luận vì ICMP có thể bị chặn. |
| Hiện trang IIS mặc định hoặc site khác | Host name binding, record trỏ đúng IP, binding trùng và URL người dùng nhập. |
| Cảnh báo HTTPS | SAN không khớp host, thiếu private key ở server, hết hạn, thiếu root/intermediate trust, đồng hồ sai. Dùng đúng FQDN. |
| HTTP không chuyển HTTPS/chuyển sai port | Binding 80 và 443, biến `ASPNETCORE_HTTPS_PORT=443`, cấu hình môi trường và log middleware. HTTPS phải chạy trước. |
| IIS `500.19`/handler không nhận diện | XML `web.config`, Hosting Bundle/ANCM có cài sau IIS hay chưa, quyền đọc và cấu hình IIS bị khóa. Không xóa `web.config` để chữa lỗi. |
| `500.30`, `500.31`, `500.32` | Xem Event Viewer/stdout; kiểm tra SigningKey, JSON, runtime 10.0, dependency và kiến trúc x64/pool 32-bit. Mã lỗi chỉ là gợi ý, cần đọc lỗi chi tiết. |
| `503 Service Unavailable` | Pool bị stop/rapid-fail, identity/quyền thư mục và lỗi startup trong Event Viewer. |
| Trang trắng, JS/CSS 404 | `wwwroot` có đúng nội dung `browser` không; `index.html` cùng bản với các file hash không; IIS physical path đúng publish root chưa. |
| Đổi tên domain xong lỗi | Đồng bộ DNS, cả binding HTTP/HTTPS, chứng chỉ SAN, `AllowedHosts`, `Frontend:Origin`; người dùng đăng nhập lại trên origin mới. |
| Đăng nhập/báo cáo trả 500 | Connection string tương ứng, port SQL, login mapping/quyền, TLS SQL; kiểm tra từ máy web bằng SSMS với cùng thông tin ứng dụng. |
| SSMS bằng tài khoản Windows của bạn chạy được, web lỗi | IIS chạy bằng pool identity khác. Với SQL Authentication, thử đúng login trong cấu hình; với Integrated Security, cấu hình tài khoản dịch vụ/domain và quyền SQL tương ứng. |
| API trả 401 | Token thiếu/hết hạn, key/issuer/audience đổi, lệch giờ, cấu hình môi trường ghi đè. Thử đăng nhập lại; không gửi token vào log hỗ trợ. |
| API trả 403 | Tài khoản không còn hoạt động hoặc thiếu role/category grant. Kiểm tra bằng chức năng quản trị và giữ đúng nguồn quyền trực tiếp/kế thừa. |
| API báo lỗi parse JSON và body là HTML | Xác nhận gọi đúng `/api/...`, site đúng, backend nhận request; không thêm rewrite mọi URL API về `index.html`. |
| Vừa cập nhật bị lỗi file khóa/thiếu chunk | Stop pool trước khi chuyển release, giữ gói frontend/backend đồng bộ; F5 để tải lại trang và kiểm tra quy trình rollback. |

### 12.1. Xem log startup khi IIS trả 500

Mở **Event Viewer → Windows Logs → Application**, tìm lỗi nguồn IIS AspNetCore Module/ứng dụng theo thời điểm request. IIS access log mặc định thường nằm dưới `C:\inetpub\logs\LogFiles`; xem site ID để chọn thư mục `W3SVC...` tương ứng. Access log ghi trạng thái HTTP, không phải log exception đầy đủ của ứng dụng.

Nếu cần stdout tạm thời, tạo thư mục trên release đang chạy:

```powershell
New-Item -ItemType Directory -Path C:\Sites\OjitexSystem\releases\RELEASE_ID\logs -Force | Out-Null
icacls 'C:\Sites\OjitexSystem\releases\RELEASE_ID\logs' /grant 'IIS AppPool\OjitexSystemPool:(OI)(CI)M'
```

Sửa `stdoutLogEnabled="true"` trong `web.config`, giữ `stdoutLogFile=".\logs\stdout"`, recycle pool rồi thực hiện lại một request. Đọc log tại server, lọc secret trước khi chia sẻ. Khi xử lý xong, đổi lại **false**, recycle pool và quản lý dung lượng log. Stdout không tự trở thành hệ thống xoay log production; xem [log của ASP.NET Core Module](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/logging-and-diagnostics?view=aspnetcore-10.0).

### 12.2. Integrated Security khác với tài khoản bạn dùng trên PC

Nếu thay SQL login bằng `Integrated Security=True`, SQL sẽ xác thực danh tính tiến trình IIS, không phải người đang dùng trình duyệt hoặc tài khoản đã RDP vào server. Với SQL ở máy khác, cần thiết kế tài khoản dịch vụ domain/gMSA hoặc quyền machine account với IT/DBA; không tự tạo login tên local `IIS AppPool\...` trên một SQL host khác rồi kỳ vọng xác thực qua mạng. Phương án SQL Authentication ở mục 7 giúp việc cài lần đầu rõ ràng khi chưa có thiết kế domain service account.

## 13. Cập nhật phiên bản, rollback và vận hành

### 13.1. Cập nhật theo từng thư mục release

1. Build/test/publish thành ZIP mới theo mục 5, ghi lại commit hoặc phiên bản source và SHA256. Không tận dụng ZIP cũ chỉ vì có sẵn trong repository.
2. Giải nén vào `C:\Sites\OjitexSystem\releases\RELEASE_MOI`, không chép đè release đang phục vụ.
3. Copy `appsettings.Production.json` từ release đang chạy sang release mới bằng kênh/file local có bảo vệ. **Giữ nguyên SigningKey**, rà soát thay đổi cấu hình cần thêm. Không chạy lại block sinh key của lần cài đầu.
4. Áp dụng các biến môi trường Production/HTTPS port trong `web.config` mới như mục 7.3. Không mang trạng thái bật stdout điều tra lỗi sang bản mới.
5. Kiểm tra file đầy đủ và quyền đọc pool được kế thừa từ `C:\Sites\OjitexSystem`.
6. Trong cửa sổ bảo trì: **Stop site**, **Stop pool**, đổi **Basic Settings → Physical path** của site sang release mới, **Start pool**, **Start site**. Có gián đoạn ngắn; báo trước cho người đang thao tác.
7. Chạy lại phép thử mục 11, gồm đăng nhập, quyền và báo cáo thật. Tab đã mở từ bản trước có thể cần F5 để nhận bộ file frontend mới.

Giữ release cũ cho tới khi bản mới ổn định. Cách đổi physical path tránh trộn DLL và file frontend từ hai phiên bản. Với triển khai vào thư mục đang chạy, phải stop pool hoặc dùng `app_offline.htm` đúng quy trình để tránh file bị khóa; không cần cách đó cho quy trình thư mục release trên.

### 13.2. Rollback

Nếu bản mới lỗi: Stop site/pool → đổi physical path về release cũ vẫn còn đầy đủ cấu hình → Start pool/site → kiểm tra lại. DNS và chứng chỉ không phải đổi nếu host/IP giữ nguyên.

Rollback file không hoàn tác dữ liệu người dùng đã ghi hoặc thay đổi schema. Hướng dẫn này không chạy migration; nếu một bản phát hành tương lai đổi database, phải có kế hoạch backup/rollback database riêng tương thích với bản ứng dụng cũ.

### 13.3. Những việc cần lưu trong hồ sơ vận hành

- URL, IP, DNS server/zone/record, VLAN được phép, IIS site/pool, physical path và phiên bản đang chạy.
- Vị trí cấu hình/backup được bảo vệ; người có quyền quản lý secret và SQL. Ghi nơi lưu, không dán password/SigningKey vào hồ sơ thông thường.
- Backup `EVOLIO`, `OJITEXHP`, cấu hình IIS và chứng chỉ/private key nếu chính sách cho phép xuất; thử quy trình phục hồi.
- Ngày hết hạn chứng chỉ IIS/SQL, người gia hạn; nếu thay chứng chỉ tự ký, phải phân phối trust mới tới máy trạm.
- Lịch vá Windows/.NET Hosting Bundle, bảo trì SQL, kiểm tra sau reboot, dung lượng ổ đĩa và log.
- Các release cũ được giữ trong thời gian đã thống nhất, dọn đúng thư mục sau khi xác nhận không phải bản đang chạy hoặc bản rollback cần giữ.

## 14. Danh sách bàn giao

- [ ] IP máy web cố định, route tới SQL và các VLAN đã kiểm tra.
- [ ] Hai database hiện hữu có dữ liệu đúng; đã kiểm tra quyền đọc/ghi cần thiết bằng danh tính ứng dụng.
- [ ] IIS và .NET 10 Hosting Bundle đúng kiến trúc đã cài.
- [ ] Gói publish mới có backend và `wwwroot/index.html`, không chứa secret từ máy build.
- [ ] IIS trỏ đúng publish root, pool riêng, Anonymous Authentication theo pool identity.
- [ ] Production có hai connection string, SigningKey ổn định, host/origin đúng; ACL bảo vệ cấu hình.
- [ ] DNS A record đúng và máy nhân viên thực sự dùng DNS nội bộ đó.
- [ ] HTTPS binding, SAN, private key, chuỗi tin cậy và hạn chứng chỉ đã kiểm tra.
- [ ] HTTP chuyển về HTTPS; firewall cho phép các subnet cần dùng.
- [ ] Đăng nhập, phân quyền, báo cáo/history và tải lại deep link đã nghiệm thu từ máy khác.
- [ ] Không lộ file cấu hình; stdout tạm đã tắt.
- [ ] Đã kiểm tra sau logout/reboot, có backup và cách rollback.

Tài liệu này mô tả thao tác cần thực hiện. Việc rà soát source/tài liệu chưa chứng minh IIS, DNS, chứng chỉ hoặc SQL Server thực tế tại công ty đã được cấu hình hay nghiệm thu.
