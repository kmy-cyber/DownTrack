import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Input,
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
  Button,
  IconButton
} from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export function Evaluation() {
  const { user } = useAuth();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedTechnician, setSelectedTechnician] = useState(null);
  const [evaluation, setEvaluation] = useState("Good"); // Stores the selected rating (Good, Regular, or Bad)
  const [technicians, setTechnicians] = useState([]);
  const [totalPages, setTotalPages] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [isSearching, setIsSearching] = useState(false);
  const [openModal, setOpenModal] = useState(false);
  const [isEvaluationSuccess, setIsEvaluationSuccess] = useState(null);
  const [evaluationMessage, setEvaluationMessage] = useState("");
  const pageSize = 10;

  useEffect(() => {
    if (!isSearching) {
      fetchTechnicians(currentPage);
    }
  }, [currentPage, isSearching]);

  const fetchTechnicians = async (pageNumber) => {
    try {
      const response = await api(`/Technician/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`);
      if (response.ok) {
        const data = await response.json();
        setTechnicians(data.items);
        setTotalPages(Math.ceil(data.totalCount / pageSize));
      }
    } catch (error) {
      console.error("Error fetching technicians:", error);
    }
  };

  const searchUserName = async (userName) => {
    if (!userName.trim()) return;
    try {
      const response = await api(`/Employee/GetByUsername?employeeUsername=${userName}`);
      if (response.ok) {
        const usr = await response.json();
        setTechnicians([usr]);
        setIsSearching(true);
      }
    } catch (error) {
      console.error("Error fetching technician by username:", error);
    }
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      e.preventDefault();
      searchUserName(searchTerm);
    }
  };

  const resetSearch = () => {
    setSearchTerm("");
    setIsSearching(false);
    fetchTechnicians(currentPage);
  };

  const openEvaluationModal = (technician) => {
    setSelectedTechnician(technician);
    setOpenModal(true);
  };

  const handleEvaluationChange = (value) => {
    setEvaluation(value); // Sets the selected evaluation (Good, Regular, Bad)
  };

  const saveEvaluation = async () => {
    if (selectedTechnician) {
      const evaluationData = {
        technicianId: selectedTechnician.id,
        sectionManagerId: user.id,
        description: evaluation // Usa el valor de evaluación seleccionado
      };

      try {
        const response = await api("/Evaluation/POST", {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(evaluationData)
        });

        if (response.ok) {
          toast.success("Evaluation saved successfully!", {
            position: "top-center",
            autoClose: 3000, // Toast se cierra después de 3 segundos
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: false,
            theme: "colored",
          });
          setIsEvaluationSuccess(true);
          setEvaluationMessage("Evaluation successfully!");
          setOpenModal(false);
        } else {
          toast.error("Failed to save evaluation", {
            position: "top-center",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: false,
            theme: "colored",
          });
          setIsEvaluationSuccess(false);
          setEvaluationMessage("Failed to save evaluation");
        }
      } catch (error) {
        console.error("Error saving evaluation:", error);
        toast.error("Error saving evaluation", {
          position: "top-center",
          autoClose: 3000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: false,
          draggable: false,
          theme: "colored",
        });
        setIsEvaluationSuccess(false);
        setEvaluationMessage("Error saving evaluation");
      }
    }
  };

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card className="shadow-xl">
        <CardHeader variant="gradient" color="gray" className="p-6">
          <div className="flex justify-between items-center">
            <Typography variant="h5" color="white" className="font-semibold">
              Evaluate Technicians
            </Typography>
            <div className="flex gap-2">
              <Input
                type="text"
                color="white"
                label="Search by Username"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                onKeyDown={handleKeyDown}
                className="text-white text-sm"
              />
              {isSearching && (
                <Button color="red" onClick={resetSearch} className="text-sm">
                  Reset
                </Button>
              )}
            </div>
          </div>
        </CardHeader>
        <CardBody className="overflow-x-scroll p-0 mt-5">
          <table className="w-full min-w-[640px] table-auto">
            <thead className="bg-gray-800 text-sm text-white">
              <tr>
                <th className="px-6 py-3 border-b text-center">Technician</th>
                <th className="px-6 py-3 border-b text-center">Specialty</th>
                <th className="px-6 py-3 border-b text-center">Years of Experience</th>
              </tr>
            </thead>
            <tbody>
              {technicians.map((technician) => (
                <tr
                  key={technician.id}
                  onClick={() => openEvaluationModal(technician)}
                  className="hover:bg-gray-100 cursor-pointer"
                >
                  <td className="px-6 py-3 border-b text-center">{technician.userName}</td>
                  <td className="px-6 py-3 border-b text-center">{technician.specialty}</td>
                  <td className="px-6 py-3 border-b text-center">{technician.expYears}</td>
                </tr>
              ))}
            </tbody>
          </table>
          {technicians.length === 0 && (
            <Typography className="text-center text-sm font-medium text-blue-gray-600 mt-4">
              No technicians found.
            </Typography>
          )}
        </CardBody>
      </Card>

      {!isSearching && (
        <div className="flex justify-center mt-4 space-x-2">
          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => setCurrentPage(currentPage - 1)}
            disabled={currentPage === 1}
          >
            <ChevronLeftIcon className="h-5 w-5" />
          </IconButton>
          <Typography variant="small" className="self-center">
            Page {currentPage} of {totalPages}
          </Typography>
          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => setCurrentPage(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            <ChevronRightIcon className="h-5 w-5" />
          </IconButton>
        </div>
      )}

      {/* Modal for Evaluation */}
      <Dialog
        open={openModal}
        handler={() => setOpenModal(false)}
        size="sm"
        className="p-6 shadow-lg rounded-xl bg-white transition-transform transform scale-95 duration-300 ease-out"
      >
        <DialogHeader className="text-lg font-semibold text-gray-800 border-b-2 border-gray-300 pb-4">
          Evaluate Technician
        </DialogHeader>
        <DialogBody>
          <div className="text-center mb-6">
            <Typography variant="h6" className="text-gray-700 font-medium">
              {selectedTechnician?.userName}
            </Typography>
          </div>

          {/* Rating Buttons - Radio Buttons styled as circles */}
          <div className="flex justify-center gap-12">
            <div className="flex flex-col items-center">
              <input
                type="radio"
                id="good"
                name="evaluation"
                value="Good"
                checked={evaluation === "Good"}
                onChange={() => handleEvaluationChange("Good")}
                className="hidden"
              />
              <label
                htmlFor="good"
                className={`w-8 h-8 rounded-full border-2 flex items-center justify-center cursor-pointer transform transition-all duration-300 ${evaluation === "Good" ? "bg-green-500 border-green-700 scale-110" : "bg-white border-gray-500"}`}
              >
                <span className={`w-5 h-5 rounded-full ${evaluation === "Good" ? "bg-white" : "bg-transparent"}`}></span>
              </label>
              <Typography variant="small" className="text-center mt-2 text-gray-600">Good</Typography>
            </div>

            <div className="flex flex-col items-center">
              <input
                type="radio"
                id="regular"
                name="evaluation"
                value="Regular"
                checked={evaluation === "Regular"}
                onChange={() => handleEvaluationChange("Regular")}
                className="hidden"
              />
              <label
                htmlFor="regular"
                className={`w-8 h-8 rounded-full border-2 flex items-center justify-center cursor-pointer transform transition-all duration-300 ${evaluation === "Regular" ? "bg-yellow-500 border-yellow-700 scale-110" : "bg-white border-gray-500"}`}
              >
                <span className={`w-5 h-5 rounded-full ${evaluation === "Regular" ? "bg-white" : "bg-transparent"}`}></span>
              </label>
              <Typography variant="small" className="text-center mt-2 text-gray-600">Regular</Typography>
            </div>

            <div className="flex flex-col items-center">
              <input
                type="radio"
                id="bad"
                name="evaluation"
                value="Bad"
                checked={evaluation === "Bad"}
                onChange={() => handleEvaluationChange("Bad")}
                className="hidden"
              />
              <label
                htmlFor="bad"
                className={`w-8 h-8 rounded-full border-2 flex items-center justify-center cursor-pointer transform transition-all duration-300 ${evaluation === "Bad" ? "bg-red-500 border-red-700 scale-110" : "bg-white border-gray-500"}`}
              >
                <span className={`w-5 h-5 rounded-full ${evaluation === "Bad" ? "bg-white" : "bg-transparent"}`}></span>
              </label>
              <Typography variant="small" className="text-center mt-2 text-gray-600">Bad</Typography>
            </div>
          </div>
        </DialogBody>

        <DialogFooter className="flex justify-between">
          <Button color="indigo" onClick={saveEvaluation} className="hover:bg-indigo-600 px-20">
            Accept
          </Button>
          <Button color="red" onClick={() => setOpenModal(false)} className="hover:bg-red-600 px-20">
            Cancel
          </Button>
        </DialogFooter>
      </Dialog>

      {/* Toast Container */}
      <ToastContainer />
    </div>
  );
}

export default Evaluation;
