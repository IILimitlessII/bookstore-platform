# Bookstore Platform

Cleaned project layout:

- `src/` runtime projects (`BookstorePlatform.Web`, `Application`, `Domain`, `Infrastructure`)
- `tests/` test project
- `docker/` Docker compose and image build files
- `docs/` setup and migration notes

## Prerequisites

- .NET 10 SDK (for local restore/build/run)
- Docker Engine with Compose plugin (for container workflow)

## Quick start

From this folder:

Prerequisite: .NET 10 SDK.

```bash
dotnet restore BookstorePlatform.sln
dotnet build BookstorePlatform.sln
dotnet run --project src/BookstorePlatform.Web/BookstorePlatform.Web.csproj
```

## Docker quick start

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

App URL: `http://localhost:8080` (default)

## Docker start (already built)

When image/container already exist and you just want to run detached:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## SQLite behavior in Docker

- Runtime connection string points to `/app/Data/bookstore.db`.
- With `Database__AutoMigrate=true`, EF Core migrations create the SQLite file automatically on first container startup if it does not exist.
- No database file is copied from the source tree into the image.

## Credentials

- Demo credentials can exist in pre-seeded/local SQLite snapshots, but are not guaranteed defaults for every environment.
- Preferred flow is creating an account via the register page for your own environment.

## Documentation

See `docs/README.md` for the standardized documentation index.
