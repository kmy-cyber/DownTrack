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
    import { useState } from "react";
import EditSectionForm from "./edit_section";
    
    export function TablesSection() {
        const [sectionList, setSectionList] = useState(sectionData);
        const [onEdit, setOnEdit] = useState(false);
        const [keyEdit, setKeyEdit] = useState(0);
        const [userData, setUserData] = useState({
            name: "",
            id: "",
            description: "",
            status: "",
            priority: ""
        });
    
        // TODO: Connect with backend and replace static values
        
        const handleSave = (updatedSection) => {
            sectionList[keyEdit] = updatedSection;
            setSectionList(sectionList);
    
            // Reset the values
            setUserData({
                name: "",
                id: "",
                description: "",
                status: "",
                priority: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
        };
    
        const editSection = (sect, key) => {
            setKeyEdit(key);
            setUserData(sect);
            setOnEdit(true);
        };
    
        const cancelEditSection = () => {
            setUserData({
                name: "",
                id: "",
                description: "",
                status: "",
                priority: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
        }
        
        const deleteSection = (id) => {
            setSectionList(sectionList.filter(sec => sec.id !== id));
        };
    
        return (
            <>
                { onEdit &&
                    <EditSectionForm 
                        sectionData={sectionData} 
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