import React, { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import api from "@/middlewares/api"; // Asegúrate de que api esté configurado correctamente

const EmployeeTable = () => {
  const [employeeList, setEmployeeList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Función para obtener los empleados de la API
    const fetchEmployees = async () => {
      try {
        const response = await api('/Employees/GET_ALL', {
          method: 'GET',
        });

        console.log(response)

        if (!response.ok) {
          throw new Error("Failed to fetch employees");
        }

        const data = await response.json();
        setEmployeeList(data); // Establecer los datos de los empleados
      } catch (err) {
        console.error("Error fetching employees:", err);
        setError("Failed to load employee data");
      } finally {
        setLoading(false);
      }
    };

    fetchEmployees();
  }, []);

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
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Email</th>
                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Role</th>
                  </tr>
                </thead>
                <tbody>
                  {employeeList.map((user) => (
                    <tr>
                      <td className="border border-gray-200 px-4 py-2">{user.name}</td>
                      <td className="border border-gray-200 px-4 py-2">{user.email || "N/A"}</td>
                      <td className="border border-gray-200 px-4 py-2">{user.userRole || "N/A"}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </CardBody>
      </Card>
    </div>
  );
};

export default EmployeeTable;
