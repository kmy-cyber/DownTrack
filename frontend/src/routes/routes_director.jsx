import {
    HomeIcon,
    RectangleStackIcon,
    NewspaperIcon,
  } from "@heroicons/react/24/solid";
import { Home } from "@/pages/dashboard_director";
import { SectionsTable } from "@/components/sections";
import { Reports } from "@/components/reports/reports"
import { element } from "prop-types";
  
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
        }
        
    ],
},];

export default routesDirector;
  