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
    InboxArrowDownIcon,
    ArrowDownRightIcon,
    ArrowRightCircleIcon,
    TrashIcon,
} from "@heroicons/react/24/solid";
import { Home} from "@/pages/dashboard_receptor";

const typeUser = 1;
const icon = {
    className: "w-5 h-5 text-inherit",
};

export const routesReceptor = [
{
    layout: "dashboard/receptor",
    pages: [
        {
        icon: <HomeIcon {...icon} />,
        name: "dashboard",
        path: "/home",
        element: <Home />,
        },
        {
        icon: <InboxArrowDownIcon {...icon} />,
        name: "Equipment Reception",
        path: "/equipment_reception",
        element: <Home />,
        },
        {
        icon: <ArrowRightCircleIcon {...icon} />,
        name: "Equipment Transfers",
        path: "/equipment_transfers",
        element: <Home />,
        },
        {
        icon: <TrashIcon {...icon} />,
        name: "Equipment Disposal",
        path: "/equipment_disposal",
        element: <Home />,
        },
    ],
},];

export default routesReceptor;
