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
import {EditUserForm} from "@/pages/dashboard_admin/edit_user";
import { UserIcon } from "@heroicons/react/24/outline";
import { sectionData } from "@/data/sections-data";
import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { useState, useEffect } from "react";
import EditSectionForm from "./edit_section";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
    
    export function TablesSection() {
        const [isLoading, setIsLoading] = useState(true);
        const[alertMessage, setAlertMessage] = useState('');
        const [alertType, setAlertType] = useState('success');

        const [onEdit, setOnEdit] = useState(false);
        const [keyEdit, setKeyEdit] = useState(0);
        const [sectionList, setSectionList] = useState([]);
        const [sectData, setSectData] = useState({
            id: "",
            name: "",
        });
    
        // TODO: Connect with backend and replace static values

        useEffect(() => {
            fetchSections();
        }, []);
        
        const fetchSections = async () => {
            try {
                const response = await fetch('http://localhost:5217/api/Section/GET_ALL', {
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
                setSectionList(data);
                setIsLoading(false);
            } catch (error) {
                console.error("Error fetching sections:", error);
                setSectionList([]);
                setIsLoading(false);
            }
        };
        
        const handleSave = (updateSection) => {
            const index = sectionList.findIndex(s => s.id === updateSection.id)
            sectionList[index] = updateSection;

            setSectionList(sectionList);

            setSectData({
                id: "",
                name: "",
            });

            setOnEdit(false);
            setKeyEdit(0);
        };
    
        const editSection = (sect, key) => {
            setKeyEdit(key);
            setSectData(sect);
            setOnEdit(true);
        };
    
        const cancelEditSection = () => {
            setSectData({
                name: "",
                id: "",
                description: "",
                status: "",
                priority: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
        }
        
        const deleteSection = async (id) => {
            try {
                const response = await fetch(`http://localhost:5217/api/Section/DELETE?sectionId=${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                },
            });
            if (!response.ok) {
                setAlertMessage('Failed to delete section');
                setAlertType('error');
                throw new Error('Failed to delete section');
            }
            else
            {
                setAlertMessage('Delete completed successfully');
                setAlertType('success');
                setSectionList(sectionList.filter(s => s.id !== id));
            }
            } catch (error) {
                setAlertMessage('Error deleting section:');
                setAlertType('error');
                console.error("Error deleting section:", error);
            }
        };
    
        return (
            <>
                <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />

                { onEdit &&
                    <EditSectionForm 
                        sectionData={sectData} 
                        onSave={handleSave} 
                        onCancel={cancelEditSection} 
                    />
                }
    
                { !onEdit &&
                    <div className="mt-12 mb-8 flex flex-col gap-12">
                    <Card>
                        <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Section Table
                        </Typography>
                        </CardHeader>
                        <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                            <tr>
                                {[ "Name"].map((el) => (
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
                            {sectionList.map(
                                (sect, key) => {
                                const className = `py-3 px-5 ${
                                    key === sectionList.length - 1
                                    ? ""
                                    : "border-b border-blue-gray-50"
                                }`;
    
                                return (
                                    <tr key={sect.name}>
                                    <td className={className}>
                                    <div className="flex items-center gap-4">
                                    <div>
                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                        {sect.name}
                                        </Typography>
                                    </div>
                                    </div>
                                    </td>
                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                            <div 
                                                className="flex items-center gap-1"
                                                onClick={() => editSection(sect, key)}
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
                                                onClick={() => deleteSection(sect.id)}
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
    
    export default TablesSection;