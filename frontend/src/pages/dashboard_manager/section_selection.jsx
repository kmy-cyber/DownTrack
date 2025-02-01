import React, { useState, useEffect } from "react";
import {
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
  Button,
  Select,
  Option,
} from "@material-tailwind/react";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const SectionSelectionModal = ({ isOpen, onClose, onSave, eqiD }) => {
  const [sections, setSections] = useState([]); // Lista de secciones
  const [departments, setDepartments] = useState([]); // Lista de departamentos
  const [selectedSection, setSelectedSection] = useState(null); // Sección seleccionada
  const [selectedDepartment, setSelectedDepartment] = useState(null); // Departamento seleccionado
  const { user } = useAuth();

  // Función para cargar las secciones
  useEffect(() => {
    const fetchSections = async () => {
      try {
        const response = await api("/Section/GET_ALL", { method: "GET" });
        if (response.ok) {
          const data = await response.json();
          setSections(data); // Lista de secciones
        }
      } catch (err) {
        console.error("Error fetching sections", err);
      }
    };
    fetchSections();
  }, []);

  // Función para cargar los departamentos de la sección seleccionada
  useEffect(() => {
    if (selectedSection) {
      const fetchDepartments = async () => {
        try {
          const response = await api(
            `/Department/GetAllDepartment_In_Section?sectionId=${selectedSection.id}`,
            { method: "GET" },
          );
          if (response.ok) {
            const data = await response.json();
            setDepartments(data); // Lista de departamentos para la sección seleccionada
            setSelectedDepartment(null); // Resetear el departamento seleccionado
          }
        } catch (err) {
          console.error("Error fetching departments", err);
        }
      };
      fetchDepartments();
    } else {
      setDepartments([]); // Si no hay sección seleccionada, limpiamos los departamentos
      setSelectedDepartment(null); // También resetear el departamento
    }
  }, [selectedSection]);

  // Función para manejar la aceptación de la selección
  const handleAccept = async () => {
    if (selectedSection && selectedDepartment) {
      try {
        const requestBody = {
          date: new Date().toISOString(),
          sectionManagerId: user.id,
          equipmentId: eqiD,
          arrivalDepartmentId: selectedDepartment,
          arrivalSectionId: selectedSection.id,
        };

        const response = await api("/TransferRequest/POST", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(requestBody),
        });

        if (response.ok) {
          const data = await response.json();
          if (typeof onSave === "function") {
            onSave({
              section: selectedSection,
              department: selectedDepartment,
            });
          }
          toast.success("Transfer successful!", {
            position: "top-center",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            theme: "colored",
          });
          onClose(); // Cerrar el modal después de guardar
        } else {
          toast.error("Transfer failed. Please try again.", {
            position: "top-center",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            theme: "colored",
          });
        }
      } catch (err) {
        console.error("Error saving data", err);
        toast.error("An error occurred. Please try again.", {
          position: "top-center",
          autoClose: 3000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: false,
          draggable: true,
          theme: "colored",
        });
      }
    } else {
      toast.warning("You must select both a section and a department.", {
        position: "top-center",
        autoClose: 3000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        theme: "colored",
      });
    }
  };

  return (
    <>
      <Dialog open={isOpen} handler={onClose} size="sm">
        <DialogHeader className="text-xl font-semibold text-gray-800">
          Select Section and Department
        </DialogHeader>
        <DialogBody className="space-y-4 p-5">
          {/* Select de Sección */}
          <Select
            label="Section"
            value={selectedSection ? selectedSection.id : ""}
            onChange={(value) => {
              const selected = sections.find((section) => section.id === value); // Encuentra la sección completa
              setSelectedSection(selected); // Establece la sección seleccionada completa
            }}
          >
            {sections.map((section) => (
              <Option key={section.id} value={section.id}>
                {section.name}
              </Option>
            ))}
          </Select>

          {/* Select de Departamento */}
          <Select
            label="Department"
            value={selectedDepartment || ""}
            onChange={(value) => setSelectedDepartment(value)}
            disabled={!selectedSection || departments.length === 0}
          >
            {departments.length === 0 ? (
              <Option disabled>No departments available</Option>
            ) : (
              departments.map((department) => (
                <Option key={department.id} value={department.id}>
                  {department.name}
                </Option>
              ))
            )}
          </Select>
        </DialogBody>
        <DialogFooter className="space-x-3">
          <Button
            variant="outlined"
            color="gray"
            onClick={onClose}
            className="w-32 rounded-md border-gray-300 py-2 text-gray-700 hover:border-gray-500"
          >
            Cancel
          </Button>
          <Button
            color="indigo"
            onClick={handleAccept}
            disabled={!selectedSection || !selectedDepartment}
            className="w-32 rounded-md bg-indigo-500 py-2 text-white hover:bg-indigo-600"
          >
            Accept
          </Button>
        </DialogFooter>
      </Dialog>

      <ToastContainer />
    </>
  );
};

export default SectionSelectionModal;
