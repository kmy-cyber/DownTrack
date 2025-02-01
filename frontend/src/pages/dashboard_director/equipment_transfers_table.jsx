import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import api from "@/middlewares/api";

const EquipmentTransferTable = () => {
  const [transferData, setTransferData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

  useEffect(() => {
    fetchTransfers(currentPage);
  }, [currentPage]);

  const fetchTransfers = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/Transfer/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        { method: "GET" },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch transfers");
      }

      const data = await response.json();
      setTransferData(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      setError("Failed to load transfer data");
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
          Equipment Transfer Records
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
              <table className="min-w-full table-auto text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Shipping Supervisor
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment Receptor
                    </th>
                    <th className="border-b px-6 py-3 text-center">Date</th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {transferData.length > 0 ? (
                    transferData.map((transfer, index) => (
                      <tr key={index}>
                        <td className="border-b px-6 py-3 text-center">
                          {transfer.shippingSupervisorName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {transfer.equipmentReceptorUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {transfer.date}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="3" className="px-6 py-3 text-center">
                        No transfer records found
                      </td>
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

export default EquipmentTransferTable;