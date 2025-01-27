import React, { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api"; // Asegúrate de que api esté configurado correctamente

const MaintenanceHistory = () => {
  const [maintenanceList, setMaintenanceList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 10;

  useEffect(() => {
    fetchMaintenanceHistory(currentPage);
  }, [currentPage]);

  const fetchMaintenanceHistory = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(`/DoneMaintenance/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
        method: "GET",
      });

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

  const handlePageChange = (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= totalPages) {
      setCurrentPage(pageNumber);
    }
  };

  const renderPaginationButtons = () => {
    const visibleButtons = 5;
    let startPage = Math.max(1, currentPage - Math.floor(visibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + visibleButtons - 1);

    if (endPage - startPage + 1 < visibleButtons) {
      startPage = Math.max(1, endPage - visibleButtons + 1);
    }

    return Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i).map((page) => (
      <Button
        key={page}
        variant={page === currentPage ? "filled" : "outlined"}
        color="gray"
        onClick={() => handlePageChange(page)}
        className="px-4 py-2"
      >
        {page}
      </Button>
    ));
  };

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Equipment Maintenance History
          </Typography>
        </CardHeader>
        <CardBody>
          {/* Show load or error message */}
          {loading && (
            <Typography variant="small" color="gray">
              Loading maintenance history...
            </Typography>
          )}
          {error && (
            <Typography variant="small" color="red">
              {error}
            </Typography>
          )}

          {/* Maintenance History Table */}
          {!loading && !error && (
            <div className="overflow-x-auto">
              <table className="w-full table-auto border-collapse border border-gray-200">
                <thead>
                  <tr>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Technician ID</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Equipment ID</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Maintenance Type</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Date</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Cost</th>
                  </tr>
                </thead>
                <tbody>
                  {maintenanceList.map((maintenance) => (
                    <tr key={maintenance.id} className="hover:bg-gray-50">
                      <td className="border border-gray-200 px-4 py-2">{maintenance.technicianId}</td>
                      <td className="border border-gray-200 px-4 py-2">{maintenance.equipmentId}</td>
                      <td className="border border-gray-200 px-4 py-2">{maintenance.type}</td>
                      <td className="border border-gray-200 px-4 py-2">{new Date(maintenance.date).toLocaleDateString()}</td>
                      <td className="border border-gray-200 px-4 py-2">${maintenance.cost}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}

          {/* Pagination */}
          {!loading && !error && totalPages > 1 && (
            <div className="flex justify-center mt-4 gap-2">
              {/* Previous button */}
              {currentPage > 1 && (
                <IconButton
                  variant="outlined"
                  size="sm"
                  color="gray"
                  onClick={() => handlePageChange(currentPage - 1)}
                  className="px-4 py-2"
                >
                  <ChevronLeftIcon className="h-5 w-5" />
                </IconButton>
              )}

              {/* Page numbers */}
              {renderPaginationButtons()}

              {/* Next button */}
              {currentPage < totalPages && (
                <IconButton
                  variant="outlined"
                  size="sm"
                  color="gray"
                  onClick={() => handlePageChange(currentPage + 1)}
                  className="px-4 py-2"
                >
                  <ChevronRightIcon className="h-5 w-5" />
                </IconButton>
              )}
            </div>
          )}
        </CardBody>
      </Card>
    </div>
  );
};

export default MaintenanceHistory;
