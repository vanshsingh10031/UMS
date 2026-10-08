import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Area, AreaChart, CartesianGrid, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts'
import { BookOpen, CalendarDays, CheckCircle2, ChevronRight, Clock3, GraduationCap, Layers3, Plus, UserCheck, Users } from 'lucide-react'
import { MetricCard, PageHeader, StatusPill } from '../components/UI'
import { useAuth } from '../hooks/useAuth'
import { api } from '../api/client'

const attendanceData = [
  { name: 'Jul 1', value: 86 }, { name: 'Jul 5', value: 89 }, { name: 'Jul 9', value: 87 },
  { name: 'Jul 13', value: 91 }, { name: 'Jul 17', value: 90 }, { name: 'Jul 21', value: 94 }, { name: 'Jul 25', value: 92 },
]

const roleContent = {
  admin: {
    greeting: 'Good morning, Meera',
    description: "Here’s what’s happening across the university today.",
    metrics: [
      [GraduationCap, 'Total students', '8,642', '+6.2% this year', 'maroon'],
      [Users, 'Total faculty', '624', '+18 this term', 'blue'],
      [BookOpen, 'Active courses', '42', '8 departments', 'gold'],
      [Layers3, 'Active batches', '31', '2022–2026', 'green'],
    ],
  },
  faculty: {
    greeting: 'Good morning, Arjun',
    description: 'Your classes, students and pending academic work at a glance.',
    metrics: [
      [BookOpen, 'Assigned subjects', '0', 'From subject allocation', 'maroon'],
      [CalendarDays, "Today's classes", '0', 'Based on timetable', 'blue'],
      [Users, 'My students', '0', 'Across assigned batches', 'gold'],
      [CheckCircle2, 'Pending tasks', '0', 'Submissions to review', 'green'],
    ],
  },
  student: {
    greeting: 'Good morning, Ananya',
    description: 'Stay on top of your semester, classes and upcoming work.',
    metrics: [
      [Layers3, 'Current semester', '06', 'B.Tech CSE', 'maroon'],
      [UserCheck, 'Attendance', '92.4%', '+1.8% this month', 'green'],
      [CalendarDays, 'Upcoming exams', '03', 'First on 05 Aug', 'blue'],
      [CheckCircle2, 'Assignments', '8/10', '2 due this week', 'gold'],
    ],
  },
}

