import { axiosClient } from './axiosClient'
import type { BursRequest, BursResponse, BursHistoryEntry } from '../types/Burs'

export const bursApi = {
  async calculateBurs(dto: BursRequest): Promise<BursResponse> {
    try {
      const res = await axiosClient.post<BursResponse>('/burs/hesapla', dto)
      return res.data
    } catch (error: any) {
      console.error('Burs calculate error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async saveHistory(dto: BursRequest): Promise<BursResponse> {
    try {
      const res = await axiosClient.post<BursResponse>('/burs/history/save', dto)
      return res.data
    } catch (error: any) {
      console.error('Burs save history error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async getHistory(studentId: number): Promise<BursHistoryEntry[]> {
    try {
      // Validate studentId before making request
      if (!studentId || studentId <= 0 || !Number.isInteger(studentId)) {
        throw new Error('Student ID must be a positive integer')
      }
      const res = await axiosClient.get<BursHistoryEntry[]>(`/burs/history/${studentId}`)
      return res.data
    } catch (error: any) {
      console.error('Burs get history error:', {
        status: error?.response?.status,
        statusText: error?.response?.statusText,
        data: error?.response?.data,
        url: error?.config?.url,
        method: error?.config?.method
      })
      throw error
    }
  }
}


