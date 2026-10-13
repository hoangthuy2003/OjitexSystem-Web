# OjitexSystemFrontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 22.2.0.

## Project structure

Application code is separated by responsibility. Pages are grouped under their business department, while interfaces and services remain in their own top-level folders:

```text
src/app/
  app.config.ts
  app.routes.ts
  app.ts
  guards/
    authenticated.guard.ts
  interfaces/
    stock-report.interface.ts
  services/
    stock-report.service.ts
  views/
    department/
      department-page.*
    home/
      home-page.*
    logistic/
      stock-report/
        stock-report-page.*
```

Keep a page inside the folder for its department under `views` (for example, Logistic). Put TypeScript contracts in `interfaces` and API/business services in `services`. Add routes in `app.routes.ts`; use lazy-loaded pages for route entry points.

After signing in, users land on the department menu at `/home`. Selecting a department opens a large-button submenu beneath the shared header. The Report menu links **Current Stock**, **Last Stock**, and **Monthly Stock** to their respective routes, which use the shared Stock Report page. The remaining report entries are placeholders until their pages are implemented.

Current Stock, Last Stock, and Monthly Stock share the Stock Report component, service, and models. The existing `/logistic/current-stock`, `/logistic/last-stock`, and `/logistic/monthly-stock` routes select the report type through route data. The shared backend API is `GET /api/StockReport/{kind}`, where `kind` is `current`, `last`, or `monthly`. Append `/{productCode}` to search a product code prefix (for example, `100000` returns product codes beginning with `100000`). **Show All** loads the complete report and caches it for the current view; **Refresh** fetches it again. The view uses AG Grid Community for client-side rendering, pagination, and copying selected cells.

Double-click **Current Stock**, **Stock**, or **Closing Stock** to load `GET /api/StockReport/{kind}/{productCode}/history` for the exact product, including zero stock values. Monthly history includes `?reportDate=YYYY-MM-DD` from the selected row and uses that report month. Current and Last history use their respective snapshot tables, with report periods calculated in Vietnam time. All three reports require logistics access.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
