import { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton } from "@material-tailwind/react";
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
            const response = await api(`/Section/sections/manager/${user.id}?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
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

    const handleViewDepartments = (sectionId, sectionName) => {
        console.log(`ID : ${sectionId}, NAME : ${sectionName}`);
        navigate("departments/", {
            state: { sectionId, sectionName }
        });
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

    return (
        <div className="mt-12 mb-8 flex flex-col gap-8">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        {user.role?.toLowerCase() === "director" ? "All Sections and Managers" : "Your Sections"}
                    </Typography>
                </CardHeader>
                <CardBody className="px-4 py-2">
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
                                    <Typography variant="small" color="gray">Manager: {section.sectionManagerId}</Typography>
                                </div>
                                <div className="flex gap-2">
                                    <Button
                                        size="sm"
                                        color="gray"
                                        className="bg-gray-700 hover:bg-gray-800"
                                        onClick={() => console.log(`View Inventory: ${section.id}`)}
                                    >
                                        View Inventory
                                    </Button>
                                    <Button
                                        size="sm"
                                        color="gray"
                                        className="bg-gray-700 hover:bg-gray-800"
                                        onClick={() => handleViewDepartments(section.id, section.name)}
                                    >
                                        View Departments
                                    </Button>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="flex justify-center mt-4 gap-2">
                        {/* Botón "Anterior" con icono */}
                        {currentPage > 1 && (
                            <IconButton
                                variant="outlined"
                                size="sm"
                                color="gray"
                                onClick={() => handlePageChange(currentPage - 1)}
                                className="px-4 py-2"
                            >
                                <ChevronLeftIcon className="h-5 w-5" />
                            </IconButton>
                        )}
                        
                        {/* Botones dinámicos de paginación */}
                        {renderPaginationButtons()}

                        {/* Botón "Siguiente" con icono */}
                        {currentPage < totalPages && (
                            <IconButton
                                variant="outlined"
                                size="sm"
                                color="gray"
                                onClick={() => handlePageChange(currentPage + 1)}
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
