# Kiến trúc và rà soát OjitexSystem

Đây là mô tả cấu trúc thực tế và báo cáo lần refactor ngày 13/10/2026. Khi phát triển tiếp, tuân theo [quy chuẩn kiến trúc và phong cách code](development-standards.md), được yêu cầu bởi [AGENTS.md](../AGENTS.md). Các số liệu kiểm thử bên dưới là kết quả lịch sử của lần rà soát đó.

## Backend

Giữ một project API vì phạm vi triển khai hiện tại còn gọn. Trong mỗi layer, tổ chức tiếp theo nhóm nghiệp vụ thay vì tạo một generic repository hoặc nhiều project chỉ để chuyển tiếp lời gọi.

```text
OjitexSystem_Backend/
  OjitexSystem-Backend/
    Controllers/{Auth,Admin,Logistics}/
    Services/{Auth,Admin,Logistics}/
    Repositories/{Auth,Admin,Logistics}/
    Dtos/{Auth,Admin,Access,Logistics}/
    Data/{Auth,Production}/
    Security/
    Infrastructure/
    Common/BusinessTime.cs
    Program.cs
  OjitexSystem-Backend.Tests/
```

Luồng chính: **HTTP → Controller → Service → Repository → DbContext → SQL Server**.

- Controller xử lý binding/validation đầu vào, danh tính từ claims, authorization và HTTP status. Không chứa LINQ truy vấn database.
- Service chứa quy tắc nghiệp vụ, kiểm tra role/category hợp lệ, bảo vệ admin cuối cùng, băm mật khẩu, tính kỳ báo cáo và phối hợp dữ liệu giữa hai database. Service chỉ dùng interface repository và unit of work; không dùng EF Core hay DbContext trực tiếp.
- Repository chứa truy vấn, projection DTO, tracking và cập nhật entity. `IQueryable` chỉ dùng nội bộ repository, không trả ra service/controller.
- `IAuthUnitOfWork` điều phối SaveChanges và transaction trên cùng AuthContext scoped. Entity lấy để cập nhật được tracking trong scope này. Các truy vấn danh sách/lookup dùng `AsNoTracking` và projection.
- Handler phân quyền gọi `IAccessService`; backend kiểm tra lại tài khoản còn hoạt động và quyền hiện tại trong database cho từng yêu cầu được bảo vệ.
- `Infrastructure/ServiceCollectionExtensions` là điểm đăng ký DI, persistence, JWT, policy, CORS và xử lý lỗi. `Program.cs` chỉ lắp pipeline và xử lý chế độ CLI hiện có.
- Các class Data là mapping/entity database đã scaffold. Refactor không sửa schema hoặc mapping và không thêm migration.

Lỗi đã xử lý vẫn phát diagnostics để giữ log/metrics phục vụ điều tra. .NET 10 mặc định tắt diagnostics khi handler trả true; cấu hình `SuppressDiagnosticsCallback` tại infrastructure giữ lại chúng theo [tài liệu ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-10.0).

### Những điểm đã xử lý

| Trước rà soát | Sau refactor |
| --- | --- |
| Authentication/Admin service và authorization handler truy vấn AuthContext trực tiếp | Repository riêng cho user, access và admin; handler đi qua access service |
| Lấy một user tải toàn bộ danh sách và quyền | Áp dụng điều kiện user ID trước khi materialize |
| Mỗi user quét lại toàn bộ permission khi dựng danh sách | Lookup theo user/role/category, loại trùng quyền trực tiếp và kế thừa |
| Xóa rồi thêm lại role/category có cùng khóa đang tracking | Đồng bộ phần chênh lệch; giữ nguyên bản ghi không đổi và quyền kế thừa |
| Kiểm tra admin cuối cùng trước khi mở transaction | Kiểm tra và ghi nằm cùng transaction Serializable; admin bị khóa/xóa không tính vào số admin hoạt động |
| Stock repository phụ thuộc cả ProductionContext và AuthContext | Stock repository chỉ đọc production; service bổ sung tên người dùng qua user repository |
| Tìm tiền tố mã sản phẩm bằng ToString / StartsWith trên cột numeric | Khoảng số tương đương cho decimal(7,0), không ép cột sang chuỗi |
| Async database thiếu cancellation | Truyền CancellationToken từ HTTP qua service/repository và EF |
| Thời gian phụ thuộc timezone máy chủ | TimeProvider, thời gian nghiệp vụ UTC+7; thời hạn JWT theo UTC |
| Ngoại lệ không có phản hồi lỗi chung | ProblemDetails cho lỗi ngoài dự kiến, giữ trường message tương thích frontend; validation trả 400 |

Các API và URL hiện có được giữ, bao gồm `/api/admin/dashboard` và `/api/StockReport/{kind}`. Với DTO request dạng record, DataAnnotations đặt trên tham số primary constructor để MVC xác thực đúng.

