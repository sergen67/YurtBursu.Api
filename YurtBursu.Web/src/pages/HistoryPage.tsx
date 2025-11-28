import { useState } from 'react'
import { bursApi } from '../api/bursApi'
import type { BursHistoryEntry } from '../types/Burs'

export const HistoryPage = () => {
  const [studentIdInput, setStudentIdInput] = useState<string>('')
  const [items, setItems] = useState<BursHistoryEntry[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const fetchHistory = async () => {
    const studentId = Number.parseInt(studentIdInput, 10)
    
    // Validate studentId before making request
    if (!studentIdInput.trim() || isNaN(studentId) || studentId <= 0) {
      setError('Please enter a valid student ID (must be a positive number)')
      return
    }

    setError(null)
    setLoading(true)
    setItems([])
    
    try {
      const res = await bursApi.getHistory(studentId)
      setItems(res)
      if (res.length === 0) {
        setError(null) // Clear error, show "No history" message instead
      }
    } catch (err: any) {
      const message = err?.response?.data?.error ?? err?.response?.data?.message ?? err?.message ?? 'Request failed'
      setError(message)
      setItems([])
    } finally {
      setLoading(false)
    }
  }

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value
    // Allow empty string or valid numbers
    if (value === '' || /^\d+$/.test(value)) {
      setStudentIdInput(value)
      setError(null) // Clear error when user types
    }
  }

  const isValidStudentId = (): boolean => {
    const studentId = Number.parseInt(studentIdInput, 10)
    return studentIdInput.trim() !== '' && !isNaN(studentId) && studentId > 0
  }

  return (
    <section>
      <h2>Burs History</h2>
      <div style={{ display: 'flex', gap: 12, alignItems: 'flex-end' }}>
        <label>
          Student Id
          <input
            type="text"
            inputMode="numeric"
            value={studentIdInput}
            onChange={handleInputChange}
            placeholder="Enter student ID"
            min={1}
            style={{ width: 200, padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
            onKeyDown={(e) => {
              if (e.key === 'Enter' && isValidStudentId() && !loading) {
                fetchHistory()
              }
            }}
          />
        </label>
        <button 
          onClick={fetchHistory} 
          disabled={loading || !isValidStudentId()} 
          style={{ padding: '10px 14px', borderRadius: 6 }}
        >
          {loading ? 'Loading...' : 'Get History'}
        </button>
      </div>

      {error && <div style={{ color: '#c00', marginTop: 12, padding: 8, background: '#ffe6e6', borderRadius: 4 }}>Error: {error}</div>}

      {loading && (
        <div style={{ marginTop: 16, color: '#666' }}>Loading history...</div>
      )}

      {!loading && items.length > 0 && (
        <div style={{ marginTop: 16, display: 'grid', gap: 12 }}>
          {items.map(it => (
            <div key={it.id} style={{ border: '1px solid #ddd', background: '#fff', padding: 12, borderRadius: 6 }}>
              <div><strong>Year/Month:</strong> {it.year}/{it.month}</div>
              <div><strong>Excluded Days:</strong> {it.excludedDays && it.excludedDays.length > 0 ? it.excludedDays.join(', ') : 'None'}</div>
              <div><strong>Calculated Burs:</strong> {it.calculatedBurs.toLocaleString('tr-TR')} ₺</div>
              <div><strong>Created At:</strong> {new Date(it.createdAt).toLocaleString('tr-TR')}</div>
            </div>
          ))}
        </div>
      )}

      {!loading && !error && items.length === 0 && studentIdInput && isValidStudentId() && (
        <div style={{ marginTop: 16, color: '#666', padding: 12, background: '#f5f5f5', borderRadius: 4 }}>
          No history found for student ID: {studentIdInput}
        </div>
      )}
    </section>
  )
}


