# Bookstore Platform - Deployment Guide

## Runtime configuration

Important settings:

- `ASPNETCORE_ENVIRONMENT`
- `Database__Provider`
- `ConnectionStrings__Sqlite`
- `Database__EnsureCreated`
- `Database__AutoMigrate`
- `APP_PORT` (compose host mapping)

## Container deployment

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Start already-built containers:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

Default URL: `http://localhost:8080`.

## SQLite persistence considerations

- The SQLite file is created at `/app/Data/bookstore.db` on first startup when migrations run (if no file exists yet).
- For persistent multi-restart environments, mount a volume for `/app/Data`.
- Prefer migration-based updates over ad-hoc schema edits.

## Production notes

- Review whether `AutoMigrate` should be enabled at startup in production.
- Keep backups of persistent SQLite files before schema changes.
