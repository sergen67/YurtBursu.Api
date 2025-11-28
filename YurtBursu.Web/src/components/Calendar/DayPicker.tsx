import { useMemo } from 'react'

export interface DayPickerProps {
  month: number
  year: number
  selectedDays: number[]
  onChange: (days: number[]) => void
}

const gridStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: 'repeat(7, 1fr)',
  gap: 8
}

const dayStyle: React.CSSProperties = {
  padding: '10px 0',
  textAlign: 'center',
  cursor: 'pointer',
  border: '1px solid #ddd',
  borderRadius: 6,
  userSelect: 'none',
  background: '#fff'
}

export const DayPicker = ({ month, year, selectedDays, onChange }: DayPickerProps) => {
  const totalDays = useMemo(() => {
    // JS Date month is 1-based input here; Date expects 0-based -> use (month, 0) to get last day of previous month
    return new Date(year, month, 0).getDate()
  }, [month, year])

  const isSelected = (d: number) => selectedDays.includes(d)

  const toggle = (d: number) => {
    const exists = isSelected(d)
    let next = exists ? selectedDays.filter(x => x !== d) : [...selectedDays, d]
    next = Array.from(new Set(next)).sort((a, b) => a - b)
    onChange(next)
  }

  return (
    <div style={gridStyle}>
      {Array.from({ length: totalDays }, (_, i) => i + 1).map(d => {
        const active = isSelected(d)
        return (
          <div
            key={d}
            role="button"
            aria-pressed={active}
            onClick={() => toggle(d)}
            style={{
              ...dayStyle,
              background: active ? '#e6f4ff' : '#fff',
              borderColor: active ? '#91caff' : '#ddd',
              color: active ? '#1677ff' : '#333',
              fontWeight: active ? 700 : 400
            }}
          >
            {d}
          </div>
        )
      })}
    </div>
  )
}


