# Giải đáp Chuyên sâu 4 Thắc mắc về Mạng Nội bộ (IP, Port, DNS) và Bảo mật (`SigningKey`) khi Host OjitexSystem trên Windows Server

Tài liệu này giải thích chi tiết và thực tế 4 câu hỏi liên quan đến cấu hình **Site Binding trên IIS** và tệp **`appsettings.Production.json`** của dự án **OjitexSystem**.

---

## Câu hỏi 1: "IP tĩnh nội bộ của máy chủ" nghĩa là sao? Có phải là IPv4 của chiếc PC/Server đó không?

### 1.1. Bản chất: Đúng, đó chính là địa chỉ IPv4 của máy chủ trong mạng LAN công ty
Khi máy chủ Windows Server kết nối vào mạng nội bộ (LAN) của công ty qua dây mạng (Ethernet), nó sẽ có một địa chỉ **IPv4** nội bộ (thường có dạng `192.168.x.x`, `10.x.x.x` hoặc `172.16.x.x`).

Để xem địa chỉ này trên máy chủ Windows Server, bạn mở **Command Prompt (CMD)** và gõ:
```powershell
ipconfig
```
Tìm đến dòng **IPv4 Address** (ví dụ: `192.168.1.100`). Đây chính là địa chỉ để các máy tính khác trong công ty tìm thấy máy chủ của bạn.

### 1.2. Tại sao phải gọi là "IP Tĩnh" (Static IP) mà không phải "IP Động" (Dynamic/DHCP)?
Trong mạng công ty có 2 cơ chế cấp IPv4:
- **IP Động (DHCP - Mặc định trên PC cá nhân)**: Mỗi khi máy tính tắt đi bật lại hoặc mất điện khởi động lại, bộ phát mạng (Router/DHCP Server) có thể cấp cho máy đó một số IPv4 mới (hôm nay là `192.168.1.100`, tuần sau khởi động lại biến thành `192.168.1.158`). Nếu máy chủ web bị đổi IP, toàn bộ nhân viên trong công ty sẽ **không truy cập được đường dẫn cũ nữa**.
- **IP Tĩnh (Static IP - Bắt buộc cho Máy chủ)**: Là địa chỉ IPv4 được quản trị viên mạng (IT) **gán cố định vĩnh viễn** cho máy chủ Windows Server đó (trong phần *Control Panel → Network Connections → IPv4 Properties → Use the following IP address*, hoặc khóa cố định theo địa chỉ MAC trên Router công ty). Dù máy chủ có khởi động lại bao nhiêu lần, IPv4 của nó vẫn luôn là `192.168.1.100`.

### 1.3. Trong cửa sổ IIS Binding, nên chọn `All Unassigned` hay chọn IPv4 cụ thể?
Khi tạo Website trên IIS, tại mục **IP address**, bạn sẽ thấy lựa chọn mặc định là **`All Unassigned`**:
- **Nên giữ nguyên là `All Unassigned` (Khuyến nghị)**: Khi để `All Unassigned`, IIS sẽ lắng nghe trên **mọi địa chỉ IP** mà máy chủ đó đang có. Nhờ vậy:
  - Bạn ngồi ngay trên máy chủ gõ `http://localhost` (IP nội tại `127.0.0.1`) → **Vào được**.
  - Nhân viên ngồi ở máy trạm khác trong công ty gõ `http://192.168.1.100` (IPv4 LAN của máy chủ) → **Cũng vào được**.
- Nếu bạn chọn cứng `192.168.1.100` thay vì `All Unassigned`, thì ngay trên máy chủ khi bạn gõ `http://localhost` sẽ không vào được (bắt buộc phải gõ đúng `http://192.168.1.100`).

---

## Câu hỏi 2: `SigningKey` trong `appsettings.Production.json` dùng để làm gì?

Trong dự án **OjitexSystem**, `Authentication:SigningKey` là **"Con dấu mật mã tối mật"** của Backend dùng cho cơ chế đăng nhập **JWT (JSON Web Token)**.

