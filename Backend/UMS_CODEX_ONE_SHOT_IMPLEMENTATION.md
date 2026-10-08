# Codex One-Shot Implementation Prompt
# University Management System (UMS)
# Backend + Core Feature Implementation Checkpoint

## 0. Mission

You are working on an existing University Management System (UMS) product.

The repository already contains a functional React + Vite frontend prototype. Your job in this one-shot execution is to turn the current prototype into a working full-stack application by:

1. Understanding the existing frontend and preserving its current UI/UX wherever practical.
2. Building the backend and database from scratch.
3. Replacing mock/in-memory data with real persistent data.
4. Implementing the core academic workflows end-to-end.
5. Connecting the existing frontend to the new backend.
6. Keeping the implementation intentionally simple and reliable for an initial load of approximately 100 users.
7. Leaving the repository in a runnable, coherent state after this single execution.

This is a development checkpoint, not the final enterprise version of the UMS.

Do NOT over-engineer the system for this checkpoint.

The priority order is:

Working functionality > correctness > clean architecture > security fundamentals > scalability > advanced features.

---

# 1. SOURCE OF TRUTH

Read and understand the existing documentation file:

`FEATURES_AND_ROADMAP.md`

It describes the current frontend state, existing functionality, limitations, target features, proposed architecture, and implementation roadmap.

The current system is documented as a React 18 + Vite 6 SPA with mock data and in-memory authentication. It already has Admin, Faculty, and Student portals and several UI workflows.

The existing documentation specifies the intended backend direction as:

- .NET 8 Web API
- C#
- ASP.NET Core
- Entity Framework Core
- Microsoft SQL Server
- JWT authentication
- Role-based authorization
- Swagger/OpenAPI
- React + Vite frontend integration

Treat the existing repository as the primary implementation source of truth.

Do not blindly recreate functionality that already exists in the frontend.

First inspect the repository and determine:

- current project structure
- package.json
- existing routes
- components
- pages
- contexts/hooks
- mock data
- forms
- tables
- existing API abstractions, if any
- authentication implementation
- styling system
- reusable UI components
- current build/run scripts

Then implement against the actual codebase.

---

# 2. CURRENT PRODUCT STATE

The current frontend already contains the following major areas.

## Authentication

- Login page
- Admin role
- Faculty role
- Student role
- Demo credentials
- Role-based dashboard redirection

Current authentication is simulated/in-memory and must be replaced with real backend authentication.

## Admin Portal

- Dashboard
- Staff Master
- Student Master
- Course Master
- Batch Master
- Subject Master
- Timetable
- Academic calendar
- Examination schedule
- Library
- Reports/analytics

## Faculty Portal

- Dashboard
- Profile
- Assigned subjects
- Class schedules
- Attendance
- Assignments
- Marks/grading
- Study materials

## Student Portal

- Dashboard
- Profile
- Enrollment information
- Timetable
- Attendance
- Assignments
- Submissions
- Academic results
- SGPA/CGPA
- Library
- Notifications

The existing frontend documentation identifies the main limitation:

There is currently no backend/database. Mutations are mock operations and data is not persistent.

Your main responsibility is to eliminate that limitation for the core workflows.

---

# 3. CHECKPOINT OBJECTIVE

At the end of this implementation, the following statement should be true:

"A user can log into the React application using real credentials, the request is authenticated by the .NET backend using JWT, the backend retrieves real data from MSSQL, role-based authorization is enforced, and the Admin/Faculty/Student users can perform the core academic operations through the existing frontend."

The application should be usable locally from a clean setup.

Do not leave the project as a collection of disconnected APIs.

The frontend and backend must actually work together.

---

# 4. TARGET ARCHITECTURE

Use a simple .NET 8 architecture.

Preferred structure:

```text
backend/
  UMS.Api/
  UMS.Core/
  UMS.Infrastructure/
```

Use:

- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server provider
- JWT Bearer authentication
- BCrypt or ASP.NET Core Identity password hashing
- Swagger/OpenAPI
- FluentValidation if it can be integrated without unnecessary complexity

Do NOT introduce unnecessary infrastructure such as:

- microservices
- Redis
- Kafka
- message brokers
- Kubernetes
- distributed caching
- CQRS/MediatR unless the existing codebase genuinely benefits from it
- event sourcing
- complicated repository abstractions
- cloud-specific architecture

