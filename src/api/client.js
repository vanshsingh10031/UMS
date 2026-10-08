export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5052/api'

export function getToken() {
  return sessionStorage.getItem('ums-token')
}

export async function api(path, options = {}) {
  const headers = new Headers(options.headers || {})
  const token = getToken()
  if (token) headers.set('Authorization', `Bearer ${token}`)
  if (options.body && !(options.body instanceof FormData)) headers.set('Content-Type', 'application/json')

  const response = await fetch(`${API_BASE_URL}${path}`, { ...options, headers })
  if (response.status === 401) {
    sessionStorage.removeItem('ums-token')
    sessionStorage.removeItem('ums-user')
  }
  const contentType = response.headers.get('content-type') || ''
  const payload = contentType.includes('application/json') ? await response.json() : null
  if (!response.ok) {
    const validation = payload?.errors ? Object.entries(payload.errors).map(([field, messages]) => `${field}: ${messages.join(', ')}`).join(' ') : ''
    const statusMessages = {
      400: 'Please check the highlighted fields and try again.',
      401: 'Your session has expired. Please sign in again.',
      403: 'You do not have permission to complete this action.',
      404: 'The requested record could not be found.',
      409: 'This record conflicts with an existing entry. Check duplicate IDs, codes, emails, or linked data.',
      500: 'The server could not save this record. Check duplicate IDs, required linked master data, and valid field formats.',
    }
    const message = payload?.message || validation || payload?.detail || payload?.title || statusMessages[response.status] || 'The request could not be completed. Please review the form and try again.'
    throw new Error(message)
  }
  return payload
}

export const unwrapList = (payload) => Array.isArray(payload) ? payload : payload?.data || []
