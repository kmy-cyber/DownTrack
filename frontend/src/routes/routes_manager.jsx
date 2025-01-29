import {
    HomeIcon,
    RectangleStackIcon,
    ArrowsRightLeftIcon,
    DocumentCheckIcon,
    CubeIcon
  } from "@heroicons/react/24/solid";
import { Home } from "@/pages/dashboard_manager/home";
import { SectionsTable } from "@/components/sections";
import { Evaluation } from "@/pages/dashboard_manager/evaluation";
import TransferRequestForm from "@/pages/dashboard_manager/request_transfer";
import InventoryTable from "@/components/inventory/inventory_table";
  
  const typeUser = 1;
  
  const icon = {
    className: "w-5 h-5 text-inherit",
  };
  
export const routesManager = [
{
    layout: "dashboard/manager",
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
          // {
          //   icon: <ArrowsRightLeftIcon {...icon}/>,
          //   name: "Request Transfer",
          //   path: "/transfer_request",
          //   element: <TransferRequestForm/>
          // },
        {
          icon: <CubeIcon {...icon}/>,
          name: "Inventory",
          path: "/inventory",
          element: <InventoryTable/>
        },
        {
          icon: <DocumentCheckIcon {...icon}/>,
          name:"Evaluate",
          path:"/evaluation",
          element: <Evaluation/>
        }
        
    ],
},];

export default routesManager;
  