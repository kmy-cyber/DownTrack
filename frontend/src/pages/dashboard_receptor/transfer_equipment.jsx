import React from 'react';
import { Card, CardHeader, CardBody, Typography, Button } from "@material-tailwind/react";
import { PencilIcon, TrashIcon , InformationCircleIcon, CheckCircleIcon  } from "@heroicons/react/24/outline";
import { useState, useEffect } from "react";
import TransferInfoForm from "./info_transfer";
import RegisterForm from "./register_shipping_resp";
import { Pagination } from '@mui/material';
import MessageAlert from '@/components/Alert_mssg/alert_mssg';
import api from "@/middlewares/api";
import { useAuth } from '@/context/AuthContext';

export function EquipmentTransferTable() {
    const [onInfo, setOnInfo] = useState(false);
    const [selectedTransfer, setSelectedTransfer] = useState(null);
    const [isRegistered, setIsRegistered] = useState(false);
    const [showRegistrationForm, setShowRegistrationForm] = useState(false);
    const [assignedPerson, setAssignedPerson] = useState("");
    const [registeredTransfers, setRegisteredTransfers] = useState([]); 
    
    const [isLoading, setIsLoading] = useState(true);

    const[alertMessage, setAlertMessage] = useState('');
    const [alertType, setAlertType] = useState('success');

    const [totalPages, setTotalPages] = useState(0);
    const [currentItems, setCurrentItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const { user } = useAuth();

    const [formData, setFormData] = useState({
        "id": 0,
        "requestId": 0,
        "shippingSupervisorId": 0,
        "equipmentReceptorId": 0,
        "date": "",

        "id": 1,
        "name": "",
        "type": "",
        "status": "",
        "departmentName": 4,
        "sectionName": 1,
        "dateRequest": "",
    });

    useEffect(() => {
        fetchTransfers(1);
    }, []);
    
    const handleShowInfo = (transfer) => {
        setSelectedTransfer(transfer);
        setOnInfo(true);
    };

    const handleCloseInfo = () => {
        setSelectedTransfer(null);
        setOnInfo(false);
    };

    const handleRegister = (transfer) => {
        setShowRegistrationForm(true);
        setSelectedTransfer(transfer);
    };

    const handlePageChange = async (event, newPage) => {
        setCurrentPage(newPage);
        await fetchTransfers(newPage);
    };

    const fetchTransfers = async (page) => {
        try {
            const response = await api(`/TransferRequest/GetPaged?PageNumber=${page}&PageSize=10`, {
                method: 'GET',
            });
            
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();

            const transfersWithEquipment = await Promise.all(
                data.items.map(async (transfer) => {
                    const equipment = await getEquipment(transfer.equipmentId);            
                    return { 
                        ...transfer, 
                        equipment 
                    };
                }
            ));
            
            setCurrentItems(transfersWithEquipment);
            setTotalPages(Math.ceil(data.totalCount / data.pageSize));

            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching employees:", error);
            setCurrentItems([]);
            setIsLoading(false);
        }
    };

    const getEquipment= async (id) => {
        try {
            const response = await api(`/Equipment/Get?EquipmentId=${id}`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return await response.json();
            
        } catch (error) {
            console.error("Error fetching equipment:", error);
            return null;

        }
    };

    const transferPostData = async (transferData) => {
        try {
            const response = await api('/Transfer/POST', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    "requestId": transferData.id,
                    "shippingSupervisorId": transferData.assignedPerson,
                    "equipmentReceptorId": user.id,
                    "date": transferData.date,
                }),
            });


            const data = await response.json();
            

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            //const data = await response.json();
            console.log("Transfer saved successfully:", data);
            await handleChangeStatus();
        } catch (error) {
            console.error("Error saving transfer:", error);
        }
    };

    const handleChangeStatus = async () => {
        try {
            const response = await api('/TransferRequest/PUT', {
                method: 'PUT',
                body: JSON.stringify({
                    "id": selectedTransfer.id,
                    "date": selectedTransfer.date,
                    "status": "Registered",
                    "sectionManagerId": selectedTransfer.sectionManagerId,
                    "equipmentId": selectedTransfer.equipmentId,
                    "arrivalDepartmentId": selectedTransfer.arrivalDepartmentId,
                    "arrivalSectionId": selectedTransfer.arrivalSectionId,
                }),
            });

            const data = await response.json();
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            
            setAlertType('success');
            setAlertMessage('Transfer saved successfully')
            setShowRegistrationForm(false);
            console.log("Transfer saved successfully:", data);
            await fetchTransfers(currentPage);
        } catch (error) {
            console.error("Error saving transfer:", error);
            
        }
    };

    

    const handleSearch = (e) => {
        const query = e.target.value;
        setSearchQuery(query);
        
        if (query) {
            setFilteredShippingS(
                shippingSupervisors.filter(
                    (receptor) => receptor.name.toLowerCase().includes(query.toLowerCase())
                )
            );
        } else {
            setFilteredShippingS(shippingSupervisors);

        }
    };

    const handleCancelRegister = () => {
        setShowRegistrationForm(false);
    };