This is a 100-user checkpoint.

A clean monolithic Web API with EF Core and MSSQL is sufficient.

---

# 5. DATABASE

Use Microsoft SQL Server.

Create a proper relational schema.

Minimum core entities:

```text
Users
Roles
Staff
Students
Courses
Batches
Subjects
Enrollments
AttendanceRecords
Assignments
Submissions
Marks
TimetableEntries
Examinations
LibraryBooks
LibraryTransactions
Notifications
AcademicCalendarEvents
```

Use primary keys, foreign keys, unique constraints where appropriate, and basic indexes.

Include:

```text
CreatedAt
UpdatedAt
```

on important entities.

Avoid premature database normalization that makes the application unnecessarily difficult to implement.

Prefer a practical schema that supports the current workflows.

---

# 6. DATABASE SEEDING

The application must be usable immediately after database setup.

Create development seed data for:

### Users

At least:

```text
1 Admin
2 Faculty
5-10 Students
```

Create realistic relationships between these users.

Example:

```text
Admin
  |
  +-- Faculty
  |     |
  |     +-- Subjects
  |     +-- Batches
  |     +-- Attendance
  |     +-- Assignments
  |
  +-- Students
        |
        +-- Enrollments
        +-- Attendance
        +-- Assignments
        +-- Marks
```

Seed enough data to make dashboards and tables useful.

Do not seed hundreds of unnecessary records.

---

# 7. AUTHENTICATION

Replace the current mock authentication.

Implement:

```text
POST /api/auth/login
GET  /api/auth/me
```

Login should:

1. Receive username/email and password.
2. Validate credentials.
3. Verify the password securely.
4. Return a JWT.
5. Include user ID and role in the token.
6. Return basic user profile information.
7. Allow the frontend to redirect based on role.

Use JWT Bearer authentication.

JWT configuration must come from configuration/environment variables, not hard-coded secrets.

Example configuration:

```json
{
  "Jwt": {
    "Key": "...",
    "Issuer": "UMS",
    "Audience": "UMS-Frontend",
    "ExpirationMinutes": 60
  }
}
```

Provide a safe development configuration/example without committing a production secret.

---

# 8. ROLE-BASED ACCESS CONTROL

Implement three roles:

```text
Admin
Faculty
Student
```

Minimum rules:

### Admin

Can manage:

- staff
- students
- courses
- batches
- subjects
- timetable
- examinations
- academic calendar
- library inventory
- basic reports

### Faculty

Can access:

- own profile
- assigned subjects
- assigned batches
- attendance
- assignments
- marks
- study materials

### Student

Can access:

- own profile
- own courses
- own timetable
- own attendance
- own assignments
- own submissions
- own marks/results
- library transactions
- notifications

Do not rely only on frontend route protection.

The backend must enforce authorization.

A student requesting an Admin endpoint must receive 403.

---

# 9. CORE ADMIN FEATURES

Implement real CRUD for:

## Staff

Fields should include at minimum:

```text
Id
EmployeeId
Name
Email
Department
Designation
UserId
CreatedAt
UpdatedAt
```

Required operations:

```text
GET
GET by ID
POST
PUT
DELETE
```

## Students

Include:

```text
Id
StudentId
Name
Email
Department
Program
BatchId
Semester
UserId
CreatedAt
UpdatedAt
```

Implement CRUD.

## Courses

Include:

```text
Id
CourseCode
Name
Credits
Department
Description
```

Implement CRUD.

## Batches

Include:

```text
Id
Name
Program
AcademicYear
Semester
```

Implement CRUD.

## Subjects

Include:

```text
Id
Code
Name
Credits
CourseId / Program relationship as appropriate
```

Implement CRUD.

Do not build a generic dynamic CRUD engine.

Build explicit, understandable endpoints.

---

# 10. SEARCH, FILTERING AND PAGINATION

For large master tables, support basic:

```text
?page=1&pageSize=20
?search=
```

Add basic filtering where useful.

Default page size:

```text
20
```

Maximum:

```text
100
```

Do not implement complex search infrastructure.

SQL/EF Core queries should be translated server-side.

Do not load an entire table into memory just to filter it.

---

