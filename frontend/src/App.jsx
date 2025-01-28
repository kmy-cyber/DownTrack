import { Routes, Route, Navigate } from "react-router-dom";
import { SignIn, Dashboard_Admin } from "@/layouts";
import Dashboard_Technic from "./layouts/dashboard_technic";
import Dashboard_Receptor from "./layouts/dashboard_receptor";
import Dashboard_Director from "./layouts/dashboard_director";
import Dashboard_Manager from "./layouts/dashboard_manager";
import Error403 from "./pages/Error403"; 
import { AuthProvider } from "./context/AuthContext"; 
import ProtectedRoute from "./components/ProtectedRoute"; 

function App() {
  return (
    <AuthProvider>
      <Routes>
        {/* Rutas protegidas con roles específicos */}
        <Route element={<ProtectedRoute allowedRoles={['Administrator']} />}>
          <Route path="/dashboard/admin/*" element={<Dashboard_Admin />} />
        </Route>

        <Route element={<ProtectedRoute allowedRoles={['Technician']} />}>
          <Route path="/dashboard/technic/*" element={<Dashboard_Technic />} />
        </Route>

        <Route element={<ProtectedRoute allowedRoles={['EquipmentReceptor']} />}>
          <Route path="/dashboard/receptor/*" element={<Dashboard_Receptor />} />
        </Route>

        <Route element={<ProtectedRoute allowedRoles={['Director']} />}>
          <Route path="/dashboard/director/*" element={<Dashboard_Director />} />
        </Route>

        <Route element={<ProtectedRoute allowedRoles={['SectionManager']} />}>
          <Route path="/dashboard/manager/*" element={<Dashboard_Manager />} />
        </Route>

        {/* Rutas de autenticación */}
        <Route element={<ProtectedRoute allowedRoles={['*']} />}>
          <Route path="/auth/*" element={<SignIn />} />
        </Route>

        {/* Ruta de error 403 para accesos no autorizados */}
        <Route path="/403" element={<Error403 />} />

        {/* Redirección por defecto al login */}
        <Route path="*" element={<Navigate to="/auth/" replace />} />
      </Routes>
    </AuthProvider>
  );
}

export default App;
