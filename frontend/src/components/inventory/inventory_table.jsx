import React, { useEffect, useState } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  IconButton,
  Input,
  Select,
  Option,
} from "@material-tailwind/react";
import { ArrowLeftIcon } from "@heroicons/react/24/outline";
import { useLocation, useNavigate } from "react-router-dom";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import SectionSelectionModal from "@/pages/dashboard_manager/section_selection";
import { ArrowsRightLeftIcon } from "@heroicons/react/24/solid";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";

const InventoryTable = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();
  const { sectionId, departmentId } = location.state || {};

  const [equipmentData, setEquipmentData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [searchType, setSearchType] = useState("name");
  const [searchTerm, setSearchTerm] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedEquipmentId, setSelectedEquipmentId] = useState(null); // Estado para el equipo seleccionado
  const isSectionManager = user.role.toLowerCase() === "sectionmanager";
  const pageSize = 12;

  useEffect(() => {
    fetchEquipments(currentPage);
  }, [currentPage]);

  const fetchEquipments = async (pageNumber) => {
    setLoading(true);
    setError(null);

    let url = `/Equipment/GetPaged/?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    if (sectionId) {
      url = `/Equipment/equipments/section/${sectionId}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    } else if (departmentId) {
      url = `/Equipment/equipments/department/${departmentId}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    } else if (isSectionManager) {
      url = `/Equipment/equipments/section-manager/${user.id}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    }

    try {
      const response = await api(url);
      if (!response.ok)
        throw new Error(
          response.status === 500
            ? "Server Internal Error."
            : "Equipment not found.",
        );
      const data = await response.json();
      setEquipmentData(data.items || []);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const getEquipmentById = async (targetId) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(`/Equipment/GET?equipmentId=${targetId}`);
      if (!response.ok)
        throw new Error(
          response.status === 500
            ? "Server Internal Error."
            : "Equipment not found.",
        );
      const equipment = await response.json();
      setEquipmentData([equipment]);
      setTotalPages(0); // No paginación al buscar por ID
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const getEquipmentByName = async (targetName) => {
    setLoading(true);
    setError(null);
    try {
      let url = isSectionManager ? 
      `/Equipment/SearchByNameAndBySectionManagerId/${user.id}?PageNumber=${currentPage}&PageSize=${pageSize}&equipmentName=${targetName}` :
      // http://localhost:5217/api/Equipment/SearchByName?PageNumber=1&PageSize=5&equipmentName=equipment_6
      `/Equipment/SearchByName?PageNumber=${1}&PageSize=${1}&equipmentName=${searchTerm}`
      const response = await api(url);
      if (!response.ok)
        throw new Error(
          response.status === 500
            ? "Server Internal Error."
            : "Equipment not found.",
        );
      const data = await response.json();
      setEquipmentData(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (pageNumber) => {
    if (
      pageNumber >= 1 &&
      pageNumber <= totalPages &&
      pageNumber !== currentPage
    ) {
      setCurrentPage(pageNumber);
    }
  };

  const getStatusColor = (status) => {
    switch (status) {
      case "Active":
        return "bg-green-100 text-green-800";
      case "UnderMaintenance":
        return "bg-yellow-100 text-yellow-800";
      case "Decommissioned":
        return "bg-red-100 text-red-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const renderPaginationButtons = () => {
    const visibleButtons = 5;
    let startPage = Math.max(1, currentPage - Math.floor(visibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + visibleButtons - 1);

    if (endPage - startPage + 1 < visibleButtons) {
      startPage = Math.max(1, endPage - visibleButtons + 1);
    }

    return Array.from(
      { length: endPage - startPage + 1 },
      (_, i) => startPage + i,
    ).map((page) => (
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
    <Card className="mt-8 rounded-lg shadow-lg">
      <CardHeader
        variant="gradient"
        color="gray"
        className="flex items-center justify-between p-6"
      >
        <IconButton
          variant="text"
          color="white"
          onClick={() => navigate(-1)}
          className="mr-4"
        >
          <ArrowLeftIcon className="h-6 w-6" />
        </IconButton>
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Equipment Inventory
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        <div className="mb-4 flex items-center gap-1 px-5">
          {/* <Select value={searchType} label="Search By" onChange={setSearchType}>
            <Option value="name">Search by Name</Option>
            <Option value="id">Search by ID</Option>
          </Select> */}
          <Input
            type="text"
            label="Search by Name"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter" && searchTerm.trim()) {
                e.preventDefault();
                getEquipmentByName(searchTerm);
              }
            }}
          />
        </div>

        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">
            {error}
          </Typography>
        ) : (
          <div className="overflow-x-auto">
            <table className="mt-0 min-w-full table-auto border-collapse text-sm text-gray-900">
              <thead className="bg-gray-800 text-white">
                <tr>
                  <th className="border-b px-6 py-3 text-center">ID</th>
                  <th className="border-b px-6 py-3 text-center">Name</th>
                  <th className="border-b px-6 py-3 text-center">Type</th>
                  <th className="border-b px-6 py-3 text-center">Status</th>
                  <th className="border-b px-6 py-3 text-center">
                    Acquisition Date
                  </th>
                  <th className="border-b px-6 py-3 text-center">Section</th>
                  <th className="border-b px-6 py-3 text-center">Department</th>
                  {isSectionManager && (
                    <th className="border-b px-6 py-3 text-center">Transfer</th>
                  )}
                </tr>
              </thead>
              <tbody className="bg-white">
                {equipmentData.length > 0 ? (
                  equipmentData.map((equipment, index) => (
                    <tr key={index}>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.id}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.name}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.type}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        <span
                          className={`inline-block rounded-full px-2 py-1 text-xs font-semibold 
                          ${getStatusColor(equipment.status)}`}
                        >
                          {equipment.status}
                        </span>
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.dateOfadquisition || "N/A"}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.sectionName || "N/A"}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {equipment.departmentName || "N/A"}
                      </td>
                      {isSectionManager && (
                        <td className="border-b px-6 py-3 text-center">
                          <IconButton
                            onClick={() => {
                              setSelectedEquipmentId(equipment.id); // Guardar el ID del equipo seleccionado
                              setIsModalOpen(true);
                            }}
                            className="rounded-full px-4 py-2"
                          >
                            <ArrowsRightLeftIcon className="h-5 w-5" />
                          </IconButton>
                        </td>
                      )}
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="6" className="px-6 py-3 text-center">
                      No equipment found
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
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

      {isSectionManager && (
        <SectionSelectionModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={null}
          eqiD={selectedEquipmentId} // Pasar el ID del equipo seleccionado al modal
        />
      )}
    </Card>
  );
};

export default InventoryTable;