export default function Dashboard() {
  const { user } = useAuth()
  const navigate = useNavigate()
  const content = roleContent[user.role]
  const [stats, setStats] = useState(null)
  useEffect(() => { api(`/dashboard/${user.role}`).then(setStats).catch(() => setStats(null)) }, [user.role])
  const todayLabel = new Date().toLocaleDateString('en-IN', { day: '2-digit', month: 'long', year: 'numeric' })
  const scheduleRows = stats?.schedule?.length ? stats.schedule : user.role === 'faculty' ? [] : [['09:00', 'Data Structures', 'C-301', 'Completed'], ['11:15', 'Database Systems', 'C-302', 'In progress'], ['13:30', user.role === 'admin' ? 'Academic Council' : 'Design Thinking', 'Studio 2', 'Upcoming']].map(([time, title, location, status]) => ({ time, title, location, status }))
  const activityRows = stats?.recentActivity?.length ? stats.recentActivity : user.role === 'faculty' ? [] : [
    { title: 'Semester exam timetable published', owner: 'Academic Office', time: '12 min ago' },
    { title: 'Innovation challenge registrations open', owner: 'Student Affairs', time: '1 hour ago' },
    { title: 'Library hours extended during exams', owner: 'Central Library', time: 'Yesterday' },
  ]
  const metrics = content.metrics.map((metric) => {
    const label = metric[1]
    if (!stats) return metric
    const value = label === 'Total students' ? stats.totalStudents : label === 'Total faculty' ? stats.totalFaculty : label === 'Active courses' ? stats.totalCourses : label === 'Active batches' ? stats.activeBatches : label === 'Assigned subjects' ? stats.assignedSubjects : label === "Today's classes" ? stats.todaysClasses : label === 'My students' ? stats.myStudents : label === 'Pending tasks' ? stats.pendingTasks : label === 'Current semester' ? String(stats.currentSemester).padStart(2, '0') : label === 'Attendance' ? `${stats.attendancePercentage}%` : label === 'Upcoming exams' ? stats.upcomingExams : label === 'Assignments' ? stats.upcomingAssignments : metric[2]
    const delta = label === 'Assigned subjects' ? 'Open subject list' : label === "Today's classes" ? (stats.nextClass || 'View timetable') : label === 'My students' ? `${stats.assignedBatches || 0} assigned batch${stats.assignedBatches === 1 ? '' : 'es'}` : label === 'Pending tasks' ? 'Open assignments' : metric[3]
    return [metric[0], metric[1], value, delta, metric[4]]
  })
  const firstName = (stats?.faculty?.name || user.name || '').split(' ')[0] || 'there'
  const greeting = user.role === 'faculty' ? `Good morning, ${firstName}` : content.greeting
  const attendancePercent = Number(stats?.attendancePercentage || 0)
  const chartData = attendancePercent > 0 ? attendanceData.map((item, index) => ({ ...item, value: Math.max(0, Math.min(100, attendancePercent - 3 + index)) })) : []
  const metricActions = {
    'Assigned subjects': () => navigate('/faculty/subjects'),
    "Today's classes": () => navigate('/faculty/timetable'),
    'My students': () => navigate('/faculty/students'),
    'Pending tasks': () => navigate('/faculty/assignments'),
  }
  return (
    <>
      <PageHeader eyebrow="Overview" title={greeting} description={content.description}>
        <button className="btn btn-secondary"><CalendarDays size={17} /> {todayLabel}</button>
        {user.role === 'admin' && <button className="btn btn-primary"><Plus size={17} /> Quick add</button>}
        {user.role === 'faculty' && <button className="btn btn-primary" onClick={() => navigate('/faculty/attendance')}><Plus size={17} /> Mark attendance</button>}
      </PageHeader>
      <section className="metrics-grid">{metrics.map(([Icon, label, value, delta, tone]) => <button className="metric-action" key={label} onClick={metricActions[label]} disabled={!metricActions[label]}><MetricCard icon={Icon} label={label} value={value} delta={delta} tone={tone} /></button>)}</section>
      <section className="dashboard-grid">
        <article className="panel chart-panel">
          <div className="panel-heading"><div><h2>{user.role === 'student' ? 'Attendance trend' : 'Attendance overview'}</h2><p>Average across the last four weeks</p></div><select><option>Last 30 days</option><option>This semester</option></select></div>
          <div className="chart-value"><strong>{attendancePercent}%</strong><span>{attendancePercent ? 'Live from attendance records' : 'No attendance recorded yet'}</span></div>
          {chartData.length ? <ResponsiveContainer width="100%" height={235}>
            <AreaChart data={chartData} margin={{ left: -20, right: 10 }}>
              <defs><linearGradient id="burgundyArea" x1="0" y1="0" x2="0" y2="1"><stop offset="5%" stopColor="#9e1b46" stopOpacity={0.25}/><stop offset="95%" stopColor="#9e1b46" stopOpacity={0}/></linearGradient></defs>
              <CartesianGrid stroke="#eee9e7" vertical={false} /><XAxis dataKey="name" axisLine={false} tickLine={false} tick={{ fill: '#8b8582', fontSize: 12 }} /><YAxis domain={[70, 100]} axisLine={false} tickLine={false} tick={{ fill: '#8b8582', fontSize: 12 }} />
              <Tooltip /><Area type="monotone" dataKey="value" stroke="#8d153a" strokeWidth={3} fill="url(#burgundyArea)" />
            </AreaChart>
          </ResponsiveContainer> : <div className="empty-state compact"><h2>No attendance trend yet</h2><p>Once attendance is submitted for assigned batches, the trend will appear here.</p></div>}
        </article>
        <article className="panel">
          <div className="panel-heading"><div><h2>{user.role === 'student' ? 'Upcoming schedule' : "Today's schedule"}</h2><p>{new Date().toLocaleDateString('en-IN', { weekday: 'long', day: '2-digit', month: 'long' })}</p></div><button className="text-button">View all <ChevronRight size={15} /></button></div>
          <div className="schedule-list">
            {scheduleRows.map((item) => (
              <div className="schedule-item" key={`${item.time}-${item.title}`}><div className="time"><Clock3 size={14} />{item.time}</div><div><strong>{item.title}</strong><span>{item.location}</span></div><StatusPill>{item.status}</StatusPill></div>
            ))}
            {!scheduleRows.length && <p className="empty-note">No classes are scheduled for you today. Open timetable to review weekly slots.</p>}
          </div>
        </article>
        <article className="panel activity-panel">
          <div className="panel-heading"><div><h2>{user.role === 'student' ? 'Announcements' : 'Recent activity'}</h2><p>Latest campus updates</p></div><button className="text-button">View all <ChevronRight size={15} /></button></div>
          <div className="activity-list">
            {activityRows.map((item, index) => <div className="activity" key={item.title}><span className={`activity-icon i${index}`}><CheckCircle2 size={17} /></span><div><strong>{item.title}</strong><span>{item.owner} · {item.time}</span></div></div>)}
            {!activityRows.length && <p className="empty-note">No recent faculty activity yet. Create an assignment or calendar item to populate this panel.</p>}
          </div>
        </article>
      </section>
    </>
  )
}
