# Bookstore Platform - Architecture

## Solution layout

- `src/BookstorePlatform.Web` - ASP.NET Core MVC app and Identity UI.
- `src/BookstorePlatform.Application` - application services.
- `src/BookstorePlatform.Domain` - domain model layer.
- `src/BookstorePlatform.Infrastructure` - infrastructure concerns.
- `tests` - automated tests.

## Current layering note (pre-split)

The Application/Domain/Infrastructure projects are currently transition shells for portfolio structure cleanup.  
`Domain` and `Infrastructure` still compile selected source files from `Web` via `Compile Include` entries in their `.csproj` files.  
This is intentional for this pass to avoid risky behavior changes before repository split.

## Web project structure

Inside `src/BookstorePlatform.Web`:

- `Controllers` and `Views` - MVC interaction.
- `Areas/Identity` - authentication/account flows.
- `Data` and `Migrations` - EF Core context and migrations.
- `Classes` and `Models` - domain/presentation data types.
- `Services` - app services (for example email integration).
- `wwwroot` - static assets.

## Runtime flow

1. Ensure SQLite data directory exists.
2. Configure EF Core provider from app configuration.
3. Configure Identity and MVC services.
4. Initialize DB via `EnsureCreated` or `Migrate` based on `Database` settings.
5. Start middleware pipeline and MVC routes.
