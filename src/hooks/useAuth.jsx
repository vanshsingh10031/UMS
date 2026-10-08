import { createContext, useContext, useMemo, useState } from 'react'
import { api } from '../api/client'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const saved = sessionStorage.getItem('ums-user')
    return saved ? JSON.parse(saved) : null
  })

  const login = async (username, password) => {
    const session = await api('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ username, password }),
    })
    sessionStorage.setItem('ums-token', session.token)
    sessionStorage.setItem('ums-user', JSON.stringify(session.user))
    setUser(session.user)
    return session.user
  }
  const logout = () => {
    sessionStorage.removeItem('ums-token')
    sessionStorage.removeItem('ums-user')
    setUser(null)
  }
  const changePassword = async (currentPassword, newPassword) => {
    const updatedUser = await api('/auth/change-password', {
      method: 'POST',
      body: JSON.stringify({ currentPassword, newPassword }),
    })
    sessionStorage.setItem('ums-user', JSON.stringify(updatedUser))
    setUser(updatedUser)
    return updatedUser
  }
  const value = useMemo(() => ({ user, login, logout, changePassword }), [user])
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export const useAuth = () => useContext(AuthContext)
