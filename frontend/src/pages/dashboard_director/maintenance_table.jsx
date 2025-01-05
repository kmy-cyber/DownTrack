import React, { useState } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { maintenanceHistData } from "@/data/maintenance-hist-data"; // Asegúrate de importar los datos correctamente

const MaintenanceHistory = () => {
  const [data] = useState(maintenanceHistData);

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Equipment Maintenance History
          </Typography>
        </CardHeader>
        <CardBody>
          {/* Tabla de historial de mantenimiento */}
          <div className="overflow-x-auto">
            <table className="w-full table-auto border-collapse border border-gray-200">
              <thead>
                <tr>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Equipment</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Maintenance Type</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Technician</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Date</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Cost</th>
                </tr>
              </thead>
              <tbody>
                {data.map((maintenance, index) => (
                  <tr key={index} className="hover:bg-gray-50">
                    <td className="border border-gray-200 px-4 py-2">{maintenance.equipment}</td>
                    <td className="border border-gray-200 px-4 py-2">{maintenance.maintenanceType}</td>
                    <td className="border border-gray-200 px-4 py-2">{maintenance.technician}</td>
                    <td className="border border-gray-200 px-4 py-2">{maintenance.date}</td>
                    <td className="border border-gray-200 px-4 py-2">{maintenance.cost}</td>
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

export default MaintenanceHistory;
