import { useEffect, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'
import { Bar, BarChart, CartesianGrid, Cell, Pie, PieChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts'
import { Award, BookOpen, CalendarDays, Check, Clock3, Download, FileText, FolderUp, Mail, MapPin, Plus, Upload, UserCheck } from 'lucide-react'
import Dashboard from './Dashboard'
import { assignments, batches, books, courses, faculty, schedule, students, subjects } from '../data/mockData'
import { DataTable, Modal, PageHeader, StatusPill, Toolbar } from '../components/UI'
import { useAuth } from '../hooks/useAuth'
import { API_BASE_URL, api, getToken, unwrapList } from '../api/client'

const titleMap = {
  staff: ['Staff Master', 'Manage faculty and administrative staff records.'],
  students: ['Students', 'Manage enrolment and student academic records.'],
  courses: ['Courses', 'Explore and manage academic programmes.'],
  batches: ['Batch Master', 'Manage active cohorts, sections and capacity.'],
  subjects: ['Subjects', 'View subject allocations and credit details.'],
  'department-masters': ['Department Master', 'Manage departments used across staff, students and courses.'],
  'designation-masters': ['Designation Master', 'Manage staff designation values.'],
  'program-masters': ['Program Master', 'Manage academic programs used by students and batches.'],
  'academic-sessions': ['Academic Master', 'Manage academic years and their semesters.'],
  'role-masters': ['Role Master', 'Manage roles used for user access control.'],
  users: ['User Master', 'Manage login accounts and role assignment.'],
  timetable: ['Weekly Timetable', 'Your complete academic schedule for the week.'],
  calendar: ['Academic Calendar', 'Key semester dates, events and deadlines.'],
  examination: ['Examinations', 'Schedules, evaluation workflow and result publication.'],
  resources: ['Library Resources', 'Digital collections and campus learning resources.'],
  books: ['Book Catalogue', 'Search the central library collection.'],
  analytics: ['University Analytics', 'Academic and institutional performance insights.'],
  profile: ['My Profile', 'Personal and university information.'],
  attendance: ['Attendance', 'Track class-wise attendance and eligibility.'],
  assignments: ['Assignments', 'Coursework, submissions and feedback.'],
  materials: ['Study Materials', 'Lecture notes, references and course resources.'],
  marks: ['Marks & Evaluation', 'Record and review student performance.'],
  results: ['Academic Results', 'Semester grades and cumulative performance.'],
  library: ['My Library', 'Discover books and manage your borrowing.'],
  notifications: ['Notifications', 'Important notices, announcements and campus events.'],
  'evaluation-components': ['Evaluation Components', 'Configure internal assessment components and weightage.'],
  'grade-rules': ['Grade Rules', 'Configure percentage bands, grade letters and grade points.'],
  'grace-mark-categories': ['Grace Mark Categories', 'Manage approved grace mark categories and limits.'],
  'grace-mark-applications': ['Grace Mark Applications', 'Apply approved grace marks to student subjects.'],
  surveys: ['Surveys', 'Create, publish and track academic surveys.'],
}

const genericColumns = [
  { key: 'id', label: 'ID' },
  { key: 'name', label: 'Name', render: (value, row) => <div className="person-cell"><span>{value.split(' ').map((part) => part[0]).slice(0, 2).join('')}</span><div><strong>{value}</strong>{row.email && <small>{row.email}</small>}</div></div> },
  { key: 'department', label: 'Department / Course' },
  { key: 'detail', label: 'Details' },
  { key: 'status', label: 'Status', render: (value) => <StatusPill>{value}</StatusPill> },
]

const masterFields = {
  staff: [
    ['employeeId', 'Employee ID', 'text', true],
    ['name', 'Full name', 'text', true],
    ['email', 'Email', 'email', true],
    ['department', 'Department', 'select', true, 'departments'],
    ['designation', 'Designation', 'select', true, 'designations'],
  ],
  students: [
    ['studentId', 'Student ID', 'text', true],
    ['name', 'Full name', 'text', true],
    ['email', 'Email', 'email', true],
    ['department', 'Department', 'select', true, 'departments'],
    ['program', 'Program', 'select', true, 'programs'],
    ['batchId', 'Batch', 'select', true, 'batches'],
    ['semester', 'Semester', 'select', true, 'semesters'],
  ],
  courses: [
    ['courseCode', 'Course code', 'text', true],
    ['name', 'Course name', 'text', true],
    ['credits', 'Credits', 'number', true],
    ['department', 'Department', 'select', true, 'departments'],
    ['description', 'Description', 'text'],
  ],
  batches: [
    ['name', 'Batch name', 'text', true],
    ['program', 'Program', 'select', true, 'programs'],
    ['academicYear', 'Academic year', 'select', true, 'academicYears'],
    ['semester', 'Semester', 'select', true, 'semesters'],
  ],
  subjects: [
    ['code', 'Subject code', 'text', true],
    ['name', 'Subject name', 'text', true],
    ['credits', 'Credits', 'number', true],
    ['semester', 'Semester', 'select', true, 'semesters'],
    ['courseId', 'Course', 'select', true, 'courses'],
  ],
  'department-masters': [
    ['code', 'Department code', 'text', true],
    ['name', 'Department name', 'text', true],
  ],
  'designation-masters': [
    ['name', 'Designation name', 'text', true],
  ],
  'program-masters': [
    ['code', 'Program code', 'text', true],
    ['name', 'Program name', 'text', true],
  ],
  'academic-sessions': [
    ['academicYear', 'Academic year', 'text', true],
    ['semester', 'Semester', 'number', true],
    ['isActive', 'Status', 'select', true, 'boolean'],
  ],
  'role-masters': [
    ['name', 'Role name', 'text', true],
    ['description', 'Description', 'text'],
  ],
  users: [
    ['username', 'Username / ID', 'text', true],
    ['displayName', 'Display name', 'text', true],
    ['email', 'Email', 'email', true],
    ['roleMasterId', 'Role', 'select', true, 'roles'],
    ['password', 'Password', 'password', true],
  ],
}

const fieldPlaceholders = {
  employeeId: 'Example: FAC102 or ADM015',
  studentId: 'Example: CB.EN.U4CSE22101',
  courseCode: 'Example: CSE or ECE',
  code: 'Example: 19CSE301',
  name: 'Enter the official name',
  displayName: 'Enter the portal display name',
  email: 'Example: user@amrita.edu',
  department: 'Choose the department from master data',
  designation: 'Choose the staff designation',
  program: 'Choose the academic program',
  batchId: 'Choose the assigned batch',
  semester: 'Choose the semester',
  credits: 'Example: 3 or 4',
  description: 'Add a short description for this record',
  academicYear: 'Example: 2025-2026',
  isActive: 'Choose whether this record is active',
  username: 'Enter university ID or email',
  roleMasterId: 'Choose the user role',
  password: 'Set a temporary password',
  subjectId: 'Choose the subject from master data',
  staffId: 'Choose the assigned faculty member',
  courseId: 'Choose the course or program',
  title: 'Enter a clear title',
  type: 'Choose the calendar item type',
  startsAt: 'Choose the start date and time',
  endsAt: 'Choose the end date and time',
  location: 'Example: C-301, Lab 2, or Online',
  room: 'Example: C-301',
  maximumMarks: 'Example: 50 or 100',
  weightage: 'Example: 20',
  minimumPercentage: 'Example: 40',
  grade: 'Example: A',
  gradePoint: 'Example: 8.5',
  marksAwarded: 'Example: 2',
  reason: 'State the reason for this entry',
  audience: 'Choose who can respond',
  status: 'Choose the current status',
  closesAt: 'Choose the closing date and time',
  isbn: 'Enter ISBN or library code',
  author: 'Enter author name',
  category: 'Example: Computer Science',
  totalCount: 'Enter total copies',
  availableCount: 'Enter available copies',
  maximum: 'Enter maximum marks',
}

const modalHelpText = {
  staff: 'Create staff records with official employee ID, email, department and designation. A user account can be linked from User Master later.',
  students: 'Create student records from department, program, batch and semester master data so academic workflows stay correctly scoped.',
  courses: 'Create course records with code, credits and department mapping used by subject and batch setup.',
  batches: 'Create batch records from program, academic year and semester master data.',
  subjects: 'Create semester-wise subjects and connect each subject to its course.',
  'department-masters': 'Create departments used across staff, students, courses and reporting.',
  'designation-masters': 'Create staff designation values used in Staff Master.',
  'program-masters': 'Create academic programs used by students and batches.',
  'academic-sessions': 'Create academic years and semesters used across batch and subject setup.',
  'role-masters': 'Create roles used for role-based portal access.',
  users: 'Create login accounts with role and temporary password details.',
  'subject-allocations': 'Allocate a subject to faculty for a batch, academic year and semester.',
  default: 'Fill the required fields using master data where available, then save the record.',
}

const operationFields = {
  'subject-allocations': [
    ['subjectId', 'Subject', 'select', true, 'subjects'],
    ['staffId', 'Faculty', 'select', true, 'staff'],
    ['batchId', 'Batch', 'select', false, 'batches'],
    ['academicYear', 'Academic year', 'select', true, 'academicYears'],
    ['semester', 'Semester', 'select', true, 'semesters'],
  ],
}

const advancedFields = {
  'evaluation-components': [
    ['name', 'Component name', 'text', true],
    ['subjectId', 'Subject', 'select', true, 'subjects'],
    ['weightage', 'Weightage %', 'number', true],
    ['maximumMarks', 'Maximum marks', 'number', true],
    ['status', 'Status', 'select', true, 'recordStatuses'],
  ],
  'grade-rules': [
    ['name', 'Rule name', 'text', true],
    ['subjectId', 'Subject', 'select', false, 'subjects'],
    ['minimumPercentage', 'Minimum %', 'number', true],
    ['grade', 'Grade', 'text', true],
    ['gradePoint', 'Grade point', 'number', true],
    ['status', 'Status', 'select', true, 'recordStatuses'],
  ],
  'grace-mark-categories': [
    ['name', 'Category name', 'text', true],
    ['description', 'Description', 'text', true],
    ['maximumMarks', 'Maximum marks', 'number', true],
    ['status', 'Status', 'select', true, 'recordStatuses'],
  ],
  'grace-mark-applications': [
    ['studentId', 'Student', 'select', true, 'students'],
    ['subjectId', 'Subject', 'select', true, 'subjects'],
    ['graceMarkCategoryId', 'Category', 'select', true, 'graceMarkCategories'],
    ['marksAwarded', 'Marks awarded', 'number', true],
    ['reason', 'Reason', 'text', true],
    ['status', 'Status', 'select', true, 'approvalStatuses'],
  ],
  surveys: [
    ['title', 'Survey title', 'text', true],
    ['description', 'Description', 'text', true],
    ['audience', 'Audience', 'select', true, 'audiences'],
    ['status', 'Status', 'select', true, 'surveyStatuses'],
    ['closesAt', 'Closes at', 'datetime-local'],
  ],
}

const buildReferenceOptions = (referenceData) => {
  const sessions = referenceData.academicSessions || []
  const unique = (values) => [...new Set(values.filter(Boolean))]
  return {
    ...referenceData,
    academicYears: unique(sessions.map((item) => item.academicYear)).map((value) => ({ value, label: value })),
    semesters: unique(sessions.map((item) => item.semester)).map((value) => ({ value, label: `Semester ${value}` })),
    boolean: [{ value: true, label: 'Active' }, { value: false, label: 'Inactive' }],
    recordStatuses: ['Active', 'Inactive'].map((value) => ({ value, label: value })),
    approvalStatuses: ['Applied', 'Approved', 'Rejected'].map((value) => ({ value, label: value })),
    surveyStatuses: ['Draft', 'Published', 'Closed'].map((value) => ({ value, label: value })),
    audiences: ['Student', 'Faculty', 'All'].map((value) => ({ value, label: value })),
  }
}

const emptyMasterRecord = (page) => Object.fromEntries(masterFields[page].map(([key]) => [key, '']))

const rowToMasterRecord = (page, row) => {
  if (page === 'staff') return { employeeId: row.id, name: row.name, email: row.email || '', department: row.department || '', designation: row.designation || row.detail || '' }
  if (page === 'students') return { studentId: row.id, name: row.name, email: row.email || '', department: row.department || '', program: row.program || row.department || '', batchId: row.batchId || '', semester: row.semester || Number(String(row.detail || '').replace(/\D/g, '')) || '' }
  if (page === 'courses') return { courseCode: row.courseCode || row.id, name: row.name, credits: row.credits || Number(String(row.detail || '').replace(/\D/g, '')) || '', department: row.department || '', description: row.description || '' }
  if (page === 'batches') return { name: row.name, program: row.program || row.department || '', academicYear: row.academicYear || row.detail || '', semester: row.semester || Number(String(row.status || '').replace(/\D/g, '')) || '' }
  if (page === 'subjects') return { code: row.code || row.id, name: row.name, credits: row.credits || Number(String(row.detail || '').replace(/\D/g, '')) || '', semester: row.semester || '', courseId: row.courseId || '' }
  if (page === 'department-masters') return { code: row.code || row.id, name: row.name }
  if (page === 'designation-masters') return { name: row.name }
  if (page === 'program-masters') return { code: row.code || row.id, name: row.name }
  if (page === 'academic-sessions') return { academicYear: row.academicYear || row.name, semester: row.semester || '', isActive: row.isActive ?? true }
  if (page === 'role-masters') return { name: row.name, description: row.description || row.detail || '' }
  if (page === 'users') return { username: row.username || row.id, displayName: row.name, email: row.email || '', roleMasterId: row.roleMasterId || '', password: '' }
  return emptyMasterRecord(page)
}

const normalizeMasterPayload = (page, form) => {
  const payload = { ...form }
  const numericKeys = new Set(['batchId', 'courseId', 'roleMasterId'])
  for (const [key, , type] of masterFields[page]) {
    if (type === 'number' || numericKeys.has(key) || key === 'semester' || key === 'credits') payload[key] = payload[key] === '' ? null : Number(payload[key])
    if (key === 'isActive') payload[key] = payload[key] === true || payload[key] === 'true'
  }
  if (page === 'students' && !payload.department) payload.department = payload.program
  return payload
}

const normalizeOperationPayload = (page, form) => {
  const payload = { ...form }
  for (const [key, , type] of operationFields[page]) {
    if (type === 'number' || key.endsWith('Id') || key === 'semester') payload[key] = payload[key] === '' ? null : Number(payload[key])
  }
  return payload
}

const emptyAdvancedRecord = (page) => {
  const defaults = Object.fromEntries(advancedFields[page].map(([key]) => [key, '']))
  if ('status' in defaults) defaults.status = page === 'surveys' ? 'Draft' : 'Active'
  if (page === 'grace-mark-applications') defaults.status = 'Applied'
  return defaults
}

const rowToAdvancedRecord = (page, row) => {
  const record = {}
  for (const [key] of advancedFields[page]) record[key] = row[key] ?? ''
  if (page === 'surveys' && record.closesAt) record.closesAt = String(record.closesAt).slice(0, 16)
  return record
}

const normalizeAdvancedPayload = (page, form) => {
  const payload = { ...form }
  for (const [key, , type] of advancedFields[page]) {
    if (type === 'number' || key.endsWith('Id') || ['weightage', 'maximumMarks', 'minimumPercentage', 'gradePoint', 'marksAwarded'].includes(key)) payload[key] = payload[key] === '' ? null : Number(payload[key])
    if (type === 'datetime-local' && payload[key] === '') payload[key] = null
  }
  return payload
}

const searchableValue = (value) => {
  if (value === null || value === undefined) return ''
  if (Array.isArray(value)) return value.join(' ')
  if (typeof value === 'object') return Object.values(value).join(' ')
  return String(value)
}

const filterRows = (rows, term) => {
  const needle = term.trim().toLowerCase()
  if (!needle) return rows
  return rows.filter((row) => Object.values(row).some((value) => searchableValue(value).toLowerCase().includes(needle)))
}

const exportRows = (filename, rows) => {
  if (!rows.length) {
    window.alert('No rows available to export.')
    return
  }
  const keys = [...new Set(rows.flatMap((row) => Object.keys(row).filter((key) => typeof row[key] !== 'object')))]
  const escapeCell = (value) => `"${String(value ?? '').replaceAll('"', '""')}"`
  const csv = [keys.join(','), ...rows.map((row) => keys.map((key) => escapeCell(row[key])).join(','))].join('\n')
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = `${filename}.csv`
  link.click()
  URL.revokeObjectURL(url)
}

const exportWeeklyTimetable = (entries) => {
  const orderedDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
  const rows = [...entries].sort((a, b) => {
    const dayOrder = orderedDays.indexOf(a.day) - orderedDays.indexOf(b.day)
    return dayOrder || String(a.startTime || '').localeCompare(String(b.startTime || ''))
  })
  const escapeHtml = (value) => String(value ?? '').replace(/[&<>"']/g, (char) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[char]))
  const tableRows = rows.map((row) => `<tr><td>${escapeHtml(row.day)}</td><td>${escapeHtml(row.startTime)}</td><td>${escapeHtml(row.endTime)}</td><td>${escapeHtml(row.subject)}</td><td>${escapeHtml(row.faculty)}</td><td>${escapeHtml(row.batch)}</td><td>${escapeHtml(row.room)}</td></tr>`).join('')
  const html = `<!doctype html><html><head><meta charset="utf-8"><title>Weekly Timetable</title><style>body{font-family:Arial,sans-serif;color:#1f1a1b}h1{color:#8d153a}table{border-collapse:collapse;width:100%;font-size:12px}th,td{border:1px solid #ddd;padding:8px;text-align:left}th{background:#f5eef1;color:#8d153a}</style></head><body><h1>Weekly Timetable</h1><p>Generated from current timetable records.</p><table><thead><tr><th>Day</th><th>Start</th><th>End</th><th>Subject</th><th>Faculty</th><th>Batch</th><th>Room</th></tr></thead><tbody>${tableRows || '<tr><td colspan="7">No timetable slots available.</td></tr>'}</tbody></table></body></html>`
  const blob = new Blob([html], { type: 'application/msword;charset=utf-8;' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = 'weekly-timetable.doc'
  link.click()
  URL.revokeObjectURL(url)
}

function RecordsPage({ page }) {
  const map = { staff: faculty, students, courses, batches, subjects }
  const [rows, setRows] = useState(map[page] || courses)
  const [error, setError] = useState('')
  const [saving, setSaving] = useState(false)
  const [referenceData, setReferenceData] = useState({})
  const [selected, setSelected] = useState(null)
  const [editing, setEditing] = useState(null)
  const [formError, setFormError] = useState('')
  const [searchTerm, setSearchTerm] = useState('')
  const [filtersOpen, setFiltersOpen] = useState(false)
  const info = titleMap[page]
  const visibleRows = filterRows(rows, searchTerm)
  const loadRows = () => api(`/${page}?pageSize=1000`).then((payload) => { setRows(unwrapList(payload)); setError('') }).catch((err) => setError(err.message))
  useEffect(() => { loadRows() }, [page])
  useEffect(() => { api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => setReferenceData({})) }, [])
  const saveRecord = async (form) => {
    setSaving(true)
    setError('')
    setFormError('')
    try {
      const payload = normalizeMasterPayload(page, form)
      if (editing?.mode === 'edit') await api(`/${page}/${editing.row.key}`, { method: 'PUT', body: JSON.stringify(payload) })
      else await api(`/${page}`, { method: 'POST', body: JSON.stringify(payload) })
      setEditing(null)
      await loadRows()
    } catch (err) {
      setFormError(err.message)
    } finally {
      setSaving(false)
    }
  }
  const deleteRecord = async (row) => {
    if (!window.confirm(`Delete ${row.name || row.id}?`)) return
    setError('')
    try {
      await api(`/${page}/${row.key}`, { method: 'DELETE' })
      await loadRows()
    } catch (err) {
      setError(err.message)
    }
  }
  return (
    <>
      <PageHeader eyebrow="Management" title={info[0]} description={info[1]}><button className="btn btn-primary" onClick={() => setEditing({ mode: 'create', row: null })}><Plus size={17} /> Add new</button></PageHeader>
      {error && <p className="form-error">{error}</p>}
      <section className="panel data-panel"><Toolbar search={`Search ${info[0].toLowerCase()}…`} searchValue={searchTerm} onSearch={setSearchTerm} filterActive={filtersOpen} onFilter={() => setFiltersOpen((value) => !value)} onExport={() => exportRows(page, visibleRows)} />{filtersOpen && <div className="filter-strip"><span>Showing rows matching the search text.</span><button className="text-button" onClick={() => { setSearchTerm(''); setFiltersOpen(false) }}>Clear filter</button></div>}<DataTable columns={genericColumns} rows={visibleRows} onView={setSelected} onEdit={(row) => setEditing({ mode: 'edit', row })} onDelete={deleteRecord} /><div className="table-footer"><span>Showing {visibleRows.length} of {rows.length} records</span><div><button disabled>Previous</button><button className="current">1</button><button disabled>Next</button></div></div></section>
      <Modal title={info[0]} item={selected} onClose={() => setSelected(null)} />
      {editing && <MasterFormModal fields={masterFields[page]} options={referenceData} title={info[0]} mode={editing.mode} initial={editing.row ? rowToMasterRecord(page, editing.row) : emptyMasterRecord(page)} saving={saving} error={formError} onClose={() => { setEditing(null); setFormError('') }} onSave={saveRecord} />}
    </>
  )
}

function MasterFormModal({ fields, options = {}, title, mode, initial, saving, error, onClose, onSave }) {
  const [form, setForm] = useState(initial)
  const formKey = Object.keys(masterFields).find((key) => masterFields[key] === fields) || Object.keys(operationFields).find((key) => operationFields[key] === fields) || Object.keys(advancedFields).find((key) => advancedFields[key] === fields)
  const update = (key, value) => setForm((current) => {
    const next = { ...current, [key]: value }
    if (key === 'batchId') {
      const batch = (options.batches || []).find((item) => String(item.value) === String(value))
      if (batch) {
        next.program = batch.program || next.program
        next.semester = batch.semester || next.semester
        next.academicYear = batch.academicYear || next.academicYear
      }
    }
    if (key === 'academicYear') {
      const semesters = (options.academicSessions || []).filter((item) => String(item.academicYear) === String(value))
      next.semester = semesters.length === 1 ? semesters[0].semester : ''
    }
    return next
  })
  const selectOptions = (source, key) => {
    if (source === 'semesters' && form.academicYear) {
      return (options.academicSessions || [])
        .filter((item) => String(item.academicYear) === String(form.academicYear))
        .map((item) => ({ value: item.semester, label: `Semester ${item.semester}` }))
    }
    return options[source] || []
  }
  return (
    <div className="modal-backdrop">
      <form className="modal form-modal" onSubmit={(event) => { event.preventDefault(); onSave(form) }}>
        <button type="button" className="modal-close" onClick={onClose}>×</button>
        <span className="eyebrow">{mode === 'edit' ? 'Edit record' : 'New record'}</span>
        <h2>{mode === 'edit' ? `Edit ${title}` : `Add to ${title}`}</h2>
        <p>{modalHelpText[formKey] || modalHelpText.default}</p>
        {error && <p className="form-error modal-error">{error}</p>}
        <div className="form-grid">
          {fields.map(([key, label, type, required, source]) => (
            <label key={key}>{label}
              {type === 'select' ? (
                <select required={Boolean(required)} value={form[key] ?? ''} onChange={(event) => update(key, event.target.value)}>
                  <option value="">Select {label.toLowerCase()}</option>
                  {selectOptions(source, key).map((item) => <option key={`${source}-${item.value}`} value={item.value}>{item.label}</option>)}
                </select>
              ) : (
                <input type={type} required={Boolean(required)} placeholder={fieldPlaceholders[key] || `Enter ${label.toLowerCase()}`} value={form[key] ?? ''} onChange={(event) => update(key, event.target.value)} />
              )}
            </label>
          ))}
        </div>
        <div className="modal-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button className="btn btn-primary" disabled={saving}>{saving ? 'Saving…' : 'Save record'}</button>
        </div>
      </form>
    </div>
  )
}

function SubjectAllocationPage() {
  const [rows, setRows] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [editing, setEditing] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')
  const [formError, setFormError] = useState('')
  const loadRows = () => api('/subject-allocations?pageSize=1000').then((payload) => setRows(unwrapList(payload))).catch((err) => setError(err.message))
  useEffect(() => { loadRows() }, [])
  useEffect(() => { api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => setReferenceData({})) }, [])
  const saveAllocation = async (form) => {
    setSaving(true)
    setError('')
    setFormError('')
    try {
      await api('/subject-allocations', { method: 'POST', body: JSON.stringify(normalizeOperationPayload('subject-allocations', form)) })
      setEditing(false)
      await loadRows()
    } catch (err) {
      setFormError(err.message)
    } finally {
      setSaving(false)
    }
  }
  return (
    <>
      <PageHeader eyebrow="Operations" title="Subject Allocation" description="Allocate subjects to faculty independently from Subject Master.">
        <button className="btn btn-primary" onClick={() => { setFormError(''); setEditing(true) }}><Plus size={17} /> Allocate subject</button>
      </PageHeader>
      {error && <p className="form-error">{error}</p>}
      <section className="panel data-panel">
        <DataTable rows={rows} columns={[
          { key: 'subject', label: 'Subject' },
          { key: 'faculty', label: 'Faculty' },
          { key: 'batch', label: 'Batch' },
          { key: 'academicYear', label: 'Academic year' },
          { key: 'semester', label: 'Semester' },
          { key: 'id', label: 'Allocation ID' },
        ]} />
      </section>
      {editing && <MasterFormModal fields={operationFields['subject-allocations']} options={referenceData} title="Subject Allocation" mode="create" initial={Object.fromEntries(operationFields['subject-allocations'].map(([key]) => [key, '']))} saving={saving} error={formError} onClose={() => { setEditing(false); setFormError('') }} onSave={saveAllocation} />}
    </>
  )
}

function AdvancedRecordsPage({ page }) {
  const { user } = useAuth()
  const info = titleMap[page]
  const [rows, setRows] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [editing, setEditing] = useState(null)
  const [saving, setSaving] = useState(false)
  const [message, setMessage] = useState('')
  const [formError, setFormError] = useState('')
  const fields = advancedFields[page]
  const canModify = user.role === 'admin' || (user.role === 'faculty' && page === 'evaluation-components')
  const load = () => api(`/${page}`).then((payload) => setRows(unwrapList(payload))).catch((err) => setMessage(err.message))
  useEffect(() => {
    load()
    if (user.role === 'faculty' && page === 'evaluation-components') {
      Promise.all([api('/reference-data'), api('/faculty/subjects')])
        .then(([referencePayload, allocatedSubjects]) => {
          const options = buildReferenceOptions(referencePayload)
          options.subjects = allocatedSubjects.map((item) => ({ value: item.subjectId, label: `${item.id} - ${item.name}` }))
          setReferenceData(options)
        })
        .catch(() => {})
      return
    }
    api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {})
  }, [page, user.role])
  const save = async (form) => {
    setSaving(true)
    setMessage('')
    setFormError('')
    try {
      const payload = normalizeAdvancedPayload(page, form)
      if (editing?.mode === 'edit') await api(`/${page}/${editing.row.id}`, { method: 'PUT', body: JSON.stringify(payload) })
      else await api(`/${page}`, { method: 'POST', body: JSON.stringify(payload) })
      setEditing(null)
      await load()
    } catch (err) {
      setFormError(err.message)
    } finally {
      setSaving(false)
    }
  }
  const remove = async (row) => {
    if (!window.confirm(`Delete ${row.name || row.title || row.id}?`)) return
    try {
      await api(`/${page}/${row.id}`, { method: 'DELETE' })
      await load()
    } catch (err) {
      setMessage(err.message)
    }
  }
  const submitSurvey = async (row) => {
    try {
      const response = window.prompt('Enter your survey response')
      if (!response) return
      await api(`/surveys/${row.id}/responses`, { method: 'POST', body: JSON.stringify({ responseJson: JSON.stringify({ response }) }) })
      setMessage('Survey response saved.')
      await load()
    } catch (err) {
      setMessage(err.message)
    }
  }
  const columns = {
    'evaluation-components': [{ key: 'name', label: 'Component' }, { key: 'subject', label: 'Subject' }, { key: 'weightage', label: 'Weightage' }, { key: 'maximumMarks', label: 'Max marks' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }],
    'grade-rules': [{ key: 'name', label: 'Rule' }, { key: 'subject', label: 'Subject' }, { key: 'minimumPercentage', label: 'Min %' }, { key: 'grade', label: 'Grade' }, { key: 'gradePoint', label: 'Point' }],
    'grace-mark-categories': [{ key: 'name', label: 'Category' }, { key: 'description', label: 'Description' }, { key: 'maximumMarks', label: 'Max marks' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }],
    'grace-mark-applications': [{ key: 'student', label: 'Student' }, { key: 'subject', label: 'Subject' }, { key: 'category', label: 'Category' }, { key: 'marksAwarded', label: 'Marks' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }],
    surveys: [{ key: 'title', label: 'Survey' }, { key: 'audience', label: 'Audience' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }, { key: 'responses', label: 'Responses' }, { key: 'closesAt', label: 'Closes' }, ...(user.role === 'admin' ? [] : [{ key: 'id', label: 'Action', render: (_, row) => <button className="text-button" onClick={() => submitSurvey(row)}>Submit response</button> }])],
  }[page]
  return <><PageHeader eyebrow="Operations" title={info[0]} description={info[1]}>{canModify && <button className="btn btn-primary" onClick={() => { setFormError(''); setEditing({ mode: 'create', row: null }) }}><Plus size={17} /> Add new</button>}</PageHeader>{message && <p className="form-error">{message}</p>}<section className="panel data-panel"><DataTable rows={rows} columns={columns} onEdit={canModify ? (row) => { setFormError(''); setEditing({ mode: 'edit', row }) } : undefined} onDelete={canModify ? remove : undefined} /></section>{editing && <MasterFormModal fields={fields} options={referenceData} title={info[0]} mode={editing.mode} initial={editing.row ? rowToAdvancedRecord(page, editing.row) : emptyAdvancedRecord(page)} saving={saving} error={formError} onClose={() => { setEditing(null); setFormError('') }} onSave={save} />}</>
}

function FacultySubjectsPage() {
  const [rows, setRows] = useState([])
  const [message, setMessage] = useState('')
  useEffect(() => { api('/faculty/subjects').then(setRows).catch((err) => setMessage(err.message)) }, [])
  return <><PageHeader eyebrow="Faculty workspace" title="My Subjects" description="Subjects allocated to you by academic administration." />{message && <p className="form-error">{message}</p>}<section className="faculty-overview-grid">{rows.map((row) => <article className="panel faculty-subject-card" key={row.key}><span className="resource-icon r1"><BookOpen size={22} /></span><small>{row.id}</small><h3>{row.name}</h3><p>{row.detail} · {row.batches} batch{row.batches === 1 ? '' : 'es'}</p><StatusPill>{row.status}</StatusPill></article>)}</section><section className="panel data-panel"><DataTable rows={rows} columns={[{ key: 'id', label: 'Code' }, { key: 'name', label: 'Subject' }, { key: 'detail', label: 'Credits' }, { key: 'batches', label: 'Batches' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }]} /></section></>
}

function FacultyStudentsPage() {
  const [subjects, setSubjects] = useState([])
  const [batches, setBatches] = useState([])
  const [students, setStudents] = useState([])
  const [filters, setFilters] = useState({ subjectId: '', batchId: '' })
  const [message, setMessage] = useState('')
  useEffect(() => { api('/faculty/subjects').then(setSubjects).catch((err) => setMessage(err.message)) }, [])
  useEffect(() => {
    const suffix = filters.subjectId ? `?subjectId=${filters.subjectId}` : ''
    api(`/faculty/batches${suffix}`).then((payload) => { setBatches(payload); if (payload.length && !filters.batchId) setFilters((current) => ({ ...current, batchId: String(payload[0].batchId) })) }).catch((err) => setMessage(err.message))
  }, [filters.subjectId])
  useEffect(() => {
    const params = new URLSearchParams()
    if (filters.subjectId) params.set('subjectId', filters.subjectId)
    if (filters.batchId) params.set('batchId', filters.batchId)
    api(`/faculty/students?${params}`).then(setStudents).catch((err) => setMessage(err.message))
  }, [filters.subjectId, filters.batchId])
  return <><PageHeader eyebrow="Faculty workspace" title="Assigned Students" description="View only batches and students connected to your allocated subjects." />{message && <p className="form-error">{message}</p>}<section className="panel form-panel"><div className="panel-heading"><div><h2>Class filter</h2><p>Select an allocated subject and batch to load the class roster.</p></div></div><div className="form-grid"><label>Subject<select value={filters.subjectId} onChange={(e) => setFilters({ subjectId: e.target.value, batchId: '' })}><option value="">All allocated subjects</option>{subjects.map((x) => <option key={x.subjectId} value={x.subjectId}>{x.id} - {x.name}</option>)}</select></label><label>Batch<select value={filters.batchId} onChange={(e) => setFilters({ ...filters, batchId: e.target.value })}><option value="">All assigned batches</option>{batches.map((x) => <option key={`${x.subjectId}-${x.batchId}`} value={x.batchId}>{x.name} · {x.subject}</option>)}</select></label></div></section><section className="panel data-panel"><DataTable rows={students} columns={[{ key: 'studentId', label: 'Student ID' }, { key: 'name', label: 'Name' }, { key: 'email', label: 'Email' }, { key: 'batch', label: 'Batch' }, { key: 'semester', label: 'Semester' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }]} /></section></>
}

function TimetablePage() {
  const { user } = useAuth()
  const [entries, setEntries] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [editing, setEditing] = useState(null)
  const [saving, setSaving] = useState(false)
  const [message, setMessage] = useState('')
  const fields = [
    ['day', 'Day', 'select', true, 'days'],
    ['startTime', 'Start time', 'time', true],
    ['endTime', 'End time', 'time', true],
    ['room', 'Room', 'text', true],
    ['subjectId', 'Subject', 'select', true, 'subjects'],
    ['staffId', 'Faculty', 'select', true, 'staff'],
    ['batchId', 'Batch', 'select', true, 'batches'],
  ]
  const emptySlot = { day: 'Monday', startTime: '', endTime: '', room: '', subjectId: '', staffId: '', batchId: '' }
  const load = () => api(user.role === 'faculty' ? '/faculty/timetable' : '/timetable').then((payload) => setEntries(unwrapList(payload))).catch(() => setEntries([]))
  useEffect(() => { load(); api('/reference-data').then((payload) => setReferenceData({ ...buildReferenceOptions(payload), days: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'].map((day) => ({ value: day, label: day })) })).catch(() => {}) }, [])
  const normalizeTime = (value) => value && value.length === 5 ? `${value}:00` : value
  const saveSlot = async (form) => {
    setSaving(true)
    setMessage('')
    const payload = { ...form, startTime: normalizeTime(form.startTime), endTime: normalizeTime(form.endTime), subjectId: Number(form.subjectId), staffId: Number(form.staffId), batchId: Number(form.batchId) }
    try {
      if (editing?.mode === 'edit') await api(`/timetable/${editing.row.id}`, { method: 'PUT', body: JSON.stringify(payload) })
      else await api('/timetable', { method: 'POST', body: JSON.stringify(payload) })
      setEditing(null)
      await load()
      setMessage('Timetable slot saved.')
    } catch (err) {
      setMessage(err.message)
    } finally {
      setSaving(false)
    }
  }
  const deleteSlot = async (row) => {
    if (!window.confirm(`Delete ${row.day} ${row.startTime} slot?`)) return
    try {
      await api(`/timetable/${row.id}`, { method: 'DELETE' })
      await load()
      setMessage('Timetable slot deleted.')
    } catch (err) {
      setMessage(err.message)
    }
  }
  const rowToSlot = (row) => ({ day: row.day, startTime: row.startTime?.slice(0, 5) || '', endTime: row.endTime?.slice(0, 5) || '', room: row.room || '', subjectId: row.subjectId || '', staffId: row.staffId || '', batchId: row.batchId || '' })
  return (
    <>
      <PageHeader eyebrow="Academic" title="Weekly Timetable" description={user.role === 'faculty' ? 'Calendar view of slots assigned to you.' : 'View class schedules, rooms and faculty assignments.'}><button className="btn btn-secondary" onClick={() => exportWeeklyTimetable(entries)}><Download size={17} /> Download</button>{user.role === 'admin' && <button className="btn btn-primary" onClick={() => setEditing({ mode: 'create', row: null })}><Plus size={17} /> Add slot</button>}</PageHeader>
      {message && <p className="form-error">{message}</p>}
      {user.role === 'faculty' && <section className="faculty-calendar panel"><div className="panel-heading"><div><h2>This week</h2><p>Your assigned teaching slots are highlighted.</p></div></div><div className="faculty-calendar-grid">{['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'].map((day) => <div className="calendar-day" key={day}><h3>{day}</h3>{entries.filter((x) => x.day === day).map((slot) => <article className="slot-card mine" key={slot.id}><strong>{slot.subject}</strong><span>{slot.startTime} - {slot.endTime}</span><small>{slot.batch} · {slot.room}</small></article>)}</div>)}</div></section>}
      <section className="panel"><div className="timetable-filters"><label>Course<select><option>B.Tech CSE</option></select></label><label>Batch<select><option>2022-26 · Section A</option></select></label><label>Faculty<select><option>{user.role === 'faculty' ? 'My slots' : 'All faculty'}</option></select></label></div>
        {entries.length > 0 ? <DataTable rows={entries} columns={[{ key: 'day', label: 'Day' }, { key: 'startTime', label: 'Start' }, { key: 'endTime', label: 'End' }, { key: 'subject', label: 'Subject' }, { key: 'faculty', label: 'Faculty' }, { key: 'batch', label: 'Batch' }, { key: 'room', label: 'Room' }]} onEdit={user.role === 'admin' ? (row) => setEditing({ mode: 'edit', row }) : undefined} onDelete={user.role === 'admin' ? deleteSlot : undefined} /> : <div className="timetable-scroll"><div className="timetable-grid"><div className="tt-head">Time</div>{['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'].map((day) => <div className="tt-head" key={day}>{day}</div>)}{schedule.map((row) => <><div className="tt-time" key={`${row.time}-time`}>{row.time}<small>60 min</small></div>{['mon', 'tue', 'wed', 'thu', 'fri'].map((day) => <div className={`tt-cell c${row[day][0].length % 4}`} key={`${row.time}-${day}`}><strong>{row[day][0]}</strong><span><MapPin size={12} />{row[day][1]}</span></div>)}</>)}</div></div>}
      </section>
      {editing && <MasterFormModal fields={fields} options={referenceData} title="Timetable Slot" mode={editing.mode} initial={editing.row ? rowToSlot(editing.row) : emptySlot} saving={saving} onClose={() => setEditing(null)} onSave={saveSlot} />}
    </>
  )
}

function ExamPage() {
  const [items, setItems] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [form, setForm] = useState({ name: '', subjectId: '', batchId: '', startsAt: '', endsAt: '', room: '' })
  const [error, setError] = useState('')
  const load = () => api('/examinations').then(setItems).catch((err) => setError(err.message))
  useEffect(() => { load(); api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {}) }, [])
  const submit = async (event) => {
    event.preventDefault()
    try {
      await api('/examinations', { method: 'POST', body: JSON.stringify({ ...form, subjectId: Number(form.subjectId), batchId: Number(form.batchId), startsAt: form.startsAt, endsAt: form.endsAt }) })
      setForm({ name: '', subjectId: '', batchId: '', startsAt: '', endsAt: '', room: '' })
      await load()
    } catch (err) { setError(err.message) }
  }
  return <><PageHeader eyebrow="Academic" title="Examinations" description="Create and view examination schedules." />{error && <p className="form-error">{error}</p>}<section className="panel form-panel"><div className="panel-heading"><div><h2>Create exam schedule</h2><p>Use master data for subject and batch selection.</p></div></div><form className="form-grid" onSubmit={submit}><label>Exam name<input required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} /></label><label>Subject<select required value={form.subjectId} onChange={(e) => setForm({ ...form, subjectId: e.target.value })}><option value="">Select subject</option>{(referenceData.subjects || []).map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Batch<select required value={form.batchId} onChange={(e) => setForm({ ...form, batchId: e.target.value })}><option value="">Select batch</option>{(referenceData.batches || []).map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Starts at<input type="datetime-local" required value={form.startsAt} onChange={(e) => setForm({ ...form, startsAt: e.target.value })} /></label><label>Ends at<input type="datetime-local" required value={form.endsAt} onChange={(e) => setForm({ ...form, endsAt: e.target.value })} /></label><label>Room<input required value={form.room} onChange={(e) => setForm({ ...form, room: e.target.value })} /></label><div className="form-actions"><button className="btn btn-primary">Save exam</button></div></form></section><section className="panel data-panel"><DataTable rows={items} columns={[{ key: 'name', label: 'Exam' }, { key: 'subject', label: 'Subject' }, { key: 'batch', label: 'Batch' }, { key: 'startsAt', label: 'Starts' }, { key: 'room', label: 'Room' }]} /></section></>
}

function CalendarPage() {
  const { user } = useAuth()
  const [payload, setPayload] = useState({ events: [], summary: [], student: null })
  const [createdEvents, setCreatedEvents] = useState([])
  const [subjectsScoped, setSubjectsScoped] = useState([])
  const [batchesScoped, setBatchesScoped] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [eventForm, setEventForm] = useState({ title: '', type: 'Quiz', subjectId: '', batchId: '', startsAt: '', endsAt: '', location: '', description: '' })
  const [selectedType, setSelectedType] = useState('All')
  const [message, setMessage] = useState('')
  const canCreateEvents = user.role === 'faculty' || user.role === 'admin'
  const loadCalendar = () => api('/student/calendar').then((data) => { setPayload(data); setMessage('') }).catch((err) => setMessage(err.message))
  const loadCreatedEvents = () => canCreateEvents && api('/calendar-events').then(setCreatedEvents).catch(() => setCreatedEvents([]))
  useEffect(() => {
    loadCalendar()
    if (canCreateEvents) loadCreatedEvents()
    if (user.role === 'faculty') api('/faculty/subjects').then(setSubjectsScoped).catch(() => setSubjectsScoped([]))
    if (user.role === 'admin') api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => setReferenceData({}))
  }, [user.role])
  useEffect(() => {
    if (!canCreateEvents) return
    if (user.role === 'faculty') {
      const suffix = eventForm.subjectId ? `?subjectId=${eventForm.subjectId}` : ''
      api(`/faculty/batches${suffix}`).then(setBatchesScoped).catch(() => setBatchesScoped([]))
    }
  }, [canCreateEvents, user.role, eventForm.subjectId])
  const subjectOptions = user.role === 'faculty' ? subjectsScoped.map((x) => ({ value: x.subjectId, label: `${x.id} - ${x.name}` })) : referenceData.subjects || []
  const batchOptions = user.role === 'faculty' ? batchesScoped.map((x) => ({ value: x.batchId, label: `${x.name} · ${x.subject}` })) : referenceData.batches || []
  const createEvent = async (event) => {
    event.preventDefault()
    try {
      await api('/calendar-events', {
        method: 'POST',
        body: JSON.stringify({
          ...eventForm,
          subjectId: eventForm.subjectId ? Number(eventForm.subjectId) : null,
          batchId: eventForm.batchId ? Number(eventForm.batchId) : null,
        }),
      })
      setEventForm({ title: '', type: 'Quiz', subjectId: '', batchId: '', startsAt: '', endsAt: '', location: '', description: '' })
      setMessage('Calendar item created and added to the student feed.')
      await loadCalendar()
      await loadCreatedEvents()
    } catch (err) { setMessage(err.message) }
  }
  const events = canCreateEvents ? createdEvents.map((event) => ({ ...event, status: event.status || 'Scheduled' })) : payload.events || []
  const types = ['All', ...new Set(events.map((event) => event.type))]
  const filtered = selectedType === 'All' ? events : events.filter((event) => event.type === selectedType)
  const calendarBase = events.length ? new Date(events[0].startsAt) : new Date()
  const calendarMonth = new Date(calendarBase.getFullYear(), calendarBase.getMonth(), 1)
  const startOffset = calendarMonth.getDay()
  const daysInMonth = new Date(calendarBase.getFullYear(), calendarBase.getMonth() + 1, 0).getDate()
  const calendarCells = [
    ...Array.from({ length: startOffset }, (_, index) => ({ key: `blank-${index}`, blank: true })),
    ...Array.from({ length: daysInMonth }, (_, index) => {
      const date = new Date(calendarBase.getFullYear(), calendarBase.getMonth(), index + 1)
      const dayEvents = filtered.filter((event) => new Date(event.startsAt).toDateString() === date.toDateString())
      return { key: date.toISOString(), date, events: dayEvents }
    }),
  ]
  const formatDay = (value) => new Date(value).toLocaleDateString('en-IN', { day: '2-digit', month: 'short' })
  const formatTime = (start, end) => `${new Date(start).toLocaleTimeString('en-IN', { hour: '2-digit', minute: '2-digit' })} - ${new Date(end).toLocaleTimeString('en-IN', { hour: '2-digit', minute: '2-digit' })}`
  return (
    <>
      <PageHeader eyebrow={user.role === 'student' ? 'Student calendar' : 'Academic'} title="Academic Calendar" description="Classes, assignments, quizzes, exams and deadlines in one descriptive timeline." />
      {message && <p className="form-error">{message}</p>}
      {canCreateEvents && <section className="panel form-panel"><div className="panel-heading"><div><h2>Create calendar item</h2><p>Feed quizzes, deadlines and events directly into the student calendar for the selected batch.</p></div></div><form className="form-grid" onSubmit={createEvent}><label>Title<input required value={eventForm.title} onChange={(e) => setEventForm({ ...eventForm, title: e.target.value })} /></label><label>Type<select value={eventForm.type} onChange={(e) => setEventForm({ ...eventForm, type: e.target.value })}><option>Quiz</option><option>Deadline</option><option>Event</option><option>Assessment</option><option>Academic</option></select></label><label>Subject<select value={eventForm.subjectId} onChange={(e) => setEventForm({ ...eventForm, subjectId: e.target.value, batchId: '' })}><option value="">General</option>{subjectOptions.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Batch<select value={eventForm.batchId} onChange={(e) => setEventForm({ ...eventForm, batchId: e.target.value })}><option value="">All applicable batches</option>{batchOptions.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Starts at<input type="datetime-local" required value={eventForm.startsAt} onChange={(e) => setEventForm({ ...eventForm, startsAt: e.target.value })} /></label><label>Ends at<input type="datetime-local" required value={eventForm.endsAt} onChange={(e) => setEventForm({ ...eventForm, endsAt: e.target.value })} /></label><label>Location<input value={eventForm.location} onChange={(e) => setEventForm({ ...eventForm, location: e.target.value })} placeholder="Room, lab or online" /></label><label>Description<input value={eventForm.description} onChange={(e) => setEventForm({ ...eventForm, description: e.target.value })} placeholder="Preparation notes or context" /></label><div className="form-actions"><button className="btn btn-primary"><Plus size={16} /> Save calendar item</button></div></form></section>}
      <section className="calendar-filters">
        {types.map((type) => <button key={type} className={selectedType === type ? 'selected' : ''} onClick={() => setSelectedType(type)}>{type}</button>)}
      </section>
      <section className="calendar-layout student-calendar-layout">
        <article className="panel month-calendar-panel">
          <div className="panel-heading"><div><h2>{calendarBase.toLocaleDateString('en-IN', { month: 'long', year: 'numeric' })}</h2><p>Calendar view for classes, quizzes, assignments, exams and deadlines.</p></div></div>
          <div className="month-calendar-grid">
            {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map((day) => <strong className="month-calendar-head" key={day}>{day}</strong>)}
            {calendarCells.map((cell) => cell.blank ? <span className="month-calendar-cell blank" key={cell.key} /> : <div className={cell.date.toDateString() === new Date().toDateString() ? 'month-calendar-cell today' : 'month-calendar-cell'} key={cell.key}><strong>{cell.date.getDate()}</strong>{cell.events.slice(0, 3).map((event) => <span className={`month-event-dot type-${event.type.toLowerCase()}`} key={event.id}>{event.title}</span>)}{cell.events.length > 3 && <small>+{cell.events.length - 3} more</small>}</div>)}
          </div>
        </article>
        <section className="panel calendar-list">
          <div className="panel-heading"><div><h2>All calendar items</h2><p>Filtered from live academic events for this student.</p></div></div>
          {filtered.map((event) => <article className={`calendar-event rich-calendar-event type-${event.type.toLowerCase()}`} key={event.id}><time><strong>{formatDay(event.startsAt)}</strong><span>{new Date(event.startsAt).toLocaleDateString('en-IN', { weekday: 'short' })}</span></time><div><div className="calendar-event-top"><StatusPill>{event.type}</StatusPill><small>{event.status}</small></div><h3>{event.title}</h3><p>{event.description}</p><div className="calendar-event-meta"><span><Clock3 size={13} />{formatTime(event.startsAt, event.endsAt)}</span><span><MapPin size={13} />{event.location}</span><span><UserCheck size={13} />{event.owner}</span><span><BookOpen size={13} />{event.subject}</span></div></div></article>)}
          {!filtered.length && <p className="empty-note">No calendar items found for this filter yet.</p>}
        </section>
      </section>
      <section className="panel data-panel calendar-table"><DataTable rows={filtered.map((event) => ({ ...event, date: formatDay(event.startsAt), time: formatTime(event.startsAt, event.endsAt) }))} columns={[{ key: 'date', label: 'Date' }, { key: 'time', label: 'Time' }, { key: 'title', label: 'Event' }, { key: 'type', label: 'Type' }, { key: 'subject', label: 'Subject' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }]} /></section>
      {canCreateEvents && <section className="panel data-panel"><div className="panel-heading padded-heading"><div><h2>Created calendar feed</h2><p>Items created from this calendar screen.</p></div></div><DataTable rows={events} columns={[{ key: 'title', label: 'Title' }, { key: 'type', label: 'Type' }, { key: 'subject', label: 'Subject' }, { key: 'batch', label: 'Batch' }, { key: 'startsAt', label: 'Starts' }, { key: 'location', label: 'Location' }]} /></section>}
    </>
  )
}

function LibraryPage({ title = 'Book Catalogue' }) {
  const { user } = useAuth()
  const [items, setItems] = useState([])
  const [transactions, setTransactions] = useState([])
  const [editing, setEditing] = useState(null)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')
  const [formError, setFormError] = useState('')
  const [searchTerm, setSearchTerm] = useState('')
  const [filtersOpen, setFiltersOpen] = useState(false)
  const visibleBooks = filterRows(items, searchTerm)
  const load = () => { api('/library/books').then((payload) => { setItems(payload); setError('') }).catch((err) => { setItems([]); setError(err.message) }); api('/library/transactions').then(setTransactions).catch(() => setTransactions([])) }
  useEffect(() => { load() }, [])
  const issue = async (book) => { try { await api(`/library/books/${book.id}/issue`, { method: 'POST' }); await load() } catch (err) { setError(err.message) } }
  const returnBook = async (tx) => { try { await api(`/library/transactions/${tx.id}/return`, { method: 'POST' }); await load() } catch (err) { setError(err.message) } }
  const fields = [['title', 'Title', 'text', true], ['author', 'Author', 'text', true], ['isbn', 'ISBN / Code', 'text', true], ['category', 'Category', 'text', true], ['totalCount', 'Total count', 'number', true], ['availableCount', 'Available count', 'number', true]]
  const saveBook = async (form) => {
    setSaving(true)
    setFormError('')
    try {
      const payload = { ...form, totalCount: Number(form.totalCount), availableCount: Number(form.availableCount) }
      if (editing?.mode === 'edit') await api(`/library/books/${editing.row.id}`, { method: 'PUT', body: JSON.stringify(payload) })
      else await api('/library/books', { method: 'POST', body: JSON.stringify(payload) })
      setEditing(null)
      await load()
    } catch (err) {
      setFormError(err.message)
    } finally {
      setSaving(false)
    }
  }
  const deleteBook = async (book) => {
    if (!window.confirm(`Delete ${book.title}?`)) return
    try { await api(`/library/books/${book.id}`, { method: 'DELETE' }); await load() } catch (err) { setError(err.message) }
  }
  const bookToForm = (book) => ({ title: book.title, author: book.author, isbn: book.isbn || book.code || '', category: book.category || '', totalCount: book.totalCount || 1, availableCount: book.availableCount ?? (book.available ? 1 : 0) })
  return (
    <>
      <PageHeader eyebrow="Central Library" title={title} description="Discover books, journals and digital resources."><button className="btn btn-secondary"><BookOpen size={17} /> Issue history</button>{user.role === 'admin' && <button className="btn btn-primary" onClick={() => { setFormError(''); setEditing({ mode: 'create', row: null }) }}><Plus size={17} /> Add book</button>}</PageHeader>
      {error && <p className="form-error">{error}</p>}
      <section className="panel library-panel"><Toolbar search="Search by title, author or ISBN…" searchValue={searchTerm} onSearch={setSearchTerm} filterActive={filtersOpen} onFilter={() => setFiltersOpen((value) => !value)} onExport={() => exportRows('library-books', visibleBooks)} />{filtersOpen && <div className="filter-strip"><span>Showing books matching the search text.</span><button className="text-button" onClick={() => { setSearchTerm(''); setFiltersOpen(false) }}>Clear filter</button></div>}<div className="book-grid">{visibleBooks.map((book) => <article className="book-card" key={book.code || book.id}><div className="book-cover" style={{ background: book.color || '#8d153a' }}><BookOpen size={30} /><small>AMRITA LIBRARY</small></div><div><span className="book-category">{book.category}</span><h3>{book.title}</h3><p>{book.author}</p><small>{book.isbn || book.code}</small><div className="book-actions"><StatusPill>{book.available || book.availableCount > 0 ? 'Available' : 'Issued'}</StatusPill>{user.role === 'admin' ? <span className="book-admin-actions"><button className="text-button" onClick={() => { setFormError(''); setEditing({ mode: 'edit', row: book }) }}>Edit</button><button className="text-button" onClick={() => deleteBook(book)}>Delete</button></span> : <button className="text-button" onClick={() => issue(book)} disabled={!(book.available || book.availableCount > 0)}>{book.available || book.availableCount > 0 ? 'Issue' : 'Unavailable'}</button>}</div></div></article>)}</div>{!visibleBooks.length && <p className="empty-note">No library books are available for this filter.</p>}</section>
      <section className="panel"><div className="panel-heading"><div><h2>My transactions</h2><p>Issued and returned books</p></div></div><DataTable rows={transactions} columns={[{ key: 'book', label: 'Book' }, { key: 'issueDate', label: 'Issued' }, { key: 'dueDate', label: 'Due' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }, { key: 'id', label: 'Action', render: (_, row) => row.status === 'Issued' ? <button className="text-button" onClick={() => returnBook(row)}>Return</button> : 'Returned' }]} /></section>
      {editing && <MasterFormModal fields={fields} title="Library Book" mode={editing.mode} initial={editing.row ? bookToForm(editing.row) : { title: '', author: '', isbn: '', category: '', totalCount: 1, availableCount: 1 }} saving={saving} error={formError} onClose={() => { setEditing(null); setFormError('') }} onSave={saveBook} />}
    </>
  )
}

function AttendancePage() {
  const { user } = useAuth()
  const [summary, setSummary] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [subjects, setSubjects] = useState([])
  const [batches, setBatches] = useState([])
  const [slots, setSlots] = useState([])
  const [register, setRegister] = useState({ subjectId: '', batchId: '', month: new Date().toISOString().slice(0, 7), slotId: '', date: new Date().toISOString().slice(0, 10), session: '' })
  const [roster, setRoster] = useState([])
  const [recent, setRecent] = useState([])
  const [error, setError] = useState('')
  const loadAttendanceData = () => {
    api('/attendance/summary').then(setSummary).catch(() => setSummary([]))
    api('/attendance/recent').then(setRecent).catch(() => setRecent([]))
  }
  useEffect(() => {
    loadAttendanceData()
    api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {})
    if (user.role === 'faculty') api('/faculty/subjects').then(setSubjects).catch(() => setSubjects([]))
  }, [user.role])
  useEffect(() => {
    if (user.role !== 'faculty') return
    const suffix = register.subjectId ? `?subjectId=${register.subjectId}` : ''
    api(`/faculty/batches${suffix}`).then((payload) => setBatches(payload)).catch(() => setBatches([]))
  }, [user.role, register.subjectId])
  useEffect(() => {
    if (!register.subjectId || !register.batchId) { setSlots([]); return }
    const [year, month] = register.month.split('-')
    api(`/attendance/slots?subjectId=${register.subjectId}&batchId=${register.batchId}&year=${year}&month=${month}`).then((payload) => setSlots(payload)).catch((err) => setError(err.message))
  }, [register.subjectId, register.batchId, register.month])
  const loadRoster = async () => {
    if (!register.batchId || !register.subjectId || !register.slotId) return
    const slot = slots.find((item) => item.id === register.slotId)
    if (!slot) return
    setRegister((current) => ({ ...current, date: slot.date, session: slot.session }))
    setRoster(await api(`/attendance/roster?batchId=${register.batchId}&subjectId=${register.subjectId}&date=${slot.date}&session=${encodeURIComponent(slot.session)}`))
  }
  const submitAttendance = async () => {
    try {
      await api('/attendance', { method: 'POST', body: JSON.stringify({ subjectId: Number(register.subjectId), batchId: Number(register.batchId), date: register.date, session: register.session, records: roster.map((x) => ({ studentId: x.id, status: x.status })) }) })
      setError('Attendance saved.')
      loadAttendanceData()
    } catch (err) { setError(err.message) }
  }
  const overall = summary.length ? Math.round(summary.reduce((sum, item) => sum + Number(item.percentage || 0), 0) / summary.length) : 0
  const data = [{ name: 'Present', value: overall, color: '#8d153a' }, { name: 'Absent', value: Math.max(0, 100 - overall), color: '#eee5e7' }]
  return (
    <>
      <PageHeader eyebrow="Academic progress" title="Attendance" description="Track attendance across your registered subjects."><button className="btn btn-secondary"><Download size={17} /> Export report</button></PageHeader>
      {error && <p className="form-error">{error}</p>}
      {user.role !== 'student' && <section className="panel form-panel attendance-register-panel"><div className="panel-heading"><div><h2>Slot-wise attendance register</h2><p>Select an assigned subject, batch and active timetable slot for the month.</p></div></div><div className="form-grid"><label>Subject<select value={register.subjectId} onChange={(e) => setRegister({ ...register, subjectId: e.target.value, batchId: '', slotId: '', session: '' })}><option value="">Select subject</option>{(user.role === 'faculty' ? subjects.map((x) => ({ value: x.subjectId, label: `${x.id} - ${x.name}` })) : referenceData.subjects || []).map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Batch<select value={register.batchId} onChange={(e) => setRegister({ ...register, batchId: e.target.value, slotId: '', session: '' })}><option value="">Select batch</option>{(user.role === 'faculty' ? batches.map((x) => ({ value: x.batchId, label: `${x.name} · ${x.subject}` })) : referenceData.batches || []).map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Month<input type="month" value={register.month} max={new Date().toISOString().slice(0, 7)} onChange={(e) => setRegister({ ...register, month: e.target.value, slotId: '', session: '' })} /></label><label>Attendance slot<select value={register.slotId} onChange={(e) => { const slot = slots.find((item) => item.id === e.target.value); setRegister({ ...register, slotId: e.target.value, date: slot?.date || register.date, session: slot?.session || '' }) }}><option value="">Select active slot</option>{slots.map((slot) => <option key={slot.id} value={slot.id} disabled={slot.disabled}>{slot.label}{slot.submitted ? ' · submitted' : ''}</option>)}</select></label></div><div className="attendance-flow-steps"><span className={register.subjectId ? 'done' : ''}>1 Subject</span><span className={register.batchId ? 'done' : ''}>2 Batch</span><span className={register.slotId ? 'done' : ''}>3 Slot</span><span className={roster.length ? 'done' : ''}>4 Roster</span></div><div className="modal-actions"><button className="btn btn-secondary" onClick={loadRoster} disabled={!register.slotId}>Load students</button><button className="btn btn-primary" onClick={submitAttendance} disabled={!roster.length || !register.subjectId || !register.session}>Submit attendance</button></div>{roster.length > 0 && <DataTable rows={roster} columns={[{ key: 'studentId', label: 'Student ID' }, { key: 'name', label: 'Name' }, { key: 'status', label: 'Attendance', render: (value, row) => <select className={`attendance-select ${String(value).toLowerCase()}`} value={value} onChange={(e) => setRoster((items) => items.map((item) => item.id === row.id ? { ...item, status: e.target.value } : item))}><option>Present</option><option>Absent</option><option>OD</option></select> }]} />}</section>}
      <section className="attendance-grid"><article className="panel donut-panel"><h2>Overall attendance</h2><div className="donut"><ResponsiveContainer width="100%" height={220}><PieChart><Pie data={data} innerRadius={70} outerRadius={91} paddingAngle={2} dataKey="value">{data.map((entry) => <Cell key={entry.name} fill={entry.color} />)}</Pie></PieChart></ResponsiveContainer><div><strong>{overall}%</strong><span>{overall >= 75 ? 'On track' : summary.length ? 'Needs attention' : 'No records'}</span></div></div><p>{summary.length ? 'Calculated from live attendance records.' : 'Attendance will appear here once faculty submits a class register.'}</p></article><article className="panel subject-attendance"><h2>Subject breakdown</h2>{summary.length ? summary.map((x) => [x.subject, x.percentage, `${x.attendedClasses}/${x.totalClasses}`]).map((item) => <div className="progress-row" key={item[0]}><div><strong>{item[0]}</strong><span>{item[2]} classes</span></div><div className="progress-track"><i style={{ width: `${item[1]}%` }} /></div><b>{item[1]}%</b></div>) : <p className="empty-note">No attendance has been recorded for this profile yet.</p>}</article></section>
      <section className="panel"><div className="panel-heading"><div><h2>Recent class history</h2><p>Last ten recorded classes from live attendance.</p></div></div><DataTable columns={[{ key: 'date', label: 'Date' }, { key: 'subject', label: 'Subject' }, { key: user.role === 'student' ? 'session' : 'student', label: user.role === 'student' ? 'Session' : 'Student' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }]} rows={recent} />{!recent.length && <p className="empty-note">No attendance history yet.</p>}</section>
    </>
  )
}

function AssignmentsPage() {
  const { user } = useAuth()
  const [items, setItems] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [subjectsScoped, setSubjectsScoped] = useState([])
  const [batchesScoped, setBatchesScoped] = useState([])
  const [form, setForm] = useState({ title: '', description: '', subjectId: '', batchId: '', staffId: '', dueDate: '', maximumMarks: 100, rubric: '', referenceLink: '', attachmentFile: null })
  const [upload, setUpload] = useState({ assignmentId: '', file: null, notes: '' })
  const [submissions, setSubmissions] = useState({ assignment: null, rows: [] })
  const [message, setMessage] = useState('')
  const loadAssignments = () => api('/assignments').then((payload) => { setItems(payload); setMessage('') }).catch((err) => { setItems([]); setMessage(err.message) })
  useEffect(() => {
    loadAssignments()
    api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {})
    if (user.role === 'faculty') api('/faculty/subjects').then(setSubjectsScoped).catch(() => setSubjectsScoped([]))
  }, [user.role])
  useEffect(() => {
    if (user.role !== 'faculty') return
    const suffix = form.subjectId ? `?subjectId=${form.subjectId}` : ''
    api(`/faculty/batches${suffix}`).then(setBatchesScoped).catch(() => setBatchesScoped([]))
  }, [user.role, form.subjectId])
  const assignmentSubjects = user.role === 'faculty' ? subjectsScoped.map((x) => ({ value: x.subjectId, label: `${x.id} - ${x.name}` })) : referenceData.subjects || []
  const assignmentBatches = user.role === 'faculty' ? batchesScoped.map((x) => ({ value: x.batchId, label: `${x.name} · ${x.subject}` })) : referenceData.batches || []
  const selectedAssignment = items.find((item) => String(item.id) === String(upload.assignmentId))
  const createAssignment = async (event) => {
    event.preventDefault()
    const description = [
      form.description?.trim(),
      form.referenceLink ? `Reference link: ${form.referenceLink}` : '',
      form.attachmentFile ? `Attachment: ${form.attachmentFile.name}` : '',
    ].filter(Boolean).join('\n')
    try {
      await api('/assignments', { method: 'POST', body: JSON.stringify({ ...form, attachmentFile: undefined, description, subjectId: Number(form.subjectId), batchId: Number(form.batchId), staffId: Number(form.staffId || 0), maximumMarks: Number(form.maximumMarks), dueDate: form.dueDate }) })
      if (form.attachmentFile) {
        const resourceData = new FormData()
        resourceData.append('title', `Assignment material - ${form.title}`)
        resourceData.append('description', form.description || 'Faculty-uploaded assignment material.')
        resourceData.append('resourceType', 'Assignment')
        resourceData.append('subjectId', form.subjectId)
        resourceData.append('batchId', form.batchId)
        resourceData.append('file', form.attachmentFile)
        await api('/resources', { method: 'POST', body: resourceData })
      }
      setMessage(form.attachmentFile ? 'Assignment created and attachment uploaded to resources.' : 'Assignment created.')
      setForm({ title: '', description: '', subjectId: '', batchId: '', staffId: '', dueDate: '', maximumMarks: 100, rubric: '', referenceLink: '', attachmentFile: null })
      await loadAssignments()
    } catch (err) {
      setMessage(err.message)
    }
  }
  const submitAssignment = async (event) => { event.preventDefault(); const data = new FormData(); data.append('file', upload.file); data.append('notes', upload.notes); try { await api(`/assignments/${upload.assignmentId}/submit`, { method: 'POST', body: data }); setMessage('Submission uploaded.'); setUpload({ assignmentId: '', file: null, notes: '' }); await loadAssignments() } catch (err) { setMessage(err.message) } }
  const viewSubmissions = async (assignment) => {
    try {
      const rows = await api(`/assignments/${assignment.id}/submissions`)
      setSubmissions({ assignment, rows })
    } catch (err) { setMessage(err.message) }
  }
  return (
    <>
      <PageHeader eyebrow="Coursework" title="Assignments" description={user.role === 'faculty' ? 'Create coursework and review student submissions.' : 'Manage deadlines and submit your coursework.'}>{user.role === 'faculty' && <button className="btn btn-primary"><Plus size={17} /> Create assignment</button>}</PageHeader>
      {message && <p className="form-error">{message}</p>}
      {user.role === 'faculty' && <section className="panel form-panel"><div className="panel-heading"><div><h2>Create assignment</h2><p>Subject and batch options are scoped to your allocations.</p></div></div><form className="form-grid" onSubmit={createAssignment}><label>Title<input required placeholder="Example: AVL Tree implementation" value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} /></label><label>Subject<select required value={form.subjectId} onChange={(e) => setForm({ ...form, subjectId: e.target.value, batchId: '' })}><option value="">Select subject</option>{assignmentSubjects.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Batch<select required value={form.batchId} onChange={(e) => setForm({ ...form, batchId: e.target.value })}><option value="">Select batch</option>{assignmentBatches.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Due date<input type="datetime-local" required value={form.dueDate} onChange={(e) => setForm({ ...form, dueDate: e.target.value })} /></label><label>Maximum marks<input type="number" min="1" value={form.maximumMarks} onChange={(e) => setForm({ ...form, maximumMarks: e.target.value })} /></label><label>Description<input placeholder="Briefly explain the task and expected submission" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} /></label><label>Rubric<input placeholder="Evaluation criteria or marking notes" value={form.rubric} onChange={(e) => setForm({ ...form, rubric: e.target.value })} /></label><label>Reference link<input type="url" placeholder="https://example.com/reference-material" value={form.referenceLink} onChange={(e) => setForm({ ...form, referenceLink: e.target.value })} /></label><label>Attachment<input type="file" onChange={(e) => setForm({ ...form, attachmentFile: e.target.files?.[0] || null })} /><span className="field-hint">{form.attachmentFile ? `${form.attachmentFile.name} will be uploaded to resources.` : 'Optional file for students to access from resources.'}</span></label><div className="form-actions"><button className="btn btn-primary">Save assignment</button></div></form></section>}
      {user.role === 'student' && <section className="panel submission-panel"><div className="panel-heading"><div><h2>Submit coursework</h2><p>Choose an active assignment, attach your file and add an optional note for the faculty.</p></div>{selectedAssignment && <StatusPill>{selectedAssignment.status}</StatusPill>}</div><form className="form-grid" onSubmit={submitAssignment}><label>Assignment<select required value={upload.assignmentId} onChange={(e) => setUpload({ ...upload, assignmentId: e.target.value })}><option value="">Select assignment</option>{items.map((x) => <option key={x.id} value={x.id}>{x.title}</option>)}</select></label><label>Submission file<input required type="file" onChange={(e) => setUpload({ ...upload, file: e.target.files[0] })} /><span className="field-hint">Upload PDF, document, image or archive as instructed.</span></label><label className="full-span">Notes<input placeholder="Optional comments about your submission" value={upload.notes} onChange={(e) => setUpload({ ...upload, notes: e.target.value })} /></label><div className="form-actions"><button className="btn btn-primary" disabled={!upload.assignmentId || !upload.file}><Upload size={16} /> Upload submission</button></div></form></section>}
      {items.length ? <div className="assignment-grid">{items.map((item) => <article className="panel assignment-card" key={item.id || item.title}><div className="assignment-top"><span><FileText size={19} /></span><StatusPill>{item.status}</StatusPill></div><small>{item.subject} · {item.batch}</small><h3>{item.title}</h3><p className="assignment-due"><Clock3 size={14} /> Due {item.due ? new Date(item.due).toLocaleDateString() : ''}</p><p className="assignment-description">{item.description || 'No additional instructions were added.'}</p><div className="assignment-card-bottom"><div className="progress-track"><i style={{ width: `${item.progress || 0}%` }} /></div><div className="assignment-foot"><span>{user.role === 'faculty' ? `${item.submissions || 0} submissions · ${item.progress || 0}%` : item.status}</span><button className="btn btn-secondary" onClick={() => user.role === 'faculty' ? viewSubmissions(item) : setUpload((current) => ({ ...current, assignmentId: String(item.id) }))}>{user.role === 'faculty' ? 'View submissions' : item.status === 'Submitted' ? 'Resubmit' : 'Submit'} </button></div></div></article>)}</div> : <section className="panel empty-state"><h2>No assignments yet</h2><p>{user.role === 'faculty' ? 'Create the first assignment for an allocated batch.' : 'Assignments created for your batch will appear here.'}</p></section>}
      {submissions.assignment && <Modal title={`Submissions · ${submissions.assignment.title}`} onClose={() => setSubmissions({ assignment: null, rows: [] })}><DataTable rows={submissions.rows} columns={[{ key: 'studentId', label: 'Student ID' }, { key: 'student', label: 'Student' }, { key: 'fileName', label: 'File' }, { key: 'notes', label: 'Notes' }, { key: 'submittedAt', label: 'Submitted' }]} />{!submissions.rows.length && <p className="empty-note">No submissions yet.</p>}</Modal>}
    </>
  )
}

function ProfilePage() {
  const { user } = useAuth()
  const storageKey = `ums-profile-${user.username || user.name || user.role}`
  const defaultProfile = {
    name: user.name,
    email: `${user.name.toLowerCase().replaceAll(' ', '.')}@amrita.edu`,
    department: 'Computer Science & Engineering',
    campus: 'Coimbatore Campus',
    academicYear: '2025-2026',
    bloodGroup: 'O+',
  }
  const readProfile = () => {
    try {
      return { ...defaultProfile, ...(JSON.parse(localStorage.getItem(storageKey) || 'null') || {}) }
    } catch {
      return defaultProfile
    }
  }
  const [profile, setProfile] = useState(readProfile)
  const [editing, setEditing] = useState(false)
  const [draft, setDraft] = useState(profile)
  const saveProfile = (event) => {
    event.preventDefault()
    setProfile(draft)
    localStorage.setItem(storageKey, JSON.stringify(draft))
    setEditing(false)
  }
  return (
    <>
      <PageHeader eyebrow="Account" title="My Profile" description="Manage your university profile and contact information." />
      <section className="profile-grid"><article className="panel profile-summary"><div className="profile-avatar">{user.initials}</div><h2>{profile.name}</h2><p>{user.title}</p><StatusPill>Active</StatusPill><div className="profile-meta"><span><Mail size={16} /> {profile.email}</span><span><MapPin size={16} /> {profile.campus}</span></div></article><article className="panel profile-details"><div className="panel-heading"><div><h2>Personal information</h2><p>Official university record</p></div><button className="btn btn-secondary" onClick={() => { setDraft(profile); setEditing(true) }}>Edit profile</button></div><div className="detail-grid"><div><span>Full name</span><strong>{profile.name}</strong></div><div><span>University role</span><strong>{user.role}</strong></div><div><span>Department</span><strong>{profile.department}</strong></div><div><span>Campus</span><strong>{profile.campus}</strong></div><div><span>Academic year</span><strong>{profile.academicYear}</strong></div><div><span>Blood group</span><strong>{profile.bloodGroup}</strong></div></div></article></section>
      {editing && <Modal title="Edit profile" onClose={() => setEditing(false)}><form className="form-grid" onSubmit={saveProfile}><label>Full name<input required placeholder="Enter your display name" value={draft.name} onChange={(e) => setDraft({ ...draft, name: e.target.value })} /></label><label>Email<input required type="email" placeholder="name@amrita.edu" value={draft.email} onChange={(e) => setDraft({ ...draft, email: e.target.value })} /></label><label>Department<input placeholder="Example: Computer Science & Engineering" value={draft.department} onChange={(e) => setDraft({ ...draft, department: e.target.value })} /></label><label>Campus<input placeholder="Example: Coimbatore Campus" value={draft.campus} onChange={(e) => setDraft({ ...draft, campus: e.target.value })} /></label><label>Academic year<input placeholder="Example: 2025-2026" value={draft.academicYear} onChange={(e) => setDraft({ ...draft, academicYear: e.target.value })} /></label><label>Blood group<input placeholder="Example: O+" value={draft.bloodGroup} onChange={(e) => setDraft({ ...draft, bloodGroup: e.target.value })} /></label><div className="modal-actions"><button type="button" className="btn btn-secondary" onClick={() => setEditing(false)}>Cancel</button><button className="btn btn-primary">Save profile</button></div></form></Modal>}
    </>
  )
}

function ResultsPage() {
  const [result, setResult] = useState({ sgpa: 0, cgpa: 0, grades: [] })
  useEffect(() => { api('/results').then(setResult).catch(() => {}) }, [])
  return <><PageHeader eyebrow="Academic record" title="Semester Results" description="Published results and cumulative academic performance."><button className="btn btn-secondary"><Download size={17} /> Grade card</button></PageHeader><section className="result-hero"><div><span>Semester SGPA</span><strong>{result.sgpa || 0}</strong><small>Published marks only</small></div><div><span>Cumulative CGPA</span><strong>{result.cgpa || 0}</strong><small>{result.grades?.reduce((sum, item) => sum + item.credit, 0) || 0} credits counted</small></div><Award size={92} /></section><section className="panel">{result.grades?.length ? <DataTable rows={(result.grades || []).map((g, i) => ({ ...g, id: i }))} columns={[{ key: 'code', label: 'Course code' }, { key: 'course', label: 'Course title' }, { key: 'credit', label: 'Credits' }, { key: 'grade', label: 'Grade', render: (v) => <span className="grade">{v}</span> }]} /> : <div className="empty-state compact"><h2>No published results yet</h2><p>Published marks and calculated grades will appear here once faculty releases them.</p></div>}</section></>
}

function MarksPage() {
  const { user } = useAuth()
  const [rows, setRows] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [subjectsScoped, setSubjectsScoped] = useState([])
  const [batchesScoped, setBatchesScoped] = useState([])
  const [studentsScoped, setStudentsScoped] = useState([])
  const [form, setForm] = useState({ studentId: '', subjectId: '', batchId: '', assignmentId: '', marksObtained: '', maximumMarks: 100, status: 'Draft' })
  const [message, setMessage] = useState('')
  const load = () => api('/marks').then(setRows).catch((err) => setMessage(err.message))
  useEffect(() => {
    load()
    api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {})
    if (user.role === 'faculty') api('/faculty/subjects').then(setSubjectsScoped).catch(() => setSubjectsScoped([]))
  }, [user.role])
  useEffect(() => {
    if (user.role !== 'faculty') return
    const suffix = form.subjectId ? `?subjectId=${form.subjectId}` : ''
    api(`/faculty/batches${suffix}`).then(setBatchesScoped).catch(() => setBatchesScoped([]))
  }, [user.role, form.subjectId])
  useEffect(() => {
    if (user.role !== 'faculty' || !form.subjectId || !form.batchId) { setStudentsScoped([]); return }
    api(`/faculty/students?subjectId=${form.subjectId}&batchId=${form.batchId}`).then(setStudentsScoped).catch(() => setStudentsScoped([]))
  }, [user.role, form.subjectId, form.batchId])
  const markSubjects = user.role === 'faculty' ? subjectsScoped.map((x) => ({ value: x.subjectId, label: `${x.id} - ${x.name}` })) : referenceData.subjects || []
  const markBatches = batchesScoped.map((x) => ({ value: x.batchId, label: `${x.name} · ${x.subject}` }))
  const markStudents = user.role === 'faculty' ? studentsScoped.map((x) => ({ value: x.id, label: `${x.studentId} - ${x.name}` })) : referenceData.students || []
  const submit = async (event) => {
    event.preventDefault()
    try {
      await api('/marks', { method: 'POST', body: JSON.stringify({ ...form, studentId: Number(form.studentId), subjectId: Number(form.subjectId), assignmentId: form.assignmentId ? Number(form.assignmentId) : null, marksObtained: Number(form.marksObtained), maximumMarks: Number(form.maximumMarks) }) })
      setMessage('Marks saved.')
      await load()
    } catch (err) { setMessage(err.message) }
  }
  return <><PageHeader eyebrow="Evaluation" title="Marks & Evaluation" description="Record and publish student marks." />{message && <p className="form-error">{message}</p>}<section className="panel"><form className="form-grid" onSubmit={submit}><label>Subject<select required value={form.subjectId} onChange={(e) => setForm({ ...form, subjectId: e.target.value, batchId: '', studentId: '' })}><option value="">Select subject</option>{markSubjects.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label>{user.role === 'faculty' && <label>Batch<select required value={form.batchId} onChange={(e) => setForm({ ...form, batchId: e.target.value, studentId: '' })}><option value="">Select batch</option>{markBatches.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label>}<label>Student<select required value={form.studentId} onChange={(e) => setForm({ ...form, studentId: e.target.value })}><option value="">Select student</option>{markStudents.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Assignment<select value={form.assignmentId} onChange={(e) => setForm({ ...form, assignmentId: e.target.value })}><option value="">Optional</option>{(referenceData.assignments || []).map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Marks<input required type="number" value={form.marksObtained} onChange={(e) => setForm({ ...form, marksObtained: e.target.value })} /></label><label>Maximum<input type="number" value={form.maximumMarks} onChange={(e) => setForm({ ...form, maximumMarks: e.target.value })} /></label><label>Status<select value={form.status} onChange={(e) => setForm({ ...form, status: e.target.value })}><option>Draft</option><option>Published</option></select></label><div className="modal-actions"><button className="btn btn-primary">Save marks</button></div></form></section><section className="panel"><DataTable rows={rows} columns={[{ key: 'student', label: 'Student' }, { key: 'subject', label: 'Subject' }, { key: 'marksObtained', label: 'Marks' }, { key: 'maximumMarks', label: 'Max' }, { key: 'status', label: 'Status', render: (v) => <StatusPill>{v}</StatusPill> }]} />{!rows.length && <p className="empty-note">No marks are available yet.</p>}</section></>
}

function NotificationsPage() {
  const [items, setItems] = useState([])
  const load = () => api('/notifications').then(setItems).catch(() => setItems([]))
  useEffect(() => { load() }, [])
  const markRead = async (item) => { await api(`/notifications/${item.id}/read`, { method: 'POST' }); await load() }
  return <><PageHeader eyebrow="University portal" title="Notifications" description="Important notices, announcements and campus events." /><div className="resource-grid">{items.map((item) => <article className="panel resource-card" key={item.id}><span className="resource-icon r0"><CalendarDays size={22}/></span><small>{item.isRead ? 'READ' : 'NEW'}</small><h3>{item.title}</h3><p>{item.message}</p><button className="text-button" onClick={() => markRead(item)}>{item.isRead ? 'Read' : 'Mark read'} →</button></article>)}</div></>
}

function ResourcesPage() {
  const { user } = useAuth()
  const [items, setItems] = useState([])
  const [subjects, setSubjects] = useState([])
  const [batches, setBatches] = useState([])
  const [referenceData, setReferenceData] = useState({})
  const [form, setForm] = useState({ title: '', description: '', resourceType: 'Notes', subjectId: '', batchId: '', file: null })
  const [message, setMessage] = useState('')
  const canUpload = user.role === 'faculty' || user.role === 'admin'
  const load = () => api('/resources').then((payload) => { setItems(payload); setMessage('') }).catch((err) => setMessage(err.message))
  useEffect(() => {
    load()
    if (user.role === 'faculty') api('/faculty/subjects').then(setSubjects).catch(() => setSubjects([]))
    if (user.role === 'admin') api('/reference-data').then((payload) => setReferenceData(buildReferenceOptions(payload))).catch(() => {})
  }, [user.role])
  useEffect(() => {
    if (user.role !== 'faculty') return
    const suffix = form.subjectId ? `?subjectId=${form.subjectId}` : ''
    api(`/faculty/batches${suffix}`).then(setBatches).catch(() => setBatches([]))
  }, [user.role, form.subjectId])
  const subjectOptions = user.role === 'faculty' ? subjects.map((x) => ({ value: x.subjectId, label: `${x.id} - ${x.name}` })) : referenceData.subjects || []
  const batchOptions = user.role === 'faculty' ? batches.map((x) => ({ value: x.batchId, label: `${x.name} · ${x.subject}` })) : referenceData.batches || []
  const uploadResource = async (event) => {
    event.preventDefault()
    const data = new FormData()
    ;['title', 'description', 'resourceType', 'subjectId', 'batchId'].forEach((key) => data.append(key, form[key] || ''))
    if (form.file) data.append('file', form.file)
    try {
      await api('/resources', { method: 'POST', body: data })
      setForm({ title: '', description: '', resourceType: 'Notes', subjectId: '', batchId: '', file: null })
      setMessage('Resource published for the selected class.')
      await load()
    } catch (err) { setMessage(err.message) }
  }
  const openResource = async (item) => {
    try {
      const response = await fetch(`${API_BASE_URL}/resources/${item.id}/download`, { headers: { Authorization: `Bearer ${getToken()}` } })
      const contentType = response.headers.get('content-type') || ''
      if (!response.ok) {
        const payload = contentType.includes('application/json') ? await response.json() : null
        throw new Error(payload?.message || `Unable to open resource (${response.status}).`)
      }
      const blob = await response.blob()
      const url = URL.createObjectURL(blob)
      const link = document.createElement('a')
      link.href = url
      link.download = item.fileName || item.title
      document.body.appendChild(link)
      link.click()
      link.remove()
      URL.revokeObjectURL(url)
    } catch (err) { setMessage(err.message) }
  }
  return (
    <>
      <PageHeader eyebrow="Course resources" title="Study Materials" description={user.role === 'student' ? 'Resources shared for your subjects and batch.' : 'Upload resources against allocated subjects and batches.'}>{canUpload && <button className="btn btn-primary"><Upload size={17}/> Upload material</button>}</PageHeader>
      {message && <p className="form-error">{message}</p>}
      {canUpload && <section className="panel form-panel"><div className="panel-heading"><div><h2>Publish resource</h2><p>Subject and batch options are scoped from master allocation data.</p></div></div><form className="form-grid" onSubmit={uploadResource}><label>Title<input required value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} /></label><label>Type<select value={form.resourceType} onChange={(e) => setForm({ ...form, resourceType: e.target.value })}><option>Notes</option><option>Slides</option><option>Reference</option><option>Lab</option><option>Reading</option></select></label><label>Subject<select required value={form.subjectId} onChange={(e) => setForm({ ...form, subjectId: e.target.value, batchId: '' })}><option value="">Select subject</option>{subjectOptions.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Batch<select value={form.batchId} onChange={(e) => setForm({ ...form, batchId: e.target.value })}><option value="">All assigned batches</option>{batchOptions.map((x) => <option key={x.value} value={x.value}>{x.label}</option>)}</select></label><label>Description<input value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} /></label><label>File<input type="file" onChange={(e) => setForm({ ...form, file: e.target.files[0] })} /></label><div className="form-actions"><button className="btn btn-primary">Publish resource</button></div></form></section>}
      {items.length ? <div className="resource-grid">{items.map((item, index) => <article className="panel resource-card" key={item.id}><span className={`resource-icon r${index % 3}`}><FolderUp size={22}/></span><small>{item.resourceType} · {item.subject}</small><h3>{item.title}</h3><p>{item.description || 'Shared course material'}</p><p>{item.batch} · {item.faculty}</p><button className="text-button" onClick={() => openResource(item)}>{item.fileName || 'Open resource'} →</button></article>)}</div> : <section className="panel empty-state"><h2>No resources yet</h2><p>Faculty uploads will appear here for the relevant students.</p></section>}
    </>
  )
}

function FeedbackPage() {
  const { user } = useAuth()
  const [options, setOptions] = useState([])
  const [rows, setRows] = useState([])
  const [form, setForm] = useState({ option: '', rating: 5, comments: '' })
  const [message, setMessage] = useState('')
  const load = () => user.role === 'student'
    ? api('/feedback/options').then(setOptions).catch((err) => setMessage(err.message))
    : api('/feedback').then(setRows).catch((err) => setMessage(err.message))
  useEffect(() => { load() }, [user.role])
  const submit = async (event) => {
    event.preventDefault()
    const selected = options.find((item) => `${item.staffId}-${item.subjectId}-${item.batchId}-${item.semester}` === form.option)
    if (!selected) return
    try {
      await api('/feedback', { method: 'POST', body: JSON.stringify({ staffId: selected.staffId, subjectId: selected.subjectId, batchId: selected.batchId, rating: Number(form.rating), comments: form.comments, academicYear: selected.academicYear, semester: selected.semester }) })
      setMessage('Anonymous feedback saved.')
      setForm({ option: '', rating: 5, comments: '' })
    } catch (err) { setMessage(err.message) }
  }
  if (user.role === 'student') {
    return <><PageHeader eyebrow="Anonymous feedback" title="Faculty Feedback" description="Share semester feedback only for professors teaching your batch." />{message && <p className="form-error">{message}</p>}<section className="panel form-panel"><div className="panel-heading"><div><h2>Submit feedback</h2><p>Your identity is used only to prevent duplicate submissions; faculty sees aggregated anonymous feedback.</p></div></div><form className="form-grid" onSubmit={submit}><label>Professor and subject<select required value={form.option} onChange={(e) => setForm({ ...form, option: e.target.value })}><option value="">Select professor</option>{options.map((x) => <option key={`${x.staffId}-${x.subjectId}-${x.batchId}-${x.semester}`} value={`${x.staffId}-${x.subjectId}-${x.batchId}-${x.semester}`}>{x.faculty} · {x.subject} · Semester {x.semester}</option>)}</select></label><label>Rating<select value={form.rating} onChange={(e) => setForm({ ...form, rating: e.target.value })}><option value="5">5 - Excellent</option><option value="4">4 - Good</option><option value="3">3 - Satisfactory</option><option value="2">2 - Needs improvement</option><option value="1">1 - Poor</option></select></label><label className="full-span">Comments<input required value={form.comments} onChange={(e) => setForm({ ...form, comments: e.target.value })} placeholder="Course delivery, clarity, mentoring, feedback..." /></label><div className="form-actions"><button className="btn btn-primary">Submit anonymous feedback</button></div></form></section></>
  }
  return <><PageHeader eyebrow="Anonymous feedback" title="Faculty Feedback" description={user.role === 'faculty' ? 'Aggregated anonymous feedback for your allocated subjects.' : 'Institution-wide anonymous feedback summary.'} />{message && <p className="form-error">{message}</p>}<section className="panel data-panel"><DataTable rows={rows} columns={[{ key: 'faculty', label: 'Faculty' }, { key: 'subject', label: 'Subject' }, { key: 'batch', label: 'Batch' }, { key: 'semester', label: 'Semester' }, { key: 'responses', label: 'Responses' }, { key: 'averageRating', label: 'Avg rating' }, { key: 'comments', label: 'Recent comments', render: (v) => (v || []).join(' | ') }]} /></section></>
}

function AnalyticsPage() {
  const reports = [
    ['student-list', 'Student list'],
    ['faculty-list', 'Faculty list'],
    ['course-list', 'Course list'],
    ['attendance-summary', 'Attendance summary'],
    ['student-attendance-detail', 'Student attendance detail'],
    ['marks-summary', 'Marks summary'],
    ['grade-result-report', 'Grade result report'],
    ['arrear-failure-summary', 'Arrear / failure summary'],
    ['evaluation-components', 'Evaluation components'],
    ['grade-rules', 'Grade rules'],
    ['grace-mark-summary', 'Grace mark summary'],
    ['survey-submission-summary', 'Survey submission summary'],
  ]
  const [selected, setSelected] = useState('student-list')
  const [rows, setRows] = useState([])
  const [error, setError] = useState('')
  const [searchTerm, setSearchTerm] = useState('')
  const [filtersOpen, setFiltersOpen] = useState(false)
  useEffect(() => {
    api(`/reports/${selected}`).then((payload) => { setRows(unwrapList(payload)); setError('') }).catch((err) => { setRows([]); setError(err.message) })
  }, [selected])
  const visibleRows = filterRows(rows, searchTerm)
  const columns = rows.length ? Object.keys(rows[0]).slice(0, 8).map((key) => ({ key, label: key.replace(/([A-Z])/g, ' $1').replace(/^./, (char) => char.toUpperCase()) })) : [{ key: 'message', label: 'Message' }]
  const departmentData = visibleRows.filter((row) => row.department).reduce((items, row) => {
    const match = items.find((item) => item.name === row.department)
    if (match) match.records += 1
    else items.push({ name: row.department, records: 1 })
    return items
  }, [])
  return <><PageHeader eyebrow="Reports" title="University Analytics" description="Operational reports generated from current university records."><button className="btn btn-secondary" onClick={() => exportRows(selected, visibleRows)}><Download size={17} /> Export report</button></PageHeader>{error && <p className="form-error">{error}</p>}<section className="metrics-grid compact"><div className="panel mini-stat"><span>Selected report</span><strong>{reports.find((report) => report[0] === selected)?.[1]}</strong><small>{visibleRows.length} of {rows.length} rows visible</small></div><div className="panel mini-stat"><span>Available reports</span><strong>{reports.length}</strong><small>Admin reports</small></div></section><section className="panel data-panel"><Toolbar search="Search report rows…" searchValue={searchTerm} onSearch={setSearchTerm} filterActive={filtersOpen} onFilter={() => setFiltersOpen((value) => !value)} onExport={() => exportRows(selected, visibleRows)}><select value={selected} onChange={(event) => { setSelected(event.target.value); setSearchTerm('') }}>{reports.map(([value, label]) => <option key={value} value={value}>{label}</option>)}</select></Toolbar>{filtersOpen && <div className="filter-strip"><span>Showing report rows matching the search text.</span><button className="text-button" onClick={() => { setSearchTerm(''); setFiltersOpen(false) }}>Clear filter</button></div>}{visibleRows.length ? <DataTable rows={visibleRows.map((row, index) => ({ id: row.id || `${selected}-${index}`, ...row }))} columns={columns} /> : <p className="empty-note">No rows are available for this report yet.</p>}</section>{departmentData.length > 0 && <section className="panel chart-panel"><div className="panel-heading"><div><h2>Records by department</h2><p>Based on the currently selected report.</p></div></div><ResponsiveContainer width="100%" height={300}><BarChart data={departmentData}><CartesianGrid stroke="#eee9e7" vertical={false}/><XAxis dataKey="name" axisLine={false} tickLine={false}/><YAxis axisLine={false} tickLine={false}/><Tooltip/><Bar dataKey="records" fill="#8d153a" radius={[7,7,0,0]} /></BarChart></ResponsiveContainer></section>}</>
}

function GenericCardsPage({ page }) {
  const info = titleMap[page] || [page, 'University portal module']
  const cards = page === 'notifications' ? [['Semester examination timetable', 'Academic Office · 12 minutes ago'], ['Campus recruitment drive', 'Career Services · 2 hours ago'], ['Library timing update', 'Central Library · Yesterday']] : [['Course overview', 'Core concepts, syllabus and learning outcomes'], ['Lecture resources', 'Notes, presentations and reference material'], ['Academic activity', 'Upcoming sessions and important milestones']]
  return <><PageHeader eyebrow="University portal" title={info[0]} description={info[1]}>{page === 'materials' && <button className="btn btn-primary"><Upload size={17}/> Upload material</button>}</PageHeader><div className="resource-grid">{cards.map((card, i) => <article className="panel resource-card" key={card[0]}><span className={`resource-icon r${i}`}>{page === 'notifications' ? <CalendarDays size={22}/> : page === 'materials' ? <FolderUp size={22}/> : <BookOpen size={22}/>}</span><small>{page === 'notifications' ? 'NOTICE' : 'ACADEMIC RESOURCE'}</small><h3>{card[0]}</h3><p>{card[1]}</p><button className="text-button">View details →</button></article>)}</div></>
}

export default function PortalPage() {
  const { user } = useAuth()
  const { page } = useParams()
  const view = useMemo(() => page?.toLowerCase(), [page])
  if (view === 'dashboard') return <Dashboard />
  if (user.role === 'faculty' && view === 'subjects') return <FacultySubjectsPage />
  if (user.role === 'faculty' && view === 'students') return <FacultyStudentsPage />
  if (['staff', 'students', 'courses', 'batches', 'subjects', 'department-masters', 'designation-masters', 'program-masters', 'academic-sessions', 'role-masters', 'users'].includes(view)) return <RecordsPage page={view} />
  if (view === 'subject-allocations') return <SubjectAllocationPage />
  if (['evaluation-components', 'grade-rules', 'grace-mark-categories', 'grace-mark-applications'].includes(view)) return <AdvancedRecordsPage page={view} />
  if (view === 'surveys') return <FeedbackPage />
  if (view === 'timetable') return <TimetablePage />
  if (view === 'calendar') return <CalendarPage />
  if (view === 'examination') return <ExamPage />
  if (view === 'materials' || view === 'resources') return <ResourcesPage />
  if (['library', 'books'].includes(view)) return <LibraryPage title={titleMap[view][0]} />
  if (view === 'attendance') return <AttendancePage />
  if (view === 'assignments') return <AssignmentsPage />
  if (view === 'marks') return <MarksPage />
  if (view === 'profile') return <ProfilePage />
  if (view === 'results') return <ResultsPage />
  if (view === 'analytics') return <AnalyticsPage />
  if (view === 'notifications') return <NotificationsPage />
  return <GenericCardsPage page={view} />
}