### 2.1. Cách `SigningKey` hoạt động trong code của bạn
Hãy nhìn vào 2 tệp trong mã nguồn Backend của bạn:
1. **`JwtTokenService.cs`** (`OjitexSystem_Backend/Oji texSystem-Backend/Security/JwtTokenService.cs`):
   ```csharp
   var credentials = new SigningCredentials(
       new SymmetricSecurityKey(signingKey),
       SecurityAlgorithms.HmacSha256);
   ```
   Khi một nhân viên nhập đúng tài khoản và mật khẩu ở màn hình Login (`/api/auth/login`), Backend sẽ tạo ra một tấm "thẻ thông hành điện tử" (**JWT Token** có hạn 1 tiếng) chứa Mã nhân viên (`userId`) và Quyền hạn (`roles`). Trước khi gửi thẻ này về trình duyệt của nhân viên, Backend dùng chuỗi `SigningKey` để **đóng dấu chữ ký số HMAC-SHA256** lên thẻ.
2. **`ServiceCollectionExtensions.cs`** (`OjitexSystem_Backend/OjitexSystem-Backend/Infrastructure/ServiceCollectionExtensions.cs`):
   ```csharp
   ValidateIssuerSigningKey = true,
   IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
   ```
   Mỗi khi nhân viên bấm xem báo cáo **Stock Report**, **Corrugator Online**, hoặc vào trang **Admin**, trình duyệt sẽ gửi kèm tấm thẻ JWT đó lên Server. Backend lấy đúng chuỗi `SigningKey` này ra để kiểm tra chữ ký trên thẻ:
   - Nếu ai đó ở trình duyệt cố tình sửa nội dung thẻ (ví dụ tự sửa quyền từ nhân viên thường thành `ADMIN`), chữ ký sẽ bị lệch với `SigningKey` và Server lập tức từ chối (`401 Unauthorized`).

### 2.2. Tại sao chạy trên máy cá nhân không cần điền mà đưa lên Windows Server lại bắt buộc phải điền?
Trong `ServiceCollectionExtensions.cs` (dòng 100–115), code của dự án được viết rất chặt chẽ về bảo mật:
```csharp
if (string.IsNullOrWhiteSpace(signingKey))
{
    if (!environment.IsDevelopment())
    {
        throw new InvalidOperationException(
            "Set Authentication:SigningKey (at least 32 characters) before starting outside Development.");
    }

    signingKey = GetDevelopmentSigningKey(environment.ContentRootPath);
}
```
- **Khi chạy ở máy cá nhân (`Development`)**: Nếu bạn chưa cấu hình `SigningKey`, code tự động sinh ra một tệp tạm tại `.local/jwt-signing-key` để bạn lập trình cho tiện.
- **Khi đưa lên IIS trên Windows Server (`Production`)**: Biến môi trường mặc định là `Production`. Để tránh việc mỗi lần triển khai lại hoặc không có quyền ghi file làm mất khóa hoặc lộ khóa, code **bắt buộc bạn phải khai báo tường minh `Authentication:SigningKey`** (dài ít nhất **32 ký tự**) trong `appsettings.Production.json`. Nếu bạn quên điền hoặc điền chuỗi ngắn hơn 32 ký tự, ứng dụng sẽ ném lỗi ngay dòng lệnh trên và IIS báo **HTTP Error 500.30**.

---

## Câu hỏi 3: Cổng (Port) dùng để làm gì? Cổng `80` và cổng `8080` khác gì nhau?

### 3.1. Cổng (Port) là gì?
Hãy hình dung:
- **Địa chỉ IP của máy chủ** (`192.168.1.100`) giống như **Địa chỉ của một tòa nhà văn phòng** (chiếc máy chủ Windows Server).
- Nhưng bên trong chiếc máy chủ đó có rất nhiều phần mềm cùng chạy một lúc (SQL Server, Remote Desktop, Chia sẻ file, và các trang Web).
- **Số cổng (Port - từ 1 đến 65535)** chính là **"Số phòng giao dịch"** bên trong tòa nhà đó để hệ điều hành Windows biết gói tin gửi đến thuộc về phần mềm nào:
  - Gói tin đến phòng `1433` → giao cho **SQL Server**.
  - Gói tin đến phòng `3389` → giao cho **Remote Desktop**.
  - Gói tin đến phòng `80` hoặc `8080` → giao cho **IIS Web Server**.

