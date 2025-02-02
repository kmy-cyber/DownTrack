import {
    HomeIcon,
    ArrowDownCircleIcon,
    WrenchScrewdriverIcon,
    ClipboardDocumentListIcon,
} from "@heroicons/react/24/solid";
import { Home, EquipmentInventory, MaintenanceCreationForm, LeaveCreationForm,EquipmentMaintenance, MaintenanceHistory} from "@/pages/dashboard_technic";
import {AssignmentOutlined, AutoMode} from '@mui/icons-material';


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
            name: "equipment_maintenance",
            path: "/equipment_maintenance",
            element: <EquipmentMaintenance />,
        },
        {
        icon: <ClipboardDocumentListIcon{...icon} />,
        name: "Equipment Inventory",
        path: "/equipment_inventory",
        element: <EquipmentInventory />,
        },
        {
            icon: <WrenchScrewdriverIcon {...icon} />,
            name: "insert maintenance",
            path: "/insert_maintenance/:id/:name/:type",
            element: <MaintenanceCreationForm />,
            hidden: true,
        },
        {
            icon: <ArrowDownCircleIcon {...icon} />,
            name: "insert technical leave",
            path: "/insert_technical_leave/:id/:name/:type",
            element: <LeaveCreationForm />,
            hidden: true,
        },
        {
            icon: <AssignmentOutlined {...icon} />,
            name: "maintenance history",
            path: "/maintenance_history",
            element: <MaintenanceHistory />,
        },
    ],
},];

export default routesTechnic;
