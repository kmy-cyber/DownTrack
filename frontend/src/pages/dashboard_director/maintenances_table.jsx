import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Input,
  IconButton,
  Select,
  Option,
} from "@material-tailwind/react";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import api from "@/middlewares/api";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";

const MaintenanceHistory = () => {
  const [maintenanceList, setMaintenanceList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [searchValue, setSearchValue] = useState("");
  const [searchCriteria, setSearchCriteria] = useState("equipmentId");
  const [isSearching, setIsSearching] = useState(false);
  const pageSize = 13;

  useEffect(() => {
    if (!isSearching) {
      fetchMaintenanceHistory(currentPage);
    } else {
      handleSearch(false);
    }
  }, [currentPage, isSearching]);

  const fetchMaintenanceHistory = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/DoneMaintenance/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        { method: "GET" }
      );

      if (!response.ok) throw new Error("Failed to fetch maintenance data");

      const data = await response.json();
      setMaintenanceList(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      console.error("Error fetching maintenance data:", err);
      setError("Failed to load maintenance data");
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async (resetPage = true) => {
    if (!searchValue) return;
    setLoading(true);
    setError(null);
    setIsSearching(true);
    if (resetPage) setCurrentPage(1);

    try {
      const query =
        searchCriteria === "equipmentId"
          ? `/DoneMaintenance/Get_Maintenances_By_EquipmentId?PageNumber=${currentPage}&PageSize=${pageSize}&equipmentId=${searchValue}`
          : `/DoneMaintenance/Get_Maintenances_By_Technician_UserName?PageNumber=${currentPage}&PageSize=${pageSize}&technicianUserName=${searchValue}`;

      const response = await api(query, { method: "GET" });

      if (!response.ok) throw new Error("Failed to fetch maintenance data");

      const data = await response.json();
      setMaintenanceList(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      console.error("Error fetching maintenance data:", err);
      setError("Failed to load maintenance data");
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setSearchValue("");
    setIsSearching(false);
    setCurrentPage(1);
  };

  const handlePageChange = (_, value) => {
    setCurrentPage(value);
  };

  return (
    <Card className="mt-8 shadow-lg">
      <CardHeader variant="gradient" color="gray" className="flex items-center justify-between p-6">
        {isSearching && (
          <IconButton variant="text" size="sm" color="white" onClick={handleReset} className="mr-4">
            <ArrowLeftIcon className="h-5 w-5" />
          </IconButton>
        )}
        <Typography variant="h6" color="white" className="text-xl font-semibold">
          Equipment Maintenance History
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        <div className="flex flex-col gap-4 px-6 md:flex-row">
          <Select label="Search Criteria" value={searchCriteria} onChange={setSearchCriteria}>
            <Option value="equipmentId">Equipment ID</Option>
            <Option value="technician">Technician</Option>
          </Select>
          <Input
            label="Search Value"
            value={searchValue}
            onChange={(e) => setSearchValue(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleSearch();
              }
            }}
          />
        </div>
        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">{error}</Typography>
        ) : (
          <>
            <div className="mt-4 overflow-x-auto">
              <table className="min-w-full table-auto text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">Technician</th>
                    <th className="border-b px-6 py-3 text-center">Equipment Name</th>
                    <th className="border-b px-6 py-3 text-center">Equipment ID</th>
                    <th className="border-b px-6 py-3 text-center">Maintenance Type</th>
                    <th className="border-b px-6 py-3 text-center">Date</th>
                    <th className="border-b px-6 py-3 text-center">Cost</th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {maintenanceList.length > 0 ? (
                    maintenanceList.map((maintenance) => (
                      <tr key={maintenance.id}>
                        <td className="border-b px-6 py-3 text-center">{maintenance.technicianUserName}</td>
                        <td className="border-b px-6 py-3 text-center">{maintenance.equipmentName}</td>
                        <td className="border-b px-6 py-3 text-center">{maintenance.equipmentId}</td>
                        <td className="border-b px-6 py-3 text-center">{maintenance.type}</td>
                        <td className="border-b px-6 py-3 text-center">{maintenance.date.split('T')[0]}</td>
                        <td className="border-b px-6 py-3 text-center">${maintenance.cost}</td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="6" className="px-6 py-3 text-center">No maintenance records found</td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}
        {!loading && !error && totalPages > 1 && (
          <div className="mt-4 flex justify-center">
            <Stack spacing={2}>
              <Pagination count={totalPages} page={currentPage} onChange={handlePageChange} />
            </Stack>
          </div>
        )}
      </CardBody>
    </Card>
  );
};

export default MaintenanceHistory;
