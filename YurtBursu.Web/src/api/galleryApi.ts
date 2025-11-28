import { axiosClient } from './axiosClient'

export interface GalleryItem {
  id: number
  imageUrl: string
  createdAt: string
}

let adminAuth: { user: string; pass: string } | null = null
let interceptorId: number | null = null

function ensureInterceptor() {
  if (interceptorId !== null) return
  interceptorId = axiosClient.interceptors.request.use((config) => {
    // Only attach BasicAuth for the admin upload endpoint
    if (config.url?.endsWith('/gallery/upload') && adminAuth) {
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

export const galleryApi = {
  async upload(url: string): Promise<GalleryItem> {
    try {
      ensureInterceptor()
      const res = await axiosClient.post<GalleryItem>('/gallery/upload', { url })
      return res.data
    } catch (error: any) {
      console.error('Gallery upload error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async list(): Promise<GalleryItem[]> {
    try {
      const res = await axiosClient.get<GalleryItem[]>('/gallery/list')
      return res.data
    } catch (error: any) {
      console.error('Gallery list error:', error?.response?.status, error?.response?.data)
      throw error
    }
  }
}


