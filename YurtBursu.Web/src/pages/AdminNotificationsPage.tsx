import { useState } from 'react'
import { notificationApi, setBasicAuth as setNotificationAuth } from '../api/notificationApi'

export const AdminNotificationsPage = () => {
  const [title, setTitle] = useState('')
  const [body, setBody] = useState('')
  const [authUser, setAuthUser] = useState('')
  const [authPass, setAuthPass] = useState('')
  const [result, setResult] = useState<string>('')
  const [error, setError] = useState<string>('')

  const onSend = async () => {
    setResult('')
    setError('')
    try {
      if (authUser && authPass) {
        setNotificationAuth(authUser, authPass)
      }
      const res = await notificationApi.sendAll(title, body)
      setResult(`Sent to ${res.sent} tokens.`)
    } catch (err: any) {
      setError(err?.response?.data?.error ?? err?.message ?? 'Send failed')
    }
  }

  return (
    <section>
      <h2>Admin Notifications</h2>
      <div style={{ display: 'grid', gap: 12, maxWidth: 520 }}>
        <label>
          Basic Auth Username
          <input value={authUser} onChange={e => setAuthUser(e.target.value)} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <label>
          Basic Auth Password
          <input type="password" value={authPass} onChange={e => setAuthPass(e.target.value)} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <label>
          Title
          <input value={title} onChange={e => setTitle(e.target.value)} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <label>
          Body
          <textarea value={body} onChange={e => setBody(e.target.value)} rows={4} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <button onClick={onSend} style={{ padding: '10px 14px', borderRadius: 6 }}>Send notification</button>
        {result && <div style={{ color: '#2b7a0b' }}>{result}</div>}
        {error && <div style={{ color: '#c00' }}>{error}</div>}
      </div>
    </section>
  )
}


