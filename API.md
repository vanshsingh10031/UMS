# UMS API

Base URL: `http://localhost:5052/api`

Use `Authorization: Bearer <token>` for protected endpoints.

## Authentication

- `POST /auth/login`
- `GET /auth/me`

Login body:

```json
{ "username": "CB.EN.U4CSE22101", "password": "amrita2026" }
```

## Master Data

All list endpoints support `page`, `pageSize`, and `search`.

- `GET /staff`
- `POST /staff` Admin
- `PUT /staff/{id}` Admin
- `DELETE /staff/{id}` Admin
- `GET /students`
- `POST /students` Admin
- `PUT /students/{id}` Admin
- `DELETE /students/{id}` Admin
- `GET /courses`
- `POST /courses` Admin
- `PUT /courses/{id}` Admin
- `DELETE /courses/{id}` Admin
- `GET /batches`
- `POST /batches` Admin
- `PUT /batches/{id}` Admin
- `DELETE /batches/{id}` Admin
- `GET /subjects`
- `POST /subjects` Admin
- `PUT /subjects/{id}` Admin
- `DELETE /subjects/{id}` Admin

## Academic Workflows

- `GET /dashboard/{role}`
- `GET /attendance/summary`
- `POST /attendance` Faculty/Admin
- `GET /timetable`
- `POST /timetable` Admin
- `GET /assignments`
- `POST /assignments` Faculty/Admin
- `POST /assignments/{id}/submit` Student, multipart file upload
- `GET /marks`
- `POST /marks` Faculty/Admin
- `GET /results`
- `GET /examinations`
- `POST /examinations` Admin
- `GET /notifications`

## Library

- `GET /library/books`
- `POST /library/books` Admin
- `PUT /library/books/{id}` Admin
- `DELETE /library/books/{id}` Admin
- `POST /library/books/{id}/issue`
- `GET /library/transactions`
- `POST /library/transactions/{id}/return`

## Reports

- `GET /reports/student-list` Admin
- `GET /reports/faculty-list` Admin
- `GET /reports/course-list` Admin
- `GET /reports/marks-summary` Admin

## Error Shape

Endpoints return simple JSON errors:

```json
{ "message": "Human readable message." }
```
