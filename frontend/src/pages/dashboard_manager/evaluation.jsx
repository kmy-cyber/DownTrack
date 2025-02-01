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
  IconButton,
} from "@material-tailwind/react";
import { ArrowLeftIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";

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
      const response = await api(
        `/Technician/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
      );
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
      const response = await api(
        `/Employee/GetByUsername?employeeUsername=${userName}`,
      );
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
        description: evaluation, // Usa el valor de evaluación seleccionado
      };

      try {
        const response = await api("/Evaluation/POST", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(evaluationData),
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
    <div className="mb-8 mt-12 flex flex-col gap-8">
      <Card className="shadow-xl">
        <CardHeader
          variant="gradient"
          color="gray"
          className="flex items-center justify-between p-6"
        >
          {isSearching && (
            <IconButton
              variant="text"
              size="sm"
              color="white"
              onClick={resetSearch}
              className="mr-4"
            >
              <ArrowLeftIcon className="h-5 w-5" />
            </IconButton>
          )}
          <div className="flex items-center justify-between">
            <Typography variant="h5" color="white" className="font-semibold">
              Evaluate Technicians
            </Typography>
          </div>
        </CardHeader>
        <CardBody>
          <div className="mb-4">
            <Input
              label="Search by Username"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onKeyDown={handleKeyDown}
              className="w-full"
            />
          </div>
          <table className="w-full min-w-[640px] table-auto">
            <thead className="bg-gray-800 text-sm text-white">
              <tr>
                <th className="border-b px-6 py-3 text-center">Technician</th>
                <th className="border-b px-6 py-3 text-center">Specialty</th>
                <th className="border-b px-6 py-3 text-center">
                  Years of Experience
                </th>
              </tr>
            </thead>
            <tbody>
              {technicians.map((technician) => (
                <tr
                  key={technician.id}
                  onClick={() => openEvaluationModal(technician)}
                  className="cursor-pointer hover:bg-gray-100"
                >
                  <td className="border-b px-6 py-3 text-center">
                    {technician.userName}
                  </td>
                  <td className="border-b px-6 py-3 text-center">
                    {technician.specialty}
                  </td>
                  <td className="border-b px-6 py-3 text-center">
                    {technician.expYears}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {technicians.length === 0 && (
            <Typography className="mt-4 text-center text-sm font-medium text-blue-gray-600">
              No technicians found.
            </Typography>
          )}
        </CardBody>
      </Card>

      {/* Paginación */}
      {!isSearching && totalPages > 1 && (
        <div className="mt-4 flex justify-center">
          <Stack spacing={2}>
            <Pagination
              count={totalPages}
              page={currentPage}
              onChange={(_, value) => setCurrentPage(value)}
            />
          </Stack>
        </div>
      )}

      {/* Modal for Evaluation */}
      <Dialog
        open={openModal}
        handler={() => setOpenModal(false)}
        size="sm"
        className="scale-95 transform rounded-xl bg-white p-6 shadow-lg transition-transform duration-300 ease-out"
      >
        <DialogHeader className="border-b-2 border-gray-300 pb-4 text-lg font-semibold text-gray-800">
          Evaluate Technician
        </DialogHeader>
        <DialogBody>
          <div className="mb-6 text-center">
            <Typography variant="h6" className="font-medium text-gray-700">
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
                className={`flex h-8 w-8 transform cursor-pointer items-center justify-center rounded-full border-2 transition-all duration-300 ${
                  evaluation === "Good"
                    ? "scale-110 border-green-700 bg-green-500"
                    : "border-gray-500 bg-white"
                }`}
              >
                <span
                  className={`h-5 w-5 rounded-full ${
                    evaluation === "Good" ? "bg-white" : "bg-transparent"
                  }`}
                ></span>
              </label>
              <Typography
                variant="small"
                className="mt-2 text-center text-gray-600"
              >
                Good
              </Typography>
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
                className={`flex h-8 w-8 transform cursor-pointer items-center justify-center rounded-full border-2 transition-all duration-300 ${
                  evaluation === "Regular"
                    ? "scale-110 border-yellow-700 bg-yellow-500"
                    : "border-gray-500 bg-white"
                }`}
              >
                <span
                  className={`h-5 w-5 rounded-full ${
                    evaluation === "Regular" ? "bg-white" : "bg-transparent"
                  }`}
                ></span>
              </label>
              <Typography
                variant="small"
                className="mt-2 text-center text-gray-600"
              >
                Regular
              </Typography>
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
                className={`flex h-8 w-8 transform cursor-pointer items-center justify-center rounded-full border-2 transition-all duration-300 ${
                  evaluation === "Bad"
                    ? "scale-110 border-red-700 bg-red-500"
                    : "border-gray-500 bg-white"
                }`}
              >
                <span
                  className={`h-5 w-5 rounded-full ${
                    evaluation === "Bad" ? "bg-white" : "bg-transparent"
                  }`}
                ></span>
              </label>
              <Typography
                variant="small"
                className="mt-2 text-center text-gray-600"
              >
                Bad
              </Typography>
            </div>
          </div>
        </DialogBody>

        <DialogFooter className="flex justify-between">
          <Button
            color="indigo"
            onClick={saveEvaluation}
            className="px-20 hover:bg-indigo-600"
          >
            Accept
          </Button>
          <Button
            color="red"
            onClick={() => setOpenModal(false)}
            className="px-20 hover:bg-red-600"
          >
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
