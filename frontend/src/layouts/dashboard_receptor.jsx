import { Routes, Route } from "react-router-dom";
import { Cog6ToothIcon } from "@heroicons/react/24/solid";
import { IconButton } from "@material-tailwind/react";
import {
    Sidenav,
    DashboardNavbar,
    Configurator,
    UserInfoSidebar,
} from "@/components/layout";
import routesReceptor from "@/routes/routes_receptor";
import { useMaterialTailwindController, setOpenConfigurator } from "@/context";
import { useEffect } from "react";
import {jwtDecode} from 'jwt-decode';

export function Dashboard_Receptor() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;
    const token = localStorage.getItem('token');
    const decodedToken = jwtDecode(token);
    const roleClaimValue = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    console.log("ROL:", roleClaimValue);


    return (
        <div className="min-h-screen bg-blue-gray-50/50">
        <Sidenav
            routes={routesReceptor}
        />
        <div className="p-4 xl:ml-80">
            <DashboardNavbar />
            
            <UserInfoSidebar
                id={decodedToken.sub}
                name={decodedToken.given_name} 
                role={roleClaimValue}
            />
            <Routes>
            {routesReceptor.map(
                ({ layout, pages }) =>
                layout === "dashboard/receptor" &&
                pages.map(({ path, element }) => (
                    <Route exact path={path} element={element} />
                ))
            )}
            </Routes>
        </div>
        </div>
    );    
}

Dashboard_Receptor.displayName = "/src/layout/dashboard_receptor.jsx";

export default Dashboard_Receptor;
