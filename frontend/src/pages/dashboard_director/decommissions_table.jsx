import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import { Pagination } from "@mui/material";
import api from "@/middlewares/api";

const EquipmentDecommissionsTable = () => {
  const [disposalsList, setDisposalsList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

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
    } catch (err) {
      setError("Failed to load decommissionings data");
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (event, newPage) => {
    setCurrentPage(newPage);
  };

  return (
    <Card className="mt-8 rounded-lg shadow-lg">
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
          Equipment Disposal Records
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
            <div className="overflow-x-auto">
              <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Technician
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment
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
        <div className="mt-4 flex justify-center">
          <Pagination
            count={totalPages}
            page={currentPage}
            onChange={handlePageChange}
            className="self-center"
          />
        </div>
      </CardBody>
    </Card>
  );
};

export default EquipmentDecommissionsTable;