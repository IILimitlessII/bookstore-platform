# Changelog

All notable changes to this project should be documented in this file.

## Unreleased

- Upgraded solution target frameworks from `net8.0` to `net10.0` and aligned Microsoft/EF/Identity/codegen dependencies to .NET 10-compatible stable versions.
- Updated Docker base images to `mcr.microsoft.com/dotnet/sdk:10.0` and `mcr.microsoft.com/dotnet/aspnet:10.0`, and validated container startup on port `8080`.
- Standardized documentation filenames and structure.
- Consolidated SQLite and migrations guidance into unified docs.

## Format

Use dated entries with concise impact notes, for example:

- `YYYY-MM-DD` - short description of what changed and why.
