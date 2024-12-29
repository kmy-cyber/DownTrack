import {
    Card,
    CardHeader,
    CardBody,
    Typography,
    Avatar,
    Chip,
    Tooltip,
    Progress,
    } from "@material-tailwind/react";
    import {EditMaintenanceForm} from "@/pages/dashboard_technic/edit_maintenance";
    import { UserIcon } from "@heroicons/react/24/outline";
    import { maintenanceHistData } from "@/data/maintenance-hist-data";
    import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
    import { useState } from "react";
    
    export function MaintenanceHistoryTable() {
        const [MaintenanceList, setMaintenanceList] = useState(maintenanceHistData);
        const [onEdit, setOnEdit] = useState(false);
        const [keyEdit, setKeyEdit] = useState(0);
        const [MaintenanceData, setMaintenanceData] = useState({
            equipment: "",
            maintenanceType: "",
            technician: "",
            date: "",
            cost: "",
        });
    
        // TODO: Connect with backend and replace static values
        
        const handleSave = (updatedMaintenance) => {
            MaintenanceList[keyEdit] = updatedMaintenance;
            setMaintenanceList(MaintenanceList);
    
            // Reset the values
            setMaintenanceData({
                equipment: "",
                maintenanceType: "",
                technician: "",
                date: "",
                cost: "",
            });
            setOnEdit(false);
            setKeyEdit(0);
        };
    
        const editMaintenance = (user, key) => {
            setKeyEdit(key);
            setMaintenanceData(user);
            setOnEdit(true);
        };
    
        const cancelEditMaintenance = () => {
            setMaintenanceData({
                equipment: "",
                maintenanceType: "",
                technician: "",
                date: "",
                cost: "",
            });
            setOnEdit(false);
            setKeyEdit(0);
        }
        
        const deleteMaintenance = (id) => {
            setMaintenanceList(MaintenanceList.filter(maint => maint.id !== id));
        };
    
        return (
            <>
                { onEdit &&
                    <EditMaintenanceForm 
                        MaintenanceData={MaintenanceData} 
                        onSave={handleSave} 
                        onCancel={cancelEditMaintenance} 
                    />
                }
    
                { !onEdit &&
                    <div className="mt-12 mb-8 flex flex-col gap-12">
                    <Card>
                        <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Mantenience History
                        </Typography>
                        </CardHeader>
                        <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                            <tr>
                                {[ "equipment","Mantenience's type", "technic", "date", "Cost"].map((el) => (
                                <th
                                    key={el}
                                    className="border-b border-blue-gray-50 py-3 px-5 text-left"
                                >
                                    <Typography
                                    variant="small"
                                    className="text-[11px] font-bold uppercase text-blue-gray-400"
                                    >
                                    {el}
                                    </Typography>
                                </th>
                                ))}
                            </tr>
                            </thead>
                            <tbody>
                            {maintenanceHistData.map(
                                (maint, key) => {
                                const className = `py-3 px-5 ${
                                    key === maintenanceHistData.length - 1
                                    ? ""
                                    : "border-b border-blue-gray-50"
                                }`;
    
                                return (
                                    <tr key={maint.equipment}>
                                    <td className={className}>
                                    <div className="flex items-center gap-4">
                                    <div>
                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                        {maint.equipment}
                                        </Typography>
                                    </div>
                                    </div>
                                    </td>
                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                        <div>
                                            <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-semibold"
                                            >
                                            {maint.maintenanceType}
                                            </Typography>
                                        </div>
                                        </div>
                                    </td>
                                    
                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                        <div>
                                            <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-semibold"
                                            >
                                            {maint.technician}
                                            </Typography>
                                        </div>
                                        </div>
                                    </td>

                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                        <div>
                                            <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-semibold"
                                            >
                                            {maint.date}
                                            </Typography>
                                        </div>
                                        </div>
                                    </td>

                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                        <div>
                                            <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-semibold"
                                            >
                                            {maint.cost}
                                            </Typography>
                                        </div>
                                        </div>
                                    </td>

                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                            <div 
                                                className="flex items-center gap-1"
                                                onClick={() => editMaintenance(maint, key)}
                                            >
                                                <Typography
                                                    as="a"
                                                    href="#"
                                                    className="text-xs font-semibold text-green-600"
                                                    >
                                                    Edit
                                                </Typography>
                                                <PencilIcon className="w-3 text-green-600"/>
                                            </div>
                                        
                                            <div 
                                                className="flex items-center gap-1"
                                                onClick={() => deleteMaintenance(maint.id)}
                                            >
                                                <Typography
                                                    as="a"
                                                    href="#"
                                                    className="text-xs font-semibold text-red-600"
                                                    >
                                                    Delete
                                                </Typography>
                                                <TrashIcon className="w-3 text-red-600"/>
                                            </div>
                                        </div>
                                    </td>
                                    </tr>
                                );
                                }
                            )}
                            </tbody>
                        </table>
                        </CardBody>
                    </Card>
    
                    </div>
                }
            </>
        );
    }
    
    export default MaintenanceHistoryTable;