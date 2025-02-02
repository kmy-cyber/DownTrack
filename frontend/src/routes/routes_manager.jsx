import {
  HomeIcon,
  RectangleStackIcon,
  NewspaperIcon,
  DocumentCheckIcon,
  CubeIcon,
} from "@heroicons/react/24/solid";
import { Home } from "@/pages/dashboard_manager/home_manager";
import { SectionsTable } from "@/components/sections";
import { Evaluation } from "@/pages/dashboard_manager/evaluation";
import { Reports } from "@/components/reports/reports";
import InventoryTable from "@/components/inventory/inventory_table";

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
        element: <Home />,
      },
      {
        icon: <RectangleStackIcon {...icon} />,
        name: "Sections",
        path: "/sections",
        element: <SectionsTable />,
      },
      {
        icon: <NewspaperIcon {...icon} />,
        name: "Reports",
        path: "/reports",
        element: <Reports />,
      },
      {
        icon: <CubeIcon {...icon} />,
        name: "Inventory",
        path: "/inventory",
        element: <InventoryTable />,
      },
      {
        icon: <DocumentCheckIcon {...icon} />,
        name: "Evaluate",
        path: "/evaluation",
        element: <Evaluation />,
      },
    ],
  },
];

export default routesManager;
