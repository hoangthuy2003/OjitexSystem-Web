# OjitexSystem Frontend

Angular 22.2 standalone, RxJS, AG Grid Community và Vitest. Xem [hướng dẫn chạy dự án](../README.md) và [kiến trúc chung](../docs/architecture.md).

Khi thêm/sửa code, tuân theo [quy chuẩn phát triển](../docs/development-standards.md), đặc biệt phần Angular, cùng [AGENTS.md](../AGENTS.md) ở repository root.

## Cấu trúc

```text
src/app/
  app.*                         # Root App chỉ render shell
  app.config.ts                 # Router, HTTP và interceptor
  app.routes.ts                 # Ghép route từng feature
  core/auth/                    # AuthService, AuthSessionStore, models, interceptor, guards
  core/http/                    # Timeout policy, API errors
  layout/app-shell/             # Header, tài khoản, đổi mật khẩu, router outlet
  features/auth/login-page/
  features/admin/user-management/
  features/logistics/stock-report/
  features/navigation/          # Catalog phòng ban/báo cáo, home, department
```

Trang, API service, models và test nằm cạnh nhau theo tính năng. Các route tải component theo nhu cầu. Mã TypeScript và template bật strict checking.

## Chạy và kiểm tra

```powershell
npm.cmd ci
npm.cmd start
npm.cmd run build
npm.cmd run test:ci
npm.cmd run format:check
```

Trên hệ điều hành khác dùng `npm` thay `npm.cmd`. Frontend chạy tại `http://localhost:4200`; `proxy.conf.json` chuyển `/api` đến `https://localhost:7013`. Backend cần chạy profile HTTPS tương ứng.

## Báo cáo tồn kho

Giữ các URL `/logistic/current-stock`, `/logistic/last-stock`, `/logistic/monthly-stock`. Route data chọn `current`, `last` hoặc `monthly` cho cùng trang báo cáo.

`GET /api/StockReport/{kind}` lấy danh sách; thêm `/{productCode}` để tìm **tiền tố** mã sản phẩm. Show All cache trong instance trang; Refresh gọi API lại. Double-click ô tồn kho gọi `/{productCode}/history` theo **mã chính xác**, kể cả tồn kho bằng 0. Monthly gửi `reportDate` từ dòng được chọn; không đoán tháng khi dữ liệu thiếu ngày báo cáo.

AG Grid dùng pagination phía client, formatter và clipboard được tách thành file riêng. Stock/history có timeout 15 giây và subscription được hủy khi rời trang.

## Quy ước

- Thêm tính năng vào thư mục nghiệp vụ phù hợp, theo [Angular style guide](https://angular.dev/style-guide).
- API service chịu trách nhiệm HTTP và timeout; component quản lý tương tác và trạng thái hiển thị.
- Session storage thuộc AuthSessionStore. Chỉ authInterceptor xử lý xóa phiên do HTTP 401 để bảo vệ phiên mới khỏi phản hồi của token cũ.
- Route guard hỗ trợ UI; quyền thực tế được backend xác minh lại.
- Mục phòng ban/báo cáo chưa triển khai ở navigation catalog; không tạo feature rỗng.
- Test `.spec.ts` nằm cạnh mã nguồn; dùng `npm.cmd run format` để định dạng.
