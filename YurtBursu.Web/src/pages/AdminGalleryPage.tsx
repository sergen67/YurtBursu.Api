import { useState } from 'react'
import { galleryApi } from '../api/galleryApi'
import { supabase } from '../lib/supabase'
import { setBasicAuth as setGalleryAuth } from '../api/galleryApi'

async function uploadImage(file: File): Promise<string> {
  const filePath = `gallery/${Date.now()}-${file.name}`;

  const { error: uploadError } = await supabase.storage
    .from("gallery")
    .upload(filePath, file);

  if (uploadError) {
    if (uploadError.message.includes('Bucket not found') || uploadError.message.includes('does not exist')) {
      throw new Error('Storage bucket "gallery" not found. Please create it in Supabase Dashboard > Storage.');
    }
    throw uploadError;
  }

  const { data: urlData } = supabase.storage
    .from("gallery")
    .getPublicUrl(filePath);

  if (!urlData?.publicUrl) {
    throw new Error('Failed to get public URL from Supabase Storage');
  }

  return urlData.publicUrl;
}

export const AdminGalleryPage = () => {
  const [file, setFile] = useState<File | null>(null)
  const [url, setUrl] = useState<string>('')
  const [toast, setToast] = useState<string>('')
  const [toastType, setToastType] = useState<'success' | 'error' | null>(null)
  const [authUser, setAuthUser] = useState('')
  const [authPass, setAuthPass] = useState('')
  const [loading, setLoading] = useState(false)

  const allowedTypes = new Set(['image/jpeg', 'image/png', 'image/webp'])

  const onUpload = async () => {
    if (!file) {
      setToast('Please select a file first.')
      setToastType('error')
      return
    }
    
    if (!allowedTypes.has(file.type)) {
      setToast('Only JPEG, PNG, or WEBP files are allowed.')
      setToastType('error')
      return
    }

    setLoading(true)
    setToast('')
    setToastType(null)
    setUrl('')

    try {
      console.log('Starting upload...', { fileName: file.name, fileSize: file.size, fileType: file.type })
      
      // configure BasicAuth for this admin call
      if (authUser && authPass) {
        setGalleryAuth(authUser, authPass)
      }

      console.log('Uploading to Supabase Storage...')
      const downloadUrl = await uploadImage(file)
      console.log('Download URL obtained:', downloadUrl)
      setUrl(downloadUrl)
      
      console.log('Saving URL to backend...')
      const item = await galleryApi.upload(downloadUrl)
      console.log('Backend save successful:', item)
      
      setToast(`Successfully uploaded and saved! Gallery item ID: #${item.id}`)
      setToastType('success')
      setFile(null)
    } catch (error: any) {
      console.error('Gallery upload error:', error)
      let errorMsg = error?.response?.data?.error || error?.message || error?.code || 'Upload failed'
      
      setToast(`Error: ${errorMsg}`)
      setToastType('error')
    } finally {
      setLoading(false)
    }
  }

  return (
    <section>
      <h2>Admin Gallery Upload</h2>
      <div style={{ display: 'grid', gap: 12, maxWidth: 520 }}>
        <label>
          Basic Auth Username
          <input value={authUser} onChange={e => setAuthUser(e.target.value)} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <label>
          Basic Auth Password
          <input type="password" value={authPass} onChange={e => setAuthPass(e.target.value)} style={{ width: '100%', padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />
        </label>
        <input 
          type="file" 
          accept="image/*" 
          onChange={e => {
            const selectedFile = e.target.files?.[0] ?? null
            setFile(selectedFile)
            setToast('')
            setToastType(null)
            setUrl('')
          }} 
          disabled={loading}
        />
        <button 
          onClick={onUpload} 
          disabled={!file || loading} 
          style={{ 
            padding: '10px 14px', 
            borderRadius: 6,
            background: (!file || loading) ? '#ccc' : '#1890ff',
            color: 'white',
            border: 'none',
            cursor: (!file || loading) ? 'not-allowed' : 'pointer'
          }}
        >
          {loading ? 'Uploading...' : 'Upload to Storage & Save URL'}
        </button>
        
        {loading && (
          <div style={{ padding: 8, background: '#e6f7ff', borderRadius: 4, color: '#1890ff' }}>
            Uploading file to Supabase Storage...
          </div>
        )}
        
        {toast && (
          <div style={{ 
            padding: 8, 
            borderRadius: 4,
            background: toastType === 'error' ? '#ffe6e6' : '#f6ffed',
            color: toastType === 'error' ? '#c00' : '#2b7a0b',
            border: `1px solid ${toastType === 'error' ? '#ffccc7' : '#b7eb8f'}`
          }}>
            {toast}
          </div>
        )}
        
        {url && (
          <div style={{ padding: 8, background: '#f5f5f5', borderRadius: 4 }}>
            <strong>Last Upload URL:</strong>
            <br />
            <a href={url} target="_blank" rel="noreferrer" style={{ color: '#1890ff', wordBreak: 'break-all' }}>
              {url}
            </a>
          </div>
        )}
      </div>
    </section>
  )
}
