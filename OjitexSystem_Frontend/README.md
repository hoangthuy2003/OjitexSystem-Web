# OjitexSystemFrontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 22.2.0.

## Project structure

Application code is separated by responsibility. Pages are grouped under their business department, while interfaces and services remain in their own top-level folders:

```text
src/app/
  app.config.ts
  app.routes.ts
  app.ts
  interfaces/
    current-stock.interface.ts
  services/
    current-stock.service.ts
  views/
    logistic/
      current-stock/
        current-stock-page.*
```

Keep a page inside the folder for its department under `views` (for example, Logistic). Put TypeScript contracts in `interfaces` and API/business services in `services`. Add routes in `app.routes.ts`; use lazy-loaded pages for route entry points.

The current-stock view searches product code prefixes through `GET /api/CurrentStock/search/{prefix}` (for example, `100000` returns product codes beginning with `100000`). The exact-code endpoint `GET /api/CurrentStock/{productCode}` remains available. The view uses the free AG Grid Community Angular component for client-side rendering and pagination. Use **Tìm tất cả** only to browse the inventory; `GET /api/CurrentStock` returns the complete list, which is cached for the current view to avoid repeatedly downloading it while browsing. **Tải lại** fetches the inventory again. AG Grid Community is free; Enterprise features require a paid licence.

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
