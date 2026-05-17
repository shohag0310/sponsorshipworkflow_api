# Sponsorship Request Approval Workflow (.NET)

Backend and supporting projects for the technical assessment.

## Solution Structure
- `SponsorshipWorkflow.API`: ASP.NET Core Web API
- `SponsorshipWorkflow.Application`: DTOs and interfaces
- `SponsorshipWorkflow.Domain`: entities and enums
- `SponsorshipWorkflow.Infrastructure`: EF Core persistence, services, seeders

## Tech Stack
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Npgsql)
- JWT Authentication
- Swagger/OpenAPI

## Prerequisites
- .NET SDK 10
- PostgreSQL 14+

## Database Setup
1. Create PostgreSQL database:
   - `sponsorship_db`
2. Update connection string in:
   - `SponsorshipWorkflow.API/appsettings.json`
3. Run API once to allow startup seeding.

Current connection string key:
- `ConnectionStrings:DefaultConnection`

## Run Backend API
```bash
cd SponsorshipWorkflow.API
dotnet restore
dotnet run
```

Default local API URL (from launch profile):
- `https://localhost:7206`

Swagger URL:
- `https://localhost:7206/swagger`

## Authentication & RBAC
JWT roles used:
- Requestor
- Manager
- FinanceAdmin
- SystemAdmin

Seeded users:
- `requestor@test.com` / `Password123!`
- `manager@test.com` / `Password123!`
- `finance@test.com` / `Password123!`
- `admin@test.com` / `Password123!`

## Workflow Logic
Status flow:
- Draft -> PendingManagerApproval -> PendingFinanceReview -> Approved
- Rejection at manager/finance sets status to Rejected
- Requestor cancellation sets status to Cancelled

Approval history is stored in `ApprovalHistories`.

## Core API Endpoints
- `POST /api/auth/login`
- `GET /api/requests`
- `POST /api/requests`
- `PUT /api/requests/{id}`
- `POST /api/requests/{id}/submit`
- `POST /api/requests/{id}/cancel`
- `POST /api/requests/{id}/manager-approve`
- `POST /api/requests/{id}/manager-reject`
- `POST /api/requests/{id}/finance-approve`
- `POST /api/requests/{id}/finance-reject`
- `GET /api/requests/{id}/history`
- `GET /api/requests/all-history` (SystemAdmin)
- `GET/POST/PUT/DELETE /api/sponsorshipTypes`

## Architecture Notes
- API layer contains transport/controller logic.
- Application layer defines contracts/DTOs.
- Domain contains business entities and status/role enums.
- Infrastructure implements services, workflow operations, auth, persistence.

## Deployment Placeholders
Add these after deployment:
- Frontend URL: `TBD`
- Backend API URL: `TBD`
- Swagger URL: `TBD`
- Repository URL: `TBD`
