import {
    HomeIcon,
    ArrowDownCircleIcon,
    WrenchScrewdriverIcon,
    ClipboardDocumentListIcon,
} from "@heroicons/react/24/solid";
import { Home, EquipmentInventory, MaintenanceCreationForm, LeaveCreationForm,EquipmentMaintenance } from "@/pages/dashboard_technic";
import {AutoMode} from '@mui/icons-material';

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
        icon: <WrenchScrewdriverIcon {...icon} />,
        name: "maintenance",
        path: "/maintenance",
        element: <MaintenanceCreationForm />,
        },
        {
            icon: <AutoMode {...icon} />,
            name: "equipment_maintenance",
            path: "/equipment_maintenance",
            element: <EquipmentMaintenance />,
            },
        {
        icon: <ArrowDownCircleIcon {...icon} />,
        name: "technical leave",
        path: "/leave",
        element: <LeaveCreationForm />,
        },
        {
        icon: <ClipboardDocumentListIcon{...icon} />,
        name: "Equipment Inventory",
        path: "/equipment_inventory",
        element: <EquipmentInventory />,
        },
    ],
},];

export default routesTechnic;
