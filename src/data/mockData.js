export const faculty = [
  { id: 'AVV-F1024', name: 'Dr. Arjun Menon', department: 'Computer Science', detail: 'Associate Professor', email: 'arjun.m@amrita.edu', status: 'Active' },
  { id: 'AVV-F1038', name: 'Dr. Lakshmi Iyer', department: 'Electronics', detail: 'Professor', email: 'lakshmi.i@amrita.edu', status: 'Active' },
  { id: 'AVV-F1051', name: 'Prof. Rohan Pillai', department: 'Mathematics', detail: 'Assistant Professor', email: 'rohan.p@amrita.edu', status: 'On leave' },
  { id: 'AVV-F1072', name: 'Dr. Nisha Varma', department: 'Humanities', detail: 'Professor', email: 'nisha.v@amrita.edu', status: 'Active' },
  { id: 'AVV-F1093', name: 'Dr. Vivek Rao', department: 'Mechanical', detail: 'Associate Professor', email: 'vivek.r@amrita.edu', status: 'Active' },
]

export const students = [
  { id: 'CB.EN.U4CSE22101', name: 'Ananya Rao', course: 'B.Tech CSE', batch: '2022–26', detail: 'Semester 6', status: 'Active' },
  { id: 'CB.EN.U4CSE22118', name: 'Ishaan Nair', course: 'B.Tech CSE', batch: '2022–26', detail: 'Semester 6', status: 'Active' },
  { id: 'CB.EN.U4ECE22206', name: 'Diya Menon', course: 'B.Tech ECE', batch: '2022–26', detail: 'Semester 6', status: 'Active' },
  { id: 'CB.EN.U4MEC22311', name: 'Kiran Das', course: 'B.Tech ME', batch: '2022–26', detail: 'Semester 6', status: 'Mentoring' },
  { id: 'CB.EN.U4CSE23142', name: 'Naina Singh', course: 'B.Tech CSE', batch: '2023–27', detail: 'Semester 4', status: 'Active' },
]

export const courses = [
  { id: 'CSE-BT', name: 'B.Tech Computer Science', department: 'Computer Science', detail: '4 years · 8 semesters', status: 'Active' },
  { id: 'ECE-BT', name: 'B.Tech Electronics & Communication', department: 'Electronics', detail: '4 years · 8 semesters', status: 'Active' },
  { id: 'MBA-PG', name: 'Master of Business Administration', department: 'Management', detail: '2 years · 4 semesters', status: 'Active' },
  { id: 'DS-MS', name: 'M.Tech Data Science', department: 'Computer Science', detail: '2 years · 4 semesters', status: 'Active' },
]

export const subjects = [
  { id: '19CSE301', name: 'Data Structures & Algorithms', department: 'Computer Science', detail: '4 credits', status: 'Dr. Arjun Menon' },
  { id: '19CSE312', name: 'Database Management Systems', department: 'Computer Science', detail: '4 credits', status: 'Dr. Lakshmi Iyer' },
  { id: '19MAT310', name: 'Probability & Statistics', department: 'Mathematics', detail: '3 credits', status: 'Prof. Rohan Pillai' },
  { id: '19CSE315', name: 'Computer Networks', department: 'Computer Science', detail: '3 credits', status: 'Dr. Nisha Varma' },
]

export const batches = [
  { id: 'CSE-2022-A', name: 'CSE 2022 · Section A', department: 'B.Tech CSE', detail: '2022–26', status: '64 students' },
  { id: 'CSE-2022-B', name: 'CSE 2022 · Section B', department: 'B.Tech CSE', detail: '2022–26', status: '61 students' },
  { id: 'ECE-2023-A', name: 'ECE 2023 · Section A', department: 'B.Tech ECE', detail: '2023–27', status: '58 students' },
]

export const schedule = [
  { time: '09:00', mon: ['Data Structures', 'C-301'], tue: ['Computer Networks', 'C-204'], wed: ['DBMS Lab', 'Lab 4'], thu: ['Data Structures', 'C-301'], fri: ['Probability', 'A-105'] },
  { time: '10:00', mon: ['Probability', 'A-105'], tue: ['Data Structures', 'C-301'], wed: ['DBMS Lab', 'Lab 4'], thu: ['Computer Networks', 'C-204'], fri: ['DBMS', 'C-302'] },
  { time: '11:15', mon: ['DBMS', 'C-302'], tue: ['Mentoring', 'C-110'], wed: ['Computer Networks', 'C-204'], thu: ['Probability', 'A-105'], fri: ['Data Structures', 'C-301'] },
  { time: '13:30', mon: ['Design Thinking', 'Studio 2'], tue: ['Open Elective', 'E-201'], wed: ['Data Structures', 'C-301'], thu: ['DBMS', 'C-302'], fri: ['Project Work', 'Innovation Lab'] },
]

