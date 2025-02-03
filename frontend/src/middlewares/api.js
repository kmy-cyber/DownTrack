import { toast } from 'react-toastify';

const api = async (url, options = {}) => {
    // Obtener el token de autenticación desde el almacenamiento local (o cualquier otra fuente)
    const token = localStorage.getItem('token');
    // console.log(token);

    // Configurar encabezados
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        ...(token ? { Authorization: `Bearer ${token.replace(/"/g, '')}` } : {}),
        'token': token
    };

    // Fusionar encabezados existentes con los nuevos
    const mergedOptions = {
        ...options,
        headers: {
            ...headers,
            ...options.headers
        }
    };

    const api_url = 'http://localhost:5217/api';

    try {
        // Realizar la solicitud
        const response = await fetch(api_url + url, mergedOptions);

        // Si la respuesta no es exitosa, lanzar un error
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Error en la solicitud');
        }

        // Retornar la respuesta en formato JSON
        return response;
    } catch (error) {
        // Mostrar notificación de error con Toastify
        toast.error(error.message || 'Ocurrió un error inesperado');
        throw error; // Re-lanzar el error para que pueda ser manejado por el llamador
    }
};

export default api;