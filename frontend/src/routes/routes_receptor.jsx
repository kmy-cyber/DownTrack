import {
    HomeIcon,
    InboxArrowDownIcon,
    ArrowRightCircleIcon,
    TrashIcon,
    NewspaperIcon,
} from "@heroicons/react/24/solid";
import { Home, EquipmentRegisterForm, EquipmentTransferTable, EquipmentDisposalTable} from "@/pages/dashboard_receptor";
import Reports from "@/components/reports/reports";
import { NewspaperOutlined } from "@mui/icons-material";

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
        element: <EquipmentRegisterForm />,
        },
        {
        icon: <ArrowRightCircleIcon {...icon} />,
        name: "Equipment Transfers",
        path: "/equipment_transfers",
        element: <EquipmentTransferTable />,
        },
        {
        icon: <TrashIcon {...icon} />,
        name: "Equipment Disposal",
        path: "/equipment_disposal",
        element: <EquipmentDisposalTable />,
        },
        {
            icon: <NewspaperIcon {...icon} />,
            name: "Reports",
            path: "/reports",
            element: <Reports />,
        },
    ],
},];

export default routesReceptor;
