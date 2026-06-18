import { defineConfig } from 'vite'

// GitHub Pages project site is served from /<repo>/ — here https://thorium.github.io/thomoku/
export default defineConfig({
  base: '/thomoku/',
  build: {
    outDir: 'dist',
    emptyOutDir: true
  }
})
