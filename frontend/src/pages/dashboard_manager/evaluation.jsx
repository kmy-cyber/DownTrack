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
  Radio,
} from "@material-tailwind/react";
import api from "@/middlewares/api"; // Suponiendo que tienes un middleware para las peticiones

export function Evaluation() {
  const [searchTerm, setSearchTerm] = useState(""); // Estado para el término de búsqueda
  const [selectedTechnician, setSelectedTechnician] = useState(null); // Estado para el técnico seleccionado
  const [evaluation, setEvaluation] = useState(""); // Estado para la evaluación seleccionada
  const [evaluations, setEvaluations] = useState({}); // Estado para las evaluaciones y sus timestamps
  const [technicians, setTechnicians] = useState([]); // Estado para los técnicos
  const [totalPages, setTotalPages] = useState(0); // Total de páginas
  const [currentPage, setCurrentPage] = useState(1); // Página actual
  const pageSize = 10; // Tamaño de la página

  // Función para obtener técnicos de la API
  const fetchTechnicians = async (pageNumber) => {
    try {
      const response = await api(`/Technician/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`
      );
      if (response.ok) {
        const data = await response.json();
        console.log(data)
        setTechnicians(data.items);
        setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcular las páginas totales
      } else {
        throw new Error("Failed to fetch technicians");
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchTechnicians(currentPage); // Llamar a la API cada vez que cambie la página
  }, [currentPage]);

  // Filtrar técnicos por el término de búsqueda
  const filteredTechnicians = technicians.filter((technician) =>
    technician.name.includes(searchTerm.toLowerCase())
  );

  // Función para manejar el cambio de página
  const handlePageChange = (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= totalPages) {
      setCurrentPage(pageNumber);
    }
  };

  // Función para renderizar los botones de paginación
  const renderPaginationButtons = () => {
    const visibleButtons = 5; // Número máximo de botones visibles
    let startPage = Math.max(1, currentPage - Math.floor(visibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + visibleButtons - 1);

    // Ajustar rango si estamos cerca del inicio o final
    if (endPage - startPage + 1 < visibleButtons) {
      startPage = Math.max(1, endPage - visibleButtons + 1);
    }

    return Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i).map((page) => (
      <Button
        key={page}
        variant={page === currentPage ? "filled" : "outlined"}
        color="gray"
        onClick={() => handlePageChange(page)}
        className="px-4 py-2"
      >
        {page}
      </Button>
    ));
  };

  // Manejar la evaluación
  const handleEvaluation = () => {
    if (selectedTechnician && evaluation) {
      const now = new Date().toISOString(); // Obtener el timestamp actual
      setEvaluations((prev) => ({
        ...prev,
        [selectedTechnician.username]: { evaluation, date: now },
      }));
      setSelectedTechnician(null); // Cerrar el modal
      setEvaluation(""); // Resetear estado de evaluación
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
            {/* Barra de búsqueda para filtrar por nombre de usuario */}
            <div className="w-72">
              <Input
                type="text"
                color="white"
                label="Search by Username"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="text-white" // Asegurando que el texto sea blanco
              />
            </div>
          </div>
        </CardHeader>
        <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
          {/* Tabla mostrando los técnicos */}
          <table className="w-full min-w-[640px] table-auto">
            <thead>
              <tr>
                {["Name", "Specialty", "Years of Experience"].map((header) => (
                  <th key={header} className="border-b border-blue-gray-50 py-3 px-5 text-left">
                    <Typography
                      variant="small"
                      className="text-[11px] font-bold uppercase text-blue-gray-400"
                    >
                      {header}
                    </Typography>
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {filteredTechnicians.map(({name,specialty,expYears}, key) => {
                const className = `py-3 px-5 ${key === filteredTechnicians.length - 1 ? "" : "border-b border-blue-gray-50"}`;

                return (
                  <tr
                    key={name}
                    onClick={() => setSelectedTechnician({ name, specialty, expYears })}
                    className="cursor-pointer hover:bg-blue-gray-50"
                  >
                    <td className={className}>
                      <Typography variant="small" color="blue-gray" className="font-semibold">
                        {name}
                      </Typography>
                    </td>
                    <td className={className}>
                      <Typography className="text-xs font-medium text-blue-gray-600">
                        {specialty || "N/A"}
                      </Typography>
                    </td>
                    <td className={className}>
                      <Typography className="text-xs font-medium text-blue-gray-600">
                        {expYears || "N/A"} years
                      </Typography>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
          {filteredTechnicians.length === 0 && (
            <Typography className="text-center text-sm font-medium text-blue-gray-600 mt-4">
              No technicians found matching "{searchTerm}".
            </Typography>
          )}
        </CardBody>
      </Card>

      {/* Modal para evaluar al técnico */}
      <Dialog open={!!selectedTechnician} handler={() => setSelectedTechnician(null)} className="max-w-sm">
        <DialogHeader>
          <Typography variant="h6" className="text-center">
            Evaluate {selectedTechnician?.name}
          </Typography>
        </DialogHeader>
        <DialogBody divider className="flex flex-col gap-2 items-center text-center">
          <Typography variant="small" className="text-blue-gray-600">
            Select an evaluation:
          </Typography>
          {/* Botones de radio para opciones de evaluación */}
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
          {/* Botón de cancelar */}
          <Button variant="text" color="red" onClick={() => setSelectedTechnician(null)} className="mr-2">
            Cancel
          </Button>
          {/* Botón de aceptar */}
          <Button variant="gradient" color="green" onClick={handleEvaluation} disabled={!evaluation}>
            Accept
          </Button>
        </DialogFooter>
      </Dialog>

      {/* Paginación */}
      <div className="flex justify-center mt-4 space-x-2">
        {/* Botón "Anterior" */}
        <Button
          variant="outlined"
          color="gray"
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
          className="px-4 py-2"
        >
          Prev
        </Button>

        {/* Botones dinámicos de paginación */}
        {renderPaginationButtons()}

        {/* Botón "Siguiente" */}
        <Button
          variant="outlined"
          color="gray"
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
          className="px-4 py-2"
        >
          Next
        </Button>
      </div>
    </div>
  );
}

export default Evaluation;
