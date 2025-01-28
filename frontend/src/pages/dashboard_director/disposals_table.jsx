import React, { useState,useEffect } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { equipmentDisposalData } from "@/data"; // Asegúrate de importar los datos correctamente
import api from "@/middlewares/api";

const EquipmentDisposalTable = () => {
    const [disposals_list,setDisposalsList] = useState([]);
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
            const response = await api(`/EquipmentDecommissioning/Get_Paged_All?PageNumber=${pageNumber}&PageSize=${pageSize}`,
                {
                    method: 'GET',
                }
            );

            if(!response.ok){
                throw new Error("Failed to fetch equipment decommissionings");
            }
            const data = await response.json();
            setDisposalsList(data.items);
            console.log(data.items);
            setTotalPages(Math.ceil(data.totalCount / pageSize)); // Calcula el total de páginas
        }
        catch (err) {
            console.error("Error fetching decommissionings:", err);
            setError("Failed to load decommissionings data");
        } finally {
            setLoading(false);
        }
    }

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
        <div className="mt-12 mb-8 flex flex-col gap-12">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
                    <Typography variant="h6" color="white">
                        Equipment Disposal Records
                    </Typography>
                </CardHeader>
                <CardBody>
                    {/* Tabla de bajas técnicas */}
                    <div className="overflow-x-auto">
                        <table className="w-full table-auto border-collapse border border-gray-200">
                            <thead>
                                <tr>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Technician</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Equipment</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Reason for Removal</th>
                                    <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                {disposals_list ? disposals_list.map((disposal) => (
                                    <tr key={disposal.id} className="hover:bg-gray-50">
                                        <td className="border border-gray-200 px-4 py-2">{disposal.technicianId}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.equipmentId}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.cause}</td>
                                        <td className="border border-gray-200 px-4 py-2">{disposal.date}</td>
                                    </tr>
                                )) : <p>Theres no disposals yet</p>

                            }
                            </tbody>
                        </table>
                    </div>

                    {/* Pagination */}
                    {!loading && !error && totalPages > 1 && (
                        <div className="flex justify-center mt-4 gap-2">
                        {/* Previous button */}
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

                        {/* Page numbers */}
                        {renderPaginationButtons()}

                        {/* Next button */}
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
                    )}
                </CardBody>
            </Card>
        </div>
    );
};

export default EquipmentDisposalTable;
