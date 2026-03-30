# Project Rules — GPSTracking Inherited Codebase

## Memory Architecture (3-Layer System)

This project uses a unified memory approach combining:

| Layer | Location | Purpose | Auto-Loaded |
|-------|----------|---------|-------------|
| **CLAUDE.md** | Project root | Rules, workflow, conventions | ✅ Always |
| **MEMORY.md** | `~/.claude/projects/<project>/memory/` | Session learnings, patterns Claude discovers | ✅ First 200 lines |
| **claude-mem** | `~/.claude-mem/` | Deep searchable history, AI-compressed | ✅ Via MCP injection |

---

## Discovery Phase — Do This EVERY Session Start

> **These steps are non-negotiable. Never skip discovery.**

### Step 1 — Understand Recent History
```bash
git log --oneline -20
```

### Step 2 — Run the Build
```bash
dotnet build E:\Projects\inherited\GPSTracking\GPSTracking.sln 2>&1
```
Record ALL warnings and errors in **Build State** below before touching anything.

### Step 3 — Read the README
Note what's documented vs. what's actually in the code.

### Step 4 — List What's Broken
Before making any changes, capture the current broken state.

---

## Build State (UPDATE EVERY SESSION)

| Field | Value |
|-------|-------|
| Last known clean build | 2026-03-30 |
| Build command | `dotnet build GPSTracking.sln` |
| Last run by Claude | 2026-03-30 (Phase 5: README + GitHub Pages demo) |

### Current Build Errors
```
None — Build succeeded (0 errors)
```

### Current Warnings
```
None — 0 warnings (NETSDK1138 ×8 eliminated by net8.0 upgrade; CS1717 eliminated by BUG-003 fix)
```

### Services Verified Building
- [x] GPSTracking.Api.Drivers
- [x] GPSTracking.Api.GPSTrackings
- [x] GPSTracking.Api.Notifications (folder: GPSTracking.Api.Payments — see TD-003)
- [x] GPSTracking.Api.Search
- [x] GPSTracking.Api.GPSTrackings.Tests
- [ ] GPSTracking.Api.GPSTracking — **ORPHANED FOLDER, NOT IN SOLUTION** (see TD-004)

---

## Solution Structure

5 projects in `GPSTracking.sln`:

| Project | Folder | Role | Port (appsettings) |
|---------|--------|------|--------------------|
| GPSTracking.Api.Drivers | `GPSTracking.Api.Drivers/` | Drivers CRUD microservice | 32014 |
| GPSTracking.Api.GPSTrackings | `GPSTracking.Api.GPSTrackings/` | GPS data microservice | 17108 |
| GPSTracking.Api.Notifications | `GPSTracking.Api.Payments/` | Notifications microservice | 51378 |
| GPSTracking.Api.Search | `GPSTracking.Api.Search/` | Aggregator/BFF — calls other 3 services | — |
| GPSTracking.Api.GPSTrackings.Tests | `GPSTracking.Api.GPSTrackings.Tests/` | xUnit tests for GPSTrackings | — |

**Orphaned (not in solution):** `GPSTracking.Api.GPSTracking/` — contains only `Db/GPSTracking.cs` and `Db/GPSTrackingsDbContext.cs`, no controllers, no .csproj registered.

---

## What We Know vs. What We Assume

- **Never assume** a pattern is intentional — it might be a bug
- **Document every non-obvious decision** discovered in the codebase
- **Flag technical debt** separately from bugs (see section below)
- If something looks wrong: read the git history before changing it

```bash
git log --follow -p -- path/to/file
git blame path/to/file
```

---

## Technical Debt Register

| ID | Location | Description | Severity | Safe to touch? |
|----|----------|-------------|----------|----------------|
| TD-001 | All services | EF Core InMemory database — data lost on restart, no real persistence | High | ✅ Done (PostgreSQL via Npgsql 8.0.11, migrations added 2026-03-30) |
| TD-002 | All .csproj files | `netcoreapp3.1` is EOL (end of support since Dec 2022) — no security updates | High | ✅ Done (upgraded to net8.0 2026-03-30) |
| TD-003 | `GPSTracking.Api.Payments/` | Folder named "Payments" contains the "Notifications" project — misleading name | Med | Yes |
| TD-004 | `GPSTracking.Api.GPSTracking/` | Orphaned folder not in solution — contains Db classes only, no csproj registered | Med | Ask first |
| TD-005 | `SearchService.cs`, `DriversService.cs` | `dynamic` return types in Search service — no compile-time type safety | Med | Ask first |
| TD-006 | Entire solution | Only 3 xUnit tests (all for GPSTrackings) — zero coverage for Drivers, Notifications, Search | Med | Yes |
| TD-007 | `docker-compose.yml` | Only 2 of 4 services in Docker Compose (missing Notifications and Search) | Med | ✅ Done (all 4 services added 2026-03-30) |
| TD-008 | All services | No Swagger/OpenAPI documentation | Low | ✅ Done (Swashbuckle 6.9.0 added to all 4 services 2026-03-30) |
| TD-009 | All services | No authentication or authorization on any endpoint | High | Ask first |
| TD-010 | All services | No HTTPS / HSTS configured | Med | Ask first |

