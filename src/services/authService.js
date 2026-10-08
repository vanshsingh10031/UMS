export const signIn = async (role = 'student') => Promise.resolve({ role, authenticated: true })
export const signOut = async () => Promise.resolve({ authenticated: false })
