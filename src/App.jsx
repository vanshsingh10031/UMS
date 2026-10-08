import { Navigate, Route, Routes } from 'react-router-dom'
import { useParams } from 'react-router-dom'
import { AuthProvider, useAuth } from './hooks/useAuth'
import Login from './pages/Login'
import PortalPage from './pages/PortalPage'
import AppLayout from './layouts/AppLayout'
import { useState } from 'react'

function RoleRoute() {
  const { user } = useAuth()
  const { role } = useParams()
  if (role !== user.role) return <Navigate to={`/${user.role}/dashboard`} replace />
  return <PortalPage />
}

function ProtectedApp() {
  const { user } = useAuth()
  if (!user) return <Navigate to="/login" replace />
  if (user.mustChangePassword) return <PasswordChangeGate />
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route path="/:role/:page" element={<RoleRoute />} />
        <Route path="/:role/:section/:page" element={<RoleRoute />} />
      </Route>
      <Route path="*" element={<Navigate to={`/${user.role}/dashboard`} replace />} />
    </Routes>
  )
}

function PasswordChangeGate() {
  const { user, changePassword, logout } = useAuth()
  const [form, setForm] = useState({ currentPassword: '', newPassword: '', confirmPassword: '' })
  const [message, setMessage] = useState('')
  const [saving, setSaving] = useState(false)
  const displayName = user?.name && user.name !== user.username ? user.name : ''
  const submit = async (event) => {
    event.preventDefault()
    setMessage('')
    if (form.newPassword !== form.confirmPassword) {
      setMessage('New password and confirmation do not match.')
      return
    }
    setSaving(true)
    try {
      await changePassword(form.currentPassword, form.newPassword)
    } catch (err) {
      setMessage(err.message)
    } finally {
      setSaving(false)
    }
  }
  return (
    <main className="password-gate">
      <form className="password-gate-card" onSubmit={submit}>
        <span className="login-kicker">First login security</span>
        <h1>Change temporary password</h1>
        <p>{displayName ? `${displayName}, update your temporary password before entering the portal.` : 'Update your temporary password before entering the portal.'}</p>
        <label className="field-label">Temporary password<input type="password" value={form.currentPassword} onChange={(event) => setForm({ ...form, currentPassword: event.target.value })} required /></label>
        <label className="field-label">New password<input type="password" value={form.newPassword} onChange={(event) => setForm({ ...form, newPassword: event.target.value })} required minLength={6} /></label>
        <label className="field-label">Confirm new password<input type="password" value={form.confirmPassword} onChange={(event) => setForm({ ...form, confirmPassword: event.target.value })} required minLength={6} /></label>
        {message && <p className="form-error">{message}</p>}
        <button className="login-submit" disabled={saving}>{saving ? 'Updating...' : 'Update password'}</button>
        <button className="text-button" type="button" onClick={logout}>Back to login</button>
      </form>
    </main>
  )
}

function AppRoutes() {
  const { user } = useAuth()
  return (
    <Routes>
      <Route path="/login" element={user ? <Navigate to={`/${user.role}/dashboard`} replace /> : <Login />} />
      <Route path="/*" element={<ProtectedApp />} />
    </Routes>
  )
}

export default function App() {
  return <AuthProvider><AppRoutes /></AuthProvider>
}
