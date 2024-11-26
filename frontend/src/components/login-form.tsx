import { useState } from "react"
import { Moon, Sun } from 'lucide-react'

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

export function LoginForm() {
  const [isDarkMode, setIsDarkMode] = useState(false)

  const toggleTheme = () => {
    setIsDarkMode(!isDarkMode)
    document.documentElement.classList.toggle("dark")
  }

  return (
    <div className="min-h-screen w-full bg-gradient-to-br from-blue-100 to-blue-300 dark:from-blue-900 dark:to-blue-950 p-4 flex items-center justify-center relative">
      <Button
        variant="ghost"
        size="icon"
        className="absolute top-4 right-4 rounded-full text-zinc-900 dark:text-white"
        onClick={toggleTheme}
      >
        {isDarkMode ? <Sun className="h-6 w-6" /> : <Moon className="h-6 w-6" />}
      </Button>
      
      <div className="w-full max-w-3xl bg-white dark:bg-zinc-900 rounded-3xl shadow-2xl overflow-hidden flex">
        <div className="w-1/2 p-8">
          <div className="space-y-6">
            <div className="justify-center justify-items-center gap-2 text-zinc-900 dark:text-white">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="currentColor"
                className="w-16 h-16"
              >
                <path
                  fillRule="evenodd"
                  d="M18.685 19.097A9.723 9.723 0 0021.75 12c0-5.385-4.365-9.75-9.75-9.75S2.25 6.615 2.25 12a9.723 9.723 0 003.065 7.097A9.716 9.716 0 0012 21.75a9.716 9.716 0 006.685-2.653zm-12.54-1.285A7.486 7.486 0 0112 15a7.486 7.486 0 015.855 2.812A8.224 8.224 0 0112 20.25a8.224 8.224 0 01-5.855-2.438zM15.75 9a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0z"
                  clipRule="evenodd"
                />
              </svg>
              <h1 className="text-2xl font-semibold">Sign In</h1>
            </div>
            
            <div className="space-y-4">
              <div className="space-y-2">
                {/* <Label htmlFor="id" className="text-zinc-900 dark:text-white">ID</Label> */}
                <Input
                  id="id"
                  placeholder="Enter your ID"
                  type="text"
                  className="bg-gray-100 dark:bg-zinc-800 text-zinc-900 dark:text-white"
                />
              </div>
              
              <div className="space-y-2">
                {/* <Label htmlFor="password" className="text-zinc-900 dark:text-white">Password</Label> */}
                <Input
                  id="password"
                  placeholder="Enter your password"
                  type="password"
                  className="bg-gray-100 dark:bg-zinc-800 text-zinc-900 dark:text-white"
                />
              </div>
            </div>
            
            <Button className="w-full bg-zinc-900 text-white hover:bg-zinc-800 dark:bg-white dark:text-zinc-900 dark:hover:bg-gray-200">
              SIGN IN
            </Button>
          </div>
        </div>
        
        <div className="w-1/2 bg-[#1a237e] p-8 flex flex-col justify-center text-white rounded-l-[80px]">
          <h2 className="text-2xl font-bold mb-2">WELCOME TO DOWNTRACK</h2>
          <p className="text-sm opacity-80">
            Register with your professional account to use this app
          </p>
        </div>
      </div>
    </div>
  )
}