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
import { useEffect } from "react";
import routesManager from "@/routes/routes_manager";
import SectionDetails from "@/components/sections/section_details"; import {jwtDecode} from 'jwt-decode';

export function Dashboard_Manager() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;
    const token = localStorage.getItem('token');
        const decodedToken = jwtDecode(token);
        const roleClaimValue = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log("ROL:", roleClaimValue);


    return (
        <div className="min-h-screen bg-blue-gray-50/50">
            {/* Sidebar */}
            <Sidenav routes={routesManager} />
            <div className="p-4 xl:ml-80">
                {/* User Info Sidebar */}
                <UserInfoSidebar 
                    id={decodedToken.sub}
                    name={decodedToken.given_name} 
                    role={roleClaimValue}
                />
                <DashboardNavbar />
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
                    {routesManager.map(
                        ({ layout, pages }) =>
                            layout === "dashboard/manager" &&
                            pages.map(({ path, element }) => (
                                <Route key={path} path={path} element={element} />
                            ))
                    )}

                    <Route path="sections/:sectionId" element={<SectionDetails/>} />
                    {/* <Route path="sections/:sectionId/:departments/:departmentId" element={<DepartmentDetails/>}/> */}
                </Routes>
                <Outlet/>
            </div>
        </div>
    );
}

Dashboard_Manager.displayName = "/src/layout/dashboard_manager.jsx";

export default Dashboard_Manager;
