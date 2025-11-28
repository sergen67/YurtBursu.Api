import { Link } from 'react-router-dom'

export const HomePage = () => {
  return (
    <section>
      <h1>Yurt Bursu Hesaplama</h1>
      <p>Welcome. Use the navigation to create students, calculate burs, or view history.</p>
      
      <div style={{ marginTop: 24, display: 'grid', gap: 16, maxWidth: 600 }}>
        <h2>Quick Links</h2>
        <div style={{ display: 'grid', gap: 12 }}>
          <Link to="/student/create" style={{ padding: '12px 16px', background: '#f5f5f5', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ddd' }}>
            <strong>Create Student</strong> - Add a new student to the system
          </Link>
          <Link to="/burs/calculate" style={{ padding: '12px 16px', background: '#f5f5f5', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ddd' }}>
            <strong>Calculate Burs</strong> - Calculate burs for a student
          </Link>
          <Link to="/burs/history" style={{ padding: '12px 16px', background: '#f5f5f5', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ddd' }}>
            <strong>History</strong> - View burs calculation history
          </Link>
          <Link to="/gallery" style={{ padding: '12px 16px', background: '#f5f5f5', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ddd' }}>
            <strong>Gallery</strong> - View gallery images
          </Link>
          <Link to="/admin/gallery" style={{ padding: '12px 16px', background: '#fff3cd', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ffc107' }}>
            <strong>Admin Gallery</strong> - Upload images to gallery
          </Link>
          <Link to="/admin/notifications" style={{ padding: '12px 16px', background: '#fff3cd', borderRadius: 6, textDecoration: 'none', color: '#333', border: '1px solid #ffc107' }}>
            <strong>Admin Notifications</strong> - Send notifications to students
          </Link>
        </div>
      </div>
    </section>
  )
}


