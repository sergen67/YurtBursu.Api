import { PropsWithChildren } from 'react'

const containerStyle: React.CSSProperties = {
  maxWidth: 1200,
  margin: '0 auto',
  padding: '16px'
}

export const Container = ({ children }: PropsWithChildren) => {
  return (
    <main style={containerStyle}>
      {children}
    </main>
  )
}


