import React, { useState } from "react";
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
  Radio,
} from "@material-tailwind/react";
import { userListData } from "@/data/users-table-data";

export function Evaluation() {
  const [searchTerm, setSearchTerm] = useState(""); // State to store search term
  const [selectedTechnician, setSelectedTechnician] = useState(null); // State to store the selected technician
  const [evaluation, setEvaluation] = useState(""); // State to store the selected evaluation
  const [evaluations, setEvaluations] = useState({}); // State to store evaluations and their timestamps

  // Filter users with the role "technician"
  const technicians = userListData.filter((user) => user.role === "technician");

  // Filter technicians by the search term
  const filteredTechnicians = technicians.filter((technician) =>
    technician.username.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Handle the evaluation submission
  const handleEvaluation = () => {
    if (selectedTechnician && evaluation) {
      const now = new Date().toISOString(); // Get the current timestamp
      setEvaluations((prev) => ({
        ...prev,
        [selectedTechnician.username]: { evaluation, date: now },
      }));
      setSelectedTechnician(null); // Close the modal
      setEvaluation(""); // Reset evaluation state
    }
  };

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
          <div className="flex justify-between items-center">
            <Typography variant="h6" color="white">
              Evaluate Technicians
            </Typography>
            {/* Search bar for filtering by username */}
            <div className="w-72">
              <Input
                type="text"
                color="white"
                label="Search by Username"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="text-white" // Ensuring text is white
              />
            </div>
          </div>
        </CardHeader>
        <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
          {/* Table displaying technicians */}
          <table className="w-full min-w-[640px] table-auto">
            <thead>
              <tr>
                {["Technician", "Username", "Specialty", "Experience"].map(
                  (header) => (
                    <th
                      key={header}
                      className="border-b border-blue-gray-50 py-3 px-5 text-left"
                    >
                      <Typography
                        variant="small"
                        className="text-[11px] font-bold uppercase text-blue-gray-400"
                      >
                        {header}
                      </Typography>
                    </th>
                  )
                )}
              </tr>
            </thead>
            <tbody>
              {filteredTechnicians.map(
                ({ username, name, specialty, experience }, key) => {
                  const className = `py-3 px-5 ${
                    key === filteredTechnicians.length - 1
                      ? ""
                      : "border-b border-blue-gray-50"
                  }`;

                  return (
                    <tr
                      key={username}
                      onClick={() =>
                        setSelectedTechnician({ username, name, specialty })
                      }
                      className="cursor-pointer hover:bg-blue-gray-50"
                    >
                      <td className={className}>
                        <Typography
                          variant="small"
                          color="blue-gray"
                          className="font-semibold"
                        >
                          {name}
                        </Typography>
                      </td>
                      <td className={className}>
                        <Typography
                          variant="small"
                          color="blue-gray"
                          className="font-semibold"
                        >
                          {username}
                        </Typography>
                      </td>
                      <td className={className}>
                        <Typography className="text-xs font-medium text-blue-gray-600">
                          {specialty || "N/A"}
                        </Typography>
                      </td>
                      <td className={className}>
                        <Typography className="text-xs font-medium text-blue-gray-600">
                          {experience || "N/A"} years
                        </Typography>
                      </td>
                    </tr>
                  );
                }
              )}
            </tbody>
          </table>
          {filteredTechnicians.length === 0 && (
            <Typography
              className="text-center text-sm font-medium text-blue-gray-600 mt-4"
            >
              No technicians found matching "{searchTerm}".
            </Typography>
          )}
        </CardBody>
      </Card>

      {/* Modal for technician evaluation */}
      <Dialog
        open={!!selectedTechnician}
        handler={() => setSelectedTechnician(null)}
        className="max-w-sm" // Smaller modal width
      >
        <DialogHeader>
          <Typography variant="h6" className="text-center">
            Evaluate {selectedTechnician?.name}
          </Typography>
        </DialogHeader>
        <DialogBody
          divider
          className="flex flex-col gap-2 items-center text-center"
        >
          <Typography variant="small" className="text-blue-gray-600">
            Select an evaluation:
          </Typography>
          {/* Radio buttons for evaluation options (in a row) */}
          <div className="flex flex-row items-center gap-4">
            <Radio
              id="good"
              name="evaluation"
              label="Good"
              onChange={() => setEvaluation("Good")}
              checked={evaluation === "Good"}
            />
            <Radio
              id="regular"
              name="evaluation"
              label="Regular"
              onChange={() => setEvaluation("Regular")}
              checked={evaluation === "Regular"}
            />
            <Radio
              id="bad"
              name="evaluation"
              label="Bad"
              onChange={() => setEvaluation("Bad")}
              checked={evaluation === "Bad"}
            />
          </div>
        </DialogBody>
        <DialogFooter className="flex justify-end">
          {/* Cancel button */}
          <Button
            variant="text"
            color="red"
            onClick={() => setSelectedTechnician(null)}
            className="mr-2"
          >
            Cancel
          </Button>
          {/* Accept button */}
          <Button
            variant="gradient"
            color="green"
            onClick={handleEvaluation}
            disabled={!evaluation}
          >
            Accept
          </Button>
        </DialogFooter>
      </Dialog>
    </div>
  );
}

export default Evaluation;
