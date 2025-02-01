import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  IconButton,
} from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api";

const EvaluationsTable = () => {
  const [evaluationsList, setEvaluationsList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 14;

  useEffect(() => {
    fetchEvaluations(currentPage);
  }, [currentPage]);

  const fetchEvaluations = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      // http://localhost:5217/api/Evaluation/GetPaged?PageNumber=1&PageSize=5
      const response = await api(
        `/Evaluation/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        {
          method: "GET",
        },
      );

      if (!response.ok) {
        throw new Error("Failed to fetch evaluations");
      }

      const data = await response.json();
      setEvaluationsList(data.items || []);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (err) {
      setError("Failed to load evaluations data");
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= totalPages) {
      setCurrentPage(pageNumber);
    }
  };

  const renderPaginationButtons = () => {
    const visibleButtons = 5;
    let startPage = Math.max(1, currentPage - Math.floor(visibleButtons / 2));
    let endPage = Math.min(totalPages, startPage + visibleButtons - 1);

    if (endPage - startPage + 1 < visibleButtons) {
      startPage = Math.max(1, endPage - visibleButtons + 1);
    }

    return Array.from(
      { length: endPage - startPage + 1 },
      (_, i) => startPage + i,
    ).map((page) => (
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

  return (
    <Card className="mt-8 rounded-lg shadow-lg">
      <CardHeader
        variant="gradient"
        color="gray"
        className="flex items-center justify-between p-6"
      >
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Evaluation Records
        </Typography>
      </CardHeader>
      <CardBody className="px-0 py-4">
        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">
            {error}
          </Typography>
        ) : (
          <>
            {/* Tabla de Evaluaciones */}
            <div className="overflow-x-auto">
              <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                <thead className="bg-gray-800 text-white">
                  <tr>
                    <th className="border-b px-6 py-3 text-center">
                      Technician
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Evaluator
                    </th>
                    <th className="border-b px-6 py-3 text-center">
                      Evaluation
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white">
                  {evaluationsList.length > 0 ? (
                    evaluationsList.map((evaluation) => (
                      <tr key={evaluation.id}>
                        <td className="border-b px-6 py-3 text-center">
                          {evaluation.technicianUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {evaluation.sectionManagerUserName}
                        </td>
                        <td className="border-b px-6 py-3 text-center">
                          {evaluation.description}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="4" className="px-6 py-3 text-center">
                        No evaluations found
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}

        {/* Paginación */}
        <div className="mt-4 flex justify-center space-x-2">
          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className="px-4 py-2"
          >
            <ChevronLeftIcon className="h-5 w-5" />
          </IconButton>

          {renderPaginationButtons()}

          <IconButton
            variant="outlined"
            color="gray"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className="px-4 py-2"
          >
            <ChevronRightIcon className="h-5 w-5" />
          </IconButton>
        </div>
      </CardBody>
    </Card>
  );
};

export default EvaluationsTable;
