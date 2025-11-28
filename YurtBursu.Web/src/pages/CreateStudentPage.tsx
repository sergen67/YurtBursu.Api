import { useState } from 'react'
import { studentApi } from '../api/studentApi'
import type { Student } from '../types/Student'

export const CreateStudentPage = () => {
  const [fullName, setFullName] = useState('')
  const [email, setEmail] = useState('')
  const [created, setCreated] = useState<Student | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    setCreated(null)
    try {
      const res = await studentApi.createStudent({ fullName, email })
      setCreated(res)
      setFullName('')
      setEmail('')
    } catch (err: any) {
      const message = err?.response?.data?.error ?? err?.message ?? 'Request failed'
      setError(message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <section>
      <h2>Create Student</h2>
      <form onSubmit={onSubmit} style={{ display: 'grid', gap: 12, maxWidth: 420 }}>
        <label>
          Full Name
          <input
            type="text"
            value={fullName}
            onChange={e => setFullName(e.target.value)}
            required
            style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
          />
        </label>
        <label>
          Email
          <input
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
            style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
          />
        </label>
        <button disabled={loading} type="submit" style={{ padding: '10px 14px', borderRadius: 6 }}>
          {loading ? 'Creating...' : 'Create'}
        </button>
      </form>

      {error && (
        <div style={{ marginTop: 16, color: '#c00' }}>
          Error: {error}
        </div>
      )}

      {created && (
        <div style={{ marginTop: 16, background: '#f6ffed', border: '1px solid #b7eb8f', padding: 12, borderRadius: 6 }}>
          <strong>Student created:</strong>
          <div>Id: {created.id}</div>
          <div>Name: {created.fullName}</div>
          <div>Email: {created.email}</div>
          <div>CreatedAt: {new Date(created.createdAt).toLocaleString()}</div>
        </div>
      )}
    </section>
  )
}


