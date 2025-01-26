import React, { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api"; // Asegúrate de que api esté configurado correctamente

const EmployeeTable = () => {
  const [employeeList, setEmployeeList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 15;

  useEffect(() => {
    fetchEmployees(currentPage);
  }, [currentPage]);

  const fetchEmployees = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(`/Employees/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
        method: "GET",
      });

      if (!response.ok) {
        throw new Error("Failed to fetch employees");
      }

      const data = await response.json();
      setEmployeeList(data.items); // Ajusta la estructura si es necesario
      console.log(data.items)
      setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcula el total de páginas
    } catch (err) {
      console.error("Error fetching employees:", err);
      setError("Failed to load employee data");
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
            Employees
          </Typography>
        </CardHeader>
        <CardBody>
          {/* Show load or error message */}
          {loading && (
            <Typography variant="small" color="gray">
              Loading employees...
            </Typography>
          )}
          {error && (
            <Typography variant="small" color="red">
              {error}
            </Typography>
          )}

          {/* Employee Table */}
          {!loading && !error && (
            <div className="overflow-x-auto">
              <table className="w-full table-auto border-collapse border border-gray-200">
                <thead>
                  <tr>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Username</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Role</th>
                  </tr>
                </thead>
                <tbody>
                  {employeeList.map((user) => (
                    <tr>
                      <td className="border border-gray-200 px-4 py-2">{user.name}</td>
                      <td className="border border-gray-200 px-4 py-2">{user.userRole || "N/A"}</td>
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

export default EmployeeTable;