# 11. ATTENDANCE

Implement the core attendance workflow.

Faculty should be able to:

1. Select subject.
2. Select batch.
3. Select date/session.
4. Retrieve enrolled students.
5. Mark:

```text
Present
Absent
OD
```

6. Submit the attendance register.

Persist the records.

Calculate attendance percentage.

Basic calculation:

```text
Attendance % =
(Present + OD if institutionally treated as attended)
/
Total Applicable Sessions
× 100
```

The exact treatment of OD should be represented consistently in the backend.

Implement a 75% warning threshold.

Students below 75% should be flagged.

The student frontend should be able to retrieve:

- total classes
- attended classes
- absent classes
- OD
- attendance percentage
- warning status

Prevent duplicate attendance records for the same student/session.

---

# 12. TIMETABLE

Implement a simple timetable system.

Admin can:

- create timetable entries
- update timetable entries
- delete timetable entries
- view timetable

Each entry should support:

```text
Day
StartTime
EndTime
Room
Subject
Faculty
Batch
```

Implement basic conflict checking.

Reject at least these conflicts:

1. Same room + overlapping time.
2. Same faculty + overlapping time.
3. Same batch + overlapping time.

Return a clear validation error.

Do not build a sophisticated optimization engine.

---

# 13. ASSIGNMENTS

Faculty:

- create assignment
- edit assignment
- delete assignment
- set due date
- add description
- optionally add rubric/maximum marks

Student:

- view assignments
- see due dates
- submit assignment

Submission should support a basic file upload.

Store uploaded files locally for this checkpoint.

Use a configurable upload directory.

Do not introduce cloud object storage yet.

Store file metadata in the database.

---

# 14. MARKS AND GRADING

Faculty should be able to:

- view enrolled students
- enter marks
- update marks
- publish marks

Student should be able to:

- view published marks

Minimum mark entity:

```text
Id
StudentId
SubjectId
AssignmentId / EvaluationComponentId
MarksObtained
MaximumMarks
Status
PublishedAt
CreatedAt
UpdatedAt
```

Do not attempt to reproduce every university grading rule yet.

Implement a clean basic grading flow.

---

# 15. RESULTS

Implement basic semester result calculation.

Support:

```text
Marks
Grade
Grade Point
Credits
SGPA
CGPA
```

Use a configurable grade mapping.

Example:

```text
A+ = 10
A  = 9
B+ = 8
B  = 7
C+ = 6
C  = 5
D  = 4
F  = 0
```

Do not hard-code this deep inside business logic.

Keep the mapping configurable in one place.

Formula:

```text
SGPA = Σ(Credit × GradePoint) / Σ(Credit)
```

CGPA should be calculated across completed semesters.

---

# 16. EXAMINATION

Implement a simple examination module.

Admin can:

- create examination
- assign subject
- assign date/time
- assign room
- view examination schedule

Perform basic conflict checking.

Expose examination schedule to students.

Do not implement a complete examination management ecosystem in this checkpoint.

---

# 17. LIBRARY

Implement basic library functionality.

Admin:

- add book
- edit book
- delete book
- manage available quantity

Student/Faculty:

- search books
- issue/borrow
- return

Track:

```text
Book
Member
IssueDate
DueDate
ReturnDate
Status
```

When a book is issued:

```text
AvailableCount -= 1
```

When returned:

```text
AvailableCount += 1
```

Prevent checkout when available count is zero.

---

# 18. NOTIFICATIONS

Implement a simple database-backed notification system.

Examples:

- new assignment
- marks published
- attendance warning
- exam schedule published

At minimum:

```text
Notification
Id
UserId
Title
Message
IsRead
CreatedAt
```

Do not build email/SMS/WhatsApp notifications yet.

In-app notifications are sufficient.

---

# 19. DASHBOARDS

Replace dashboard mock numbers with backend data.

## Admin Dashboard

Show basic:

- total students
- total faculty
- total courses
- total subjects
- average attendance
- active batches

## Faculty Dashboard

Show:

- assigned subjects
- assigned batches
- today's classes
- pending grading
- recent assignments

## Student Dashboard

Show:

- current semester
- enrolled subjects
- attendance percentage
- upcoming assignments
- upcoming exams
- recent grades
- unread notifications

