import React from 'react';
import { Card, CardHeader, CardBody, Typography, Button } from "@material-tailwind/react";
import { equipmentDisposalData } from "@/data/equipment-disposal-data";
import { PencilIcon } from "@heroicons/react/24/solid";
import {InformationCircleIcon,CheckCircleIcon,TrashIcon} from "@heroicons/react/24/outline";
import { useState, useEffect } from "react";
import { Pagination } from '@mui/material';
import DisposalInfoForm from "./info_disposal";
import MessageAlert from '@/components/Alert_mssg/alert_mssg';
import api from "@/middlewares/api";
import { useAuth } from '@/context/AuthContext';
import DropdownMenu from '@/components/DropdownMenu';

export function EquipmentDisposalTable() {
    const [onInfo, setOnInfo] = useState(false);
    const [selectedDisposal, setSelectedDisposal] = useState(null);
    const [registeredTransfers, setRegisteredTransfers] = useState([]);

    const [isLoading, setIsLoading] = useState(true);
    
    const[alertMessage, setAlertMessage] = useState('');
    const [alertType, setAlertType] = useState('success');

    const [totalPages, setTotalPages] = useState(0);
    const [currentItems, setCurrentItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    
    const {user} = useAuth();

    useEffect(() => {
        fetchDecommissions(1);
    }, []);

    const options =(disposal) => [
        { 
            label: 'Information', 
            className: 'text-blue-500 h-5 w-5', 
            icon: InformationCircleIcon,
            action: () => handleShowInfo(disposal)
        },
        { 
            label: 'Register', 
            className: 'text-green-500 h-5 w-5', 
            icon: CheckCircleIcon,
            action: () => registerItem(disposal.id)
        },
        {
            label: 'Delete', 
            className: 'text-red-500 h-5 w-5', 
            icon: TrashIcon,
            action: () => deleteItem(disposal.id)
        }

    ];

    const handleShowInfo = (disposal) => {
        setSelectedDisposal(disposal);
        setOnInfo(true);
    };

    const handleCloseInfo = () => {
        setSelectedDisposal(null);
        setOnInfo(false);
    };

    const handlePageChange = async (event, newPage) => {
        setCurrentPage(newPage);
        await fetchDecommissions(newPage);
    };

    const fetchDecommissions = async (page) => {
        try {
            const response = await api(`/EquipmentDecommissioning/Get_Paged_All_By_ReceptorId/${user.id}?PageNumber=${page}&PageSize=10`, {
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
            setCurrentItems([]);
            setIsLoading(false);
        }
    };

    const registerItem = async(equipmentId) => {
        try {
            const response = await api(`/EquipmentDecommissioning/${equipmentId}/accept`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
            
                setAlertType('success');
                setAlertMessage("Decommision accepted successfully");
                await fetchDecommissions(currentPage);
                setIsLoading(false);
                setShowDialog(false);

            } else {
                setAlertType('error');
                setAlertMessage('Failed to register item');
            }
        } catch (error) {
            console.error('Error registering item:', error);

        }
    };

    const deleteItem = async(equipmentId) => {
        try {
            const response = await api(`/EquipmentDecommissioning/${equipmentId}/delete`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
                setAlertType('success');
                setAlertMessage("Successful deletion");
                const updatedData = currentItems.filter(item => item.id !== equipmentId);
                setCurrentItems(updatedData);
                await fetchDecommissions(currentPage);

            } else {
                setAlertType('error');
                setAlertMessage("Failed to delete")

            }
        } catch (error) {
            console.error('Error deleting item:', error);
        }
    };


    return (
        <>
            {onInfo && selectedDisposal && (
                <DisposalInfoForm 
                    disposal={selectedDisposal}
                    onClose={handleCloseInfo}
                />
            )}
            <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />
            { !onInfo &&
                (<div className="mt-12 mb-8 flex flex-col gap-12 ">
                <Card>
                    <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Equipment Decommission
                        </Typography>
                    </CardHeader>
                    <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                                <tr>
                                    {[ "Technic", "Equipment", "Date", "Cause", "Status",""].map((el) => (
                                        <th
                                            key={el}
                                            className="border-b border-r border-blue-gray-50 py-3 px-5 text-left last:border-r-0 bg-gray-300"
                                        >
                                            <Typography
                                                variant="small"
                                                className="text-[11px] font-extrabold uppercase text-blue-gray-800"
                                            >
                                                {el}
                                            </Typography>
                                        </th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {currentItems.map(
                                    (disposal, index) => {
                                        const className = `py-3 px-5 ${
                                            index === currentItems.length - 1
                                                ? ""
                                                : "border-b border-blue-gray-50"
                                        }`;

                                        return (
                                            <tr key={disposal.id}>
                                                <td className={className}>
                                                    <div className="flex items-center gap-4">
                                                        <div>
                                                            <Typography className="text-xs font-semibold text-blue-gray-600">
                                                                {disposal.technicianUserName}
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
                                                                {disposal.equipmentName}
                                                            </Typography>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className={className}>
                                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                                        {disposal.date}
                                                    </Typography>
                                                </td>
                                                <td className={className}>
                                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                                        {disposal.cause}
                                                    </Typography>
                                                </td>
                                                <td className={className}>
                                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                                        {disposal.status}
                                                    </Typography>
                                                </td>
                                                <td className={className}>
                                                    <div className="text-right">
                                                    <td className={className + "items-center text-right"}>
                                                            <DropdownMenu options={options(disposal)} />
                                                    </td>

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

export default EquipmentDisposalTable;
