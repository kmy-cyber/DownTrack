import { Routes, Route } from "react-router-dom";
import { Cog6ToothIcon } from "@heroicons/react/24/solid";
import { IconButton } from "@material-tailwind/react";
import {
    Sidenav,
    DashboardNavbar,
    Configurator,
    UserInfoSidebar
} from "@/components/layout";
import routesAdmin from "@/routes/routes_admin";
//import routes from "@/routes1";
import { useMaterialTailwindController, setOpenConfigurator } from "@/context";
import { useEffect } from "react";

export function Dashboard_Admin() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;

    return (
        <div className="min-h-screen bg-blue-gray-50/50">
        <Sidenav
            routes={routesAdmin}
            
        />
        <div className="p-4 xl:ml-80">
            <DashboardNavbar />
            <UserInfoSidebar
                id="12345"
                name="John Doe"
                role="Admin"
            />
            <Configurator />
            <IconButton
            size="lg"
            color="white"
            className="fixed bottom-8 right-8 z-40 rounded-full shadow-blue-gray-900/10"
            ripple={false}
            onClick={() => setOpenConfigurator(dispatch, true)}
            >
            <Cog6ToothIcon className="h-5 w-5" />
            </IconButton>
            <Routes>
            {routesAdmin.map(
                ({ layout, pages }) =>
                layout === "dashboard/admin" &&
                pages.map(({ path, element }) => (
                    <Route exact path={path} element={element} />
                ))
            )}
            </Routes>
        </div>
        </div>
    );    
}

Dashboard_Admin.displayName = "/src/layout/dashboard_admin.jsx";

export default Dashboard_Admin;
