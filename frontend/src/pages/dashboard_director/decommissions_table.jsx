import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Input,
  IconButton
} from "@material-tailwind/react";
import { Pagination } from "@mui/material";
import api from "@/middlewares/api";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";

const EquipmentDecommissionsTable = () => {
  const [disposalsList, setDisposalsList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searching, setSearching] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 13;

  useEffect(() => {
    fetchDisposals(currentPage);
  }, [currentPage]);

  const fetchDisposals = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/EquipmentDecommissioning/Get_Paged_All?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch equipment decommissionings");
      }

      const data = await response.json();
      setDisposalsList(data.items || []);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
      console.log(data.items);
    } catch (err) {
      setError("Failed to load decommissionings data");
    } finally {
      setLoading(false);
    }
  };

  const searchById = async (equipmentId) => {
    setLoading(true);
    setError(null);
    try {
      // http://localhost:5217/api/EquipmentDecommissioning/Get_Decomissions_By_Equipment_Id/1
      const response = await api(
        `/EquipmentDecommissioning/Get_Decomissions_By_Equipment_Id/${equipmentId}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch equipment decommissionings");
      }

      const equipment = await response.json();
      console.log(equipment);
      setDisposalsList([equipment]);
    } catch (err) {
      setError("The equipment has not been decommissioned yet.");
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (event, newPage) => {
    setCurrentPage(newPage);
  };

  const handleKeyDown = (e) => {
                            // is number
    if (e.key === "Enter" && searchTerm.match(/\d+/)) {
      e.preventDefault();
      setSearching(true);
      searchById(searchTerm);
    }
  };

  const resetSearch = () => {
    setSearchTerm("");
    setSearching(false);
    fetchDisposals(1);
  };

  return (
    <Card className="mt-8 rounded-lg shadow-lg">
      <CardHeader
        variant="gradient"
        color="gray"
        className="flex items-center justify-between p-6"
      >
      {searching && (
        <IconButton
          variant="text"
          size="sm"
          color="white"
          onClick={resetSearch}
          className="mr-4"
        >
          <ArrowLeftIcon className="h-5 w-5" />
        </IconButton>
      )}
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Equipment Decommissionings Records
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        <div>
          <Input
            label="Search by Id"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={handleKeyDown}
          ></Input>
        </div>

        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">
            {error}
          </Typography>
        ) : (
          <>
            <div className="mt-3 overflow-x-auto">
              <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Technician
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment ID
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment Name
                    </th>
                    <th className="border-b px-6 py-3 text-center">Receptor</th>
                    <th className="border-b px-6 py-3 text-center">
                      Reason for Removal
                    </th>
                    <th className="border-b px-6 py-3 text-center">Date</th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {disposalsList.length > 0 ? (
                    disposalsList.map((disposal) => (
                      <tr key={disposal.id}>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.technicianUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.equipmentId}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.equipmentName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.receptorUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.cause}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {disposal.date}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="5" className="px-6 py-3 text-center">
                        No disposals found
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}

        {/* Paginación */}
        {!searching && (
          <div className="mt-4 flex justify-center">
            <Pagination
              count={totalPages}
              page={currentPage}
              onChange={handlePageChange}
              className="self-center"
            />
          </div>
        )}
      </CardBody>
    </Card>
  );
};

export default EquipmentDecommissionsTable;
