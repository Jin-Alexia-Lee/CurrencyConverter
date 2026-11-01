import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // If changing this port, update the backend CORS settings as well (CurrencyConverter.Api/Program.cs).
    port: 5173,
    strictPort: true,
  },
});
