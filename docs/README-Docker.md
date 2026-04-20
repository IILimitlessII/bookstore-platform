# Bookstore Platform - Docker

Run the project as a SQLite-first containerized app.

## Quick start

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Open `http://localhost:8080` (or the `APP_PORT` override from `docker/.env`).

## Start existing containers (detached)

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## Runtime behavior in Docker

Compose sets:

- `Database__Provider=Sqlite`
- `ConnectionStrings__Sqlite=Data Source=/app/Data/bookstore.db`
- `Database__EnsureCreated=false`
- `Database__AutoMigrate=true`

The SQLite file is created at `/app/Data/bookstore.db` on first startup when migrations run (if the file is not already present and `/app/Data` is writable).

## Environment variables

| Variable | Purpose |
| --- | --- |
| `APP_PORT` | Host port for the web app (default `8080`) |

## Notes

- Docker startup is migration-first (`AutoMigrate=true`).
- Since `EnsureCreated=false`, schema updates should be managed through EF Core migrations.
