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
  UserPlusIcon,
} from "@heroicons/react/24/solid";
import { Home, Tables, UserCreationForm, TablesSection, SectionCreationForm,TablesDepartment, DepartmentCreationForm} from "@/pages/dashboard_admin";
import { AddBusinessRounded, AddHomeRounded, Business, BusinessCenterRounded, DomainAdd, HomeMaxRounded, HomeRounded, HomeWorkRounded } from '@mui/icons-material';

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
        icon: <UserPlusIcon {...icon} />,
        name: "Add Employee",
        path: "/add_employee",
        element: <UserCreationForm />,
      },
      {
        icon: <UserGroupIcon {...icon} />,
        name: "Employees tables",
        path: "/tables_employees",
        element: <Tables />,
      },
      {
        icon: <AddBusinessRounded {...icon} />,
        name: "Add Section",
        path: "/add_section",
        element: <SectionCreationForm />,
      },
      {
        icon: <Business {...icon} />,
        name: "Section tables",
        path: "/tables_section",
        element: <TablesSection />,
      },
      {
        icon: <AddHomeRounded {...icon} />,
        name: "Add Department",
        path: "/add_department",
        element: <DepartmentCreationForm />,
      },
      {
        icon: <HomeWorkRounded {...icon} />,
        name: "Departament tables",
        path: "/tables_departament",
        element: <TablesDepartment />,
      },
    ],
  },
  
];

export default routes;