---

## Bug Register

| ID | Location | Symptom | Root Cause | Fixed? |
|----|----------|---------|------------|--------|
| BUG-001 | `Search/Services/DriversService.cs:35` | `GetDriverAsync` always returns `IsSuccess = false` even on HTTP 200 | Returns `(false, result, null)` — should be `(true, result, null)` | ✅ Yes |
| BUG-002 | `Search/Services/NotificationsService.cs:37` | `GetNotificationsAsync` always returns `IsSuccess = false` even on HTTP 200 | Returns `(false, result, null)` — should be `(true, result, null)` | ✅ Yes |
| BUG-003 | `Search/Services/SearchService.cs:17` | `gPSTrackingsService` is never set from constructor parameter — always null at runtime | Constructor param is `gPSTrakingsService` (typo), field is `gPSTrackingsService`; self-assignment; CS1717 warning | ✅ Yes |
| BUG-004 | `Search/Services/GPSTrackingsService.cs:30` | HttpClient `" GPSTrackinsService "` not found — wrong name with leading/trailing spaces and typo ("Trackings" → "Trackins") | Registered as `"GPSTrackingsService"`, requested as `" GPSTrackinsService "` | ✅ Yes |
| BUG-005 | `Search/Services/NotificationsService.cs:30` | HttpClient `" NotificationsService "` not found — wrong name with leading/trailing spaces | Registered as `"NotificationsService"`, requested as `" NotificationsService "` | ✅ Yes |

> **BUG-001–005 fixed 2026-03-30.** The Search endpoint (`POST /api/search`) should now wire up end-to-end correctly.

---

## Build Progress (KEEP UPDATED)

### ✅ COMPLETED
- `GPSTracking.Api.Drivers` — GET /api/drivers, GET /api/drivers/{id} (`DriversController.cs`, `DriversProvider.cs`)
- `GPSTracking.Api.GPSTrackings` — GET /api/gPSTrackings, GET /api/gPSTrackings/{id} (`GPSTrackingsController.cs`, `GPSTrackingsProvider.cs`)
- `GPSTracking.Api.Notifications` — GET /api/notifications/{driverId} (`NotificationsController.cs`, `NotificationsProvider.cs`)
- `GPSTracking.Api.Search` — POST /api/search (end-to-end wired and functional)
- xUnit tests for GPSTrackings (3 tests: GetAll, GetById valid, GetById invalid)
- AutoMapper profiles for Drivers and GPSTrackings
- EF Core InMemory seed data for Drivers and GPSTrackings (seed data removed from services; InMemory kept in Tests project)
- PostgreSQL via Npgsql 8.0.11; InitialCreate migrations for Drivers, GPSTrackings, Notifications; auto-migrate at startup (TD-001) ✅ 2026-03-30
- docker-compose.override.yml: postgres:16 service, env-var connection strings, port mappings for all 4 services ✅ 2026-03-30
- Docker support: Dockerfiles for all 4 services; all 4 in docker-compose.yml (TD-007) ✅ 2026-03-30
- Swagger/OpenAPI via Swashbuckle 6.9.0 on all 4 services (TD-008) ✅ 2026-03-30
- Fix BUG-001: `DriversService.GetDriverAsync` — `(false, result, null)` → `(true, result, null)` ✅ 2026-03-30
- Fix BUG-002: `NotificationsService.GetNotificationsAsync` — `(false, result, null)` → `(true, result, null)` ✅ 2026-03-30
- Fix BUG-003: `SearchService` constructor — renamed param `gPSTrakingsService` → `gPSTrackingsService` ✅ 2026-03-30
- Fix BUG-004: `GPSTrackingsService` — HttpClient name `" GPSTrackinsService "` → `"GPSTrackingsService"` ✅ 2026-03-30
- Fix BUG-005: `NotificationsService` — HttpClient name `" NotificationsService "` → `"NotificationsService"` ✅ 2026-03-30
- Upgrade all 5 projects from netcoreapp3.1 to net8.0 (TD-002) ✅ 2026-03-30
- README.md: architecture diagram, tech stack, quick-start, API reference ✅ 2026-03-30
- GitHub Pages demo (`docs/index.html`): Leaflet map, 4 animated vehicles, dark UI, settings drawer ✅ 2026-03-30

### 🔨 IN PROGRESS
<!-- Nothing currently in progress -->

