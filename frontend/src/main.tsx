import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { DashboardComponent } from './components/dashboard'
import { LoginForm } from './components/login-form'
//const login_verificate = false

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <LoginForm />
  </StrictMode>,
)
