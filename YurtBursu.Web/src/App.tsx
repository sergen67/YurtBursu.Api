import { AppRouter } from './router/AppRouter'
import { Navbar } from './components/Layout/Navbar'
import { Container } from './components/Layout/Container'

function App() {
  return (
    <>
      <Navbar />
      <Container>
        <AppRouter />
      </Container>
    </>
  )
}

export default App


