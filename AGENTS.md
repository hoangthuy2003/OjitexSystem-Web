# Quy tắc làm việc trong OjitexSystem

Áp dụng cho toàn bộ repository. Trước khi sửa code, đọc [quy chuẩn phát triển](docs/development-standards.md) và phần code/test liên quan. Đây là quy chuẩn cần tuân theo cho các lần thêm tính năng, sửa lỗi và refactor tiếp theo; không cần người dùng nhắc lại.

## Nguồn quy chuẩn

- `docs/development-standards.md`: nguồn chuẩn về kiến trúc, cách đặt tên, phong cách code và kiểm tra thay đổi.
- `docs/architecture.md`: mô tả cấu trúc thực tế, quy tắc nghiệp vụ đang có và kết quả rà soát trước đây. Các số lượng test trong báo cáo là lịch sử, không phải ngưỡng cố định.
- `README.md`: cách chạy, cấu hình và lệnh kiểm tra dự án.
- Nếu có `AGENTS.md` ở thư mục con, đọc khi làm việc trong phạm vi đó. Hướng dẫn con bổ sung quy tắc theo phạm vi; khi phát hiện mâu thuẫn, nêu rõ và giải quyết theo yêu cầu hiện tại của người dùng, không âm thầm bỏ qua.

## Ranh giới bắt buộc

- Backend đi theo **Controller → Service → Repository → DbContext**. Controller không gọi repository/DbContext. Service và authorization handler không dùng EF Core/DbContext trực tiếp. Service dùng interface repository; handler gọi `IAccessService`.
- Mỗi layer backend chia tiếp theo feature `Auth`, `Admin`, `Logistics`. Repository không trả `IQueryable`/`DbSet` ra ngoài. EF entity có thể được service dùng nội bộ để cập nhật, nhưng API phải dùng DTO.
- Service điều phối nghiệp vụ và transaction qua unit of work. Không chuyển kiểm tra nghiệp vụ xuống controller hoặc tự gọi `SaveChanges` trong từng repository. Truyền `CancellationToken` đến thao tác I/O.
- Frontend tổ chức theo `core`, `layout`, `features`. Page, API service, models và test thuộc cùng feature nằm cạnh nhau. Không khôi phục các thư mục toàn cục `views`, `services`, `interfaces`, `guards` dưới `src/app`.
- `core` chứa cơ chế toàn ứng dụng; `layout` chứa shell; `features` chứa nghiệp vụ. Chỉ tạo phần dùng chung khi có nhu cầu tái sử dụng thực tế. Không tạo module rỗng cho các mục phòng ban dự kiến.
- Root App chỉ render shell. Route feature được ghép ở `app.routes.ts`; page được tải theo nhu cầu. Không đưa HTTP trực tiếp vào component/guard hoặc logic nghiệp vụ vào template.
- `AuthSessionStore` sở hữu browser storage của phiên đăng nhập. `authInterceptor` xử lý phiên khi API trả 401 và bảo vệ phiên mới trước phản hồi của token cũ. 403 không được tự xóa phiên.
- Dùng timeout chung trong API service, `takeUntilDestroyed` cho subscription do component giữ, cleanup timer và bảo vệ state khỏi phản hồi cũ.

## Các hành vi phải giữ

- Giữ API/URL/JSON và UI đang hoạt động trừ phần người dùng yêu cầu thay đổi. Tên thư mục `logistics` không phải lý do đổi URL `/logistic/...` hoặc `/api/StockReport/{kind}`.
- Không cho phép xóa/hạ quyền admin hoạt động cuối cùng; kiểm tra và ghi phải nằm trong cùng transaction phù hợp. Quyền trực tiếp và quyền kế thừa phải được xử lý đúng nguồn.
- Stock search tìm tiền tố mã; stock history tìm mã chính xác. Current/Last dùng snapshot tương ứng; Monthly dùng tháng `reportDate` của dòng được chọn.
- Thời gian nghiệp vụ dùng `TimeProvider` và `BusinessTime` UTC+7; thời hạn JWT dùng UTC.
- Không thay schema/mapping đã scaffold hoặc chạy migration/reset mật khẩu hàng loạt như một bước kiểm tra refactor. Chỉ thực hiện khi thuộc yêu cầu được giao. Không ghi secret/token/password vào code hoặc log.

## Cách hoàn thành công việc

1. Kiểm tra working tree; giữ các thay đổi đang có. Xác định feature và layer trước khi viết code, tận dụng mẫu hiện có phù hợp quy chuẩn.
2. Hoàn thành thay đổi trong phạm vi yêu cầu. Không thêm framework, generic repository, base class hoặc tầng trung gian nếu chưa giải quyết một nhu cầu cụ thể.
3. Thêm/cập nhật test cho hành vi nghiệp vụ hoặc lỗi hồi quy khi cần. Chạy các kiểm tra tương ứng bên dưới; sửa lỗi do thay đổi gây ra. Với thay đổi chỉ có tài liệu, kiểm tra nội dung và liên kết, không cần build/test ứng dụng.
4. Tự kiểm tra diff về ranh giới layer, quyền truy cập, tương thích và định dạng. Báo rõ đã thay đổi gì, đã kiểm tra gì và giới hạn chưa kiểm chứng.
5. Nếu yêu cầu hiện tại cần đổi kiến trúc, nêu lý do và cập nhật quy chuẩn/mô tả kiến trúc cùng thay đổi. Không sửa quy chuẩn chỉ để hợp thức hóa việc code sai layer. Lựa chọn triển khai thông thường trong kiến trúc này được tự thực hiện.

## Lệnh kiểm tra

Backend, chạy tại repository root:

```powershell
dotnet build OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
dotnet test OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
```

Frontend, chạy tại `OjitexSystem_Frontend`:

```powershell
npm.cmd run build
npm.cmd run test:ci
npm.cmd run format:check
```

Restore/install chỉ khi dependency chưa sẵn có hoặc thay đổi. Trên hệ điều hành khác dùng `npm`. Khi file build bị tiến trình đang chạy giữ khóa, dùng output riêng theo README. Không báo đã kiểm tra SQL Server thực tế chỉ từ EF InMemory hoặc `ToQueryString`.
