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
import { toast } from "react-toastify";
import api from "@/middlewares/api";
import DropdownMenu from "@/components/DropdownMenu";
import { DeleteForeverOutlined, EditNoteOutlined, PersonPinCircleOutlined } from "@mui/icons-material";


export function Tables() {
    const [onEdit, setOnEdit] = useState(false);
    const [keyEdit, setKeyEdit] = useState(0);
    const [isLoading, setIsLoading] = useState(true);

    const [totalPages, setTotalPages] = useState(0);
    const [currentItems, setCurrentItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const [userData, setUserData] = useState({
        id:0,
        name: "",
        gmail:"",
        userName: "",
        userRole: "",
    });

    const options =(user, key) => [
        { 
            label: 'Edit',
            className: 'text-green-500 h-5 w-5', 
            icon: EditNoteOutlined,
            action: () => editUser(user, key)
        },
        { 
            label: 'Delete',
            className: 'text-red-500 h-5 w-5', 
            icon: DeleteForeverOutlined,
            action: () => deleteUser(user.id)
        },
    ];
    
    useEffect(() => {
        fetchEmployees(1);
    }, []);
    

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
            toast.success('Delete completed successfully');
            setCurrentItems(currentItems.filter(user => user.id !== id));
            await fetchEmployees(currentPage);
        }
        } catch (error) {
            toast.error('Error deleting employee:');
            console.error("Error deleting employee:", error);
        }
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
                        Users Table
                    </Typography>
                    </CardHeader>
                    <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                    <table className="w-full min-w-[640px] table-auto">
                        <thead>
                        <tr>
                            {[ "employee","username","role", "gmail",""].map((el) => (
                            <th
                                key={el}
                                className="border-b border-r border-blue-gray-50 py-3 px-5 text-center last:border-r-0 bg-gray-800 border-collapse"
                            >
                                <Typography
                                    variant="small"
                                    className="text-[11px] font-extrabold uppercase text-white"
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
                                : "border-b border-blue-gray-50 text-center"
                            }`;

                            return (
                                <tr key={user.id}>
                                <td className={className}>
                                    <div className="flex items-center gap-4">
                                    {/* <Avatar src={img} alt={name} size="sm" variant="rounded" /> */}
                                    <UserIcon className="w-5"/>
                                    <div>
                                        <Typography
                                        variant="small"
                                        color="blue-gray"
                                        className="font-semibold text-center"
                                        >
                                        {user.name}
                                        </Typography>
                                    </div>
                                    </div>
                                </td>
                                <td className={className}>
                                    <Typography className="text-xs font-semibold text-blue-gray-600 text-center">
                                    {user.userName}
                                    </Typography>
                                </td>
                                <td className={className}>
                                    <Typography className="text-xs font-semibold text-blue-gray-600 text-center">
                                    {user.userRole}
                                    </Typography>
                                </td>
                                <td className={className}>
                                    <Typography className="text-xs font-semibold text-blue-gray-600 text-center">
                                    {user.email}
                                    </Typography>
                                </td>
                                    <td className={className + "items-center text-right"}>
                                            <DropdownMenu options={options(user, key)} />
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