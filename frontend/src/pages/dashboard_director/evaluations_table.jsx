import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
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
      const response = await api(
        `/Evaluation/GetPaged?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        { method: "GET" },
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
                      <td colSpan="3" className="px-6 py-3 text-center">
                        No evaluations found
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}

        {!loading && !error && totalPages > 1 && (
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
      </CardBody>
    </Card>
  );
};

export default EvaluationsTable;