import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  Input,
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
} from "@material-tailwind/react";
import { departmentData } from "@/data"; // Asume que los departamentos ya están definidos
import { equipmentData } from "@/data"; // Asume que los equipos ya están definidos

export const TransferRequestForm = () => {
  const [formData, setFormData] = useState({
    equipment: "",
    department: "",
    transferDate: "",
  });
  const [searchQuery, setSearchQuery] = useState("");
  const [filteredEquipment, setFilteredEquipment] = useState(equipmentData);
  const [filteredDepartments, setFilteredDepartments] = useState(departmentData);
  const [showEquipmentModal, setShowEquipmentModal] = useState(false);
  const [showDepartmentModal, setShowDepartmentModal] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    // Genera la fecha y hora actuales en formato MySQL (YYYY-MM-DD HH:MM:SS)
    const now = new Date();
    const formattedDate = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, "0")}-${String(
      now.getDate()
    ).padStart(2, "0")} ${String(now.getHours()).padStart(2, "0")}:${String(
      now.getMinutes()
    ).padStart(2, "0")}:${String(now.getSeconds()).padStart(2, "0")}`;
    setFormData((prev) => ({ ...prev, transferDate: formattedDate }));
  }, []);

  const handleSearch = (e, type) => {
    const query = e.target.value;
    setSearchQuery(query);

    if (type === "equipment") {
      setFilteredEquipment(
        equipmentData.filter((equipment) =>
          equipment.name.toLowerCase().includes(query.toLowerCase())
        )
      );
    } else if (type === "department") {
      setFilteredDepartments(
        departmentData.filter((department) =>
          department.name.toLowerCase().includes(query.toLowerCase())
        )
      );
    }
  };

  const handleModalOpen = (type) => {
    if (type === "equipment") {
      setShowEquipmentModal(true);
    } else if (type === "department") {
      setShowDepartmentModal(true);
    }
  };

  const handleModalClose = (type) => {
    if (type === "equipment") {
      setShowEquipmentModal(false);
    } else if (type === "department") {
      setShowDepartmentModal(false);
    }
  };

  const handleEquipmentSelect = (equipmentId) => {
    setFormData((prev) => ({ ...prev, equipment: equipmentId }));
    handleModalClose("equipment");
  };

  const handleDepartmentSelect = (departmentId) => {
    setFormData((prev) => ({ ...prev, department: departmentId }));
    handleModalClose("department");
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.equipment || !formData.department) {
      setErrorMessage("Please select equipment and department.");
      return;
    }

    setErrorMessage("");
    console.log("Transfer Request to MySQL:", formData);
  };

  return (
    <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
      <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
        <Typography variant="h6" color="white">
          Request Transfer
        </Typography>
      </CardHeader>
      <CardBody>
        <form onSubmit={handleSubmit}>
          {/* Selección de equipo */}
          <div className="mb-6">
            <label htmlFor="equipment" className="block text-sm font-medium text-gray-700">
              Select Equipment
            </label>
            <Button
              type="button"
              onClick={() => handleModalOpen("equipment")}
              className="mt-1 w-full"
            >
              {formData.equipment
                ? equipmentData.find((eq) => eq.id === formData.equipment)?.name
                : "Select Equipment"}
            </Button>
          </div>

          {/* Separador */}
          <div className="my-6 border-t border-gray-300"></div>

          {/* Selección de departamento */}
          <div className="mb-6">
            <label htmlFor="department" className="block text-sm font-medium text-gray-700">
              Select Destination Department
            </label>
            <Button
              type="button"
              onClick={() => handleModalOpen("department")}
              className="mt-1 w-full"
            >
              {formData.department
                ? departmentData.find((dept) => dept.id === formData.department)?.name
                : "Select Department"}
            </Button>
          </div>

          {/* Fecha generada automáticamente */}
          <div className="mb-6">
            <label htmlFor="transferDate" className="block text-sm font-medium text-gray-700">
              Transfer Date
            </label>
            <Input
              type="text"
              id="transferDate"
              name="transferDate"
              value={formData.transferDate}
              className="mt-1 w-full"
              readOnly
            />
          </div>

          {/* Mensaje de error */}
          {errorMessage && <div className="text-red-500 text-sm mb-4">{errorMessage}</div>}

          {/* Botón de envío */}
          <Button type="submit" color="indigo" fullWidth className="mt-6">
            Submit Transfer Request
          </Button>
        </form>
      </CardBody>
    </div>
  );
};

export default TransferRequestForm;