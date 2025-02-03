import { Routes, Route, Outlet } from "react-router-dom";
import { Cog6ToothIcon } from "@heroicons/react/24/solid";
import { IconButton } from "@material-tailwind/react";
import {
    Sidenav,
    DashboardNavbar,
    Configurator,
    UserInfoSidebar,
} from "@/components/layout";
import { useMaterialTailwindController, setOpenConfigurator } from "@/context";
import routesDirector from "@/routes/routes_director";
import DepartmentsTable from "@/components/departments/departments_table"; // Asegúrate de que el componente esté importado
import {jwtDecode} from 'jwt-decode';
import InventoryTable from "@/components/inventory/inventory_table";

export function Dashboard_Director() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;
    const token = localStorage.getItem('token');
        const decodedToken = jwtDecode(token);
        const roleClaimValue = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log("ROL:", roleClaimValue);

        
    return (
        <div className="min-h-screen bg-blue-gray-50/50">
            {/* Sidebar */}
            <Sidenav routes={routesDirector} />
            <div className="p-4 xl:ml-80">
                {/* User Info Sidebar */}
                <UserInfoSidebar
                    id={decodedToken.sub}
                    name={decodedToken.given_name} 
                    role={roleClaimValue}
                />
                <DashboardNavbar />
                

                <Routes>
                    {routesDirector.map(
                        ({ layout, pages }) =>
                            layout === "dashboard/director" &&
                            pages.map(({ path, element }) => (
                                <Route key={path} path={path} element={element} />
                            ))
                    )}
                    <Route path="sections/departments" element={<DepartmentsTable />} />
                    <Route path="sections/inventory" element={<InventoryTable/>}/>
                    <Route path="sections/departments/inventory" element={<InventoryTable/>}/>
                </Routes>
                <Outlet/>
            </div>
        </div>
    );
}

Dashboard_Director.displayName = "/src/layout/dashboard_director.jsx";

export default Dashboard_Director;
