# UMS Feature Checklist
## Current System Audit → Development Checkpoint

> Source: UI/menu screenshots provided for the existing Amrita Academic Management Suite. This checklist records functionality visibly exposed in those screenshots. It does not assume hidden submenu functionality that was not shown.

---

# 0. Current Implementation Snapshot

Updated after the React/Vite + .NET 8 + SQL Server checkpoint build.

## Completed For Current Checkpoint

- [x] .NET 8 Web API
- [x] SQL Server LocalDB persistence
- [x] EF Core migrations
- [x] Development seed data
- [x] JWT login and `/api/auth/me`
- [x] Admin, faculty, and student authentication
- [x] Backend role authorization
- [x] Frontend protected routes and role URL guard
- [x] Admin master CRUD for users, staff, students, courses, batches, subjects, departments, designations, programs, academic sessions, and roles
- [x] Subject allocation operation
- [x] Attendance register, duplicate prevention, summary, and below-75 flag
- [x] Assignment creation, student submission upload API/UI, and assignment notifications
- [x] Marks entry, publish status, result grades, SGPA, and CGPA
- [x] Timetable create/update/delete with room/faculty/batch conflict checks
- [x] Examination schedule creation with conflict check
- [x] Library book listing, issue, return, transaction history, and backend book CRUD
- [x] Notifications list and read/unread status
- [x] Basic reports backend and admin reports UI
- [x] Shared Admin/Faculty/Student portal navigation
- [x] Frontend build
- [x] Backend build
- [x] Critical API smoke checks

## Remaining Before Full Feature Checklist Completion

- [x] Evaluation component setup
- [x] Class grade rules
- [ ] Grade history workflow
- [x] Grace mark categories/application workflows
- [x] Survey creation/submission tracking workflow
- [ ] Deep survey question builder/results analytics
- [ ] Assignment edit/delete, grading, and feedback workflow
- [ ] Examination update/delete and full exam-admin reporting
- [x] Full admin library book CRUD UI
- [ ] Server-side search/filter/sort/pagination
- [ ] Report export polish
- [ ] Audit activity logging
- [ ] Automated test suite

## Current Checkpoint Status

The app is now a functional 100-user checkpoint build for core academic workflows. The remaining items are advanced modules and polish work, not blockers for the current core UMS checkpoint.

---

# 1. Current System — Visible Features

## 1.1 Evaluation

- [x] Create Component
- [x] Create Exam
- [x] Mark Entry
- [x] Class Grade Rule
- [x] Assign Grade
- [x] Change Grade
- [x] Class Grade Reports
- [x] Classwise Student Reports
- [x] Attendance Reports
- [x] Track Registration of Student
- [x] Grace Mark Category Detail
- [x] Apply Grace Mark

**Visible feature count: 12**

---

## 1.2 Exam Admin

- [x] Mark/Grade Entry Status
- [x] Student Wise Arrear Report
- [x] Course Wise Arrear Report
- [x] Number of Arrears Report
- [x] Performance Analysis Report

**Visible feature count: 5**

---

## 1.3 Survey

- [x] Survey Not Submitted Users Report

**Visible feature count: 1**

---

## 1.4 Reports

The screenshots show the following report categories. Their internal submenus are not visible in the provided screenshots.

- [x] Student Reports
- [x] Staff Reports
- [x] Attendance Reports
- [x] Course Offering Reports
- [x] Mark Reports
- [x] Grade Reports
- [x] Grace Mark Reports

**Visible category count: 7**

---

# 2. Current System — Top-Level Navigation

The screenshots visibly show:

- [x] Dashboard
- [x] Academics
- [x] Course Admin
- [x] Attendance
- [x] Evaluation
- [x] Exam Admin
- [x] Survey
- [x] Reports
- [x] Personal
- [x] Search
- [x] Library
- [x] OPAC
- [x] Help
- [x] Exam Application

**Visible top-level module count: 14**

---

# 3. Current Visible Feature Count

| Area | Count |
|---|---:|
| Evaluation | 12 |
| Exam Admin | 5 |
| Survey | 1 |
| Reports categories | 7 |
| **Total directly visible functions/categories** | **25** |

> Note: 25 is a screenshot-based visible count, not the complete feature count of the legacy system. The Reports categories contain expandable functionality that was not visible in the screenshots.

---

# 4. Development Checkpoint — Features To Build

