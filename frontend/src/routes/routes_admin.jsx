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
  RectangleGroupIcon,
} from "@heroicons/react/24/solid";
import { Home, Tables, UserCreationForm, TablesSection, SectionCreationForm,TablesDepartment, DepartmentCreationForm} from "@/pages/dashboard_admin";
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
        name: "Employees tables",
        path: "/tables_employees",
        element: <Tables />,
      },
      {
        icon: <UserGroupIcon {...icon} />,
        name: "Add Employee",
        path: "/add_employee",
        element: <UserCreationForm />,
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "Section tables",
        path: "/tables_section",
        element: <TablesSection />,
      },
      {
        icon: <RectangleStackIcon {...icon} />,
        name: "Add Section",
        path: "/add_section",
        element: <SectionCreationForm />,
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "Departament tables",
        path: "/tables_departament",
        element: <TablesDepartment />,
      },
      {
        icon: <RectangleGroupIcon {...icon} />,
        name: "Add Department",
        path: "/add_department",
        element: <DepartmentCreationForm />,
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
