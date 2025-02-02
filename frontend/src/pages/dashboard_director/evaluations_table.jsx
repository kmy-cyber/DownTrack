import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  IconButton,
  Input,
} from "@material-tailwind/react";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import api from "@/middlewares/api";

const EvaluationsTable = () => {
  const [evaluationsList, setEvaluationsList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [isSearching, setIsSearching] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
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

  const searchUserName = async (userName) => {
    if (!userName.trim()) return;
    try {
      // Fix this
      const response = await api(
        `/Technician/Search_By_UserName?username=${userName}`,
      );
      if (response.ok) {
        const usr = await response.json();
        setEvaluationsList([usr]);
        setIsSearching(true);
      }
    } catch (error) {
      console.error("Error fetching technician by username:", error);
    }
  };

  const resetSearch = () => {
    setSearchTerm("");
    setIsSearching(false);
    fetchEvaluations(1);
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      e.preventDefault();
      searchUserName(searchTerm);
    }
  };

  return (
    <Card className="mt-8 rounded-lg shadow-lg">
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
        <Typography
          variant="h6"
          color="white"
          className="text-xl font-semibold"
        >
          Evaluation Records
        </Typography>
      </CardHeader>
      <CardBody className="py-4">
        {loading ? (
          <Typography className="text-center">Loading...</Typography>
        ) : error ? (
          <Typography color="red" className="text-center">
            {error}
          </Typography>
        ) : (
          <>
            <div className="mb-4">
              <Input
                label="Search by Username"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                onKeyDown={handleKeyDown}
                className="w-full"
              />
            </div>
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
