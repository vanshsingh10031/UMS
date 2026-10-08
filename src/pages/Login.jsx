import { useState } from 'react'
import { ArrowRight, Eye, EyeOff, LockKeyhole, ShieldCheck } from 'lucide-react'
import { useAuth } from '../hooks/useAuth'
import Brand from '../components/Brand'
import campusHero from '../assets/campus-hero.png'

export default function Login() {
  const { login } = useAuth()
  const [username, setUsername] = useState('admin@amrita.edu')
  const [password, setPassword] = useState('amrita2026')
  const [forgotOpen, setForgotOpen] = useState(false)
  const [recoveryId, setRecoveryId] = useState('')
  const [recoverySent, setRecoverySent] = useState(false)
  const [error, setError] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [loading, setLoading] = useState(false)
  const submit = async (event) => {
    event.preventDefault()
    setLoading(true)
    setError('')
    try {
      await login(username, password)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }
  return (
    <main className="login-page">
      <section className="login-visual" style={{ backgroundImage: `url(${campusHero})` }}>
        <div className="login-visual__shade" />
        <div className="login-visual__content">
          <Brand />
          <div><span className="login-kicker">University Management System</span><h1>Knowledge that<br />transforms lives.</h1><p>A unified digital campus for learning, collaboration and academic excellence.</p></div>
          <small>© 2026 Amrita Vishwa Vidyapeetham</small>
        </div>
      </section>
      <section className="login-panel">
        <div className="mobile-brand"><Brand /></div>
        <form onSubmit={submit}>
          <div className="login-heading"><span className="login-icon"><LockKeyhole size={20} /></span><h2>Welcome back</h2><p>Use your university ID or email. Your portal opens automatically based on your account role.</p></div>
          <label className="field-label" htmlFor="username">University ID or email</label>
          <div className="input-wrap"><input id="username" value={username} onChange={(event) => setUsername(event.target.value)} /></div>
          <div className="password-row"><label className="field-label" htmlFor="password">Password</label><button type="button" onClick={() => { setForgotOpen(true); setRecoveryId(username); setRecoverySent(false) }}>Forgot password?</button></div>
          <div className="input-wrap"><input id="password" type={showPassword ? 'text' : 'password'} value={password} onChange={(event) => setPassword(event.target.value)} /><button type="button" className="reveal" onClick={() => setShowPassword(!showPassword)}>{showPassword ? <EyeOff size={18} /> : <Eye size={18} />}</button></div>
          {error && <p className="form-error">{error}</p>}
          <label className="remember"><input type="checkbox" defaultChecked /><span>Keep me signed in on this device</span></label>
          <button className="login-submit" disabled={loading}>{loading ? 'Signing in…' : <>Sign in to portal <ArrowRight size={18} /></>}</button>
          <div className="secure-note"><ShieldCheck size={16} /><span>Protected by secure university authentication</span></div>
        </form>
        <div className="login-help">Need help? <button>Contact IT support</button></div>
      </section>
      {forgotOpen && <div className="modal-backdrop"><form className="forgot-card" onSubmit={(event) => { event.preventDefault(); setRecoverySent(true) }}><button className="modal-close" type="button" onClick={() => setForgotOpen(false)}>×</button><span className="login-kicker">Account recovery</span><h2>Forgot password</h2><p>This prototype screen only records your reset request. The IT helpdesk flow will be connected later.</p><label className="field-label">University ID or email<input value={recoveryId} onChange={(event) => setRecoveryId(event.target.value)} required /></label>{recoverySent && <p className="form-success">Reset request captured for {recoveryId}. Please contact IT support to receive a temporary password.</p>}<button className="login-submit">Request reset</button></form></div>}
    </main>
  )
}
