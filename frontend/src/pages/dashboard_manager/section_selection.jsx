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
  const [sections, setSections] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [selectedSection, setSelectedSection] = useState(null);
  const [selectedDepartment, setSelectedDepartment] = useState(null);
  const { user } = useAuth();

  useEffect(() => {
    const fetchSections = async () => {
      try {
        const response = await api("/Section/GET_ALL", { method: "GET" });
        if (response.ok) {
          const data = await response.json();
          setSections(data);
        }
      } catch (err) {
        console.error("Error fetching sections", err);
      }
    };
    fetchSections();
  }, []);

  useEffect(() => {
    if (selectedSection) {
      const fetchDepartments = async () => {
        try {
          const response = await api(
            `/Department/GetAllDepartment_In_Section?sectionId=${selectedSection.id}`,
            { method: "GET" }
          );
          if (response.ok) {
            const data = await response.json();
            setDepartments(data);

            // Mantener el departamento seleccionado si sigue existiendo
            if (!data.some((d) => d.id === selectedDepartment?.id)) {
              setSelectedDepartment(null);
            }
          }
        } catch (err) {
          console.error("Error fetching departments", err);
        }
      };
      fetchDepartments();
    } else {
      setDepartments([]);
      setSelectedDepartment(null);
    }
  }, [selectedSection]);

  const handleAccept = async () => {
    if (selectedSection && selectedDepartment) {
      try {
        const requestBody = {
          date: new Date().toISOString().split("T")[0],
          status: "Pending",
          sectionManagerId: user.id,
          equipmentId: eqiD,
          arrivalDepartmentId: selectedDepartment,
          arrivalSectionId: selectedSection.id,
        };

        console.log(requestBody);

        const response = await api("/TransferRequest/POST", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(requestBody),
        });

        if (response.ok) {
          if (typeof onSave === "function") {
            onSave({
              section: selectedSection,
              department: selectedDepartment,
            });
          }
          toast.success("Transfer Successfully Requested!", { position: "top-center", autoClose: 3000 });
          onClose();
        } else {
          toast.error("Transfer Request Failed. Please try again.", { position: "top-center", autoClose: 3000 });
        }
      } catch (err) {
        console.error("Error saving data", err);
        toast.error("An error occurred. Please try again.", { position: "top-center", autoClose: 3000 });
      }
    } else {
      toast.warning("You must select both a section and a department.", { position: "top-center", autoClose: 3000 });
    }
  };

  return (
    <>
      <Dialog open={isOpen} handler={onClose} size="sm">
        <DialogHeader>Select Section and Department</DialogHeader>
        <DialogBody className="space-y-4 p-5">
          <Select
            label="Section"
            value={selectedSection?.id || ""}
            onChange={(value) => {
              const selected = sections.find((section) => section.id === value);
              setSelectedSection(selected);
            }}
          >
            {sections.map((section) => (
              <Option key={section.id} value={section.id}>
                {section.name}
              </Option>
            ))}
          </Select>

          <Select
            label="Department"
            value={selectedDepartment?.id || ""}
            onChange={(value) => {
              setSelectedDepartment(value);
            }}
            disabled={!selectedSection || departments.length === 0}
          >
            {departments.map((department) => (
              <Option key={department.id} value={department.id}>
                {department.name}
              </Option>
            ))}
          </Select>
        </DialogBody>
        <DialogFooter className="space-x-3">
          <Button variant="outlined" color="gray" onClick={onClose}>
            Cancel
          </Button>
          <Button color="indigo" onClick={handleAccept} disabled={!selectedSection || !selectedDepartment}>
            Accept
          </Button>
        </DialogFooter>
      </Dialog>

      <ToastContainer />
    </>
  );
};

export default SectionSelectionModal;