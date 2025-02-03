import { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  IconButton,
  Input,
} from "@material-tailwind/react";
import { ArrowLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import { useNavigate } from "react-router-dom";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";

export function SectionsTable() {
  const { user } = useAuth();
  const [sectionList, setSectionList] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [searchTerm, setSearchTerm] = useState(""); // Para el cuadro de búsqueda
  const [searchingByName, setSearchingByName] = useState(false); // Nuevo estado para verificar si estamos buscando por ID
  const navigate = useNavigate();
  const pageSize = 6;
  const role = user.role.toLowerCase();

  useEffect(() => {
    if (role === "director") {
      fetchSectionsForDirector(currentPage);
    } else if (role === "sectionmanager") {
      fetchSectionsForManager(currentPage);
    }
  }, [role, currentPage]);

  const fetchSectionsForDirector = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/Section/GetPaged?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        { method: "GET" },
      );
      if (!response.ok) {
        if (response.status === 500) {
          throw new Error("There is no sections yet.");
        }
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
      setSectionList(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (error) {
      setError(error.message || "Failed to fetch sections");
    } finally {
      setLoading(false);
    }
  };

  const fetchSectionsForManager = async (pageNumber) => {
    setLoading(true);
    setError(null);
    try {
      // http://localhost:5217/api/Section/GetSectionsByManager?PageNumber=1&PageSize=7&sectionManagerId=2
      const response = await api(
        `/Section/GetSectionsByManager?PageNumber=${pageNumber}&PageSize=${pageSize}&sectionManagerId=${user.id}`,
        { method: "GET" },
      );
      if (!response.ok) {
        if (response.status === 500) {
          throw new Error("There is no sections yet.");
        }
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
      setSectionList(data.items);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (error) {
      setError(error.message || "Failed to fetch sections");
    } finally {
      setLoading(false);
    }
  };

  const searchSectionByName = async (name) => {
    setLoading(true);
    setError(null);
    try {
      const response = await api(
        `/Section/GetBySectionName?sectionName=${name}`,
        { method: "GET" },
      );
      if (!response.ok) {
        if (response.status === 500) {
          setSectionList([]);
          throw new Error("There is no section with that name.");
        }
        if (response.status === 404) {
          throw new Error("There is no section with that name.");
        }
        throw new Error("Network response was not ok");
      }
      const section = await response.json();
      setSectionList([section]);
      setSearchingByName(true); // Establecemos que estamos buscando por nombre
    } catch (error) {
      setError(error.message || "Failed to fetch sections");
    } finally {
      setLoading(false);
    }
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && searchTerm.trim()) {
      e.preventDefault(); // Prevenimos el comportamiento por defecto (como submit de un formulario)
      searchSectionByName(searchTerm);
      setSearchingByName(true);
    }
  };

  const handleBackToAllSections = () => {
    setSearchingByName(false);
    setSearchTerm(""); // Opcionalmente, puedes limpiar el término de búsqueda.

    if (role == "director") {
      fetchSectionsForDirector(1); // Vuelve a cargar todas las secciones
    } else {
      fetchSectionsForManager(1);
    }
  };

  return (
    <div className="mb-8 mt-12 flex flex-col gap-8">
      <Card>
        <CardHeader
          variant="gradient"
          color="gray"
          className="mb-8 flex items-center justify-between p-6"
        >
          {/* Aquí colocamos el botón de retroceso si estamos buscando por nombre */}
          {searchingByName && (
            <IconButton
              variant="text"
              size="sm"
              color="white"
              onClick={handleBackToAllSections}
              className="mr-4"
            >
              <ArrowLeftIcon className="h-5 w-5" />
            </IconButton>
          )}
          <Typography variant="h6" color="white">
            {role === "director"
              ? "All Sections and Managers"
              : "Your Sections"}
          </Typography>
        </CardHeader>
        <CardBody className="px-4 py-2">
          {role === "director" && (
            <div className="mb-4">
              <Input
                label="Search Section by Name"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                onKeyDown={handleKeyDown} // Solo se ejecuta en Enter
                className="w-full"
              />
            </div>
          )}

          {loading && (
            <Typography variant="small" color="gray">
              Loading sections...
            </Typography>
          )}
          {error && (
            <Typography variant="small" color="red">
              {error}
            </Typography>
          )}

          <div className="space-y-4">
            {sectionList.map((section) => (
              <div
                key={section.id}
                className="flex items-center justify-between rounded-lg border border-gray-200 p-4 shadow-sm transition duration-300 hover:bg-gray-200 hover:shadow-md"
              >
                <div>
                  <Typography variant="h6" color="blue-gray">
                    Section: {section.name}
                  </Typography>
                  <Typography variant="small" color="gray">
                    Manager: {section.sectionManagerUserName}
                  </Typography>
                </div>
                <div className="flex gap-2">
                  <Button
                    size="sm"
                    color="gray"
                    className="bg-gray-700 hover:bg-gray-800"
                    onClick={() =>
                      navigate("inventory/", {
                        state: { sectionId: section.id, departmentId: null },
                      })
                    }
                  >
                    View Inventory
                  </Button>
                  <Button
                    size="sm"
                    color="gray"
                    className="bg-gray-700 hover:bg-gray-800"
                    onClick={() =>
                      navigate("departments/", {
                        state: {
                          sectionId: section.id,
                          sectionName: section.name,
                        },
                      })
                    }
                  >
                    View Departments
                  </Button>
                </div>
              </div>
            ))}
          </div>

          {/* Paginación */}
          {!loading && !error && !searchingByName && totalPages > 1 && (
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
    </div>
  );
}

SectionsTable.displayName = "/src/pages/dashboard_director/sections_table.jsx";
export default SectionsTable;
