# Implementation Status

## Implemented

- .NET 8 Web API in `Backend/UMS.Api`
- EF Core SQL Server context, migrations, and seed data
- Seeded Admin, Faculty, Student users
- BCrypt password verification
- JWT login and `/auth/me`
- Swagger with JWT bearer support
- Role-protected create/update/delete for Admin master data
- Master CRUD APIs and UI for users, staff, students, courses, batches, subjects, departments, designations, programs, academic sessions, and roles
- Reference-data backed selects for dependent master fields
- Subject allocation operation separated from Subject Master
- Attendance submission, duplicate prevention, percentage summary, and 75% warning flag
- Timetable list plus conflict-checked create/update/delete
- Assignment list, creation, student upload submission, and assignment notifications
- Marks creation, published marks filtering, and result SGPA/CGPA calculation
- Basic examination schedule creation with conflict check
- Library catalog, issue, return, transaction tracking, backend book CRUD, and admin book CRUD UI
- Notification read/unread workflow
- Basic report endpoints and admin reports UI
- Evaluation component CRUD API/UI
- Grade rule CRUD API/UI
- Grace mark category and application CRUD API/UI
- Survey create/publish/list workflow plus student/faculty response submission
- Advanced module reports for evaluation components, grade rules, grace marks, and survey submission summary
- Explicit decimal precision configuration for marks, assignments, grades, evaluation weightage, and grace marks
- Frontend login now uses the backend and stores JWT
- Frontend attaches JWT on API requests
- Frontend route protection redirects users away from mismatched role URL segments
- Frontend dashboard, master CRUD, timetable, assignments, marks, results, library, attendance, notifications, examinations, and reports call backend endpoints
- Shared top navigation is common across Admin, Faculty, and Student portals

## Partially Implemented

- Search, filtering, sorting, and pagination are visually present in places; server-side pagination/search exists for masters, but advanced filtering/sorting is not complete.
- Assignment grading/feedback and assignment edit/delete are not complete.
- Examination edit/delete and advanced exam admin reports are not complete.
- Reports are basic table/chart screens; export polish is deferred by request.
- Automated tests and audit logging are not complete.

## Deferred

- Refresh tokens
- MFA/SSO
- Email/SMS/WhatsApp notifications
- Advanced timetable optimization
- Advanced report exports
- Full test suite
- Cloud storage for submissions
- Multi-campus or multi-university tenancy
- Deep survey analytics and question builder
- Advanced grade history/audit workflow

## Known Limitations

- Development JWT secret is checked in only as a local example and must be replaced outside development.
- Validation is basic and endpoint-local rather than comprehensive FluentValidation.
- EF decimal precision warnings are harmless for current marks values but should be explicitly configured next.
- File upload restrictions cover size but not deep content inspection.
- A few non-critical pages still use curated placeholder content until their full workflow is prioritized.

## Next Recommended Phase

1. Add evaluation component, grade rule, grace mark, and survey modules.
2. Add assignment edit/delete, grading, and feedback.
3. Add examination edit/delete and exam-admin reports.
4. Add server-side search/filter/sort/pagination.
5. Add service-level tests for auth, attendance, timetable conflicts, marks, and library issue/return.
6. Tighten validation and configure decimal precision explicitly.
