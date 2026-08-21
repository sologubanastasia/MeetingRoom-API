# MeetingRoom database scripts

This folder is the SQL Server source of truth for the `meeting_room_anastasia` schema. Entity Framework Core migrations are not used.

## Structure

- `Tables` contains one file per table, including keys and constraints.
- `Indexes` contains indexes grouped by owning table.
- `Types` contains table-valued parameter types.
- `Functions` contains reusable database functions.
- `Triggers` contains one trigger per file.
- `StoredProcedures` contains one procedure per application operation.
- `Seeds` contains idempotent reference data.
- `Scripts` contains orchestration and verification scripts.

## Initial deployment

The database administrator must create the schema and grant the application user permissions first. In SQL Server Management Studio, enable **Query > SQLCMD Mode**, open `Scripts/InitializeDatabase.sql`, connect it to `rg-academy-6`, and execute it. The `:r` commands include the object files in dependency order.

Run `Scripts/InitializeDatabase.sql` only against an empty personal schema. Run `Scripts/VerifyDatabase.sql` after deployment.

For the existing development database where the four tables are already present, execute `Scripts/UpdateExistingDatabase.sql` in SQLCMD mode instead of the initialization script.

## Application access

Repositories should call the procedures in `StoredProcedures` through `Microsoft.Data.SqlClient` or Dapper. Every command must use parameters; application code must not concatenate user input into SQL.
