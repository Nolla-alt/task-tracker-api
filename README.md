# TaskManager API
![CI](https://github.com/Nolla-alt/task-tracker-api/actions/workflows/ci.yml/badge.svg)
A layered ASP.NET Core Web API for managing users, categories and tasks, with JWT authentication.

## Features
- Register and log in (passwords hashed with BCrypt)
- JWT bearer tokens, with `[Authorize]` on every endpoint except register and login
- CRUD for Users, Categories and Task items
- Clear error responses (400 for rule failures, 401 for bad login or token)
- Unit tests with xUnit (no database needed)

## Tech stack
C#, .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server, JWT, BCrypt.Net, Swagger, xUnit

## Project structure
| Project | Job |
|---|---|
| TaskManager.API | Controllers, DTOs, Program.cs, middleware |
| TaskManager.Business | Services and rules (validation, hashing, tokens) |
| TaskManager.Data | Entities, repositories, database access |
| TaskManager.Tests | xUnit tests |

Request flow: Controller -> Service -> Repository -> SQL Server.

## Getting started
1. Install the .NET 8 SDK and SQL Server (or SQL Express).
2. Set the connection string in `TaskManager.API/appsettings.json`.
3. Set a long random `Jwt:Key` (at least 32 characters). Never commit a real key.
4. Run the API from Visual Studio, or:

```
dotnet run --project TaskManager.API
```

5. Open Swagger in your browser (the address is printed in the console).

## Authentication
1. `POST /api/Users` to register.
2. `POST /api/Auth/login` to get a token.
3. In Swagger, click **Authorize** and paste only the token.
4. `GET /api/Auth/me` returns the logged-in user.

## Run the tests
```
dotnet test
```

## Roadmap
- [x] Layered architecture and EF Core
- [x] JWT authentication
- [x] Unit tests
- [x] GitHub Actions CI
- [ ] Docker and Docker Compose
- [ ] Deployment
