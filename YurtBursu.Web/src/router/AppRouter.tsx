import { Routes, Route } from 'react-router-dom'
import { HomePage } from '../pages/HomePage'
import { CreateStudentPage } from '../pages/CreateStudentPage'
import { CalculateBursPage } from '../pages/CalculateBursPage'
import { HistoryPage } from '../pages/HistoryPage'
import { AdminGalleryPage } from '../pages/AdminGalleryPage'
import { GalleryPage } from '../pages/GalleryPage'
import { AdminNotificationsPage } from '../pages/AdminNotificationsPage'

export const AppRouter = () => {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/student/create" element={<CreateStudentPage />} />
      <Route path="/burs/calculate" element={<CalculateBursPage />} />
      <Route path="/burs/history" element={<HistoryPage />} />
      <Route path="/gallery" element={<GalleryPage />} />
      <Route path="/admin/gallery" element={<AdminGalleryPage />} />
      <Route path="/admin/notifications" element={<AdminNotificationsPage />} />
    </Routes>
  )
}


