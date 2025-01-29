import React, { useEffect, useState } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton, Input, Select, Option } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon, ArrowLeftIcon, MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { useLocation, useNavigate } from 'react-router-dom';
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";

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
  const pageSize = 12;

  useEffect(() => {
    fetchEquipments();
  }, [currentPage]);

  const fetchEquipments = async (pageNumber = currentPage) => {
    setLoading(true);
    setError(null);
  
    let url = `/Equipment/GetPaged/?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    console.log(`idSect:${sectionId} idDept ${departmentId}`);
  
    if (sectionId) {
      url = `/Equipment/equipments/section/${sectionId}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    } else if (departmentId) {
      url = `/Equipment/equipments/department/${departmentId}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    } else if (user.role.toLowerCase() !== "director"){
      url = `/Equipment/equipments/section-manager/${user.id}?PageNumber=${pageNumber}&PageSize=${pageSize}`
    }
  
    if (searchTerm) {
      url =
        searchType === "id"
          ? `/Equipment/equipments/${searchTerm}`
          : `/Equipment/equipments/search?name=${searchTerm}`;
    }
  
    try {
      const response = await api(url);
      if (!response.ok)
        throw new Error(
          response.status === 500 ? "Server Internal Error." : "Equipment not found."
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

  const getEquipment = async (targetId) => {
    setLoading(true);
    setError(null);
    try {
      // http://localhost:5217/api/Equipment/GET?equipmentId=1
      const response = await api(`/Equipment/GET?equipmentId=${targetId}`);
      if (!response.ok)
        throw new Error(
          response.status === 500 ? "Server Internal Error." : "Equipment not found."
        );
      const equipment = await response.json();
      setEquipmentData([equipment]);
      // setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }
  
  // Función de cambio de página con validación de rango
  const handlePageChange = (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= totalPages && pageNumber !== currentPage) {
      setCurrentPage(pageNumber);
    }
  };
  

  const getStatusColor = (status) => {
    switch (status) {
      case "Active":
        return "bg-green-100 text-green-800"; // Green for Active
      case "UnderMaintenance":
        return "bg-yellow-100 text-yellow-800"; // Yellow for Under Maintenance
      case "Decommissioned":
        return "bg-red-100 text-red-800"; // Red for Decommissioned
      default:
        return "bg-gray-100 text-gray-800"; // Default if status is unknown
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
    <Card className="mt-8 shadow-lg rounded-lg">
      <CardHeader variant="gradient" color="gray" className="p-6 flex items-center justify-between">
        <IconButton variant="text" color="white" onClick={() => navigate(-1)} className="mr-4">
          <ArrowLeftIcon className="h-6 w-6" />
        </IconButton>
        <Typography variant="h6" color="white" className="text-xl font-semibold">
          Equipment Inventory
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        <div className="flex items-center gap-4 mb-4 px-5">
          <Select value={searchType} label="Search By" onChange={setSearchType}>
            <Option value="name">Search by Name</Option>
            <Option value="id">Search by ID</Option>
          </Select>
          <Input 
            type="text" 
            label="Search"
            value={searchTerm} 
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && getEquipment(searchTerm)}
          />
        </div>

        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">{error}</Typography>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full table-auto text-sm text-gray-900 border-collapse">
              <thead className="bg-gray-800 text-white">
                <tr>
                  <th className="px-6 py-3 border-b text-center">Name</th>
                  <th className="px-6 py-3 border-b text-center">Type</th>
                  <th className="px-6 py-3 border-b text-center">Status</th>
                  <th className="px-6 py-3 border-b text-center">Acquisition Date</th>
                  <th className="px-6 py-3 border-b text-center">Section</th>
                  <th className="px-6 py-3 border-b text-center">Department</th>
                </tr>
              </thead>
              <tbody className="bg-white">
                {equipmentData.length > 0 ? (
                  equipmentData.map((equipment, index) => (
                    <tr key={index}>
                      <td className="px-6 py-3 border-b text-center">{equipment.name}</td>
                      <td className="px-6 py-3 border-b text-center">{equipment.type}</td>
                      <td className="px-6 py-3 border-b text-center">
                        <span className={`inline-block px-2 py-1 text-xs font-semibold rounded-full 
                          ${getStatusColor(equipment.status)}`}>
                          {equipment.status}
                        </span>
                      </td>
                      <td className="px-6 py-3 border-b text-center">{equipment.dateOfadquisition || "N/A"}</td>
                      <td className="px-6 py-3 border-b text-center">{equipment.sectionName || "N/A"}</td>
                      <td className="px-6 py-3 border-b text-center">{equipment.departmentName || "N/A"}</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="6" className="px-6 py-3 text-center">No equipment found</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}

        {totalPages > 1 && (
          <div className="flex justify-center mt-4 space-x-2">
            <IconButton
              variant="outlined"
              color="gray"
              onClick={() => handlePageChange(currentPage - 1)}
              disabled={currentPage === 1}
              className="px-4 py-2"
            >
              <ChevronLeftIcon className="h-5 w-5" />
            </IconButton>

            {renderPaginationButtons()}

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
        )}
      </CardBody>
    </Card>
  );
};

export default InventoryTable;
