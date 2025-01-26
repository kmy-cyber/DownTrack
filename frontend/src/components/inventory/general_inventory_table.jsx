import React, { useEffect, useState } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import api from "@/middlewares/api";

const GeneralInventoryTable = () => {
  const [equipmentData, setEquipmentData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const pageSize = 15;

  useEffect(() => {
    fetchAllEquipments();
  }, []);

  const fetchAllEquipments = async () => {
    let allEquipments = [];
    let pageNumber = 1;
    let hasMore = true;

    try {
      while (hasMore) {
        const response = await api(`/Equipment/GetPaged/?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
          method: 'GET',
          params: {
            PageNumber: pageNumber,
            PageSize: pageSize,
          },
        });

        if (!response.ok) {
          throw new Error('Failed to fetch equipment data');
        }

        console.log(response)
        const data = await response.json();

        console.log(data);

        if (!data.items) {
          throw new Error('Unexpected response structure');
        }

        allEquipments = [...allEquipments, ...data.items];

        if (data.items.length < pageSize) {
          hasMore = false; // Si hay menos registros que pageSize, no hay más páginas
        } else {
          pageNumber += 1; // Avanza al siguiente número de página
        }
      }

      setEquipmentData(allEquipments);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };
  
  return (
    <Card className="mt-8 shadow-lg">
      <CardHeader variant="gradient" color="gray" className="p-6">
        <Typography variant="h6" color="white" className="text-xl font-semibold">
          Equipment Inventory
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">
            {error}
          </Typography>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full table-auto text-sm text-gray-900">
              <thead className="bg-gray-800 text-white">
                <tr>
                  <th className="px-6 py-3 border-b text-left">Name</th>
                  <th className="px-6 py-3 border-b text-left">Type</th>
                  <th className="px-6 py-3 border-b text-left">Status</th>
                  <th className="px-6 py-3 border-b text-left">Acquisition Date</th>
                  <th className="px-6 py-3 border-b text-left">Location</th>
                </tr>
              </thead>
              <tbody className="bg-white">
                {equipmentData.length > 0 ? (
                  equipmentData.map((equipment, index) => (
                    <tr key={index}>
                      <td className="px-6 py-3 border-b">{equipment.name}</td>
                      <td className="px-6 py-3 border-b">{equipment.type}</td>
                      <td className="px-6 py-3 border-b">
                        <span
                          className={`inline-block px-2 py-1 text-xs font-semibold rounded-full ${
                            equipment.status.toLowerCase() === "active"
                              ? "bg-green-100 text-green-800"
                              : equipment.status.toLowerCase() === "inactive"
                              ? "bg-red-100 text-red-800"
                              : "bg-yellow-100 text-yellow-800"
                          }`}
                        >
                          {equipment.status}
                        </span>
                      </td>
                      <td className="px-6 py-3 border-b">
                        {equipment.dateOfadquisition || "N/A"}
                      </td>
                      <td className="px-6 py-3 border-b">{equipment.departmentId || "N/A"}</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="5" className="px-6 py-3 text-center">No equipment found</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </CardBody>
    </Card>
  );
};

export default GeneralInventoryTable;
