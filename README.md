# Blazor Generic List

A reusable, generic data grid component for **Blazor WebAssembly**, with a demo app that uses it.

**Live demo: [mohammad-diab.github.io/BlazorGenericList](https://mohammad-diab.github.io/BlazorGenericList/)**

## Features
- **Generic:** `GenericList<T>` works with any item type. You pass the header and the row template.
- **Paging, sorting and filtering**, with a configurable number of items per page.
- **Single or multi select**, also inside a modal dialog.
- **Add, edit and delete** items, including deleting several at once, with a confirm dialog and notifications.
- **Export** the current page as JSON, XML, CSV or PDF.
- Small reusable parts: autocomplete, dropdown, pagination, modal, confirm dialog, notification, lazy content.

## Demo pages
- **Home:** a grid in a modal; pick users with single or multi select.
- **Edit Grid:** the full grid with add, edit, delete and export.
- **Settings:** items per page, and an admin switch that shows or hides the edit actions.

The demo has no back end: `ServerSimulator` plays the server inside the browser.

## Run locally
```
dotnet run --project BlazorApp
```
Then open the address it prints. The project targets Blazor WebAssembly 3.2 (.NET Core 3.1 SDK era); the current .NET SDK still builds it.

## Deployment
Each push to `master` runs `.github/workflows/pages.yml`, which builds the site and deploys it with GitHub Pages' own deployment (no personal token). It can also be run by hand from the Actions tab.

## History
The first prototype (May 2020, before this repository started) is kept on the [`prototype`](https://github.com/Mohammad-Diab/BlazorGenericList/tree/prototype) branch.

## License
[Apache 2.0](LICENSE)
