import {
  HomeIcon,
  UserCircleIcon,
  TableCellsIcon,
  InformationCircleIcon,
  ServerStackIcon,
  RectangleStackIcon,
  FireIcon,
  ArrowPathIcon,
  UserGroupIcon,
} from "@heroicons/react/24/solid";
import { Home, Tables, UserCreationForm } from "@/pages/dashboard_admin";
//import { Home } from "@/pages/dashboard";

const typeUser = 1;

const icon = {
  className: "w-5 h-5 text-inherit",
};

export const routes = [
  {
    layout: "dashboard/admin",
    pages: [
      {
        icon: <HomeIcon {...icon} />,
        name: "dashboard",
        path: "/home",
        element: <Home />,
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "tables",
        path: "/tables",
        element: <Tables />,
      },
      {
        icon: <UserGroupIcon {...icon} />,
        name: "Add Employee",
        path: "/add_employee",
        element: <UserCreationForm />,
      },
      //{
      //  icon: <InformationCircleIcon {...icon} />,
      //  name: "notifications",
      //  path: "/admin/notifications",
      //  element: <Notifications />,
      //},
    ],
  },
  
];

export default routes;
