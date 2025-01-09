import React from 'react';
import { Card, CardHeader, CardBody, Typography, Pagination } from "@material-tailwind/react";
import { equipmentData } from '@/data/equipment-data';
import { PencilIcon, TrashIcon , InformationCircleIcon, CheckCircleIcon  } from "@heroicons/react/24/outline";
import { useState } from "react";

export function EquipmentInventory() {
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 10;

    // TODO: Connect with backend and replace static values
    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = equipmentData.slice(indexOfFirstItem, indexOfLastItem);

    const totalPages = Math.ceil(equipmentData.length / itemsPerPage);

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage);
    };
    

return (
    <>
        {(<div className={`mt-12 mb-8 flex flex-col gap-12 `}>
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Equipment Inventory
                    </Typography>
                </CardHeader>
                <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                    <table className="w-full min-w-[640px] table-auto">
                        <thead>
                            <tr>
                                {[ "ID", "Equipment", "type"].map((el) => (
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
                        <tbody >
                            {currentItems.map(
                                (equipment, index) => {
                                    const className = `py-3 px-5 ${
                                        index === currentItems.length - 1
                                            ? ""
                                            : "border-b border-blue-gray-50"
                                    }`;
                                    return (
                                        <tr key={equipment.id} >
                                            <td className="py-3 px-5 border-b border-r border-blue-gray-50 last:border-r-0">
                                                <div className="flex items-center gap-4">
                                                    <div>
                                                        <Typography className="text-xs font-semibold text-blue-gray-600">
                                                            {equipment.id}
                                                        </Typography>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="py-3 px-5 border-b border-r border-blue-gray-50 last:border-r-0">
                                                <div className="flex items-center gap-4">
                                                    <div>
                                                        <Typography
                                                            variant="small"
                                                            color="blue-gray"
                                                            className="font-semibold"
                                                        >
                                                            {equipment.name}
                                                        </Typography>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className={className}>
                                                <Typography className="text-xs font-semibold text-blue-gray-600">
                                                    {equipment.type}
                                                </Typography>
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
                    total={totalPages}
                    current={currentPage}
                    onChange={handlePageChange}
                    className="self-center"
            />
            </div>)
        }
    </>
);
}

export default EquipmentInventory;