export const books = [
  { title: 'Introduction to Algorithms', author: 'Cormen, Leiserson, Rivest', category: 'Computer Science', code: 'CS-ALGO-128', available: true, color: '#8d153a' },
  { title: 'Designing Data-Intensive Applications', author: 'Martin Kleppmann', category: 'Technology', code: 'CS-DATA-204', available: true, color: '#274c77' },
  { title: 'The Design of Everyday Things', author: 'Don Norman', category: 'Design', code: 'DS-HCI-082', available: false, color: '#8b5e34' },
  { title: 'Clean Code', author: 'Robert C. Martin', category: 'Software Engineering', code: 'CS-SE-114', available: true, color: '#455a64' },
]

export const assignments = [
  { title: 'AVL Tree Implementation', subject: 'Data Structures', due: '29 Jul, 11:59 PM', progress: 78, status: 'In progress' },
  { title: 'Normalization Case Study', subject: 'Database Systems', due: '02 Aug, 11:59 PM', progress: 42, status: 'Not submitted' },
  { title: 'Network Topology Design', subject: 'Computer Networks', due: '06 Aug, 5:00 PM', progress: 100, status: 'Submitted' },
]

export const navByRole = {
  admin: [
    { label: 'Dashboard', icon: 'LayoutDashboard', path: 'dashboard' },
    { label: 'Masters', heading: true },
    { label: 'Staff Master', icon: 'Users', path: 'masters/staff' },
    { label: 'Student Master', icon: 'GraduationCap', path: 'masters/students' },
    { label: 'Course Master', icon: 'BookOpen', path: 'masters/courses' },
    { label: 'Batch Master', icon: 'Layers3', path: 'masters/batches' },
    { label: 'Subject Master', icon: 'LibraryBig', path: 'masters/subjects' },
    { label: 'Department Master', icon: 'Building2', path: 'masters/department-masters' },
    { label: 'Designation Master', icon: 'BadgeCheck', path: 'masters/designation-masters' },
    { label: 'Program Master', icon: 'BookCopy', path: 'masters/program-masters' },
    { label: 'Academic Master', icon: 'CalendarClock', path: 'masters/academic-sessions' },
    { label: 'Role Master', icon: 'ShieldCheck', path: 'masters/role-masters' },
    { label: 'User Master', icon: 'UserCog', path: 'masters/users' },
    { label: 'Operations', heading: true },
    { label: 'Subject Allocation', icon: 'Network', path: 'operations/subject-allocations' },
    { label: 'Evaluation Setup', icon: 'ClipboardList', path: 'operations/evaluation-components' },
    { label: 'Grade Rules', icon: 'ListChecks', path: 'operations/grade-rules' },
    { label: 'Grace Categories', icon: 'BadgePlus', path: 'operations/grace-mark-categories' },
    { label: 'Grace Applications', icon: 'Award', path: 'operations/grace-mark-applications' },
    { label: 'Surveys', icon: 'ClipboardPenLine', path: 'operations/surveys' },
    { label: 'Academic', heading: true },
    { label: 'Timetable', icon: 'CalendarDays', path: 'academic/timetable' },
    { label: 'Academic Calendar', icon: 'CalendarRange', path: 'academic/calendar' },
    { label: 'Examination', icon: 'ClipboardCheck', path: 'academic/examination' },
    { label: 'Library', heading: true },
    { label: 'Resources', icon: 'Library', path: 'library/resources' },
    { label: 'Books', icon: 'BookMarked', path: 'library/books' },
    { label: 'Reports', heading: true },
    { label: 'Analytics', icon: 'ChartNoAxesCombined', path: 'reports/analytics' },
  ],
  faculty: [
    { label: 'Dashboard', icon: 'LayoutDashboard', path: 'dashboard' },
    { label: 'Profile', icon: 'UserRound', path: 'profile' },
    { label: 'My Subjects', icon: 'BookOpen', path: 'subjects' },
    { label: 'Timetable', icon: 'CalendarDays', path: 'timetable' },
    { label: 'Students', icon: 'Users', path: 'students' },
    { label: 'Attendance', icon: 'UserCheck', path: 'attendance' },
    { label: 'Assignments', icon: 'NotebookPen', path: 'assignments' },
    { label: 'Study Materials', icon: 'FolderUp', path: 'materials' },
    { label: 'Marks', icon: 'ClipboardCheck', path: 'marks' },
    { label: 'Evaluation Setup', icon: 'ClipboardList', path: 'evaluation-components' },
    { label: 'Surveys', icon: 'ClipboardPenLine', path: 'surveys' },
  ],
  student: [
    { label: 'Dashboard', icon: 'LayoutDashboard', path: 'dashboard' },
    { label: 'Profile', icon: 'UserRound', path: 'profile' },
    { label: 'Courses', icon: 'BookOpen', path: 'courses' },
    { label: 'Timetable', icon: 'CalendarDays', path: 'timetable' },
    { label: 'Attendance', icon: 'PieChart', path: 'attendance' },
    { label: 'Assignments', icon: 'NotebookPen', path: 'assignments' },
    { label: 'Study Materials', icon: 'FolderDown', path: 'materials' },
    { label: 'Results', icon: 'Trophy', path: 'results' },
    { label: 'Library', icon: 'Library', path: 'library' },
    { label: 'Notifications', icon: 'Bell', path: 'notifications' },
    { label: 'Surveys', icon: 'ClipboardPenLine', path: 'surveys' },
  ],
}