## 4.1 Authentication & Access Control

- [ ] Real login
- [ ] JWT authentication
- [ ] Admin authentication
- [ ] Faculty authentication
- [ ] Student authentication
- [ ] Role-based authorization
- [ ] Protected frontend routes
- [ ] Backend authorization
- [ ] `/api/auth/login`
- [ ] `/api/auth/me`
- [ ] Unauthorized request handling
- [ ] Forbidden request handling

---

# 5. Admin / Master Data

## 5.1 Staff Management

- [ ] Staff listing
- [ ] Create staff
- [ ] View staff
- [ ] Update staff
- [ ] Delete staff
- [ ] Staff search
- [ ] Staff filtering
- [ ] Staff pagination

## 5.2 Student Management

- [ ] Student listing
- [ ] Create student
- [ ] View student
- [ ] Update student
- [ ] Delete student
- [ ] Student search
- [ ] Student filtering
- [ ] Student pagination

## 5.3 Course Management

- [ ] Course listing
- [ ] Create course
- [ ] View course
- [ ] Update course
- [ ] Delete course
- [ ] Course search
- [ ] Course filtering

## 5.4 Batch Management

- [ ] Batch listing
- [ ] Create batch
- [ ] View batch
- [ ] Update batch
- [ ] Delete batch

## 5.5 Subject Management

- [ ] Subject listing
- [ ] Create subject
- [ ] View subject
- [ ] Update subject
- [ ] Delete subject

---

# 6. Academic Management

- [ ] Academic session management
- [ ] Batch/course association
- [ ] Subject allocation
- [ ] Student enrollment
- [ ] Faculty-subject assignment
- [ ] Faculty-batch assignment
- [ ] Academic calendar
- [ ] Academic events
- [ ] Course offering management

---

# 7. Attendance Management

- [ ] Faculty attendance dashboard
- [ ] Select subject
- [ ] Select batch
- [ ] Select class/session
- [ ] Retrieve enrolled students
- [ ] Mark Present
- [ ] Mark Absent
- [ ] Mark OD
- [ ] Submit attendance
- [ ] Update attendance
- [ ] Prevent duplicate attendance
- [ ] Calculate attendance percentage
- [ ] Attendance history
- [ ] Student attendance view
- [ ] Below-75% attendance flag
- [ ] Attendance warning

---

# 8. Evaluation & Internal Assessment

## 8.1 Evaluation Components

- [ ] Create component
- [ ] Update component
- [ ] Delete component
- [ ] Configure component weightage
- [ ] Assign component to subject

## 8.2 Exams

- [ ] Create exam
- [ ] Update exam
- [ ] Delete exam
- [ ] Assign exam to subject
- [ ] Configure maximum marks
- [ ] Exam schedule

## 8.3 Mark Entry

- [ ] Faculty mark entry
- [ ] Update marks
- [ ] Validate marks
- [ ] Publish marks
- [ ] Student marks view
- [ ] Mark entry status

## 8.4 Grading

- [ ] Class grade rules
- [ ] Assign grade
- [ ] Change grade
- [ ] Grade point calculation
- [ ] Grade publishing
- [ ] Grade history

## 8.5 Grace Marks

- [ ] Grace mark category
- [ ] Grace mark rules
- [ ] Grace mark application
- [ ] Apply grace marks
- [ ] Grace mark report

---

# 9. Assignments & Submissions

## Faculty

- [ ] Create assignment
- [ ] Edit assignment
- [ ] Delete assignment
- [ ] Set due date
- [ ] Assignment description
- [ ] Maximum marks
- [ ] View submissions
- [ ] Grade submission
- [ ] Provide feedback

## Student

- [ ] View assignments
- [ ] View due dates
- [ ] Upload submission
- [ ] Update submission before deadline
- [ ] View submission status
- [ ] View marks
- [ ] View faculty feedback

---

# 10. Timetable & Scheduling

- [ ] Create timetable entry
- [ ] Update timetable entry
- [ ] Delete timetable entry
- [ ] View timetable
- [ ] Faculty timetable
- [ ] Student timetable
- [ ] Batch timetable
- [ ] Room allocation
- [ ] Faculty allocation
- [ ] Batch allocation
- [ ] Detect room conflict
- [ ] Detect faculty conflict
- [ ] Detect batch conflict

---