Do not build complicated analytics.

Simple SQL/EF Core aggregations are enough.

---

# 20. REPORTING

For this checkpoint, implement basic report endpoints rather than recreating every legacy report.

Minimum reports:

```text
Student list
Faculty list
Course list
Attendance summary
Student attendance detail
Marks summary
Grade/result report
Arrear/failure summary
```

Support basic filters such as:

```text
student
batch
course
subject
semester
date range
```

Return JSON for frontend consumption.

If the existing frontend already has charts, connect them to these real endpoints.

---

# 21. FRONTEND INTEGRATION

This is mandatory.

Do not build a backend and leave the frontend disconnected.

Replace mock calls/data with real API calls.

Create a centralized API layer, for example:

```text
src/api/
  client.js
  auth.js
  students.js
  staff.js
  courses.js
  attendance.js
  assignments.js
  marks.js
  timetable.js
  exams.js
  library.js
  notifications.js
```

Use the frontend's existing conventions if they already have an API abstraction.

The frontend must:

- login against the API
- persist the authentication state appropriately
- attach JWT to protected requests
- handle 401
- handle 403
- display API validation errors
- load real tables
- submit real forms
- update real records
- retrieve real dashboard data

Preserve the existing UI design as much as possible.

Do not redesign the entire frontend unless necessary for integration.

---

# 22. FRONTEND AUTHENTICATION BEHAVIOR

Replace demo authentication with:

```text
POST /api/auth/login
```

Store the returned token using the project's existing safe client-side session strategy.

Configure the API client to attach:

```text
Authorization: Bearer <token>
```

Protect frontend routes by role.

However, remember:

Frontend route protection is only UX.

Backend authorization is the actual security boundary.

---

# 23. ERROR HANDLING

Implement consistent API errors.

Preferred format:

```json
{
  "message": "Human readable message",
  "errors": {
    "field": [
      "Validation message"
    ]
  }
}
```

Use appropriate status codes:

```text
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

Use 409 for conflicts such as timetable collisions or duplicate records where appropriate.

Do not expose stack traces to clients.

---

# 24. VALIDATION

Implement server-side validation for:

- required fields
- email format
- IDs
- dates
- marks
- duplicate records
- timetable conflicts
- invalid foreign keys
- file size/type where relevant

Frontend validation may remain, but backend validation is mandatory.

Never trust client-side validation.

---

# 25. DATABASE MIGRATIONS

Use EF Core migrations.

The repository should contain the migration required to create the database schema.

Provide clear commands in README:

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

If EF tooling is required, document installation.

---

# 26. CONFIGURATION

Use configuration/environment variables for:

```text
ConnectionStrings
JWT secret
JWT issuer
JWT audience
upload directory
CORS origins
```

Provide:

```text
appsettings.json
appsettings.Development.json
.env.example
```

Do not commit real secrets.

The application should be easy to configure on another development machine.

---

# 27. CORS

Configure CORS for the existing React development server.

Support the actual frontend development origin discovered in the repository.

Do not use:

```text
AllowAnyOrigin
```

with credentials.

Use an explicit allowed origin configuration.

---

# 28. SWAGGER

Enable Swagger/OpenAPI in development.

Document:

- authentication
- major CRUD endpoints
- attendance
- assignments
- marks
- timetable
- examinations
- library

Configure JWT authentication in Swagger so protected APIs can be tested.

---

# 29. SECURITY BASELINE

This is a development checkpoint, but do not introduce obvious security problems.

Required:

- password hashing
- JWT validation
- role authorization
- server-side validation
- parameterized/EF queries
- file upload restrictions
- no secrets in source
- no password returned by APIs
- no unrestricted Admin endpoints
- basic audit timestamps

Not required yet:

- SSO
- MFA
- refresh-token rotation
- enterprise identity provider
- WAF
- advanced secrets management
- distributed authorization service

---

# 30. PERFORMANCE TARGET

Initial target:

```text
~100 concurrent users
```

Do not optimize for thousands/millions of users yet.

Nevertheless:

- use async EF Core operations
- use pagination
- avoid N+1 queries
- use AsNoTracking for read-only queries where appropriate
- add indexes to common lookup fields
- do not load unnecessary columns/relations
- avoid synchronous blocking calls
- do not perform expensive calculations on every request when unnecessary

A single .NET API instance + MSSQL is acceptable for this checkpoint.

---

# 31. LOGGING

Add basic structured application logging.

Log:

- startup
- database connection/migration issues
- authentication failures at a safe level
- important business-operation failures
- unexpected exceptions

Never log:

- passwords
- JWT secrets
- full tokens
- sensitive personal information unnecessarily

A simple built-in ASP.NET Core logging setup is sufficient.

---

# 32. TESTING

Do not attempt massive test coverage.

Implement enough tests to establish confidence in the core business logic.

At minimum test:

### Authentication

- valid login
- invalid login
- unauthorized access
- role restriction

### Attendance

- attendance submission
- duplicate prevention
- percentage calculation
- below-75% warning

### Timetable

- valid schedule
- room conflict
- faculty conflict
- batch conflict

### Marks

- valid marks
- invalid marks
- published result visibility

### Library

- issue available book
- reject unavailable book
- return book

If integration testing becomes too time-consuming, prioritize service/business logic tests and manually verify the full critical flows.

---

# 33. REQUIRED END-TO-END FLOWS

Before declaring completion, manually verify these flows.

## Flow 1 — Admin

```text
Login
  ↓
