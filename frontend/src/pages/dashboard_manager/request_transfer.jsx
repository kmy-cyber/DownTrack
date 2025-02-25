import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Input,
  Button,
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
  const [filteredDepartments, setFilteredDepartments] =
    useState(departmentData); // Nuevo estado para departamentos
  const [showEquipmentModal, setShowEquipmentModal] = useState(false);
  const [showDepartmentModal, setShowDepartmentModal] = useState(false); // Nuevo modal para departamentos
  const [errorMessage, setErrorMessage] = useState(""); // Estado para manejar errores de validación

  // Establecer automáticamente la fecha y hora en formato MySQL (YYYY-MM-DD HH:MM:SS) al cargar el componente
  useEffect(() => {
    const currentDate = new Date();
    const formattedDate = currentDate
      .toISOString()
      .slice(0, 19) // Recorta para obtener la fecha y hora en formato YYYY-MM-DDTHH:MM:SS
      .replace("T", " "); // Reemplaza "T" con espacio para formato MySQL
    setFormData((prev) => ({ ...prev, transferDate: formattedDate }));
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSearch = (e, type) => {
    const query = e.target.value;
    setSearchQuery(query);

    if (type === "equipment") {
      if (query) {
        setFilteredEquipment(
          equipmentData.filter((equipment) =>
            equipment.name.toLowerCase().includes(query.toLowerCase()),
          ),
        );
      } else {
        setFilteredEquipment(equipmentData);
      }
    } else if (type === "department") {
      if (query) {
        setFilteredDepartments(
          departmentData.filter((department) =>
            department.name.toLowerCase().includes(query.toLowerCase()),
          ),
        );
      } else {
        setFilteredDepartments(departmentData);
      }
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

    // Validación: Asegurarse de que todos los campos estén llenos
    if (!formData.equipment || !formData.department || !formData.transferDate) {
      setErrorMessage(
        "Please select equipment, department, and transfer date.",
      );
      return; // Evitar el envío del formulario si hay campos vacíos
    }

    // Si todos los campos son válidos, realizar el envío
    setErrorMessage(""); // Limpiar cualquier mensaje de error previo
    console.log("Transfer Request:", formData);
  };

  return (
    <div className="mx-auto mt-10 max-w-3xl rounded-md bg-white p-6 shadow-md">
      <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
        <Typography variant="h6" color="white">
          Request Transfer
        </Typography>
      </CardHeader>
      <CardBody>
        <form onSubmit={handleSubmit}>
          {/* Equipo Selección */}
          <div className="mb-6">
            <label
              htmlFor="equipment"
              className="block text-sm font-medium text-gray-700"
            >
              Select Equipment
            </label>
            <div className="relative">
              <Button
                type="button"
                onClick={() => handleModalOpen("equipment")}
                className="mt-1 w-full"
              >
                {formData.equipment
                  ? equipmentData.find((eq) => eq.id === formData.equipment)
                      ?.name
                  : "Select Equipment"}
              </Button>
            </div>
          </div>

          {/* Espacio entre el equipo y el destino */}
          <div className="my-6 border-t border-gray-300"></div>

          {/* Destino de Transferencia (Departamento) */}
          <div className="mb-6">
            <label
              htmlFor="department"
              className="block text-sm font-medium text-gray-700"
            >
              Select Destination Department
            </label>
            <div className="relative">
              <Button
                type="button"
                onClick={() => handleModalOpen("department")}
                className="mt-1 w-full"
              >
                {formData.department
                  ? departmentData.find(
                      (dept) => dept.id === formData.department,
                    )?.name
                  : "Select Department"}
              </Button>
            </div>
          </div>

          <div className="mb-6">
            <label
              htmlFor="transferDate"
              className="block text-sm font-medium text-gray-700"
            >
              Transfer Date
            </label>
            <Input
              type="text"
              id="transferDate"
              name="transferDate"
              value={formData.transferDate}
              readOnly // Deshabilitar la edición
              className="mt-1 w-full cursor-not-allowed bg-gray-100" // Estilo solo lectura
            />
          </div>

          {/* Mostrar mensaje de error si algún campo no se ha seleccionado */}
          {errorMessage && (
            <div className="mb-4 text-sm text-red-500">{errorMessage}</div>
          )}

          {/* Botón para enviar la solicitud */}
          <Button
            type="submit"
            // color="indigo"
            fullWidth
            className="mt-6"
          >
            Submit Transfer Request
          </Button>
        </form>
      </CardBody>

      {/* Modal de selección de equipo */}
      <Dialog
        open={showEquipmentModal}
        handler={() => handleModalClose("equipment")}
      >
        <DialogHeader>Select Equipment</DialogHeader>
        <DialogBody>
          <Input
            type="text"
            placeholder="Search Equipment"
            value={searchQuery}
            onChange={(e) => handleSearch(e, "equipment")}
            className="mb-4 w-full"
          />
          <div className="mt-3 max-h-72 overflow-y-auto">
            {filteredEquipment.map((equipment) => (
              <div
                key={equipment.id}
                className="cursor-pointer p-2 hover:bg-gray-100"
                onClick={() => handleEquipmentSelect(equipment.id)}
              >
                {equipment.name} - {equipment.type}
              </div>
            ))}
          </div>
        </DialogBody>
        <DialogFooter>
          <Button
            onClick={() => handleModalClose("equipment")}
            variant="outlined"
          >
            Close
          </Button>
        </DialogFooter>
      </Dialog>

      {/* Modal de selección de departamento */}
      <Dialog
        open={showDepartmentModal}
        handler={() => handleModalClose("department")}
      >
        <DialogHeader>Select Department</DialogHeader>
        <DialogBody>
          <Input
            type="text"
            placeholder="Search Department"
            value={searchQuery}
            onChange={(e) => handleSearch(e, "department")}
            className="mb-4 w-full"
          />
          <div className="mt-3 max-h-72 overflow-y-auto">
            {filteredDepartments.map((department) => (
              <div
                key={department.id}
                className="cursor-pointer p-2 hover:bg-gray-100"
                onClick={() => handleDepartmentSelect(department.id)}
              >
                {department.name}
              </div>
            ))}
          </div>
        </DialogBody>
        <DialogFooter>
          <Button
            onClick={() => handleModalClose("department")}
            variant="outlined"
          >
            Close
          </Button>
        </DialogFooter>
      </Dialog>
    </div>
  );
};

export default TransferRequestForm;
