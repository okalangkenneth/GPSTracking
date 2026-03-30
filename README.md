# GPS Fleet Tracking Platform

A microservices-based GPS fleet tracking system built with .NET 8, PostgreSQL, and Docker.
Demonstrates distributed systems design, inter-service communication, and real-time
vehicle tracking across a fleet of drivers.

## Live Demo

[**View Live Demo →**](https://okalangkenneth.github.io/GPSTracking/)

## Architecture

The platform consists of 4 independent microservices:

| Service | Port | Responsibility |
|---------|------|----------------|
| **GPSTrackings API** | 17108 | Stores and retrieves vehicle GPS coordinates, speed, and timestamps |
| **Drivers API** | 32014 | Manages driver profiles, license data, and contact information |
| **Notifications API** | 51378 | Tracks driver alerts — speed limit violations, restricted area entries |
| **Search API** | 5010 | Aggregator/BFF — composes data from all 3 services into a unified response |

```
┌─────────────────────────────────────────────────────────┐
│                      Client / Demo UI                    │
└──────────────────────────┬──────────────────────────────┘
                           │ POST /api/search
                           ▼
                  ┌─────────────────┐
                  │   Search API    │  :5010
                  │  (Aggregator)   │
                  └────────┬────────┘
           ┌───────────────┼───────────────┐
           │               │               │
           ▼               ▼               ▼
  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐
  │  Drivers API │ │GPSTrackings  │ │Notifications │
  │    :32014    │ │   API :17108 │ │  API :51378  │
  └──────┬───────┘ └──────┬───────┘ └──────┬───────┘
         │                │                │
         └────────────────┴────────────────┘
                          │
                   ┌──────▼──────┐
                   │  PostgreSQL │
                   │    :5432    │
                   └─────────────┘
```

## Tech Stack

- **Runtime**: .NET 8 / ASP.NET Core 8
- **Database**: PostgreSQL 16 with EF Core 8 (code-first migrations)
- **ORM**: Entity Framework Core 8 + Npgsql
- **Mapping**: AutoMapper 12
- **Documentation**: Swagger / OpenAPI (Swashbuckle 6.9)
- **Containerisation**: Docker + Docker Compose
- **Testing**: xUnit 2.9 + coverlet

## Running Locally

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Start all services
```bash
git clone https://github.com/okalangkenneth/GPSTracking.git
cd GPSTracking
docker compose up --build
```

All 4 services and PostgreSQL start automatically. Databases are created and
seeded on first run.

### Swagger UIs (Development mode)

| Service | URL |
|---------|-----|
| GPSTrackings | http://localhost:17108/swagger |
| Drivers | http://localhost:32014/swagger |
| Notifications | http://localhost:51378/swagger |
| Search | http://localhost:5010/swagger |

## API Reference

### GET /api/drivers
Returns all registered drivers.

### GET /api/drivers/{id}
Returns a single driver by ID.

### GET /api/gPSTrackings
Returns all vehicle GPS records (location, speed, timestamp).

### GET /api/gPSTrackings/{id}
Returns a single GPS tracking record.

### GET /api/notifications/{driverId}
Returns all notifications for a driver (speed alerts, geofence violations).

### POST /api/search
Aggregates driver profile + notifications + vehicle GPS data in one call.

Request body:
```json
{ "driverId": 1 }
```

## Project Structure

```
GPSTracking/
├── GPSTracking.Api.GPSTrackings/        # Vehicle tracking microservice
├── GPSTracking.Api.Drivers/             # Driver management microservice
├── GPSTracking.Api.Payments/            # Notifications microservice
├── GPSTracking.Api.Search/              # Aggregator / BFF
├── GPSTracking.Api.GPSTrackings.Tests/  # xUnit test suite
├── docker-compose.yml                   # Service definitions
├── docker-compose.override.yml          # Dev overrides + port mappings
└── docs/                                # GitHub Pages demo
```

## Key Design Decisions

- **Aggregator pattern**: The Search service composes responses from 3 upstream services
  via named `IHttpClientFactory` clients — no shared libraries or databases between services.
- **Database-per-service**: Each data service has its own isolated PostgreSQL database,
  enforcing bounded contexts.
- **EF Core migrations**: Schema is managed via code-first migrations; `db.Database.Migrate()`
  runs automatically on startup so the system is self-bootstrapping.
- **Seed data**: All services seed realistic test data on first run for easy demo setup.
