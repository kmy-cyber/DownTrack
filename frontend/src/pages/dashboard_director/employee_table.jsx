import React from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import userListData from "@/data/users-table-data"; // Ajusta la ruta según tu estructura de proyecto

const EmployeeTable = () => {
  // Filtrar usuarios con rol de "technician"
  const technicians = userListData.filter((user) => user.role === "technician");

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Employee Management Table
          </Typography>
        </CardHeader>
        <CardBody>
          {/* Tabla de Técnicos */}
          <div className="overflow-x-auto">
            <table className="w-full table-auto border-collapse border border-gray-200">
              <thead>
                <tr>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Name</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Username</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Section ID</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Experience</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Specialty</th>
                  <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Salary</th>
                </tr>
              </thead>
              <tbody>
                {technicians.map((user, index) => (
                  <tr key={index} className="hover:bg-gray-50">
                    <td className="border border-gray-200 px-4 py-2">{user.name}</td>
                    <td className="border border-gray-200 px-4 py-2">{user.username}</td>
                    <td className="border border-gray-200 px-4 py-2">{user.sectionId || "N/A"}</td>
                    <td className="border border-gray-200 px-4 py-2">{user.experience || "N/A"}</td>
                    <td className="border border-gray-200 px-4 py-2">{user.specialty || "N/A"}</td>
                    <td className="border border-gray-200 px-4 py-2">
                      {user.salary ? `$${user.salary}k` : "N/A"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </CardBody>
      </Card>
    </div>
  );
};

export default EmployeeTable;
