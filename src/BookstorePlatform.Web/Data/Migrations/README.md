# Migrations folder

SQLite migrations are now generated in the top-level `Migrations` folder (`InitialSqlite`).

`Database:EnsureCreated` should remain `false` and `Database:AutoMigrate` should remain `true` for migration-based startup.

See [database docs](../../../../docs/README-Database.md) for commands to add new migrations.
