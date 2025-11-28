import { Link, NavLink } from 'react-router-dom'

const navStyle: React.CSSProperties = {
  background: '#fff',
  borderBottom: '1px solid #e5e5e5',
  position: 'sticky',
  top: 0,
  zIndex: 10
}

const innerStyle: React.CSSProperties = {
  maxWidth: 1200,
  margin: '0 auto',
  padding: '12px 16px',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  gap: 16
}

const linksStyle: React.CSSProperties = {
  display: 'flex',
  gap: 12
}

const linkStyle: React.CSSProperties = {
  textDecoration: 'none',
  padding: '6px 10px',
  borderRadius: 6,
  color: '#333'
}

export const Navbar = () => {
  return (
    <nav style={navStyle}>
      <div style={innerStyle}>
        <Link to="/" style={{ ...linkStyle, fontWeight: 700 }}>
          Yurt Bursu
        </Link>
        <div style={linksStyle}>
          <NavLink to="/" style={linkStyle}>Home</NavLink>
          <NavLink to="/student/create" style={linkStyle}>Create Student</NavLink>
          <NavLink to="/burs/calculate" style={linkStyle}>Calculate Burs</NavLink>
          <NavLink to="/burs/history" style={linkStyle}>History</NavLink>
          <NavLink to="/gallery" style={linkStyle}>Gallery</NavLink>
          <NavLink to="/admin/gallery" style={linkStyle}>Admin Gallery</NavLink>
          <NavLink to="/admin/notifications" style={linkStyle}>Notifications</NavLink>
        </div>
      </div>
    </nav>
  )
}


