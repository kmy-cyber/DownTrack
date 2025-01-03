import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth, Dashboard_Admin } from "@/layouts";
import Dashboard_Technic from "./layouts/dashboard_technic";
import Dashboard_Receptor from "./layouts/dashboard_receptor";

function App() {
  return (
    <Routes>
      <Route path="/dashboard/admin/*" element={<Dashboard_Admin />} />
      <Route path="/dashboard/technic/*" element={<Dashboard_Technic />} />
      <Route path="/dashboard/receptor/*" element={<Dashboard_Receptor />} />
      <Route path="/dashboard/*" element={<Dashboard />} />
      <Route path="/auth/*" element={<Auth />} />
      <Route path="*" element={<Navigate to="/dashboard/home" replace />} />
    </Routes>
  );
}

export default App;
