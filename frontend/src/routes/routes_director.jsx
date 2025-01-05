import {
    HomeIcon,
    RectangleStackIcon,
    NewspaperIcon,
    CubeIcon,
  } from "@heroicons/react/24/solid";
import { Home } from "@/pages/dashboard_director";
import { SectionsTable } from "@/components/sections";
import { Reports } from "@/components/reports/reports"
import GeneralInventoryTable from "@/components/inventory/general_inventory_table";
  
  const typeUser = 1;
  
  const icon = {
    className: "w-5 h-5 text-inherit",
  };
  
export const routesDirector = [
{
    layout: "dashboard/director",
    pages: [
        {
            icon: <HomeIcon {...icon} />,
            name: "overview",
            path: "/home",
            element: <Home/>,
        },
        {
            icon: <RectangleStackIcon {...icon}/>  ,
            name: "Sections",
            path: "/sections",
            element: <SectionsTable/>,
        },
        {
          icon: <NewspaperIcon {...icon}/>,
          name: "Reports",
          path: "/reports",
          element: <Reports/>
        },
        {
          icon: <CubeIcon {...icon}/>,
          name: "Inventory",
          path: "/inventory",
          element: <GeneralInventoryTable/>
        }
        
    ],
},];

export default routesDirector;
  