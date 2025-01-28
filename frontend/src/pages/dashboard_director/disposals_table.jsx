import React, { useState, useEffect } from "react";
import {
    Card, CardHeader, CardBody, Typography, Button, IconButton
} from "@material-tailwind/react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";
import api from "@/middlewares/api";

const EquipmentDisposalTable = () => {
    const [disposalsList, setDisposalsList] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const pageSize = 14;

    useEffect(() => {
        fetchDisposals(currentPage);
    }, [currentPage]);

    const fetchDisposals = async (pageNumber) => {
        setLoading(true);
        setError(null);
        try {
            const response = await api(`/EquipmentDecommissioning/Get_Paged_All?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
                method: 'GET',
            });

            if (!response.ok) {
                throw new Error("Failed to fetch equipment decommissionings");
            }

            const data = await response.json();
            setDisposalsList(data.items || []);
            setTotalPages(Math.ceil(data.totalCount / pageSize));
        } catch (err) {
            setError("Failed to load decommissionings data");
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
        <Card className="mt-8 shadow-lg">
            <CardHeader variant="gradient" color="gray" className="p-6 flex items-center justify-between">
                <Typography variant="h6" color="white" className="text-xl font-semibold">
                    Equipment Disposal Records
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
                        {/* Tabla de Desincorporación de Equipos */}
                        <div className="overflow-x-auto">
                            <table className="min-w-full table-auto text-sm text-gray-900">
                                <thead className="bg-gray-800 text-white">
                                    <tr>
                                        <th className="px-6 py-3 border-b text-center">Technician</th>
                                        <th className="px-6 py-3 border-b text-center">Equipment</th>
                                        <th className="px-6 py-3 border-b text-center">Receptor</th>
                                        <th className="px-6 py-3 border-b text-center">Reason for Removal</th>
                                        <th className="px-6 py-3 border-b text-center">Date</th>
                                    </tr>
                                </thead>
                                <tbody className="bg-white">
                                    {disposalsList.length > 0 ? (
                                        disposalsList.map((disposal) => (
                                            <tr key={disposal.id}>
                                                <td className="px-6 py-3 border-b text-center">{disposal.technicianUserName}</td>
                                                <td className="px-6 py-3 border-b text-center">{disposal.equipmentId}</td>
                                                <td className="px-6 py-3 border-b text-center">{disposal.receptorUserName}</td>
                                                <td className="px-6 py-3 border-b text-center">{disposal.cause}</td>
                                                <td className="px-6 py-3 border-b text-center">{disposal.date}</td>
                                            </tr>
                                        ))
                                    ) : (
                                        <tr>
                                            <td colSpan="5" className="px-6 py-3 text-center">No disposals found</td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </>
                )}

                {/* Paginación */}
                <div className="flex justify-center mt-4 space-x-2">
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

export default EquipmentDisposalTable;
