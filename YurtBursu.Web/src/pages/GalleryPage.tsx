import { useEffect, useState } from 'react'
import { galleryApi, type GalleryItem } from '../api/galleryApi'

export const GalleryPage = () => {
  const [items, setItems] = useState<GalleryItem[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    (async () => {
      setLoading(true)
      setError(null)
      try {
        const data = await galleryApi.list()
        setItems(data)
      } catch (err: any) {
        setError(err?.response?.data?.error ?? err?.message ?? 'Request failed')
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  return (
    <section>
      <h2>Gallery</h2>
      {loading && <SkeletonGrid />}
      {error && <div style={{ color: '#c00' }}>Error: {error}</div>}
      <div style={gridStyle}>
        {items.map(it => (
          <div key={it.id} style={cardStyle}>
            <img src={it.imageUrl} alt={`img-${it.id}`} loading="lazy" style={{ width: '100%', display: 'block', borderRadius: 8 }} />
            <div style={{ fontSize: 12, color: '#666', marginTop: 6 }}>{new Date(it.createdAt).toLocaleString()}</div>
          </div>
        ))}
      </div>
    </section>
  )
}

const SkeletonGrid = () => {
  return (
    <div style={gridStyle}>
      {Array.from({ length: 8 }).map((_, i) => (
        <div key={i} style={{ ...cardStyle, background: '#f2f2f2', height: 180, animation: 'pulse 1.5s infinite ease-in-out' }} />
      ))}
      <style>
        {`
        @keyframes pulse {
          0% { opacity: 0.6; }
          50% { opacity: 1; }
          100% { opacity: 0.6; }
        }
        `}
      </style>
    </div>
  )
}

const gridStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(220px, 1fr))',
  gap: 12
}

const cardStyle: React.CSSProperties = {
  background: '#fff',
  border: '1px solid #eee',
  padding: 8,
  borderRadius: 8
}


