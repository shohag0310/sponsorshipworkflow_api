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
dotnet ef database update -p SponsorshipWorkflow.Infrastructure -s SponsorshipWorkflow.API
```

4. Run:
```bash
dotnet restore
dotnet run
```

5. Open:
- API: `https://localhost:7206/api`
- Swagger: `https://localhost:7206/swagger`

## Solution Architecture
- `SponsorshipWorkflow.API`: controllers, auth/cors/swagger setup
- `SponsorshipWorkflow.Application`: DTOs + interfaces
- `SponsorshipWorkflow.Domain`: entities + enums
- `SponsorshipWorkflow.Infrastructure`: EF Core, services, workflow logic, seeders

## Core Entities
- `Users`
- `SponsorshipRequests`
- `SponsorshipTypes`
- `ApprovalHistories`

## Workflow Logic
- `Draft -> PendingManagerApproval -> PendingFinanceReview -> Approved`
- Manager reject -> `Rejected`
- Finance reject -> `Rejected`
- Requestor cancel -> `Cancelled`

## RBAC Roles
- `Requestor`
- `Manager`
- `FinanceAdmin`
- `SystemAdmin`

## Core Endpoints
- `POST /api/auth/login`
- `GET /api/requests`
- `GET /api/requests/dashboard-stats`
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
