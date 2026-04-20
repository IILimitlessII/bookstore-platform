# Bookstore Platform - Database Guide

## Database engine and location

- Engine: SQLite.
- Canonical database file: `src/BookstorePlatform.Web/Data/bookstore.db`.

## Configuration keys

- `ConnectionStrings:Sqlite` (default `Data Source=Data/bookstore.db`)
- `Database:Provider` (default `Sqlite`)
- `Database:EnsureCreated` (recommended `false` when using migrations)
- `Database:AutoMigrate` (recommended `true`)

## Startup behavior

At startup:

- If provider is SQLite and `Database:EnsureCreated=true`, the app uses `EnsureCreated`.
- Otherwise, if `Database:AutoMigrate=true`, the app applies EF Core migrations.

Current default configuration is migration-first.

## Migration workflow

Prerequisites:

- .NET 10 SDK
- EF CLI tool (`dotnet-ef`)

Commands from `src/BookstorePlatform.Web`:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Demo credentials note

Some historical docs listed demo login values (`test@email.com`, `rahul@email.com`, `Password@123`).  
Treat these as environment-specific sample data, not guaranteed defaults for all runs.
