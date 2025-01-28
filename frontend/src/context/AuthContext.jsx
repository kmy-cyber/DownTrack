import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {jwtDecode} from 'jwt-decode';

// Crear el contexto
const AuthContext = createContext();

// Proveedor de autenticación
export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    // Simulación de carga de usuario desde localStorage
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            const userData = getUserData(token);
            setUser(userData);
        }
    }, []);

    const getUserData = (token) => {
        const decodedToken = jwtDecode(token);
        const roleClaimValue = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        return {
            id: decodedToken.sub,
            name: decodedToken.given_name,
            role: roleClaimValue
        }
    }

    // Función para iniciar sesión
    const login = (token) => {
        const userData = getUserData(token);

        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(userData));
        setUser(userData);
    };

    // Función para cerrar sesión
    const logout = () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, getUserData }}>
        {children}
        </AuthContext.Provider>
    );
};

// Hook para usar el contexto de autenticación
export const useAuth = () => {
    return useContext(AuthContext);
};
