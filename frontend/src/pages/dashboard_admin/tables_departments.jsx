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
import {EditDepartmentForm} from "@/pages/dashboard_admin/edit_department";
import { UserIcon } from "@heroicons/react/24/outline";
import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { useState, useEffect } from "react";   
import MessageAlert from "@/components/Alert_mssg/alert_mssg";

    export function TablesDepartment() {
        const [isLoading, setIsLoading] = useState(true);
        const[alertMessage, setAlertMessage] = useState('');
        const [alertType, setAlertType] = useState('success');

        const [departmentList, setDepartmentList] = useState([]);
        const [onEdit, setOnEdit] = useState(false);
        const [keyEdit, setKeyEdit] = useState(0);
        const [dptData, setDptData] = useState({
            name: "",
            id: "",
            sectionId: "",
            description: ""
        });
    
        // TODO: Connect with backend and replace static values

        useEffect(() => {
            fetchDepartments();
        }, []);
    
        const fetchDepartments = async () => {
            try {
                const response = await fetch('http://localhost:5217/api/Department/GET_ALL', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json',
                    },
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setDepartmentList(data);
                setIsLoading(false);
            } catch (error) {
                console.error("Error fetching departments:", error);
                setDepartmentList([]);
                setIsLoading(false);
            }
        };
        
        const handleSave = (updatedDepartment) => {
            departmentList[keyEdit] = updatedDepartment;
            setDepartmentList(departmentList);
    
            // Reset the values
            setDptData({
                name: "",
                id: "",
                sectionId: "",
                description: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
            };
            
        const editDepartment = (dept, key) => {
            setKeyEdit(key);
            setDptData(dept);
            setOnEdit(true);
        };
        
    
        const cancelEditDepartment = () => {
            setDptData({
                name: "",
                id: "",
                sectionId: "",
                description: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
        }
        
        const deleteDepartment = async (id, sectionId) => {
            try {
                const response = await fetch(`http://localhost:5217/api/Department/DELETE?departmentId=${id}&SectionId=${sectionId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                },
            });
            if (!response.ok) {
                setAlertMessage('Failed to delete department');
                setAlertType('error');
                throw new Error('Failed to delete department');
            }
            else
            {
                setAlertMessage('Delete completed successfully');
                setAlertType('success');
                setDepartmentList(departmentList.filter(d => d.id !== id));
            }
            } catch (error) {
                setAlertMessage('Error deleting department:');
                setAlertType('error');
                console.error("Error deleting department:", error);
            }
        };
    
        return (
            <>
                <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />

                { onEdit &&
                    <EditDepartmentForm 
                        departmentData={dptData} 
                        onSave={handleSave} 
                        onCancel={cancelEditDepartment} 
                    />
                }
    
                { !onEdit &&
                    <div className="mt-12 mb-8 flex flex-col gap-12">
                    <Card>
                        <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Department's Table
                        </Typography>
                        </CardHeader>
                        <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                            <tr>
                                {[ "name","section"].map((el) => (
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
                            {departmentList.map(
                                (dept, key) => {
                                const className = `py-3 px-5 ${
                                    key === departmentList.length - 1
                                    ? ""
                                    : "border-b border-blue-gray-50"
                                }`;
    
                                return (
                                    <tr key={dept.name}>
                                    <td className={className}>
                                    <div className="flex items-center gap-4">
                                    <div>
                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                        {dept.name}
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
                                            {dept.sectionId}
                                            </Typography>
                                        </div>
                                        </div>
                                    </td>
                                    
                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                            <div 
                                                className="flex items-center gap-1"
                                                onClick={() => editDepartment(dept, key)}
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
                                                onClick={() => deleteDepartment(dept.id, dept.sectionId)}
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
    
    export default TablesDepartment;