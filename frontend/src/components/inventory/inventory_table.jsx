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

  const fetchEquipments = async () => {
    setLoading(true);
    setError(null);
    let url = `/Equipment/GetPaged/?pageNumber=${currentPage}&pageSize=${pageSize}`;

    if (searchTerm) {
      url = searchType === "id"
        ? `/Equipment/equipments/${searchTerm}`
        : `/Equipment/equipments/search?name=${searchTerm}`;
    }

    try {
      const response = await api(url);
      if (!response.ok) throw new Error(response.status === 500 ? "No equipment found in database." : "Equipment not found.");
      const data = await response.json();
      setEquipmentData(data.items || []);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
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

  return (
    <Card className="mt-8 shadow-lg">
      <CardHeader variant="gradient" color="gray" className="p-6 flex items-center justify-between">
        <IconButton variant="text" color="white" onClick={() => navigate(-1)} className="mr-4">
          <ArrowLeftIcon className="h-6 w-6" />
        </IconButton>
        <Typography variant="h6" color="white" className="text-xl font-semibold">Equipment Inventory</Typography>
      </CardHeader>
      <CardBody className="px-6 py-4">
        <div className="flex items-center gap-4 mb-4">
          <Select value={searchType} label="Search By" onChange={setSearchType}>
            <Option value="name">Search by Name</Option>
            <Option value="id">Search by ID</Option>
          </Select>
          <Input 
            type="text" 
            label="Search"
            value={searchTerm} 
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && fetchEquipments()}
          />
        </div>

        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">{error}</Typography>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full table-auto text-sm text-gray-900">
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
                          ${equipment.status.toLowerCase() === "active" ? 
                                                                "bg-green-100 text-green-800" : 
                                                                "bg-yellow-100 text-yellow-800"}`}>
                          {equipment.status}</span>
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
            >
              <ChevronLeftIcon className="h-5 w-5" />
            </IconButton>
            <Typography className="px-4 py-2">Page {currentPage} of {totalPages}</Typography>
            <IconButton
              variant="outlined"
              color="gray"
              onClick={() => handlePageChange(currentPage + 1)}
              disabled={currentPage === totalPages}
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
