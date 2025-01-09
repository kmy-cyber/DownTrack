import React from 'react';
import { Card, CardHeader, CardBody, Typography, Button } from "@material-tailwind/react";
import { equipmentTransferData } from "@/data/equipment-transfer-data";
import { PencilIcon, TrashIcon , InformationCircleIcon, CheckCircleIcon  } from "@heroicons/react/24/outline";
import { useState } from "react";
import TransferInfoForm from "./info_transfer";
import RegisterForm from "./register_shipping_resp";

export function EquipmentTransferTable() {
    const [onInfo, setOnInfo] = useState(false);
    const [selectedTransfer, setSelectedTransfer] = useState(null);
    const [isRegistered, setIsRegistered] = useState(false);
    const [showRegistrationForm, setShowRegistrationForm] = useState(false);
    const [assignedPerson, setAssignedPerson] = useState("");
    const [registeredTransfers, setRegisteredTransfers] = useState([]); 
    // TODO: Connect with backend and replace static values
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

    const handleAcceptRegister = (person) => {
        console.log("handleAcceptRegister called with person:", person); 
        if (selectedTransfer) {
            console.log("selectedTransfer:", selectedTransfer); 
            const updatedData = [...equipmentTransferData];
            const index = updatedData.findIndex(item => item.id === selectedTransfer.id);
            if (index !== -1) {
                console.log("Updating transfer at index:", index); 
                updatedData[index].registered = true;
                updatedData[index].assignedPerson = person;
                equipmentTransferData.splice(index, 1, ...updatedData.slice(index, index + 1));
                setIsRegistered(true);
                setShowRegistrationForm(false);
                setAssignedPerson("");
                setRegisteredTransfers([...registeredTransfers, { id: selectedTransfer.id, assignedPerson: person }]);
                alert('Successful registration');
            }
            else {
                console.log("Transfer not found in data"); 
            }
        } 
        else {
            console.log("No selected transfer or already registered");
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
                                {[ "Source Section", "Equipment", "Date"].map((el) => (
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
                            {equipmentTransferData.map(
                                (transfer, index) => {
                                    const className = `py-3 px-5 ${
                                        index === equipmentTransferData.length - 1
                                            ? ""
                                            : "border-b border-blue-gray-50"
                                    }`;
                                    return (
                                        <tr key={transfer.id}>
                                            <td className={className}>
                                                <div className="flex items-center gap-4">
                                                    <div>
                                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                                            {transfer.sourceSection}
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
                                                            {transfer.equipment}
                                                        </Typography>
                                                    </div>
                                                </div>
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
            </div>)
        }
    </>
);
}

export default EquipmentTransferTable;