# 11. Examination Administration

- [ ] Examination creation
- [ ] Examination schedule
- [ ] Subject assignment
- [ ] Room assignment
- [ ] Exam conflict detection
- [ ] Mark/grade entry status
- [ ] Student-wise arrear report
- [ ] Course-wise arrear report
- [ ] Number of arrears report
- [ ] Performance analysis report

---

# 12. Results & Academic Performance

- [ ] Semester marks
- [ ] Semester grades
- [ ] Grade points
- [ ] Credits
- [ ] SGPA calculation
- [ ] CGPA calculation
- [ ] Semester result
- [ ] Result publishing
- [ ] Academic history
- [ ] Arrear/failure status
- [ ] Performance summary

---

# 13. Reports

## Student Reports

- [ ] Student list report
- [ ] Student academic report
- [ ] Student enrollment report
- [ ] Student performance report
- [ ] Student attendance report

## Staff Reports

- [ ] Staff list report
- [ ] Faculty workload report
- [ ] Faculty assignment report

## Attendance Reports

- [ ] Attendance summary
- [ ] Student attendance detail
- [ ] Batch attendance report
- [ ] Subject attendance report
- [ ] Attendance shortage report

## Course Offering Reports

- [ ] Course offering report
- [ ] Subject allocation report
- [ ] Batch-course report

## Mark Reports

- [ ] Mark summary
- [ ] Subject-wise marks
- [ ] Student-wise marks
- [ ] Evaluation-wise marks

## Grade Reports

- [ ] Grade summary
- [ ] Subject-wise grades
- [ ] Student-wise grades
- [ ] Semester grade report

## Grace Mark Reports

- [ ] Grace mark summary
- [ ] Student grace mark report
- [ ] Grace mark category report

---

# 14. Library

- [ ] Book listing
- [ ] Add book
- [ ] Edit book
- [ ] Delete book
- [ ] Search books
- [ ] Book availability
- [ ] Issue book
- [ ] Return book
- [ ] Due date
- [ ] Borrowing history
- [ ] Available quantity tracking

---

# 15. Survey

- [ ] Create survey
- [ ] Publish survey
- [ ] Student/faculty survey submission
- [ ] Track survey submissions
- [ ] Survey Not Submitted Users Report
- [ ] Survey results

---

# 16. Notifications

- [ ] Notification creation
- [ ] Student notifications
- [ ] Faculty notifications
- [ ] Assignment notification
- [ ] Marks published notification
- [ ] Attendance warning notification
- [ ] Exam schedule notification
- [ ] Read/unread status

---

# 17. Student Portal

- [ ] Student dashboard
- [ ] Student profile
- [ ] Enrollment information
- [ ] Course list
- [ ] Timetable
- [ ] Attendance
- [ ] Attendance warning
- [ ] Assignments
- [ ] Assignment submission
- [ ] Marks
- [ ] Grades
- [ ] SGPA
- [ ] CGPA
- [ ] Examination schedule
- [ ] Arrear status
- [ ] Library
- [ ] Notifications

---

# 18. Faculty Portal

- [ ] Faculty dashboard
- [ ] Faculty profile
- [ ] Assigned subjects
- [ ] Assigned batches
- [ ] Class timetable
- [ ] Attendance
- [ ] Assignments
- [ ] Student submissions
- [ ] Marks entry
- [ ] Grade assignment
- [ ] Study materials
- [ ] Faculty notifications

---

# 19. Admin Dashboard & Analytics

- [ ] Total students
- [ ] Total faculty
- [ ] Total courses
- [ ] Total subjects
- [ ] Active batches
- [ ] Average attendance
- [ ] Academic performance overview
- [ ] Attendance overview
- [ ] Basic charts
- [ ] Quick navigation

---

# 20. System / Platform Features

- [ ] Global search
- [ ] User profile
- [ ] Role management
- [ ] Permission management
- [ ] Audit timestamps
- [ ] Basic activity logging
- [ ] API error handling
- [ ] Form validation
- [ ] Server-side validation
- [ ] Pagination
- [ ] Filtering
- [ ] Sorting
- [ ] Swagger/OpenAPI
- [ ] Database migrations
- [ ] Development seed data

---

# 21. Development Priority

## P0 — Foundation

