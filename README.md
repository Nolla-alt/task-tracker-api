# Task Tracker API

A RESTful API for managing tasks, built with C#, ASP.NET Core, and SQL Server, following a 3-tier layered architecture.

## Architecture

- **TaskManager.API** — Presentation layer: Controllers, DTOs, request/response handling
- **TaskManager.Business** — Business layer: Services, business rules, validation
- **TaskManager.Data** — Data access layer: Entities, Repositories, EF Core and ADO.NET implementations

Dependencies flow in one direction only: API → Business → Data.

## Entities

- **User** — owns tasks
- **Category** — groups tasks
- **TaskItem** — the core entity, linked to one User and one Category

## Tech Stack

- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- ADO.NET (raw SQL, built alongside EF Core for comparison and learning)
- SQL Server
- Git / GitHub

## Status

🚧 In active development.

- [x] Project architecture and setup
- [x] Entity models (User, Category, TaskItem)
- [x] Repository layer (ADO.NET + EF Core, Category and User complete)
- [ ] Repository layer (TaskItem)
- [ ] Service layer
- [ ] API Controllers
- [ ] Authentication & security (JWT)
- [ ] Testing
- [ ] Deployment

## Getting Started

1. Clone the repo
2. Update the connection string in `TaskManager.API/appsettings.json`
3. Run database migrations (coming soon)
4. `dotnet run` from `TaskManager.API`