# Contributing

## Local setup

Prerequisite: .NET 10 SDK.

```bash
dotnet restore BookstorePlatform.sln
dotnet build BookstorePlatform.sln
dotnet run --project src/BookstorePlatform.Web/BookstorePlatform.Web.csproj
```

For containerized setup, follow `README-Docker.md`.

## Contribution workflow

1. Create a focused branch for each change.
2. Keep commits scoped and reviewable.
3. Run build/tests relevant to your update.
4. Update docs when runtime/config behavior changes.

## Database changes

- Add EF migrations for schema updates.
- Keep `README-Database.md` current with migration steps.

## Pull request checklist

- Project builds successfully.
- Relevant tests pass.
- New environment variables are documented.
- Docs links remain valid.
