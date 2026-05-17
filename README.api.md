# Sponsorship Workflow API (Backend)

Backend API for sponsorship workflow with JWT auth, RBAC, and staged approval logic.

## Tech Stack
- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core
- PostgreSQL (Npgsql)
- JWT Authentication / Role Authorization
- Swagger/OpenAPI

## Repository
- https://github.com/shohag0310/sponsorshipworkflow_api.git

## Local Run Guide

## Prerequisites
- .NET SDK 10+
- PostgreSQL 14+

## Setup
1. Clone:
```bash
git clone https://github.com/shohag0310/sponsorshipworkflow_api.git
cd sponsorshipworkflow_api/SponsorshipWorkflow.API
```

2. Configure database in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=sponsorship_db;Username=postgres;Password=your_password;"
}
```

3. Apply migration:
```bash
dotnet ef database update --project ../SponsorshipWorkflow.Infrastructure --startup-project .
```

4. Run:
```bash
dotnet restore
dotnet run
```

5. Open:
- API: `https://localhost:7206/api`
- Swagger: `https://localhost:7206/swagger`

## Core Endpoints
- `POST /api/auth/login`
- `GET /api/requests`
- `GET /api/requests/dashboard-stats`
- `GET /api/requests/{id}`
- `POST /api/requests`
- `PUT /api/requests/{id}`
- `POST /api/requests/{id}/submit`
- `POST /api/requests/{id}/cancel`
- `POST /api/requests/{id}/manager-approve`
- `POST /api/requests/{id}/manager-reject`
- `POST /api/requests/{id}/finance-approve`
- `POST /api/requests/{id}/finance-reject`
- `GET /api/requests/{id}/history`
- `GET /api/requests/all-history`
- `GET/POST/PUT/DELETE /api/sponsorshipTypes`

## Architecture Explanation (Backend Focus)

### Backend Architecture
- Layered architecture with clear boundaries:
  - `API`: HTTP transport, middleware, auth pipeline, swagger
  - `Application`: DTOs and service interfaces
  - `Domain`: entities and enums (core business model)
  - `Infrastructure`: EF Core DbContext, concrete services, seeders
- Business logic is implemented in service layer, not controllers.

### Frontend Structure (Context)
- Frontend is feature-based and consumes API through a centralized axios client.
- Role-based routes and menu behavior mirror backend authorization model.

### Workflow Logic
- Status flow:
  - `Draft -> PendingManagerApproval -> PendingFinanceReview -> Approved`
- Manager/finance rejection sets status `Rejected`.
- Requestor cancellation moves to `Cancelled` from allowed states.
- Approval transitions are persisted into `ApprovalHistories` for auditability.

### RBAC Logic
- Role claims included in JWT (`Requestor`, `Manager`, `FinanceAdmin`, `SystemAdmin`).
- Endpoint access enforced with `[Authorize]` and `[Authorize(Roles = ...)]`.
- Role-aware filtering applied in request retrieval and dashboard stats.

### Database Design
- `Users`: identity and role
- `SponsorshipRequests`: core request data + workflow status
- `SponsorshipTypes`: controlled request categorization
- `ApprovalHistories`: immutable status transition log
- Main relationships:
  - one user to many requests
  - one request to many history entries
  - one sponsorship type to many requests

### Assumptions and Tradeoffs
- Single sequential approval chain chosen for clarity and assessment scope.
- Seeded users/types support predictable reviewer testing.
- Simplified non-functional scope:
  - no advanced observability
  - no asynchronous notification pipeline
  - no multi-tenant data partitioning
  - no file storage pipeline

## Seeded Test Accounts
- `requestor@test.com` / `Password123!`
- `manager@test.com` / `Password123!`
- `finance@test.com` / `Password123!`
- `admin@test.com` / `Password123!`

## Live URLs
- API: https://sponsorshipworkflow-api.onrender.com/api
- Swagger: https://sponsorshipworkflow-api.onrender.com/swagger/index.html

## Deployment Notes
- Backend deployed on Render
- PostgreSQL used as persistent store
- Swagger enabled for evaluator testing
