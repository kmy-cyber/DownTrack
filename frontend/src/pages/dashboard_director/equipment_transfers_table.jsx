import React, { useState } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { equipmentTransferData } from "@/data/equipment-transfer-data"; // Asegúrate de importar correctamente

const EquipmentTransferTable = () => {
  const [data] = useState(equipmentTransferData);

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Equipment Transfer Records
          </Typography>
        </CardHeader>
        <CardBody>
          {/* Tabla de transferencias de equipo */}
          <div className="overflow-x-auto">
            <table className="w-full table-auto border-collapse border border-gray-200">
              <thead>
                <tr>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">
                    Source Section
                  </th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">
                    Equipment
                  </th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">
                    Date
                  </th>
                </tr>
              </thead>
              <tbody>
                {data.map((transfer, index) => (
                  <tr key={index} className="hover:bg-gray-50">
                    <td className="border border-gray-200 px-4 py-2">{transfer.sourceSection}</td>
                    <td className="border border-gray-200 px-4 py-2">{transfer.equipment}</td>
                    <td className="border border-gray-200 px-4 py-2">{transfer.date}</td>
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

export default EquipmentTransferTable;
