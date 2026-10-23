# Deploying HireLink to Render

HireLink is a root-level ASP.NET Core MVC application targeting .NET 10. The included `Dockerfile` builds `HireLink.csproj` and starts `HireLink.dll` on `0.0.0.0` using Render's `PORT` value. It does not run EF Core migrations.

## Required Render configuration

Create a **Web Service** from the GitHub repository and use these settings:

| Render setting | Value |
| --- | --- |
| Branch | `main` |
| Runtime | Docker |
| Root Directory | Leave blank (repository root) |
| Dockerfile Path | `./Dockerfile` |
| Docker Build Context | `.` |
| Health Check Path | `/` (after a reachable database is configured) |
| Auto-Deploy | Yes, on commit |

Add this environment variable in Render's dashboard (mark it secret):

| Variable | Value |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | The complete connection string for the remotely hosted SQL Server database |
| `ASPNETCORE_ENVIRONMENT` | `Production` (recommended explicitly) |

Do not add this connection string to `appsettings.json`, Git, Docker build arguments, or Render's plain-text service configuration. The double underscore maps to the `ConnectionStrings:DefaultConnection` configuration key used by the application. Render provides `PORT`; do not set it manually unless Render explicitly requires it.

## SQL Server and migrations

The application uses `Microsoft.EntityFrameworkCore.SqlServer` and currently has one migration, `20261017162020_AddUserStatusAndCreatedAt`. Its local connection string used SQL Server Express with Windows integrated authentication, which cannot be used from Render.

Provision a network-accessible SQL Server-compatible database and allow incoming connections from Render as required by that provider. Use a SQL-authentication connection string, normally including server hostname, database name, user ID, password, encryption settings, and any provider-required trust/certificate options. Obtain all values from the database host; do not guess them.

Before the first deploy, take a backup and apply migrations deliberately from a trusted machine or CI job that has network access to the database:

```powershell
$env:ConnectionStrings__DefaultConnection = '<remote SQL Server connection string>'
dotnet ef database update --project .\HireLink.csproj
```

Review the generated SQL first if the database already contains the `Users` and `Jobs` tables. This project's only checked-in migration adds `IsActive` and `CreatedAt`; it does not create the original schema. Therefore, an empty remote database cannot be initialized from the migrations in this repository alone: obtain and apply the original schema script or add a reviewed baseline migration in a separate, tested change before using an empty database. Do not use `EnsureCreated`, `database drop`, or any reset command in production. The application intentionally does not migrate databases on startup.

Existing accounts whose `PasswordHash` column still contains a legacy plaintext value can sign in once; that successful sign-in replaces the value with an ASP.NET Core password hash. Newly registered accounts are always stored as hashes. This needs no database migration, but a backup before the first deployment remains essential.

## GitHub auto-deploy

1. Push this deployment configuration to the repository's `main` branch yourself.
2. In Render, choose **New +** -> **Web Service** and connect GitHub if prompted.
3. Select `farhanchowdhury738/job-internship-platform`, then apply the settings above.
4. Add `ConnectionStrings__DefaultConnection` and save it.
5. Deploy manually once and check the Render build and runtime logs.
6. After the service is healthy, confirm **Auto-Deploy** is set to deploy on every commit to `main`.
7. Make a small non-secret commit to `main`; the Render service's Events/Deploys page should show a new deployment for that commit SHA. Confirm the public URL loads and that login, role-restricted pages, and static CSS work.

## Operational notes

- The production error page does not disclose exception details. Detailed errors remain available only in the Development environment.
- Render terminates HTTPS before forwarding requests to the container. The app consumes `X-Forwarded-Proto`, redirects HTTP requests to HTTPS, and uses secure cookies outside Development.
- Authentication cookies use ASP.NET Core data-protection keys stored in the container by default. Because Render containers can be replaced, users may need to sign in again after a deploy or restart. Use a persistent, shared data-protection key store before requiring sessions to survive replacements or before scaling beyond one instance.
- The application loads Bootstrap, Google Fonts, and validation scripts from public CDNs; those assets require browser access to those CDNs.

## Free-tier and cost considerations

Render's Free web services are for demos and testing, not durable production: they spin down after 15 idle minutes, can take about a minute to wake, have ephemeral filesystems, may restart, and are limited to 750 free instance-hours per workspace each calendar month. Outbound bandwidth and build-pipeline limits also apply; exceeding included limits can create charges if a payment method is present or suspend services otherwise. Free services cannot use persistent disks or scale beyond one instance.

This application requires SQL Server, whereas Render's managed relational database is PostgreSQL. Keep the SQL Server provider and use an externally hosted SQL Server unless you explicitly decide to change providers. Budget separately for that database host, backups, and network access. Do not use a free Render Postgres database for this SQL Server application without a separately approved provider migration.
