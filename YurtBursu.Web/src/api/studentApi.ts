import { axiosClient } from './axiosClient'
import type { Student, StudentCreateDto } from '../types/Student'

export const studentApi = {
  async createStudent(dto: StudentCreateDto): Promise<Student> {
    try {
      const res = await axiosClient.post<Student>('/student/create', dto)
      return res.data
    } catch (error: any) {
      console.error('Student create error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async getStudentById(id: number): Promise<Student> {
    try {
      const res = await axiosClient.get<Student>(`/student/${id}`)
      return res.data
    } catch (error: any) {
      console.error('Student get by id error:', error?.response?.status, error?.response?.data)
      throw error
    }
  },
  async getAllStudents(): Promise<Student[]> {
    try {
      const res = await axiosClient.get<Student[]>('/student/all')
      return res.data
    } catch (error: any) {
      console.error('Student get all error:', error?.response?.status, error?.response?.data)
      throw error
    }
  }
}


