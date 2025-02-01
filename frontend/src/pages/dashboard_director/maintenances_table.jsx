import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import api from "@/middlewares/api"; // Asegúrate de que api esté configurado correctamente

const MaintenanceHistory = () => {
  const [maintenanceList, setMaintenanceList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

  useEffect(() => {
    fetchMaintenanceHistory(currentPage);
  }, [currentPage]);

  const fetchMaintenanceHistory = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/DoneMaintenance/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch maintenance data");
      }

      const data = await response.json();
      setMaintenanceList(data.items); // Ajusta la estructura si es necesario
      setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcula el total de páginas
    } catch (err) {
      console.error("Error fetching maintenance data:", err);
      setError("Failed to load maintenance data");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="mt-8 shadow-lg">
      <CardHeader
        variant="gradient"
        color="gray"
        className="flex items-center justify-between p-6"
      >
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Equipment Maintenance History
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
          <>
            {/* Maintenance History Table */}
            <div className="overflow-x-auto">
              <table className="min-w-full table-auto text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Technician
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment Name
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment ID
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Maintenance Type
                    </th>
                    <th className="border-b px-6 py-3 text-center">Date</th>
                    <th className="border-b px-6 py-3 text-center">Cost</th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {maintenanceList.length > 0 ? (
                    maintenanceList.map((maintenance) => (
                      <tr key={maintenance.id}>
                        <td className="border-b px-6 py-3 text-center">
                          {maintenance.technicianUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {maintenance.equipmentName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {maintenance.equipmentId}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {maintenance.type}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {new Date(maintenance.date).toLocaleDateString()}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          ${maintenance.cost}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="6" className="px-6 py-3 text-center">
                        No maintenance records found
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}

        {/* Paginación */}
        {!loading && !error && totalPages > 1 && (
          <div className="mt-4 flex justify-center">
            <Stack spacing={2}>
              <Pagination
                count={totalPages}
                page={currentPage}
                onChange={(_, value) => setCurrentPage(value)}
              />
            </Stack>
          </div>
        )}
      </CardBody>
    </Card>
  );
};

export default MaintenanceHistory;