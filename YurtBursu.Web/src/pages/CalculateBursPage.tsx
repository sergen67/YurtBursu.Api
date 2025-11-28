import { useMemo, useState } from 'react'
import { DayPicker } from '../components/Calendar/DayPicker'
import { bursApi } from '../api/bursApi'
import type { BursRequest, BursResponse } from '../types/Burs'

export const CalculateBursPage = () => {
  const [studentId, setStudentId] = useState<number>(0)
  const [year, setYear] = useState<number>(new Date().getFullYear())
  const [month, setMonth] = useState<number>(new Date().getMonth() + 1) // 1..12
  const [days, setDays] = useState<number[]>([])
  const [result, setResult] = useState<BursResponse | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [saveSuccess, setSaveSuccess] = useState(false)

  const totalDaysInMonth = useMemo(() => new Date(year, month, 0).getDate(), [year, month])

  const onCalculate = async () => {
    setError(null)
    setResult(null)
    setSaveSuccess(false)
    setLoading(true)
    const payload: BursRequest = { studentId, year, month, excludedDays: days }
    try {
      const res = await bursApi.calculateBurs(payload)
      setResult(res)
    } catch (err: any) {
      const message = err?.response?.data?.error ?? err?.message ?? 'Request failed'
      setError(message)
    } finally {
      setLoading(false)
    }
  }

  const onSaveToHistory = async () => {
    if (!result) return
    
    setError(null)
    setSaveSuccess(false)
    setSaving(true)
    const payload: BursRequest = { studentId, year, month, excludedDays: days }
    try {
      await bursApi.saveHistory(payload)
      setSaveSuccess(true)
      // Clear success message after 3 seconds
      setTimeout(() => setSaveSuccess(false), 3000)
    } catch (err: any) {
      const message = err?.response?.data?.error ?? err?.message ?? 'Save failed'
      setError(message)
    } finally {
      setSaving(false)
    }
  }

  return (
    <section>
      <h2>Calculate Burs</h2>
      <div style={{ display: 'grid', gap: 12, maxWidth: 640 }}>
        <label>
          Student Id
          <input
            type="number"
            value={studentId || ''}
            onChange={e => setStudentId(Number(e.target.value))}
            min={1}
            style={{ width: 200, padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
          />
        </label>
        <div style={{ display: 'flex', gap: 12 }}>
          <label>
            Year
            <input
              type="number"
              value={year}
              onChange={e => setYear(Number(e.target.value))}
              min={2000}
              max={new Date().getFullYear() + 1}
              style={{ width: 120, padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
            />
          </label>
          <label>
            Month
            <input
              type="number"
              value={month}
              onChange={e => setMonth(Number(e.target.value))}
              min={1}
              max={12}
              style={{ width: 120, padding: 8, borderRadius: 6, border: '1px solid #ccc' }}
            />
          </label>
        </div>

        <div>
          <div style={{ marginBottom: 8, fontWeight: 600 }}>Excluded Days</div>
          <DayPicker
            month={month}
            year={year}
            selectedDays={days}
            onChange={setDays}
          />
          <div style={{ marginTop: 6, fontSize: 13, color: '#666' }}>
            Selected: {days.join(', ') || 'None'} | Total days in month: {totalDaysInMonth}
          </div>
        </div>

        <button onClick={onCalculate} disabled={loading || !studentId} style={{ padding: '10px 14px', borderRadius: 6 }}>
          {loading ? 'Calculating...' : 'Calculate'}
        </button>

        {error && (
          <div style={{ color: '#c00', padding: 8, background: '#ffe6e6', borderRadius: 4 }}>Error: {error}</div>
        )}

        {saveSuccess && (
          <div style={{ color: '#2b7a0b', padding: 8, background: '#f6ffed', border: '1px solid #b7eb8f', borderRadius: 4 }}>
            ✓ Successfully saved to history!
          </div>
        )}

        {result && (
          <div style={{ background: '#f6ffed', border: '1px solid #b7eb8f', padding: 12, borderRadius: 6 }}>
            <div><strong>StudentId:</strong> {result.studentId}</div>
            <div><strong>TotalDays:</strong> {result.totalDays}</div>
            <div><strong>ExcludedCount:</strong> {result.excludedCount}</div>
            <div><strong>Rate:</strong> {result.rate} ₺</div>
            <div><strong>CalculatedBurs:</strong> {result.calculatedBurs.toLocaleString('tr-TR')} ₺</div>
            <div style={{ marginTop: 12 }}>
              <button 
                onClick={onSaveToHistory} 
                disabled={saving || !studentId} 
                style={{ 
                  padding: '8px 16px', 
                  borderRadius: 6, 
                  background: saving ? '#ccc' : '#1890ff',
                  color: 'white',
                  border: 'none',
                  cursor: saving ? 'not-allowed' : 'pointer'
                }}
              >
                {saving ? 'Saving...' : 'Save to History'}
              </button>
            </div>
          </div>
        )}
      </div>
    </section>
  )
}


