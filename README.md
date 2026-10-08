# University Management System

React + Vite frontend with a .NET 8 Web API backend and SQL Server persistence.

## Run Backend

```bash
cd Backend/UMS.Api
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:5052
```

Swagger is available at `http://localhost:5052/swagger` in development.

## Run Frontend

```bash
npm install
npm run dev
```

The frontend defaults to `http://localhost:5052/api`. Override with:

```bash
VITE_API_BASE_URL=http://localhost:5052/api
```

## Configuration

Backend settings live in `Backend/UMS.Api/appsettings.json`.

- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`
- `Cors:Origins`
- `Uploads:Directory`
- `GradePoints`

The checked-in JWT key is development-only. Change it for any shared environment.

## Seed Credentials

All development users use password `amrita2026`.

- Admin: `admin@amrita.edu`
- Faculty: `AVV-F1024`
- Faculty: `AVV-F1038`
- Student: `CB.EN.U4CSE22101`
- Student: `CB.EN.U4CSE22118`

## Verification

Validated during implementation:

- `dotnet build`
- `dotnet ef database update`
- `npm run build`
- Live login against `POST /api/auth/login`
- Authenticated read against `GET /api/students`
