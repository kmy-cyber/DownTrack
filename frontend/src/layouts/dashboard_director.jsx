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
import routesDirector from "@/routes/routes_director";
import SectionDetails from "@/components/sections/section_details"; // Import SectionDetails
import SectionsTable from "@/components/sections/sections_table"; // Import SectionsTable

export function Dashboard_Director() {
    const [controller, dispatch] = useMaterialTailwindController();
    const { sidenavType } = controller;

    return (
        <div className="min-h-screen bg-blue-gray-50/50">
            {/* Sidebar */}
            <Sidenav routes={routesDirector} />
            <div className="p-4 xl:ml-80">
                {/* User Info Sidebar */}
                <UserInfoSidebar id="12345" name="John Doe" role="Director" />
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
                    {routesDirector.map(
                        ({ layout, pages }) =>
                            layout === "dashboard/director" &&
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

Dashboard_Director.displayName = "/src/layout/dashboard_director.jsx";

export default Dashboard_Director;