return (
    <>
        {onInfo && selectedTransfer && (
            <TransferInfoForm 
                transfer={selectedTransfer}
                onClose={handleCloseInfo}
            />
        )}
        {showRegistrationForm && (
        <RegisterForm
            onAccept={handleAcceptRegister}
            onCancel={handleCancelRegister}
        />
        )}

        <MessageAlert message={alertMessage} type="success" onClose={() => setAlertMessage('')} />
        
        { !onInfo &&
            (<div className={`mt-12 mb-8 flex flex-col gap-12 ${showRegistrationForm ? 'blur-background' : ''}`}>
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Equipment Transfer
                    </Typography>
                </CardHeader>
                <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                    <table className="w-full min-w-[640px] table-auto">
                        <thead>
                            <tr>
                                {[ "Source Section","Source Department", "Equipment","Type","Date"].map((el) => (
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
                                (transfer, index) => {
                                    const className = `py-3 px-5 ${
                                        index === currentItems.length - 1
                                            ? ""
                                            : "border-b border-blue-gray-50"
                                    }`;
                                    return (
                                        <tr key={transfer.id}>
                                            <td className={className}>
                                                <div className="flex items-center gap-4">
                                                    <div>
                                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                                            {transfer.equipment.sectionName}
                                                        </Typography>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className={className}>
                                                <Typography className="text-xs font-semibold text-blue-gray-600">
                                                    {transfer.equipment.departmentName}
                                                </Typography>
                                            </td>
                                            <td className={className}>
                                                <Typography className="text-xs font-semibold text-blue-gray-600">
                                                    {transfer.equipment.name}
                                                </Typography>
                                            </td>
                                            <td className={className}>
                                                <Typography className="text-xs font-semibold text-blue-gray-600">
                                                    {transfer.equipment.status}
                                                </Typography>
                                            </td>
                                            <td className={className}>
                                                <Typography className="text-xs font-semibold text-blue-gray-600">
                                                    {transfer.date}
                                                </Typography>
                                            </td>
                                            <td className={className}>
                                                <div className="flex items-center gap-4">
                                                    <div 
                                                        className="flex items-center gap-1"
                                                        onClick={() => handleShowInfo(transfer)}
                                                    >
                                                        <Typography
                                                            as="a"
                                                            href="#"
                                                            className="text-xs font-semibold text-blue-600"
                                                        >
                                                            Info
                                                        </Typography>
                                                        <InformationCircleIcon className="w-5 text-blue-600" />
                                                    </div>
                                                    <div 
                                                        className="flex items-center gap-1"
                                                        onClick={() => handleRegister(transfer)}
                                                    >
                                                        <Typography
                                                            as="a"
                                                            href="#"
                                                            className="text-xs font-semibold text-green-600"
                                                        >
                                                            Register
                                                        </Typography>
                                                        <CheckCircleIcon className="w-5 text-green-600" />
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
            </div>)
        }
    </>
);
}

export default EquipmentTransferTable;