### Quy tắc báo cáo tồn kho được giữ

- Tìm mã sản phẩm là tìm **tiền tố**; lịch sử là tìm **mã chính xác**. `0` vẫn là mã đầu vào hợp lệ; mã có tối đa bảy chữ số nguyên không âm.
- Current và Last lấy toàn bộ lịch sử của snapshot tương ứng, kể cả dòng có ngày làm việc nằm ngoài kỳ hiển thị.
- Monthly history chọn tháng của `reportDate` từ dòng đã chọn, không đoán theo ngày nhập/ngày giao dịch. Thiếu `reportDate` trả 400.
- Kỳ Current từ đầu tháng đến hôm nay UTC+7; kỳ Last là tháng trước; Monthly history bao gồm toàn bộ tháng báo cáo, kể cả tháng nhuận.
- Chính sách mật khẩu/reset hiện có được giữ. Lệnh reset hàng loạt password legacy chỉ chạy khi gọi chế độ CLI tương ứng; lần refactor này không chạy lệnh đó trên database.

## Frontend

Tổ chức theo tính năng, theo [Angular style guide](https://angular.dev/style-guide). Service API, models, trang, cấu hình và test của một tính năng nằm cạnh nhau. Các chức năng toàn ứng dụng đặt ở core; layout đặt ở layout. Không tạo thư mục shared rỗng khi chưa có phần UI thực sự dùng chung.

```text
src/app/
  app.{ts,html,scss,spec.ts}
  app.config.ts
  app.routes.ts
  core/
    auth/                         # API, session store, interceptor, models, constants, guards
    http/                         # timeout policy, đọc message/ProblemDetails
  layout/app-shell/               # header, tài khoản, đổi mật khẩu, router outlet
  features/
    auth/                        # route đăng nhập và login-page
    admin/                       # routes, user-management: API, models, page, tests
    logistics/                   # routes, stock-report: API, models, grid, clipboard, page, tests
    factory/                     # routes, corrugator-online: API, models, columns, page, tests
    navigation/                  # catalog, routes, home, department
```

- Root App chỉ render shell. Các route entry point tải component qua `loadComponent`; AG Grid chỉ tải khi mở trang báo cáo tồn kho hoặc Corrugator Online, không import vào root/core.
- `app.routes.ts` ghép route từng feature và guard xác thực toàn vùng. Giữ URL `/logistic/...` để tương thích dù thư mục nghiệp vụ tên `logistics`.
- `AuthService` xử lý HTTP và expose thông tin phiên. `AuthSessionStore` xử lý storage, schema/expiry validation, memory cache và thay đổi storage từ tab khác.
- `authInterceptor` gắn token cho API nội bộ và xử lý 401, bảo vệ phiên mới khỏi phản hồi của token cũ. Component hiển thị lỗi; lỗi 403 quản trị điều hướng về home và giữ phiên đăng nhập.
- Timeout 15 giây nằm trong API service, dùng một constant, áp dụng cả stock/history. Component không đặt lại timeout trùng lặp.
- Subscription ở component dùng `takeUntilDestroyed`; timer thông báo được cleanup. ID yêu cầu bảo vệ kết quả hiện tại khỏi phản hồi cũ. Cache Show All chỉ tồn tại trong instance trang; Refresh lấy lại dữ liệu.
- Cấu hình và formatter AG Grid, xử lý clipboard được tách khỏi component trang. Danh mục phòng ban/báo cáo nằm trong navigation catalog.
- Editor quản trị không đóng/chuyển sang tài khoản khác trong lúc lưu. TypeScript strict và Angular strictTemplates được bật; định dạng mã dùng Prettier.
- `ag-grid-community` được khai báo là dependency trực tiếp vì ứng dụng import package này trực tiếp.

## Kiểm thử và giới hạn

### Corrugator Online (16/10/2026)

- Trang `/factory/corrugator-online` được tải theo nhu cầu từ Factory Menu; nguồn dữ liệu là `GET /api/factory/corrugator-online`. Các chức năng Factory còn lại vẫn chờ triển khai.
- Backend bổ sung nhóm `Factory` trong Controllers, Services, Repositories và Dtos, giữ luồng Controller → Service → Repository → ProductionContext. Dùng entity `TCorrugatorOnlineReport` đã scaffold; không đổi mapping/schema.
- Mọi tài khoản đang hoạt động đều được xem, không yêu cầu category Factory. Policy `ActiveUser` kiểm tra lại tài khoản chưa khóa/chưa xóa qua `IAccessService` ở mỗi yêu cầu.
- Repository lọc `coor_no40_corrugated_datetime` theo tiền tố `yyMMdd` của ngày UTC+7 do service cung cấp, sắp theo timestamp rồi order no/date input để ổn định thứ tự khi trùng thời điểm. Không lọc theo plan date hoặc date input.
- Bảng hiển thị 20 cột theo Excel. Service đổi giây sang phút, làm tròn một chữ số theo SQL ROUND; AV Speed = FLOOR(quality item × corru length / drive minutes đã làm tròn / 1000). Trả 0 khi quality/length/drive time không dương hoặc drive minutes làm tròn về 0 để tránh chia cho 0. Giá trị giấy giữ quy tắc ba ký tự đầu trống thì trả chuỗi rỗng, còn lại trim dấu cách. Ngày sáu chữ số dùng ngưỡng năm hai chữ số mặc định SQL Server 2049; ngày/giờ không hợp lệ trả null.
- Trang tự tải khi mở; Refresh truy vấn lại dữ liệu, không cache. Clear chỉ xóa bảng phía trình duyệt và hủy request đang chờ, không xóa dữ liệu database. Có trạng thái loading/error/empty. Bảng dùng AG Grid/theme Quartz như Stock Report, cùng kiểu khung, header, toolbar và nút thao tác; hỗ trợ cuộn ngang, sắp xếp, kéo giãn cột và phân trang 25/50/100 dòng. Giữ đủ 20 cột; định dạng số theo locale như Stock Report, giá trị trống hiển thị dấu —. Cấu hình/formatter cột thuộc feature Factory, không import nội bộ feature Logistics và không thêm thư viện.
- Corrugator Online dùng bố cục gọn để ưu tiên dữ liệu: tiêu đề nằm giữa thanh thông tin và nhóm Refresh/Clear; bảng chiếm phần chiều cao màn hình còn lại phía trên Go Back. Theme Quartz dùng chữ 13px, dòng 32px, padding ô 8px; cột có độ rộng tối thiểu riêng 50–140px theo nội dung, header tự xuống dòng để giữ đầy đủ tên cột. Màn hình hẹp cho toolbar xuống dòng và giữ cuộn ngang bảng.
- Kiểm thử tự động bao phủ công thức, làm tròn, null/ngày lỗi, chuyển ngày UTC+7, lọc và thứ tự dòng, policy tài khoản, HTTP 401/403/200, Refresh/Clear và hủy request. Test repository dùng khóa chỉ trong model InMemory để seed bảng keyless; mapping sản xuất giữ nguyên. Chưa xác minh trên SQL Server thật.

Các kiểm thử chạy ngày 13/10/2026:

- Backend: build không warning/error, 45 test xUnit; test service/repository bằng EF InMemory, kiểm tra SQL Server dịch biểu thức bằng `ToQueryString`, HTTP pipeline bằng ASP.NET TestHost.
- Frontend: production build, 92 test Vitest và kiểm tra định dạng đều qua. Hai kỳ vọng test cũ được cập nhật theo giao diện đang có: thông báo quyền truy cập tiếng Anh và bảy user mỗi trang; không đổi giao diện để khớp kỳ vọng cũ.
- HTTP pipeline kiểm tra anonymous → 401, login thiếu dữ liệu → 400 có message/errors, lỗi ngoài dự kiến → 500 không lộ nội dung exception. OpenAPI được kiểm tra khi chạy ứng dụng local.

Không kết nối database nghiệp vụ để thay đổi hoặc benchmark dữ liệu. InMemory không chứng minh locking/rollback/hiệu năng SQL Server thực tế. Trước khi triển khai, chạy kiểm thử tích hợp trên SQL Server thử nghiệm cho đồng thời xóa/hạ quyền admin, đồng bộ quyền và báo cáo với dữ liệu thực tế; đo execution plan và thời gian truy vấn để lượng hóa cải thiện của bộ lọc tiền tố.

Không bổ sung server pagination, index database hoặc thay đổi hợp đồng Show All vì chưa có dữ liệu về kích thước báo cáo và yêu cầu phân trang. Chưa có bộ E2E trình duyệt cho dự án; test giao diện hiện tại dùng Angular TestBed/Vitest.

## Khi thêm tính năng

1. Thêm route/page/models/API service vào feature nghiệp vụ phù hợp; chỉ tạo feature mới khi có phạm vi nghiệp vụ triển khai thực sự.
2. Backend: DTO và controller cho HTTP, service cho quy tắc, repository cho truy vấn; đăng ký DI ở infrastructure.
3. Giữ kiểm tra quyền ở backend. Frontend guard chỉ hỗ trợ điều hướng và hiển thị.
4. Truyền cancellation, dùng thời gian nghiệp vụ đã thống nhất và giữ transaction đủ bao phủ invariant cần bảo vệ.
5. Thêm test cho quy tắc nghiệp vụ, boundary HTTP hoặc lỗi hồi quy; chạy build, test và format check trước khi tích hợp.
