import React, { useEffect, useState } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline"; // Importar los íconos
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";

const GeneralInventoryTable = () => {
  const [equipmentData, setEquipmentData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1); // Página actual
  const [totalPages, setTotalPages] = useState(0); // Total de páginas
  const pageSize = 15;

  const { user } = useAuth();

  useEffect(() => {
    if (user.role.toLowerCase() === "director") {
      fetchAllDirectorEquipments();
    } else if (user.role.toLowerCase() === "sectionmanager") {
      fetchAllManagerEquipments();
    }
  }, [user.role, currentPage]); // Re-fetch cuando cambie la página

  const fetchAllDirectorEquipments = async () => {
    let allEquipments = [];
    const pageNumber = currentPage;

    try {
      const response = await api(`/Equipment/GetPaged/?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
        method: "GET",
        params: {
          PageNumber: pageNumber,
          PageSize: pageSize,
        },
      });

      if (!response.ok) {
        throw new Error("Failed to fetch equipment data");
      }

      const data = await response.json();

      if (!data.items) {
        throw new Error("Unexpected response structure");
      }

      allEquipments = data.items;

      // Establecer los datos y la cantidad total de páginas
      setEquipmentData(allEquipments);
      setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcular el número total de páginas
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchAllManagerEquipments = async () => {
    let allEquipments = [];
    const pageNumber = currentPage;

    try {
      const response = await api(`/Equipment/GetPaged/?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
        method: "GET",
        params: {
          PageNumber: pageNumber,
          PageSize: pageSize,
        },
      });

      if (!response.ok) {
        throw new Error("Failed to fetch equipment data");
      }

      const data = await response.json();

      if (!data.items) {
        throw new Error("Unexpected response structure");
      }

      allEquipments = data.items;

      // Establecer los datos y la cantidad total de páginas
      setEquipmentData(allEquipments);
      setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcular el número total de páginas
    } catch (err) {
      setError(err.message);
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
    const visibleButtons = 5; // Número máximo de botones visibles
    let startPage = Math.max(1, currentPage - Math.floor(visibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + visibleButtons - 1);

    // Ajustar rango si estamos cerca del inicio o final
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
        {/* Paginación */}
        <div className="flex justify-center mt-4 space-x-2">
          {/* Botón "Anterior" con ícono de Chevron */}
          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className="px-4 py-2"
          >
            <ChevronLeftIcon className="h-5 w-5" />
          </IconButton>

          {/* Botones dinámicos de paginación */}
          {renderPaginationButtons()}

          {/* Botón "Siguiente" con ícono de Chevron */}
          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className="px-4 py-2"
          >
            <ChevronRightIcon className="h-5 w-5" />
          </IconButton>
        </div>
      </CardBody>
    </Card>
  );
};

export default GeneralInventoryTable;
