# Docker setup (Windows)

## One-time laptop setup

1. Open PowerShell **as Administrator** and run:

   ```powershell
   wsl --install
   ```

2. Restart Windows.
3. Install Docker Desktop from https://www.docker.com/products/docker-desktop/
   and keep the WSL 2 backend enabled.
4. Open Docker Desktop and wait until it reports that the engine is running.

## Run the project

From the repository root:

```powershell
docker compose up --build
```

Open Swagger at http://localhost:8080/swagger.

The API waits for PostgreSQL to become healthy and applies Entity Framework
migrations automatically. Database data persists between restarts.

## Stop the project

```powershell
docker compose down
```

To remove the local database data as well:

```powershell
docker compose down --volumes
```
