export interface BursRequest {
  studentId: number
  year: number
  month: number
  excludedDays: number[]
}

export interface BursResponse {
  studentId: number
  totalDays: number
  excludedCount: number
  rate: number
  calculatedBurs: number
}

export interface BursHistoryEntry {
  id: number
  studentId: number
  year: number
  month: number
  excludedDays: number[]
  calculatedBurs: number
  createdAt: string
}


