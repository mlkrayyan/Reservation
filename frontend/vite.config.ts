import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // The SPA now uses a relative /api base. In dev the API is a separate
    // process on 5139, so proxy to it rather than reintroducing an absolute URL.
    proxy: {
      '/api': {
        target: 'http://localhost:5139',
        changeOrigin: true,
      },
    },
  },
})