### ❌ REMAINING
- Add tests for Drivers, Notifications, Search services
- POST/PUT/DELETE endpoints for all CRUD services
- Authentication/Authorization
- Resolve orphaned folder `GPSTracking.Api.GPSTracking/` (add to solution or delete)
- Rename folder `GPSTracking.Api.Payments/` → `GPSTracking.Api.Notifications/`

---

## Tech Stack

- **Runtime**: .NET 8
- **Framework**: ASP.NET Core 8
- **Database**: PostgreSQL 16 via Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11
- **ORM**: Entity Framework Core 8.0.11
- **Mapping**: AutoMapper 12.0.1 (`AutoMapper.Extensions.Microsoft.DependencyInjection`)
- **Testing**: xUnit 2.9.2, coverlet.collector 6.0.2
- **Package Manager**: NuGet
- **API Docs**: Swashbuckle.AspNetCore 6.9.0 (Swagger UI at `/swagger` in Development)
- **Infrastructure**: Docker Compose (Windows containers, `DockerDefaultTargetOS=Windows`); all 4 services
- **Inter-service communication**: `IHttpClientFactory` with named clients
- **Memory**: Native auto-memory + claude-mem for deep history

---

## API Surface

| Service | Endpoint | Method | Status |
|---------|----------|--------|--------|
| Drivers | `GET /api/drivers` | GET | ✅ Working |
| Drivers | `GET /api/drivers/{id}` | GET | ✅ Working |
| GPSTrackings | `GET /api/gPSTrackings` | GET | ✅ Working |
| GPSTrackings | `GET /api/gPSTrackings/{id}` | GET | ✅ Working |
| Notifications | `GET /api/notifications/{driverId}` | GET | ✅ Working |
| Search | `POST /api/search` | POST | ✅ Fixed (BUG-001–005 resolved 2026-03-30) |

---

## Workflow

```
1. Run discovery phase (git log, build, README, list broken)
2. Make only the explicitly requested change
3. Typecheck / build: dotnet build GPSTracking.sln
4. Run tests: dotnet test GPSTracking.sln
5. Verify nothing regressed
6. Commit: conventional commits (feat:, fix:, chore:)
```

### Before Every Change
- Only modify what was explicitly requested
- Ask if <90% confident about intent
- Offer 2-3 options for significant decisions
- If you find something suspicious — read git history before changing it

---

## Git Conventions

- **Branching**: `main` / `master` = production. Feature: `git checkout -b feat/name`
- **Commits**: `feat:`, `fix:`, `chore:`, `docs:`, `refactor:`, `test:`
- **Before commit**: run build + tests
- **Never commit**: `.env`, API keys, secrets, connection strings

---

## Critical Rules

### Code Quality
- NO placeholders (`YOUR_API_KEY`, `TODO`, `FIXME`) — unless already present in inherited code
- Environment variables for secrets
- Remove unused imports only in files you're actively modifying
- Add logging for API calls/errors

### Inherited Code Caution
- Do NOT refactor code you weren't asked to change
- Do NOT "improve" surrounding code when fixing a bug
- Do NOT rename things without checking all usages
- Do NOT delete code that looks unused — verify with `git grep` first

### Money Handling (NON-NEGOTIABLE)
- ALL money as INTEGER cents/öre
- NEVER `parseFloat()` for financial values
- 100% test coverage for money calculations

---

## Memory Commands

| Command | Purpose |
|---------|---------|
| `/memory` | View/toggle auto-memory, edit CLAUDE.md |
| `/remember` | Suggest patterns to save permanently |
| `/compact` | Instant (uses pre-written Session Memory) |
| `/dream` | Manually trigger memory consolidation |
| `Ctrl+O` | Expand "Recalled/Wrote memories" details |

---

## Session Management

### Starting
1. CLAUDE.md and MEMORY.md auto-load
2. **Run the discovery phase** (see above — never skip)
3. Check "Recalled X memories" for Session Memory
4. Update **Build State** section with current findings

### During
- "Wrote X memories" = Claude saved learnings
- Use `/remember` to promote patterns to permanent memory
- Update Bug Register / Tech Debt Register as you find things

### Ending
- Update Build Progress section
- Update Build State with any new errors/fixes
- Claude auto-saves to MEMORY.md — no manual export needed

### End-of-Session Prompt
```
Update the Build State, Build Progress, and any new Technical Debt or Bug Register entries in CLAUDE.md, then stop.
```

---

## Corrections Log

| Date | Mistake | Rule |
|------|---------|------|
| | Changed unrelated code | Only modify what's requested |
| | Assumed pattern was intentional | Check git history before assuming |
| | Deleted "unused" code | Verify with git grep before deleting |

---

## GitHub Repository

**Repo**: https://github.com/okalangkenneth/GPSTracking

### Rules
- Commit AND push (`git push origin master`) at the end of **every prompt**, not just when done
- Keep commit messages descriptive
- Never push `.env` or `.env.local`
