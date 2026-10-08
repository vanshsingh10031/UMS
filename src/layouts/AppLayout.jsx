import * as Icons from 'lucide-react'
import { NavLink, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import Brand from '../components/Brand'
import { navByRole } from '../data/mockData'
import { useAuth } from '../hooks/useAuth'

export default function AppLayout() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [mobileOpen, setMobileOpen] = useState(false)
  const [profileOpen, setProfileOpen] = useState(false)
  const [openGroup, setOpenGroup] = useState(null)
  useEffect(() => setMobileOpen(false), [location.pathname])
  useEffect(() => setOpenGroup(null), [location.pathname])

  const leave = () => { logout(); navigate('/login') }
  const roleNav = navByRole[user.role] || []
  const groups = roleNav.reduce((items, item) => {
    if (item.heading) {
      items.push({ label: item.label, links: [] })
      return items
    }
    if (!items.length) items.push({ label: 'Workspace', links: [] })
    items[items.length - 1].links.push(item)
    return items
  }, [])
  const activeGroup = groups.find((group) => group.links.some((item) => location.pathname.includes(`/${user.role}/${item.path}`)))?.label
  const portalLabel = user.role === 'faculty' ? 'Faculty Portal' : `${user.role.charAt(0).toUpperCase()}${user.role.slice(1)} Portal`
  return (
    <div className="app-shell">
      {mobileOpen && <button className="scrim" onClick={() => setMobileOpen(false)} aria-label="Close menu" />}
      <header className="portal-navbar">
        <div className="navbar-inner">
          <button className="menu-button" onClick={() => setMobileOpen(true)} aria-label="Open navigation"><Icons.Menu size={21} /></button>
          <div className="navbar-brand"><Brand /><span>{portalLabel}</span></div>
          <nav className="mega-nav" aria-label="Primary navigation">
            {groups.map((group) => (
              <div className="mega-group" key={group.label}>
                <button type="button" className={activeGroup === group.label || openGroup === group.label ? 'active' : ''} onClick={() => setOpenGroup(openGroup === group.label ? null : group.label)}>
                  {group.label}<Icons.ChevronDown size={14} />
                </button>
                {openGroup === group.label && (
                  <div className="mega-panel">
                    <div>
                      <span className="mega-kicker">{portalLabel}</span>
                      <h2>{group.label}</h2>
                      <p>Move through the portal using consistent grouped actions for your role.</p>
                    </div>
                    <div className="mega-links">
                      {group.links.map((item) => {
                        const Icon = Icons[item.icon]
                        return <NavLink key={item.path} to={`/${user.role}/${item.path}`} className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setOpenGroup(null)}><Icon size={17} /><span>{item.label}</span></NavLink>
                      })}
                    </div>
                  </div>
                )}
              </div>
            ))}
          </nav>
          <div className="top-actions">
            <button className="icon-button has-badge" title="Live notifications are active" aria-label="Notifications"><Icons.Bell size={20} /><i /></button>
            <button className="profile-button" onClick={() => setProfileOpen(!profileOpen)}>
              <span>{user.initials}</span><div><strong>{user.name}</strong><small>{user.title}</small></div><Icons.ChevronDown size={16} />
            </button>
            {profileOpen && <div className="profile-menu"><button onClick={() => navigate(`/${user.role}/profile`)}><Icons.UserRound size={16} /> My profile</button><button onClick={leave}><Icons.LogOut size={16} /> Sign out</button></div>}
          </div>
        </div>
        <div className="quickbar">
          {groups.flatMap((group) => group.links).slice(0, 5).map((item) => {
            const Icon = Icons[item.icon]
            return <NavLink key={item.path} to={`/${user.role}/${item.path}`} className={({ isActive }) => isActive ? 'active' : ''}><Icon size={15} />{item.label}</NavLink>
          })}
        </div>
      </header>

      <aside className={`sidebar mobile-nav ${mobileOpen ? 'is-open' : ''}`}>
        <div className="sidebar__brand"><Brand /><button className="sidebar-close" onClick={() => setMobileOpen(false)}><Icons.X size={20} /></button></div>
        <div className="role-chip">{portalLabel}</div>
        <nav>
          {groups.map((group) => (
            <div key={group.label}>
              <span className="nav-heading">{group.label}</span>
              {group.links.map((item) => {
                const Icon = Icons[item.icon]
                return <NavLink key={item.path} to={`/${user.role}/${item.path}`} className={({ isActive }) => isActive ? 'active' : ''}>
                  <Icon size={18} /><span>{item.label}</span>
                </NavLink>
              })}
            </div>
          ))}
        </nav>
        <div className="sidebar__footer"><span>Academic year</span><strong>2025-2026</strong><small>Odd Semester</small></div>
      </aside>

      <section className="main-shell">
        <main className="content"><Outlet /></main>
      </section>
    </div>
  )
}
