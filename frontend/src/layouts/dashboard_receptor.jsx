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

export function Dashboard_Receptor() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;

    return (
        <div className="min-h-screen bg-blue-gray-50/50">
        <Sidenav
            routes={routesReceptor}
        />
        <div className="p-4 xl:ml-80">
            <DashboardNavbar />
            
            <UserInfoSidebar
                id="12345"
                name="John Doe"
                role="Admin"
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