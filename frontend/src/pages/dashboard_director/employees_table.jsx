import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import { Pagination } from "@mui/material";
import api from "@/middlewares/api";

const EmployeesTable = () => {
  const [employeeList, setEmployeeList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

  useEffect(() => {
    fetchEmployees(currentPage);
  }, [currentPage]);

  const fetchEmployees = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/Employee/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch employees");
      }

      const data = await response.json();
      setEmployeeList(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      console.error("Error fetching employees:", err);
      setError("Failed to load employee data");
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = async (event, newPage) => {
    setCurrentPage(newPage);
    await fetchEmployees(newPage);
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
          Employees
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
                  <th className="border-b px-6 py-3 text-center">Username</th>
                  <th className="border-b px-6 py-3 text-center">Role</th>
                </tr>
              </thead>
              <tbody className="bg-white">
                {employeeList.length > 0 ? (
                  employeeList.map((user) => (
                    <tr key={user.id}>
                      <td className="border-b px-6 py-3 text-center">
                        {user.userRole.toLowerCase() === "shippingsupervisor"
                          ? user.name
                          : user.userName}
                      </td>
                      <td className="border-b px-6 py-3 text-center">
                        {user.userRole || "N/A"}
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="2" className="px-6 py-3 text-center">
                      No employees found
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}

        {!loading && !error && totalPages > 1 && (
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

export default EmployeesTable;