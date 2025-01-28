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
    


    useEffect(() => {
        fetchDecommissions();
    }, []);

    const handleShowInfo = (disposal) => {
        setSelectedDisposal(disposal);
        setOnInfo(true);
    };

    const handleCloseInfo = () => {
        setSelectedDisposal(null);
        setOnInfo(false);
    };

    const fetchDecommissions = async () => {
        try {
            const response = await api(`/EquipmentDecommissioning/GET_ALL`, {
                method: 'GET',
            });
            
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setCurrentItems(data);

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
                const updatedData = [...currentItems];
                const index = updatedData.findIndex(item => item.id === selectedDisposal.id);
                if (index !== -1) {
                    updatedData[index].registered = true;
                    setCurrentItems(updatedData);
                    alert('Successful registration');
                }
            } else {
                alert('Failed to register item');
            }
        } catch (error) {
            console.error('Error registering item:', error);
            alert('Error registering item');
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
                const updatedData = currentItems.filter(item => item.id !== equipmentId);
                setCurrentItems(updatedData);
                alert('Successful deletion');
            } else {
                alert('Failed to delete item');
            }
        } catch (error) {
            console.error('Error deleting item:', error);
            alert('Error deleting item');
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
            { !onInfo &&
                (<div className="mt-12 mb-8 flex flex-col gap-12 ">
                <Card>
                    <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Equipment Disposal
                        </Typography>
                    </CardHeader>
                    <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                                <tr>
                                    {[ "Technic", "Equipment", "Date", "Cause", "Status"].map((el) => (
                                        <th
                                            key={el}
                                            className="border-b border-blue-gray-100 py-3 px-5 text-left"
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
                                                                {disposal.technicianId}
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
                                                                {disposal.equipmentId}
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
                                                    <div className="flex items-center gap-4">
                                                        <div 
                                                            className="flex items-center gap-1"
                                                            onClick={() => handleShowInfo(disposal)}
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
                                                            onClick={() => registerItem(disposal.id)}
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

                                                        <div 
                                                            className="flex items-center gap-1"
                                                            onClick={() => deleteItem(disposal.id)}
                                                        >
                                                            <Typography
                                                                as="a"
                                                                href="#"
                                                                className="text-xs font-semibold text-red-800"
                                                            >
                                                                Delete
                                                            </Typography>
                                                            <TrashIcon className="w-4 text-red-800" />
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
                </div>)
            }
        </>
    );
}

export default EquipmentDisposalTable;
