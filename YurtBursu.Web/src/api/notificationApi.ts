import { axiosClient } from './axiosClient'

let adminAuth: { user: string; pass: string } | null = null
let interceptorId: number | null = null

function ensureInterceptor() {
  if (interceptorId !== null) return
  interceptorId = axiosClient.interceptors.request.use((config) => {
    // Only attach BasicAuth for the admin send endpoint
    if (config.url?.endsWith('/notification/send') && adminAuth) {
      const token = btoa(`${adminAuth.user}:${adminAuth.pass}`)
      config.headers = config.headers ?? {}
      ;(config.headers as any).Authorization = `Basic ${token}`
    }
    return config
  })
}

export function setBasicAuth(user: string, pass: string) {
  adminAuth = { user, pass }
  ensureInterceptor()
}

export function clearBasicAuth() {
  adminAuth = null
}

export const notificationApi = {
  async registerToken(studentId: number, token: string): Promise<void> {
    try {
      await axiosClient.post('/notification/register', { studentId, token })
    } catch (error: any) {
      console.error('Notification register error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async sendAll(title: string, body: string): Promise<{ sent: number }> {
    try {
      ensureInterceptor()
      const res = await axiosClient.post<{ sent: number }>('/notification/send', { title, body })
      return res.data
    } catch (error: any) {
      console.error('Notification send error:', error?.response?.status, error?.response?.data)
      throw error
    }
  }
}


