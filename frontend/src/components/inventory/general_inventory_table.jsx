import React from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { equipmentData } from "@/data/equipment-data"; // Ajusta la ruta de importación si es necesario.

const GeneralInventoryTable = () => {
  return (
    <Card className="mt-8 shadow-lg">
      <CardHeader variant="gradient" color="indigo" className="p-6">
        <Typography variant="h6" color="white" className="text-xl font-semibold">
          Equipment Inventory
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        <div className="overflow-x-auto">
          <table className="min-w-full table-auto text-sm text-gray-900">
            <thead className="bg-indigo-500 text-white">
              <tr>
                <th className="px-6 py-3 border-b text-left">Name</th>
                <th className="px-6 py-3 border-b text-left">Type</th>
                <th className="px-6 py-3 border-b text-left">Status</th>
                <th className="px-6 py-3 border-b text-left">Acquisition Date</th>
                <th className="px-6 py-3 border-b text-left">Location</th>
              </tr>
            </thead>
            <tbody className="bg-white">
              {equipmentData.map((equipment) => (
                <tr key={equipment.id} className="hover:bg-gray-100">
                  <td className="px-6 py-3 border-b">{equipment.name}</td>
                  <td className="px-6 py-3 border-b">{equipment.type}</td>
                  <td className="px-6 py-3 border-b">
                    <span
                      className={`inline-block px-2 py-1 text-xs font-semibold rounded-full ${
                        equipment.status === "active"
                          ? "bg-green-100 text-green-800"
                          : equipment.status === "inactive"
                          ? "bg-red-100 text-red-800"
                          : "bg-yellow-100 text-yellow-800"
                      }`}
                    >
                      {equipment.status}
                    </span>
                  </td>
                  <td className="px-6 py-3 border-b">{equipment.acquisitionDate}</td>
                  <td className="px-6 py-3 border-b">{equipment.location}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </CardBody>
    </Card>
  );
};

export default GeneralInventoryTable;
