import React from 'react';
import { Card, CardHeader, CardBody, Typography, Button } from "@material-tailwind/react";
import { equipmentDisposalData } from "@/data/equipment-disposal-data";
import { PencilIcon } from "@heroicons/react/24/solid";
import {InformationCircleIcon,CheckCircleIcon,TrashIcon} from "@heroicons/react/24/outline";
import { useState } from "react";
import DisposalInfoForm from "./info_disposal";

export function EquipmentDisposalTable() {
    const [onInfo, setOnInfo] = useState(false);
    const [selectedDisposal, setSelectedDisposal] = useState(null);
    const [isRegistered, setIsRegistered] = useState(false);


    // TODO: Connect with backend and replace static values
    const handleShowInfo = (disposal) => {
        setSelectedDisposal(disposal);
        setOnInfo(true);
    };

    const handleCloseInfo = () => {
        setSelectedDisposal(null);
        setOnInfo(false);
    };

    const registerItem = () => {
        if (selectedDisposal && !isRegistered) {
            const updatedData = [...equipmentDisposalData];
            const index = updatedData.findIndex(item => item.id === selectedDisposal);
            if (index !== -1) {
                updatedData[index].registered = true;
                equipmentDisposalData.splice(index, 1, ...updatedData.slice(index, index + 1));
                setIsRegistered(true);
                alert('Successful registration');
            }
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
                                    {[ "Technic", "Equipment", "Date"].map((el) => (
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
                                {equipmentDisposalData.map(
                                    (disposal, index) => {
                                        const className = `py-3 px-5 ${
                                            index === equipmentDisposalData.length - 1
                                                ? ""
                                                : "border-b border-blue-gray-50"
                                        }`;

                                        return (
                                            <tr key={disposal.id}>
                                                <td className={className}>
                                                    <div className="flex items-center gap-4">
                                                        <div>
                                                            <Typography className="text-xs font-semibold text-blue-gray-600">
                                                                {disposal.technician}
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
                                                                {disposal.equipment}
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
                                                            onClick={() => registerItem()}
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
                                                            onClick={() => registerItem()}
                                                        >
                                                            <Typography
                                                                as="a"
                                                                href="#"
                                                                className="text-xs font-semibold text-red-800"
                                                            >
                                                                Eliminate
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
