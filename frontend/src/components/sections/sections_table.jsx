import { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton, Input } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import { useNavigate } from "react-router-dom";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";

export function SectionsTable() {
    const { user } = useAuth();
    const [sectionList, setSectionList] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [searchTerm, setSearchTerm] = useState(""); // Para el cuadro de búsqueda
    const navigate = useNavigate();
    const pageSize = 7;

    useEffect(() => {
        if (user.role?.toLowerCase() === "director") {
            fetchSectionsForDirector(currentPage);
        } else if (user.role?.toLowerCase() === "sectionmanager") {
            fetchSectionsForManager(currentPage);
        }
    }, [user.role, currentPage]);

    const fetchSectionsForDirector = async (pageNumber) => {
        setLoading(true);
        setError(null);
        try {
            const response = await api(`/Section/GetPaged?pageNumber=${pageNumber}&pageSize=${pageSize}`, { method: "GET" });
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setSectionList(data.items);
            setTotalPages(Math.ceil(data.totalCount / pageSize));
        } catch (error) {
            setError("Failed to fetch sections");
        } finally {
            setLoading(false);
        }
    };

    const fetchSectionsForManager = async (pageNumber) => {
        setLoading(true);
        setError(null);
        try {
            const response = await api(`/Section/GetSectionsByManager?PageNumber=${pageNumber}&PageSize=${pageSize}&sectionManagerId=${user.id}`, {
                method: "GET",
            });
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setSectionList(data.items);
            setTotalPages(Math.ceil(data.totalCount / pageSize));
        } catch (error) {
            setError("Failed to fetch sections");
        } finally {
            setLoading(false);
        }
    };

    const searchSectionsByName = async (name) => {
        setLoading(true);
        setError(null);
        try {
            const response = await api(`/Section/SearchByName?name=${name}`, { method: "GET" });
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setSectionList(data.items);
            setTotalPages(Math.ceil(data.totalCount / pageSize));
        } catch (error) {
            setError("Failed to fetch sections");
        } finally {
            setLoading(false);
        }
    };

    const handleSearchKeyDown = (e) => {
        if (e.key === "Enter") {
            e.preventDefault(); // Prevenimos el comportamiento por defecto (como submit de un formulario)
            if (searchTerm.trim()) { // Solo hacemos la búsqueda si hay algo que buscar
                searchSectionsByName(searchTerm);
            }
        }
    };

    return (
        <div className="mt-12 mb-8 flex flex-col gap-8">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        {user.role?.toLowerCase() === "director" ? "All Sections and Managers" : "Your Sections"}
                    </Typography>
                </CardHeader>
                <CardBody className="px-4 py-2">
                    {user.role?.toLowerCase() === "director" && (
                        <div className="mb-4">
                            <Input
                                label="Search Section by Name"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                                onKeyDown={handleSearchKeyDown} // Solo se ejecuta en Enter
                                className="w-full"
                            />
                        </div>
                    )}

                    {loading && <Typography variant="small" color="gray">Loading sections...</Typography>}
                    {error && <Typography variant="small" color="red">{error}</Typography>}

                    <div className="space-y-4">
                        {sectionList.map((section) => (
                            <div
                                key={section.id}
                                className="flex items-center justify-between border border-gray-200 rounded-lg shadow-sm p-4 hover:bg-gray-200 hover:shadow-md transition duration-300"
                            >
                                <div>
                                    <Typography variant="h6" color="blue-gray">Section: {section.name}</Typography>
                                    <Typography variant="small" color="gray">Manager: {section.sectionManagerUserName}</Typography>
                                </div>
                                <div className="flex gap-2">
                                    <Button
                                        size="sm"
                                        color="gray"
                                        className="bg-gray-700 hover:bg-gray-800"
                                        onClick={() => navigate("inventory/", { state: { sectionId: section.id, departmentId: null } })}
                                    >
                                        View Inventory
                                    </Button>
                                    <Button
                                        size="sm"
                                        color="gray"
                                        className="bg-gray-700 hover:bg-gray-800"
                                        onClick={() => navigate("departments/", { state: { sectionId: section.id, sectionName: section.name } })}
                                    >
                                        View Departments
                                    </Button>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="flex justify-center mt-4 gap-2">
                        {currentPage > 1 && (
                            <IconButton
                                variant="outlined"
                                size="sm"
                                color="gray"
                                onClick={() => setCurrentPage(currentPage - 1)}
                                className="px-4 py-2"
                            >
                                <ChevronLeftIcon className="h-5 w-5" />
                            </IconButton>
                        )}

                        {Array.from({ length: totalPages }, (_, i) => (
                            <Button
                                key={i}
                                variant={currentPage === i + 1 ? "filled" : "outlined"}
                                color="gray"
                                onClick={() => setCurrentPage(i + 1)}
                                className="px-4 py-2"
                            >
                                {i + 1}
                            </Button>
                        ))}

                        {currentPage < totalPages && (
                            <IconButton
                                variant="outlined"
                                size="sm"
                                color="gray"
                                onClick={() => setCurrentPage(currentPage + 1)}
                                className="px-4 py-2"
                            >
                                <ChevronRightIcon className="h-5 w-5" />
                            </IconButton>
                        )}
                    </div>
                </CardBody>
            </Card>
        </div>
    );
}

SectionsTable.displayName = "/src/pages/dashboard_director/sections_table.jsx";
export default SectionsTable;
