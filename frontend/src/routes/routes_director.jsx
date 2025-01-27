import {
    HomeIcon,
    RectangleStackIcon,
    NewspaperIcon,
    CubeIcon,
    MinusCircleIcon,
    WrenchScrewdriverIcon,
    ArrowsRightLeftIcon,
    UserIcon
  } from "@heroicons/react/24/solid";
import { Home } from "@/pages/dashboard_director";
import { SectionsTable } from "@/components/sections";
import { Reports } from "@/components/reports/reports"
import InventoryTable from "@/components/inventory/inventory_table";
import EquipmentDisposalTable from "@/pages/dashboard_director/disposals_table";
import MaintenanceHistory from "@/pages/dashboard_director/maintenances_table";
import EquipmentTransferTable  from "@/pages/dashboard_director/equipment_transfers_table";
import UserTable from "@/pages/dashboard_director/employees_table";
  
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
          element: <InventoryTable/>
        },
        {
          icon: <MinusCircleIcon {...icon}/>,
          name: "Disposals",
          path: "/disposals",
          element: <EquipmentDisposalTable/>
        },
        {
          icon: <WrenchScrewdriverIcon {...icon}/>,
          name: "Maintenances",
          path: "/maintenances",
          element: <MaintenanceHistory/>
        },
        {
          icon: <ArrowsRightLeftIcon {...icon}/>,
          name: "Transfers",
          path: "/transfers",
          element: <EquipmentTransferTable/>
        },
        {
          icon: <UserIcon {...icon}/>,
          name: "Employees",
          path: "/employees",
          element: <UserTable/>
        }
        
    ],
},];

export default routesDirector;
  