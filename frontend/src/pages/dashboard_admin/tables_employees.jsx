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
import { UserIcon, KeyIcon} from "@heroicons/react/24/outline";
//import { userListData } from "@/data";
import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { useState, useEffect } from "react";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";


export function Tables() {
    const [onEdit, setOnEdit] = useState(false);
    const [keyEdit, setKeyEdit] = useState(0);
    const [isLoading, setIsLoading] = useState(true);
    const[alertMessage, setAlertMessage] = useState('');
    const [alertType, setAlertType] = useState('success');
    const [userList, setUserList] = useState([]);
    const [userData, setUserData] = useState({
        id:0,
        name: "",
        username: "",
        userRole: "",
        department: "",
        experience: "",
        specialty: "",
        salary: "",
        password: "",
    });
    
    useEffect(() => {
        fetchEmployees();
    }, []);
    
    const fetchEmployees = async () => {
        try {
            const response = await fetch('http://localhost:5217/api/Employee/GET_ALL/', {
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
            console.log(data);
            console.log(userList);
            setUserList(data);
            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching employees:", error);
            setUserList([]);
            setIsLoading(false);
        }
    };

    const handleSave = (updatedUser) => {
        const index = userList.findIndex(user => user.id === updatedUser.id);
        userList[index] = updatedUser;
        setUserList([...userList]);
        setUserData({
            id:id,
            name: "",
            username: "",
            userRole: "",
            department: "",
            experience: "",
            specialty: "",
            salary: "",
            password: "",
        });
        setOnEdit(false);
        setKeyEdit(0);
    };

    const editUser = (user, key) => {
        setKeyEdit(key);
        setUserData(user);
        setOnEdit(true);
        console.log(userData);
    };

    const cancelEditUser = () => {
        setUserData({
            id :0,
            name: "",
            username: "",
            userRole: "",
            department: "",
            experience: "",
            specialty: "",
            salary: "",
            password: "",
        });
        setOnEdit(false);
        setKeyEdit(0);
    }
    
    const deleteUser = async (id) => {
        try {
            const response = await fetch(`http://localhost:5217/api/Employee/DELETE?employeeId=${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
        });
        if (!response.ok) {
            setAlertMessage('Failed to delete employee');
            setAlertType('error');
            throw new Error('Failed to delete employee');
        }
        else
        {
            setAlertMessage('Delete completed successfully');
            setAlertType('success');
            setUserList(userList.filter(user => user.id !== id));
        }
        } catch (error) {
            setAlertMessage('Error deleting employee:');
            setAlertType('error');
            console.error("Error deleting employee:", error);
        }
    };

    return (
        <>
            <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />
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
                        Users Table
                    </Typography>
                    </CardHeader>
                    <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                    <table className="w-full min-w-[640px] table-auto">
                        <thead>
                        <tr>
                            {[ "employee","password","username","role", "gmail"].map((el) => (
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
                                    {/* <Avatar src={img} alt={name} size="sm" variant="rounded" /> */}
                                    <UserIcon className="w-5"/>
                                    <div>
                                        <Typography
                                        variant="small"
                                        color="blue-gray"
                                        className="font-semibold"
                                        >
                                        {user.name}
                                        </Typography>
                                    </div>
                                    </div>
                                </td>
                                <td className={className}>
                                    <div className="flex items-center gap-4">
                                    <KeyIcon className="w-4"/>
                                    <div>
                                        <Typography
                                        variant="small"
                                        color="blue-gray"
                                        className="font-semibold"
                                        >
                                        {user.password}
                                        </Typography>
                                    </div>
                                    </div>
                                </td>
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
                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                    {user.userRole}
                                    </Typography>
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

export default Tables;