### 3.2. Sự khác biệt thực tế giữa Cổng `80` và Cổng `8080`

Về mặt tốc độ và kỹ thuật, cổng `80` và `8080` hoạt động giống hệt nhau. Sự khác biệt nằm ở **quy ước mặc định của trình duyệt web** và **trải nghiệm gõ địa chỉ của người dùng**:

| Tiêu chí | Cổng `80` (Cổng chuẩn mặc định của HTTP) | Cổng `8080` (hoặc `5000`, `8081`... - Cổng phụ tùy chọn) |
| :--- | :--- | :--- |
| **Cách nhân viên gõ địa chỉ trên Chrome / Edge** | **Không cần gõ số cổng!**<br>Chỉ cần gõ: `http://192.168.1.100` hoặc `http://ojitex-svr`<br>*(Trình duyệt thấy `http://` sẽ tự động hiểu ngầm là cổng 80)*. | **Bắt buộc phải gõ kèm `:8080` ở đuôi!**<br>Phải gõ: `http://192.168.1.100:8080` hoặc `http://ojitex-svr:8080`<br>*(Nếu quên `:8080`, trình duyệt sẽ tìm vào cổng 80 và không thấy trang web)*. |
| **Khi nào nên dùng?** | **Nên ưu tiên số 1** nếu máy chủ Windows Server đó chỉ chạy duy nhất trang web `OjitexSystem` (hoặc bạn muốn `OjitexSystem` là trang web chính thức dễ nhớ nhất). | Dùng khi trên máy chủ Windows Server đó **đã có sẵn một phần mềm web nội bộ khác của công ty đang chạy ở cổng `80`** và bạn không được phép tắt phần mềm cũ đó. |
| **Lưu ý khi cấu hình trên IIS** | Khi mới cài IIS, Microsoft tạo sẵn một trang mẫu tên là `Default Web Site` đang chiếm cổng `80`. Bạn chỉ cần chuột phải vào `Default Web Site` → chọn **Stop**, rồi gán cổng `80` cho `OjitexSystem`. | Không cần tắt `Default Web Site`, hai trang web có thể chạy song song trên cùng 1 máy chủ (một trang ở cổng `80`, một trang ở cổng `8080`). |

*(Lưu ý bổ sung: Tương tự như cổng `80` là mặc định của `http://`, thì cổng `443` là cổng mặc định của giao thức có khóa bảo mật `https://`).*

---

## Câu hỏi 4: Cách cấu hình để có tên dễ nhớ (Hostname / DNS nội bộ) thay vì phải nhớ dãy số IP
   
Để nhân viên trong công ty có thể gõ một cái tên dễ nhớ (như `http://ojitex` hoặc `http://ojitex.local`) thay vì phải nhớ dãy số `http://192.168.1.100`, bạn có **3 cách từ dễ nhất (không cần cài đặt thêm) đến chuẩn doanh nghiệp**:

### Cách 1: Dùng ngay Tên Máy Chủ (Computer Name / Hostname) của Windows Server — *Dễ nhất, làm được ngay trong 1 phút!*
Trong mạng LAN nội bộ dùng hệ điều hành Windows, các máy tính có khả năng tự nhận diện tên của nhau (qua giao thức NetBIOS / LLMNR / mDNS / Active Directory).

1. **Bước 1**: Trên máy chủ Windows Server, mở **CMD** và gõ lệnh:
   ```powershell
   hostname
   ```
   Giả sử màn hình hiện ra tên máy chủ là `OJITEX-SERVER` (hoặc bạn có thể đổi tên máy chủ trong *System Properties → Computer Name* thành `ojitex` rồi khởi động lại máy chủ).
2. **Bước 2**: Khi cấu hình **Binding** của Website trên IIS:
   - **IP address**: Để là `All Unassigned`.
   - **Port**: `80` (hoặc `8080`).
   - **Host name**: **ĐỂ TRỐNG (Không điền gì cả)**.
3. **Kết quả**: Bất kỳ máy tính nào trong cùng mạng LAN công ty chỉ cần mở trình duyệt và gõ:
   - `http://ojitex-server` (nếu dùng cổng 80)
   - Hoặc `http://ojitex-server:8080` (nếu dùng cổng 8080)
   là vào thẳng trang web **OjitexSystem** mà không cần cài đặt DNS Server phức tạp!

