import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth, Dashboard_Admin } from "@/layouts";
import Dashboard_Technic from "./layouts/dashboard_technic";
import Dashboard_Receptor from "./layouts/dashboard_receptor";
import Dashboard_Director from "./layouts/dashboard_director";
import Dashboard_Manager from "./layouts/dashboard_manager";

function App() {
  return (
    <Routes>
      <Route path="/dashboard/admin/*" element={<Dashboard_Admin />} />
      <Route path="/dashboard/technic/*" element={<Dashboard_Technic />} />
      <Route path="/dashboard/receptor/*" element={<Dashboard_Receptor />} />
      <Route path="/dashboard/director/*" element={<Dashboard_Director />}/>
      <Route path="/dashboard/manager/*" element={<Dashboard_Manager/>}/>
      <Route path="/auth/*" element={<SignIn />} />
      <Route path="*" element={<Navigate to="/auth/" replace />} />
    </Routes>
  );
}

export default App;
