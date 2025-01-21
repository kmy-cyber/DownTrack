const api = async (url, options = {}) => {
    // Obtener el token de autenticación desde el almacenamiento local (o cualquier otra fuente)
    const token = localStorage.getItem('token');
    console.log(token);
    // Configurar encabezados
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        ...(token ? { Authorization:`Bearer ${token.replace(/"/g, '')}` } : {}),
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

    return fetch(api_url + url, mergedOptions);
};

export default api;