---

### Cách 2: Cấu hình trên DNS Server Nội bộ của Công ty — *Chuẩn Doanh nghiệp (Dùng tên miền tùy ý như `http://ojitex.local` hoặc `http://erp.ojitex.vn`)*
Nếu công ty bạn đã có máy chủ quản lý miền (**Windows Server Active Directory / Domain Controller**) hoặc **Router/Firewall doanh nghiệp** đóng vai trò là DNS Server nội bộ cho toàn công ty:

1. **Bước 1: Mở công cụ quản lý DNS trên máy chủ DNS của công ty**
   - Trên máy chủ Domain Controller / DNS Server của công ty, nhấn `Windows + R`, gõ `dnsmgmt.msc` và nhấn Enter để mở **DNS Manager**.
2. **Bước 2: Tạo bản ghi trỏ tên miền về IP của máy chủ Web**
   - Mở rộng nhánh **Forward Lookup Zones**.
   - **Trường hợp A (Công ty đã có sẵn Zone nội bộ, ví dụ `ojitex.local` hoặc `company.local`)**:
      - Nhấp chuột phải vào Zone đó → chọn **New Host (A or AAAA)...**
      - Ô **Name**: Điền `erp` hoặc `web` (khi đó tên đầy đủ sẽ là `erp.ojitex.local`). Nếu để trống ô Name thì tên truy cập sẽ là `ojitex.local`.
      - Ô **IP address**: Điền địa chỉ IPv4 tĩnh của máy chủ chạy IIS (ví dụ `192.168.1.100`).
      - Nhấn **Add Host**.
   - **Trường hợp B (Muốn tạo một tên miền nội bộ hoàn toàn mới, ví dụ `ojitex.local`)**:
      - Nhấp chuột phải vào **Forward Lookup Zones** → chọn **New Zone...** → chọn **Primary zone** → đặt **Zone name** là `ojitex.local` → nhấn Next cho đến khi Finish.
      - Nhấp chuột phải vào vùng `ojitex.local` vừa tạo → chọn **New Host (A or AAAA)...**
      - Để trống ô **Name** (để dùng trực tiếp tên `ojitex.local`), điền **IP address** là `192.168.1.100` → nhấn **Add Host**.
3. **Bước 3: Cấu hình ô `Host name` trong IIS Binding (Tùy chọn)**
   - Trên máy chủ chạy IIS, nếu bạn để trống ô **Host name** trong Binding (như Cách 1), thì nhân viên gõ `http://192.168.1.100`, `http://ojitex-server` hay `http://ojitex.local` **đều vào được tất cả**.
   - Bạn chỉ cần điền `ojitex.local` vào ô **Host name** của IIS khi và chỉ khi bạn muốn **chạy nhiều trang web khác nhau trên cùng một cổng `80`** (ví dụ: gõ `http://hr.ojitex.local` cổng 80 thì vào web Nhân sự, còn gõ `http://erp.ojitex.local` cổng 80 thì vào `OjitexSystem`). Nếu máy chủ chỉ chạy 1 web ở cổng đó, **hãy để trống ô Host name trong IIS** để vào bằng cách nào cũng được!

---

### Cách 3: Sửa file `hosts` trên từng máy trạm (Chỉ dùng khi công ty không có DNS Server và chỉ có vài máy trạm cần tên riêng)
Nếu công ty chưa có DNS Server nội bộ mà bạn không muốn dùng Computer Name ở Cách 1, bạn có thể mở Notepad với quyền **Run as Administrator** trên máy tính của người dùng, mở tệp:
`C:\Windows\System32\drivers\etc\hosts`
Và thêm 1 dòng ở cuối tệp:
```text
192.168.1.100    ojitex.local
```
Lưu lại, từ đó máy tính đó gõ `http://ojitex.local` sẽ tự động hiểu là `192.168.1.100`. *(Tuy nhiên với mạng nhiều máy tính trong nhà máy, **Cách 1** hoặc **Cách 2** luôn tiện lợi hơn rất nhiều).*
