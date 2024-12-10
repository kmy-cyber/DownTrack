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
    import { userListData } from "@/data";
    import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
    import { useState } from "react";
    
    export function TablesSection() {
        const [userList, setUserList] = useState(userListData);
        const [onEdit, setOnEdit] = useState(false);
        const [keyEdit, setKeyEdit] = useState(0);
        const [userData, setUserData] = useState({
            name: "",
            id: "",
            
        });
    
        // TODO: Connect with backend and replace static values
        
        const handleSave = (updatedUser) => {
            userList[keyEdit] = updatedUser;
            setUserList(userList);
    
            // Reset the values
            setUserData({
                name: "",
                id: "",
            });
            setOnEdit(false);
            setKeyEdit(0);
        };
    
        const editUser = (user, key) => {
            setKeyEdit(key);
            setUserData(user);
            setOnEdit(true);
        };
    
        const cancelEditUser = () => {
            setUserData({
                name: "",
                id: ""
            });
            setOnEdit(false);
            setKeyEdit(0);
        }
        
        const deleteUser = (id) => {
            setUserList(userList.filter(user => user.id !== id));
        };
    
        return (
            <>
                { onEdit &&
                    <EditUserForm 
                        userData={userData} 
                        onSave={handleSave} 
                        onCancel={cancelEditUser} 
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
                            {userList.map(
                                (user, key) => {
                                const className = `py-3 px-5 ${
                                    key === userList.length - 1
                                    ? ""
                                    : "border-b border-blue-gray-50"
                                }`;
    
                                return (
                                    <tr key={user.name}>
                                    <td className={className}>
                                    <div className="flex items-center gap-4">
                                    <div>
                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                        {user.username}
                                        </Typography>
                                    </div>
                                    </div>
                                    </td>
                                    <td className={className}>
                                        <div className="flex items-center gap-4">
                                            <div 
                                                className="flex items-center gap-1"
                                                onClick={() => editUser(user, key)}
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
                                                onClick={() => deleteUser(user.id)}
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