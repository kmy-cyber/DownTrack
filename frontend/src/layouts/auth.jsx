import React, { useState, useEffect } from "react";
import {
  Card,
  Input,
  Button,
  Typography,
} from "@material-tailwind/react";
import { UserCircleIcon } from "@heroicons/react/24/solid";
import "@/assets/css/mystyles.css";
import {jwtDecode} from 'jwt-decode';


const getDashboardPath = (role) => {
  switch (role) {
    case 'Administrator':
      return '/dashboard/admin/home';
    case 'Director':
      return '/dashboard/director/home';
    case 'SectionManager':
      return '/dashboard/manager/home';
    case 'Technician':
      return '/dashboard/technic/home';
    case 'EquipmentReceptor':
      return '/dashboard/receptor/home';
    default:
      return '/auth/';
  }
};

export function SignIn() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [shouldRedirect, setShouldRedirect] = useState(false);
  const [token, setToken] = useState('');

  const [userGlobalData, setUserGlobalData] = useState({
    id: "",
    name: "",
    //email: "",
    role: ""
  })

  const handleSubmit = async (e) => {
    console.log("Login ");
    e.preventDefault(); // Previene la recarga de la página
    setIsLoading(true);
    setError(null);
    try {
      const response = await fetch("http://localhost:5217/api/Authentication/login/", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      console.log(response.status);
      
      // if (!response.ok) {
        //   throw new Error("Login failed");
        // }
        
        const token = await response.text(); // Obtenemos el token como texto
        console.log(response.ok);

        setToken(token);
        localStorage.setItem('token', token); // Guardar token en localStorage

      
      if (response.ok && token) {
        console.log("Token obtenido:", token);
        
        // Decodificamos el token JWT
        const decodedToken = jwtDecode(token);
        const roleClaimValue = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log("ROL:", roleClaimValue);
        // Extraemos las propiedades relevantes del token decodificado
      const userData = {
        id: decodedToken.sub,
        name: decodedToken.given_name,
        //email: decodedToken.email,
        role: roleClaimValue
      }
      console.log("Datos del usuario:", userData);
      console.log("Datos del token:", decodedToken);
      // Guardamos los datos del usuario globalmente
      setUserGlobalData(userData)

      // Redirige al dashboard si el login es correcto
      const dashboardPath = getDashboardPath(userData.role);
        console.log("Dashboard path:", dashboardPath);
        setShouldRedirect(dashboardPath);
      } else {
        console.error("Login failed");
        setError("Failed to login");
      }
      
      
    } catch (error) {
      console.error("Error logging in:", error);
      setError('An error occurred during the login process');
    } finally {
      setIsLoading(false);
    }
  };
  useEffect(() => {
    if (shouldRedirect && !isLoading) {
      window.location.href = shouldRedirect;
    }
  }, [shouldRedirect, isLoading]);


  return (
    <section className="m-8 flex gap-4">
      <div className="w-full lg:w-3/5 mt-24">
        <div className="text-center">
          <UserCircleIcon className="user-circle"></UserCircleIcon>
          <Typography variant="h2" className="font-bold mb-4">Sign In</Typography>
          <Typography variant="paragraph" color="blue-gray" className="text-lg font-normal">Enter your ID and Password.</Typography>
        </div>
        <form
          className="mt-8 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2"
          onSubmit={handleSubmit}
        >
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Username:
            </Typography>
            <Input
              size="lg"
              placeholder="Enter your username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="!border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
            />
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Password
            </Typography>
            <Input
              type="password"
              size="lg"
              placeholder="********"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="!border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
            />
          </div>
          <Button className="mt-6" fullWidth type="submit">
            Sign In
          </Button>
        </form>
      </div>
      <div className="w-2/5 h-full hidden lg:block">
        <img
          src="/img/pattern.png"
          className="h-full w-full object-cover rounded-3xl"
        />
      </div>
    </section>
  );
}

export default SignIn;
