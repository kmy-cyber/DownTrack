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
//import { currentItemsData } from "@/data";
import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { useState, useEffect } from "react";
import { Pagination } from '@mui/material';
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
import api from "@/middlewares/api";


export function Tables() {
    const [onEdit, setOnEdit] = useState(false);
    const [keyEdit, setKeyEdit] = useState(0);
    const [isLoading, setIsLoading] = useState(true);

    const [totalPages, setTotalPages] = useState(0);
    const [currentItems, setCurrentItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const[alertMessage, setAlertMessage] = useState('');
    const [alertType, setAlertType] = useState('success');
    const [userData, setUserData] = useState({
        id:0,
        name: "",
        gmail:"",
        userName: "",
        userRole: "",
    });
    
    useEffect(() => {
        fetchEmployees(1);
    }, []);
    
    // funcion que se llama cada vez que se cambia de pagina
    const handlePageChange = async (event, newPage) => {
        setCurrentPage(newPage);
        await fetchEmployees(newPage);
    };

    const fetchEmployees = async (page) => {
        try {
            const response = await api(`/Employee/GetPaged?PageNumber=${page}&PageSize=10`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();

            setCurrentItems(data.items);
            setTotalPages(Math.ceil(data.totalCount / data.pageSize));

            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching employees:", error);
            setCurrentItems([]);
            setIsLoading(false);
        }
    };

    const handleSave = (updatedUser) => {
        const index = currentItems.findIndex(user => user.id === updatedUser.id);
        currentItems[index] = updatedUser;
        setCurrentItems([...currentItems]);
        setUserData({
            id:id,
            name: "",
            email:"",
            userName: "",
            userRole: "",
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
            email:"",
            userName: "",
            userRole: "",
        });
        setOnEdit(false);
        setKeyEdit(0);
    }
    
    const deleteUser = async (id) => {
        try {
            const response = await api(`/Employee/DELETE?employeeId=${id}`, {
            method: 'DELETE',
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
            setCurrentItems(currentItems.filter(user => user.id !== id));
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
                            {[ "employee","username","role", "gmail"].map((el) => (
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
                        {currentItems.map(
                            (user, key) => {
                            const className = `py-3 px-5 ${
                                key === currentItems.length - 1
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
                                <div>
                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                    {user.userName}
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
                                <div>
                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                    {user.email}
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
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    className="self-center"
                />
                </div>
            }
        </>
    );
}

export default Tables;