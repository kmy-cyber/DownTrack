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
    ArrowDownCircleIcon,
    WrenchIcon,
} from "@heroicons/react/24/solid";
import { Home, MaintenanceHistoryTable, MaintenanceCreationForm, LeaveCreationForm } from "@/pages/dashboard_technic";

const typeUser = 1;
const icon = {
    className: "w-5 h-5 text-inherit",
};

export const routesTechnic = [
{
    layout: "dashboard/technic",
    pages: [
        {
        icon: <HomeIcon {...icon} />,
        name: "dashboard",
        path: "/home",
        element: <Home />,
        },
        {
        icon: <TableCellsIcon {...icon} />,
        name: "maintenance",
        path: "/maintenance",
        element: <MaintenanceCreationForm />,
        },
        {
        icon: <ArrowDownCircleIcon {...icon} />,
        name: "technical leave",
        path: "/leave",
        element: <LeaveCreationForm />,
        },
        {
        icon: <WrenchIcon {...icon} />,
        name: "Maintenance History",
        path: "/maintenance_hist",
        element: <MaintenanceHistoryTable />,
        },
    ],
},];

export default routesTechnic;
