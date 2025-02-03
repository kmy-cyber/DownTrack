import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  IconButton,
} from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import { Pagination } from "@mui/material";
import Stack from "@mui/material/Stack";
import api from "@/middlewares/api"; // Asegúrate de que api esté configurado correctamente

const TransferRequestsTable = () => {
  const [transferRequestsList, setTransferRequestsList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

  useEffect(() => {
    fetchTransferRequests(currentPage);
  }, [currentPage]);

  const fetchTransferRequests = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      // http://localhost:5217/api/TransferRequest/GetPaged?PageNumber=1&PageSize=5
      const response = await api(
        `/TransferRequest/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch transfer requests");
      }

      const data = await response.json();
      setTransferRequestsList(data.items || []);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
      console.log(data.items);
    } catch (err) {
      setError("Failed to fetch transfer requests data");
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
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Transfer Requests Records
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
            {/* Tabla de Solicitudes de Transferencia */}
            <div className="overflow-x-auto">
              <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Requester
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment ID
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Equipment Name
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Source Section
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Source Department
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Arrival Section
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Arrival Department
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Request Date
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {transferRequestsList.length > 0 ? (
                    transferRequestsList.map((request) => (
                      <tr key={request.id}>
                        <td className="border-b px-6 py-3 text-center">
                          {request.sectionManagerUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.equipmentId}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.equipmentName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.sourceSectionName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.sourceDepartmentName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.arrivalSectionName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.arrivalDepartmentName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {request.date}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="6" className="px-6 py-3 text-center">
                        No transfer requests found
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

export default TransferRequestsTable;