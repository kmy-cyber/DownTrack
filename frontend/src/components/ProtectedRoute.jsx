import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '@/context/AuthContext'; 

const ProtectedRoute = ({ allowedRoles }) => {
    const { user } = useAuth();

    // Si no hay usuario autenticado, redirigir al login
    if (!user && window.location.pathname !== '/auth/sign-in') {
        return <Navigate to="/auth/sign-in" replace />;
    }

    if (user && window.location.pathname === '/auth/sign-in') {
        // Redirect to the appropriate dashboard based on the user's role
        switch (user.role) {
            case 'Administrator':
                return <Navigate to="/dashboard/admin/home" replace />;
            case 'Director':
                return <Navigate to="/dashboard/director/home" replace />;
            case 'SectionManager':
                return <Navigate to="/dashboard/manager/home" replace />;
            case 'Technician':
                return <Navigate to="/dashboard/technic/home" replace />;
            case 'EquipmentReceptor':
                return <Navigate to="/dashboard/receptor/home" replace />;
        }
    }

    // Si el rol del usuario no está permitido, mostrar error 403
    if (user && !allowedRoles.includes(user.role)) {
        return <Navigate to="/403" replace />;
    }

    return <Outlet />;
};

export default ProtectedRoute;
