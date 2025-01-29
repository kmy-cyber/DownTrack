import React, { useState, useEffect } from "react";
import { Dialog, DialogHeader, DialogBody, DialogFooter, Button, Select, Option } from "@material-tailwind/react";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";

const SectionSelectionModal = ({ isOpen, onClose, onSave, eqiD }) => {
  const [sections, setSections] = useState([]); // Lista de secciones
  const [departments, setDepartments] = useState([]); // Lista de departamentos
  const [selectedSection, setSelectedSection] = useState(null); // Sección seleccionada
  const [selectedDepartment, setSelectedDepartment] = useState(null); // Departamento seleccionado
  const [showStatusPopup, setShowStatusPopup] = useState(false); // Para mostrar el pop-up de estado
  const [statusMessage, setStatusMessage] = useState(""); // Mensaje para el pop-up
  const { user } = useAuth();
  console.log(eqiD);

  // Función para cargar las secciones
  useEffect(() => {
    const fetchSections = async () => {
      try {
        const response = await api('/Section/GET_ALL', { method: 'GET' });
        if (response.ok) {
          const data = await response.json();
          setSections(data); // Lista de secciones
        }
      } catch (err) {
        console.error('Error fetching sections', err);
      }
    };
    fetchSections();
  }, []);

  // Función para cargar los departamentos de la sección seleccionada
  useEffect(() => {
    if (selectedSection) {
      const fetchDepartments = async () => {
        try {
          const response = await api(`/Department/GetAllDepartment_In_Section?sectionId=${selectedSection.id}`, { method: 'GET' });
          if (response.ok) {
            const data = await response.json();
            setDepartments(data); // Lista de departamentos para la sección seleccionada
            setSelectedDepartment(null); // Resetear el departamento seleccionado
          }
        } catch (err) {
          console.error('Error fetching departments', err);
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

        const response = await api('/TransferRequest/POST', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(requestBody),
        });

        if (response.ok) {
          const data = await response.json();
          if (typeof onSave === 'function') {
            onSave({ section: selectedSection, department: selectedDepartment });
          } else {
            console.error('onSave is not a function');
          }
          setStatusMessage("Transfer successful!");  // Mensaje de éxito
          setShowStatusPopup(true);  // Mostrar el pop-up de éxito
          onClose(); // Cerrar el modal después de guardar
        } else {
          console.error('Failed to save data');
          setStatusMessage("Transfer failed. Please try again.");  // Mensaje de error
          setShowStatusPopup(true);  // Mostrar el pop-up de error
        }
      } catch (err) {
        console.error('Error saving data', err);
        setStatusMessage("An error occurred. Please try again.");  // Mensaje de error
        setShowStatusPopup(true);  // Mostrar el pop-up de error
      }
    } else {
      console.error("You must select both a section and a department.");
    }
  };

  return (
    <>
      <Dialog open={isOpen} handler={onClose} size="sm">
        <DialogHeader className="text-xl font-semibold text-gray-800">Select Section and Department</DialogHeader>
        <DialogBody className="p-5 space-y-4">
          {/* Select de Sección */}
          <Select
            label="Section"
            value={selectedSection ? selectedSection.id : ""}
            onChange={(value) => {
              const selected = sections.find(section => section.id === value); // Encuentra la sección completa
              setSelectedSection(selected);                                   // Establece la sección seleccionada completa
            }}  
          >
            {sections.map((section) => (
              <Option key={section.id} value={section.id}>{section.name}</Option>
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
            className="w-32 py-2 text-gray-700 border-gray-300 hover:border-gray-500 rounded-md"
          >
            Cancel
          </Button>
          <Button 
            color="indigo" 
            onClick={handleAccept} 
            disabled={!selectedSection || !selectedDepartment} 
            className="w-32 py-2 text-white bg-indigo-500 hover:bg-indigo-600 rounded-md"
          >
            Accept
          </Button>
        </DialogFooter>
      </Dialog>

      {/* Pop-up de estado */}
      <Dialog open={showStatusPopup} handler={() => setShowStatusPopup(false)} size="sm">
        <DialogHeader className="text-xl font-semibold text-gray-800">Transfer Status</DialogHeader>
        <DialogBody className="p-5">
          <p className="text-center">{statusMessage}</p>
        </DialogBody>
        <DialogFooter className="space-x-3">
          <Button 
            color="indigo" 
            onClick={() => setShowStatusPopup(false)} 
            className="w-32 py-2 text-white bg-indigo-500 hover:bg-indigo-600 rounded-md"
          >
            Close
          </Button>
        </DialogFooter>
      </Dialog>
    </>
  );
};

export default SectionSelectionModal;
