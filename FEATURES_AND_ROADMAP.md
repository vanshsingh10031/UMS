# University Management System (UMS) - Feature Specification & Current State Audit

## 1. What We Currently Have (System Audit)

The repository is currently a **React + Vite single-page frontend prototype** (`amrita-ums-prototype`) equipped with static mock data and in-memory simulated authentication.

### Current Architecture & Tech Stack
- **Framework & Tooling**: React 18 (`react`, `react-dom`), Vite 6 (`@vitejs/plugin-react`).
- **Routing**: `react-router-dom` v7 with role-based parameterized routing (`/:role/:page` and `/:role/:section/:page`).
- **Icons & Visuals**: `lucide-react`, Recharts (`recharts` for analytics charts & attendance breakdown).
- **Styling**: Vanilla CSS (`styles.css`, ~26 KB) featuring dark/light branding tokens, responsive sidebars, data tables, and card views.
- **State & Data**:
  - In-memory mock datasets in [`src/data/mockData.js`](file:///d:/My%20code/UMS/src/data/mockData.js).
  - Context-based authentication state (`useAuth.jsx`) storing session user in `localStorage`.

### Current Role Support & Implemented Pages
1. **Authentication (`/login`)**:
   - Role picker pre-filled with demo credentials:
     - **Admin** (`AVV-ADM-01`)
     - **Faculty** (`AVV-FAC-42`)
     - **Student** (`AVV-STU-88`)
   - Direct redirect to respective dashboard upon login.

2. **Admin Portal**:
   - **Dashboard**: High-level university metrics (total students, faculty, active programs, attendance averages) + quick navigation links.
   - **Master Data Management**: Generic records table with view/dummy-create modals for:
     - Staff Master (`masters/staff`)
     - Student Master (`masters/students`)
     - Course Master (`masters/courses`)
     - Batch Master (`masters/batches`)
     - Subject Master (`masters/subjects`)
   - **Academic**: Timetable viewer, academic calendar events, examination schedule list.
   - **Library**: Book catalog and digital resources card list.
   - **Reports & Analytics**: Recharts-based bar & pie distributions for admissions and department strength.

3. **Faculty Portal**:
   - **Dashboard & Profile**: Assigned subject overview, class schedules, and profile details.
   - **Attendance Tracking**: Mock class attendance view with toggleable status.
   - **Assignments & Marks**: List of assignments and grading interfaces with mock progress indicators.
   - **Study Materials**: Upload/view course material placeholders.

4. **Student Portal**:
   - **Dashboard & Profile**: Enrollment status, upcoming assignments, and semester summary.
   - **Course Timetable & Attendance**: Daily/weekly schedule view and graphical attendance breakdown.
   - **Assignments & Submissions**: Assignment tracker with due dates and submission statuses.
   - **Academic Results**: SGPA/CGPA summary and semester grade table.
   - **Library & Notifications**: Borrowed books tracker and announcement list.

### Current Limitations
- **No Backend / Database**: All mutations (e.g., adding staff or submitting assignments) only trigger dummy UI dismissals; no persistent state or REST/GraphQL API.
- **Mock Data Coverage**: Minimal records (~4–5 items per master entity).
- **Validation & Business Logic**: Lacks real authentication, JWT/session verification, RBAC route guarding by token, form validation, and server-side filtering/pagination.

---

## 2. Feature Specification: What We Have to Build

Below is the structured feature specification organized by epics and functional user stories (Gherkin/BDD style) for the target system.

```gherkin
Feature: Core Authentication & Role-Based Access Control (RBAC)
  As a university member (Admin, Faculty, Student)
  I want secure credential-based authentication with role-enforced access
  So that confidential academic and institutional data is protected

  Scenario: Successful login with valid credentials
    Given the user is on the login page
    When the user submits valid username and password
    Then the system authenticates the user via JWT session token
    And navigates the user to their designated role dashboard

  Scenario: Unauthorized route prevention
    Given a logged-in student user
    When the student attempts to access "/admin/masters/staff" directly
    Then the system blocks the request with a 403 Forbidden error or redirects to the student dashboard
```

```gherkin
Feature: Master Data Management (Admin Portal)
  As an Administrator
  I need full CRUD capabilities over university master records
  So that academic sessions, staff, courses, and batches are properly structured

  Scenario: Create a new faculty/staff record
    Given the admin is on the Staff Master page
    When the admin submits valid staff information (Name, Email, Dept, Designation, Employee ID)
    Then the staff record is saved to the database
    And an automated onboarding notification or default credential is generated

  Scenario: Batch & Course assignment
    Given existing courses and batches
    When the admin allocates a curriculum and subjects to a batch
    Then students registered in that batch automatically inherit the course syllabus
```

```gherkin
Feature: Academic Attendance Management
  As a Faculty member
  I want to take and update daily session attendance
  So that student attendance percentages and exam eligibility are accurately computed

  Scenario: Faculty records daily lecture attendance
    Given a scheduled lecture session for a specific batch and subject
    When the faculty marks students as Present, Absent, or On Duty (OD)
    And submits the attendance register
    Then the attendance ledger is updated
    And students falling below the 75% threshold are flagged for condonation/alerts
```

```gherkin
Feature: Continuous Internal Evaluation & Grading
  As Faculty and Students
  We want streamlined coursework assignment submission and grade publishing
  So that performance is tracked transparently throughout the semester

  Scenario: Faculty publishes an assignment
    Given a faculty member for subject "Data Structures"
    When the faculty posts an assignment with title, description, rubric, and due date
    Then enrolled students receive an in-app notice and see the task in their portal

  Scenario: Student submits assignment and faculty grades it
    Given a pending assignment
    When a student uploads their submission file prior to the deadline
    And the faculty reviews and assigns marks with feedback
    Then the student's marks ledger updates and displays in their Results portal
```

```gherkin
Feature: Real-time Examination & Timetable Scheduling
  As an Academic Administrator
  I want conflict-free scheduling for weekly timetables and semester exams
  So that rooms, faculty proctors, and student batches do not overlap

  Scenario: Detect conflict in timetable allocation
    Given classroom "C-301" is already allocated on "Monday at 09:00"
    When an admin attempts to schedule another batch into "C-301" at the same slot
    Then the system rejects the allocation and flags a room scheduling conflict
```

```gherkin
Feature: Library Resource Management
  As a Student or Faculty member
  I want to search, reserve, and borrow library books
  So that physical and digital academic materials are tracked

  Scenario: Check out a library book
    Given an active library member and an available book copy
    When the issue request is processed
    Then the available count decreases by 1
    And a due date is assigned with automated reminder notifications
```

---

## 3. Recommended Development Roadmap

### Target Architecture
- **Backend**: **.NET 8 Web API** (C#, ASP.NET Core)
  - Entity Framework Core (EF Core 8) / Dapper for ORM & data access
  - ASP.NET Core Identity & JWT Bearer token authentication
  - Swagger / OpenAPI documentation
  - Clean Architecture / MediatR / CQRS or repository pattern
- **Database**: **Microsoft SQL Server (MSSQL)**
  - Relational schema with foreign keys, constraints, and indexed audit fields (`CreatedAt`, `UpdatedAt`)
  - Code-First or Database-First EF Core Migrations
- **Frontend**: React + Vite (Connecting via Axios/Fetch API client to .NET 8 endpoints)

### Implementation Phases

1. **Phase 1: Backend (.NET 8 Web API) & DB (MSSQL) Foundations**
   - Create .NET 8 Web API solution and projects (API, Core/Domain, Infrastructure).
   - Configure MSSQL connection strings and Entity Framework Core (`Microsoft.EntityFrameworkCore.SqlServer`).
   - Define relational entities: `Users`, `Roles`, `Staff`, `Students`, `Courses`, `Batches`, `Subjects`, `Enrollments`, `AttendanceRecords`, `Assignments`, `Submissions`, `Marks`, `LibraryBooks`.
   - Run initial EF Core migration to seed MSSQL tables.
   - Implement JWT authentication & authorization middleware with role policies (`Admin`, `Faculty`, `Student`).

2. **Phase 2: Master Management & CRUD Integration**
   - Build REST controllers for Masters (Staff, Students, Courses, Batches, Subjects).
   - Add pagination, filtering, and search query parameters.
   - Connect React admin frontend tables to live .NET 8 endpoints.
   - FluentValidation for request payload validation.

3. **Phase 3: Academic Workflows (Attendance, Timetable, Exams)**
   - Attendance controller and service: batch student rosters, daily submission, and threshold calculation logic (< 75% alerts).
   - Conflict-checking timetable builder API and schedule slot persistence in MSSQL.

4. **Phase 4: Student & Faculty Portal Completeness**
   - File upload handling in .NET 8 for assignments and study materials.
   - Assignment submission, evaluation, and grading endpoints.
   - Semester result publishing & automated SGPA/CGPA calculation engine.
   - Library inventory tracking and book check-out/return workflows in MSSQL.
