import { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, IconButton } from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";

export function SectionsTable() {
    const { user } = useAuth();
    const [sectionList, setSectionList] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const pageSize = 7;

    useEffect(() => {
        if (user.role?.toLowerCase() === "director") {
            fetchSectionsForDirector(currentPage);
        } else if (user.role?.toLowerCase() === "sectionmanager") {
            fetchSectionsForManager(currentPage);
        }
    }, [user.role, currentPage]);

    const fetchSectionsForDirector = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await api(`/Section/GET_ALL`, {
                method: "GET",
            });
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setSectionList(data);
            setTotalPages(Math.ceil(data.length / pageSize));
        } catch (error) {
            console.error("Error fetching sections for director:", error);
            setError("Failed to fetch sections");
            setSectionList([]);
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
            console.error("Error fetching sections for manager:", error);
            setError("Failed to fetch sections");
            setSectionList([]);
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

    return (
        <div className="mt-12 mb-8 flex flex-col gap-8">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        {user.role?.toLowerCase() === "director" ? "All Sections and Managers" : "Your Sections"}
                    </Typography>
                </CardHeader>
                <CardBody className="px-4 py-2">
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
                        {Array.isArray(sectionList) && sectionList.length > 0 ? (
                            sectionList.map((section) => (
                                <div
                                    key={section.id}
                                    className="border border-gray-200 rounded-lg shadow-sm p-4 hover:bg-gray-200 hover:shadow-md transition duration-300"
                                >
                                    <Typography variant="h6" color="blue-gray">
                                        Section: {section.name}
                                    </Typography>
                                    <Typography variant="small" color="gray">
                                        Manager ID: {section.sectionManagerId}
                                    </Typography>
                                </div>
                            ))
                        ) : (
                            !loading && (
                                <Typography variant="small" color="gray">
                                    No sections available.
                                </Typography>
                            )
                        )}
                    </div>
                    
                    {user.role.toLowerCase() == "sectionmanager" &&
                        <div className="flex justify-center mt-4 space-x-2">
                            {/* Botón "Anterior" */}
                            <IconButton
                                variant="outlined"
                                color="gray"
                                onClick={() => handlePageChange(currentPage - 1)}
                                disabled={currentPage === 1}
                            >
                                <ChevronLeftIcon className="w-4 h-4" />
                            </IconButton>

                            {/* Botones dinámicos */}
                            {renderPaginationButtons()}

                            {/* Botón "Siguiente" */}
                            <IconButton
                                variant="outlined"
                                color="gray"
                                onClick={() => handlePageChange(currentPage + 1)}
                                disabled={currentPage === totalPages}
                            >
                                <ChevronRightIcon className="w-4 h-4" />
                            </IconButton>
                        </div>
                    }
                </CardBody>
            </Card>
        </div>
    );
}

SectionsTable.displayName = "/src/pages/dashboard_director/sections_table.jsx";
export default SectionsTable;