Admin Dashboard
  ↓
Create Student
  ↓
Create Course
  ↓
Create Subject
  ↓
Create Batch
  ↓
Assign/associate academic data
  ↓
View updated records
```

## Flow 2 — Faculty

```text
Login
  ↓
Faculty Dashboard
  ↓
View assigned subject/batch
  ↓
Open attendance
  ↓
Mark attendance
  ↓
Submit
  ↓
Attendance persists
```

## Flow 3 — Assignment

```text
Faculty Login
  ↓
Create Assignment
  ↓
Student Login
  ↓
See Assignment
  ↓
Submit Assignment
  ↓
Faculty sees submission
  ↓
Faculty assigns marks
  ↓
Student sees published result
```

## Flow 4 — Student

```text
Login
  ↓
Student Dashboard
  ↓
View timetable
  ↓
View attendance
  ↓
View assignments
  ↓
View marks/results
  ↓
View notifications
```

## Flow 5 — Library

```text
Admin adds book
  ↓
Student searches book
  ↓
Student borrows book
  ↓
Available count decreases
  ↓
Student returns book
  ↓
Available count increases
```

---

# 34. WHAT NOT TO BUILD NOW

Explicitly avoid expanding scope into:

- payment systems
- hostel management
- transport management
- HR/payroll
- biometric attendance integration
- WhatsApp/SMS integration
- email automation
- AI/ML prediction
- advanced analytics
- mobile application
- microservices
- Kubernetes
- cloud deployment
- Redis
- Kafka
- Elasticsearch
- advanced notification infrastructure
- complex timetable optimization
- SSO
- MFA
- multi-university tenancy

These may be future phases.

The objective is a reliable 100-user full-stack academic core.

---

# 35. IMPORTANT IMPLEMENTATION RULES

## Rule 1 — Inspect before changing

Do not assume the frontend structure.

Inspect the actual repository.

## Rule 2 — Reuse existing UI

Do not throw away working frontend components.

Connect them to the API.

## Rule 3 — No fake completion

Do not leave functions such as:

```javascript
console.log(...)
alert("Coming soon")
setTimeout(...)
dummyData
mockData
```

where the feature is supposed to be functional.

If a feature is intentionally deferred, document it explicitly.

## Rule 4 — No disconnected backend

Every implemented core API must have a meaningful frontend integration where the UI already provides that workflow.

## Rule 5 — Keep architecture simple

Do not create abstractions merely for architectural appearance.

## Rule 6 — Preserve existing routes where practical

Avoid unnecessary breaking changes to frontend routing.

## Rule 7 — Do not rewrite the project unnecessarily

Modify only what is needed to produce the working checkpoint.

---

# 36. DEFINITION OF DONE

The task is complete only when all of the following are true:

### Backend

- [ ] .NET 8 API builds successfully
- [ ] EF Core builds successfully
- [ ] MSSQL schema is defined
- [ ] Migration exists
- [ ] Seed data exists
- [ ] JWT authentication works
- [ ] Admin/Faculty/Student authorization works
- [ ] CRUD APIs work
- [ ] Attendance works
- [ ] Timetable works
- [ ] Assignments work
- [ ] Submissions work
- [ ] Marks work
- [ ] Results work
- [ ] Examination schedule works
- [ ] Library workflow works
- [ ] Notifications work
- [ ] Basic reports work
- [ ] Swagger works
- [ ] Validation works
- [ ] Error handling works

### Frontend

- [ ] Login uses backend
- [ ] JWT is sent with API requests
- [ ] Admin portal uses real data
- [ ] Faculty portal uses real data
- [ ] Student portal uses real data
- [ ] Master CRUD works
- [ ] Attendance works end-to-end
- [ ] Assignment workflow works end-to-end
- [ ] Marks workflow works end-to-end
- [ ] Results are visible
- [ ] Timetable uses real data
- [ ] Library uses real data
- [ ] Notifications use real data
- [ ] Loading states exist
- [ ] API errors are handled
- [ ] 401/403 states are handled

### Quality

- [ ] No hard-coded credentials
- [ ] No real secrets committed
- [ ] No obvious security bypass
- [ ] No major console errors
- [ ] No broken imports
- [ ] No TypeScript/build errors if TypeScript is used
- [ ] Backend builds
- [ ] Frontend builds
- [ ] Critical flows have been tested
- [ ] README explains setup
- [ ] README explains database configuration
- [ ] README explains seed credentials
- [ ] README explains how to run frontend and backend

---

# 37. FINAL DOCUMENTATION REQUIREMENT

At the end, update/create:

```text
README.md
IMPLEMENTATION_STATUS.md
API.md
```

`IMPLEMENTATION_STATUS.md` should contain:

```text
Implemented
Partially Implemented
Deferred
Known Limitations
Next Recommended Phase
```

Clearly state what was actually completed.

Do not claim functionality that was not implemented.

---

# 38. ONE-SHOT EXECUTION STRATEGY

You have one execution opportunity.

Therefore:

1. Inspect the entire repository first.
2. Understand the frontend before making architectural decisions.
3. Create the backend structure.
4. Create the database model.
5. Create migrations.
6. Seed development data.
7. Implement authentication.
8. Implement authorization.
9. Implement core APIs.
10. Connect frontend APIs.
11. Replace mock data for implemented modules.
12. Build and test backend.
13. Build and test frontend.
14. Fix integration issues.
15. Run the critical end-to-end flows.
16. Update documentation.
17. Leave the repository in a runnable state.

Do not stop after scaffolding.

Do not provide only a plan.

Do not ask for permission for every implementation step.

Make reasonable implementation decisions autonomously.

Only stop when the checkpoint is in the strongest working state that can reasonably be achieved in this execution.

---

# 39. PRIORITY IF TIME/COMPUTE BECOMES LIMITED

If you cannot finish every feature, prioritize in exactly this order:

### P0 — Mandatory

1. Backend project
2. MSSQL database
3. Authentication
4. RBAC
5. Student/Faculty/Admin entities
6. Master CRUD
7. Frontend API integration

### P1 — Core academic workflows

8. Attendance
9. Assignments
10. Submissions
11. Marks
12. Results
13. Timetable

### P2 — Supporting workflows

14. Exams
15. Library
16. Notifications
17. Reports

### P3 — Polish

18. Advanced validation
19. Additional analytics
20. UI refinements
21. Additional tests

A working P0 + P1 system is preferable to an incomplete implementation of every feature.

---

# 40. FINAL OUTPUT FROM CODEX

At the end of the execution, report:

```text
UMS IMPLEMENTATION CHECKPOINT

Frontend:
[status]

Backend:
[status]

Database:
[status]

Authentication:
[status]

RBAC:
[status]

Master Data:
[status]

Attendance:
[status]

Assignments:
[status]

Marks:
[status]

Results:
[status]

Timetable:
[status]

Examinations:
[status]

Library:
[status]

Notifications:
[status]

Reports:
[status]

Build:
[status]

Tests:
[status]

Known Limitations:
[list]

Deferred Features:
[list]

How to Run:
[commands]

Seed Credentials:
[list development credentials]
```

Do not give a generic summary.

Report the actual state of the repository after implementation.

# END OF PROMPT
