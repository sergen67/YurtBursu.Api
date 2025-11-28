import axios from 'axios'

// Resolve API base URL from environment, with sensible fallbacks for dev/prod
// Priority: ENV variable → localhost:5000 (dev)
// NOTE: In production, you MUST set VITE_API_BASE_URL environment variable
// or deploy your backend and update the PRODUCTION_API_URL below
const envBase = (import.meta as any).env?.VITE_API_BASE_URL as string | undefined
const isProduction = import.meta.env.MODE === 'production' || import.meta.env.PROD

// Production backend URL - UPDATE THIS with your deployed backend URL
// Examples:
// - Render: https://your-app.onrender.com/api
// - Azure: https://your-app.azurewebsites.net/api
// - Heroku: https://your-app.herokuapp.com/api
const PRODUCTION_API_URL = 'http://localhost:5000/api' // ⚠️ WARNING: localhost won't work in production!

// Default to backend port 5000 for dev
const API_BASE_URL = envBase || (isProduction ? PRODUCTION_API_URL : 'http://localhost:5000/api')

export const axiosClient = axios.create({ baseURL: API_BASE_URL, timeout: 10_000 })

axiosClient.interceptors.request.use((config) => {
  // Ensure headers object exists and then mutate to satisfy Axios v1 types
  config.headers = config.headers ?? {}
  ;(config.headers as any)['Content-Type'] = 'application/json'
  ;(config.headers as any)['Accept'] = 'application/json'
  return config
})

axiosClient.interceptors.response.use(
  (res) => res,
  (err) => {
    // Log 404 errors with helpful debugging info
    if (err?.response?.status === 404) {
      const method = err?.config?.method?.toUpperCase() || 'UNKNOWN'
      const url = err?.config?.url || 'UNKNOWN'
      const fullUrl = `${API_BASE_URL}${url.startsWith('/') ? url : '/' + url}`
      console.error(`[404 Not Found] ${method} ${fullUrl}`)
      console.error('Check if backend is running and endpoint path matches exactly.')
    }
    // Helpful console hint for mixed content or bad baseURL
    const isBrowser = typeof window !== 'undefined'
    if (isBrowser && window.location.protocol === 'https:' && String(API_BASE_URL).startsWith('http://')) {
      // eslint-disable-next-line no-console
      console.warn('API base URL is http on an https page. Browsers will block mixed content. Set VITE_API_BASE_URL to an https backend URL.')
    }
    return Promise.reject(err)
  }
)