- [ ] .NET 8 Web API
- [ ] MSSQL database
- [ ] EF Core
- [ ] Database migrations
- [ ] Seed data
- [ ] JWT authentication
- [ ] RBAC
- [ ] API error handling
- [ ] Frontend API integration

## P1 — Core Academic System

- [ ] Staff CRUD
- [ ] Student CRUD
- [ ] Course CRUD
- [ ] Batch CRUD
- [ ] Subject CRUD
- [ ] Enrollment
- [ ] Attendance
- [ ] Timetable
- [ ] Assignments
- [ ] Submissions
- [ ] Marks
- [ ] Grades
- [ ] SGPA/CGPA

## P2 — Supporting Academic Features

- [ ] Examination management
- [ ] Library
- [ ] Notifications
- [ ] Surveys
- [ ] Reports
- [ ] Grace marks
- [ ] Arrear tracking

## P3 — Polish

- [ ] Dashboard analytics
- [ ] Advanced filtering
- [ ] Better validation
- [ ] Automated tests
- [ ] Audit logging
- [ ] UI refinement
- [ ] Performance optimization

---

# 22. Deferred / Future Scope

The following should NOT be part of the current 100-user checkpoint:

- [ ] Payment gateway
- [ ] Hostel management
- [ ] Transport management
- [ ] HR/payroll
- [ ] Biometric attendance integration
- [ ] WhatsApp integration
- [ ] SMS integration
- [ ] Email automation
- [ ] AI/ML prediction
- [ ] Mobile application
- [ ] Microservices
- [ ] Kubernetes
- [ ] Redis
- [ ] Kafka
- [ ] Elasticsearch
- [ ] SSO
- [ ] MFA
- [ ] Advanced timetable optimization
- [ ] Multi-university tenancy

---

# 23. Checkpoint Definition of Done

## Backend

- [ ] Backend builds successfully
- [ ] Database can be created from migrations
- [ ] Seed data works
- [ ] Login works
- [ ] JWT validation works
- [ ] RBAC works
- [ ] Core CRUD APIs work
- [ ] Attendance works
- [ ] Assignments work
- [ ] Submissions work
- [ ] Marks work
- [ ] Results work
- [ ] Timetable works
- [ ] Examination workflow works
- [ ] Library workflow works
- [ ] Notifications work
- [ ] Basic reports work
- [ ] Swagger works

## Frontend

- [ ] Login uses real backend
- [ ] JWT is attached to requests
- [ ] Admin dashboard uses real data
- [ ] Faculty dashboard uses real data
- [ ] Student dashboard uses real data
- [ ] Master CRUD uses real API
- [ ] Attendance uses real API
- [ ] Assignments use real API
- [ ] Submissions use real API
- [ ] Marks use real API
- [ ] Results use real API
- [ ] Timetable uses real API
- [ ] Library uses real API
- [ ] Notifications use real API
- [ ] API errors are displayed
- [ ] 401/403 states are handled

## Quality

- [ ] No mock data remains in implemented workflows
- [ ] No fake authentication remains
- [ ] No hard-coded production secrets
- [ ] No obvious authorization bypass
- [ ] Frontend builds
- [ ] Backend builds
- [ ] Critical flows tested
- [ ] README updated
- [ ] Implementation status documented

---

# 24. Overall Progress

| Category | Status |
|---|---|
| Existing UI audit | [x] Complete |
| Existing feature identification | [x] Complete |
| Backend | [x] Core complete |
| Database | [x] Core complete |
| Authentication | [x] Complete |
| RBAC | [x] Core complete |
| Master Data | [x] Complete |
| Attendance | [x] Core complete |
| Evaluation | [x] Component/rule module complete; grade history pending |
| Assignments | [x] Core complete |
| Marks & Grades | [x] Core complete |
| Results | [x] Core complete |
| Timetable | [x] Complete |
| Examination | [x] Basic workflow complete |
| Library | [x] Core workflow complete |
| Surveys | [x] Core workflow complete |
| Notifications | [x] Core complete |
| Reports | [x] Basic reports complete |
| Frontend integration | [x] Core complete |
| Testing | [ ] Manual smoke complete; automated tests pending |
| Documentation | [x] Updated |

---

## Checkpoint Target

**Initial target:** A simple, reliable full-stack UMS capable of supporting approximately **100 users**, with the existing React/Vite frontend connected to a .NET 8 + MSSQL backend and the core academic workflows functioning end-to-end.
