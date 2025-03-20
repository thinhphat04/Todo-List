# Todo-List API DROPPII TEST

RESTful To‑Do List API built with ASP.NET Core .NET 8 and SQL Server, full docker for easy setup.

## Installation

### Prerequisites
- Docker
- Docker Compose
- .NET 8 SDK (for migrations)

### Setup Steps

1️⃣ Clone the repository:
```bash
git clone https://github.com/thinhphat04/Todo-List.git
cd Todo-Api
```

2️⃣ Build and run using Docker Compose:
```bash
docker-compose up --build -d
```

3️⃣ Run database migrations to create or update your database schema:
```bash
docker exec -it todo-api dotnet ef migrations add InitialCreate
docker exec -it todo-api dotnet ef database update
```

4️⃣ Verify services:
- **API** available at: `http://localhost:5000`
- **SQL Server** listening on port `1433`

## 🗃️ Database Design

![Database Design](Design-Database/database.png)

### Core Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/task` | List tasks (supports search, pagination) |
| GET | `/api/task/{id}` | Retrieve task by ID |
| POST | `/api/task` | Create a new task |
| PUT | `/api/task/{id}` | Update task details |
| PUT | `/api/task/{id}/status` | Update task status |
| DELETE | `/api/task/{id}` | Delete a task |
| GET | `/api/task/{id}/dependencies` | Retrieve full dependency graph |
| POST | `/api/task/{id}/dependencies?dependencyId={depId}` | Add a dependency |
| PUT | `/api/task/{id}/status` | Toggle task completion status |
| DELETE | `/api/task/{id}/dependencies/{depId}` | Remove a dependency |

