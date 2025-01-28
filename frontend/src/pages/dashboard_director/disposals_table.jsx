import React, { useState } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { equipmentDisposalData } from "@/data"; // Asegúrate de importar los datos correctamente

const EquipmentDisposalTable = () => {
    const [data] = useState(equipmentDisposalData);

    return (
        <div className="mt-12 mb-8 flex flex-col gap-12">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
                    <Typography variant="h6" color="white">
                        Equipment Disposal Records
                    </Typography>
                </CardHeader>
                <CardBody>
                    {/* Tabla de bajas técnicas */}
                    <div className="overflow-x-auto">
                        <table className="w-full table-auto border-collapse border border-gray-200">
                            <thead>
                                <tr>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Technician</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Equipment</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Reason for Removal</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                {data.map((disposal) => (
                                    <tr key={disposal.id} className="hover:bg-gray-50">
                                        <td className="border border-gray-200 px-4 py-2">{disposal.technician}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.equipment}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.reasonForRemoval}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.date}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </CardBody>
            </Card>
        </div>
    );
};

export default EquipmentDisposalTable;
