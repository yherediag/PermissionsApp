import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { env } from "process";
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  server: {
    proxy: {
      "^/api": {
        target: env.ASPNETCORE_HTTPS_PORT
          ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
          : "https://localhost:5001",
        changeOrigin: true,
        secure: false,
      },
    },
    port: 3000,
  },
});
