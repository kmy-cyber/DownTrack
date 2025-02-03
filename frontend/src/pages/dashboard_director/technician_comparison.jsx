import React, { useState, useEffect } from "react";
import {
    Card,
    CardBody,
    CardHeader,
    Typography,
    Checkbox,
    Button,
    Input,
} from "@material-tailwind/react";
import StatisticsChart from "@/components/charts/statistics-chart";
import api from "@/middlewares/api";
import { Pagination, Stack } from "@mui/material";

const TechnicianComparison = () => {
    const [technicians, setTechnicians] = useState([]);
    const [selectedTechnicians, setSelectedTechnicians] = useState([]);
    const [stats, setStats] = useState({});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [searchTerm, setSearchTerm] = useState(""); // Para búsqueda
    const [currentPage, setCurrentPage] = useState(1); // Paginación
    const [totalPages, setTotalPages] = useState(1); // Total de páginas
    const PageSize = 5;

    useEffect(() => {
        fetchTechnicians();
    }, [currentPage, searchTerm]);

    const fetchTechnicians = async () => {
        setLoading(true);
        try {
            // Asegúrate de que PageSize sea 5
            const response = await api(
                `/Technician/GetPaged?PageNumber=${currentPage}&PageSize=${PageSize}&SearchTerm=${searchTerm}`,
            );
            if (!response.ok) throw new Error("Failed to fetch technicians");
            const data = await response.json();
            setTechnicians(data.items);
            setTotalPages(Math.ceil(data.totalCount / PageSize)); // Total de páginas basado en totalCount
        } catch (err) {
            setError("Error loading technicians");
        } finally {
            setLoading(false);
        }
    };

    const fetchStats = async () => {
        if (selectedTechnicians.length === 0) return;
        setLoading(true);
        try {
            const newStats = {};
            for (const technician of selectedTechnicians) {
                const [evaluationRes, statsRes] = await Promise.all([
                    api(
                        `/Evaluation/Get_Evaluation_By_TechnicianId?PageNumber=1&PageSize=10000&technicianId=${technician}`,
                    ),
                    api(`/Statistics/Technician?technicianId=${technician}`),
                ]);
                if (!evaluationRes.ok || !statsRes.ok)
                    throw new Error("Failed to fetch stats");
                const evaluationsData = await evaluationRes.json();
                const statsData = await statsRes.json();
                newStats[technician] = {
                    evaluations: evaluationsData.items,
                    stats: statsData,
                };
            }
            setStats(newStats);
        } catch (err) {
            setError("Error loading statistics");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchStats();
    }, [selectedTechnicians]);

    const handleCheckboxChange = (technicianId) => {
        setSelectedTechnicians((prevSelectedTechnicians) => {
            if (prevSelectedTechnicians.includes(technicianId)) {
                return prevSelectedTechnicians.filter(
                    (id) => id !== technicianId,
                );
            } else {
                return [...prevSelectedTechnicians, technicianId];
            }
        });
    };

    const handleSearchChange = (e) => {
        setSearchTerm(e.target.value);
        setCurrentPage(1); // Reiniciar a la primera página cuando se cambie la búsqueda
    };

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    // Gráfico de Evaluaciones (Barra)
    const evaluationChart = {
        type: "bar",
        series: selectedTechnicians.map((technicianId) => {
            // Obtenemos las evaluaciones para este técnico
            const evaluations = stats[technicianId]?.evaluations || [];
            console.log(evaluations);
            

            // Contamos las evaluaciones por categoría
            const good = evaluations.filter(
                (evaluation) => evaluation.description === "Good",
            ).length;
            const regular = evaluations.filter(
                (evaluation) => evaluation.description === "Regular",
            ).length;
            const bad = evaluations.filter(
                (evaluation) => evaluation.description === "Bad",
            ).length;

            return {
                name: `Technician ${technicianId}`, // Usamos el ID o nombre del técnico
                data: [good, regular, bad], // Datos de las evaluaciones por categoría
            };
        }),
        options: {
            xaxis: {
                categories: ["Good", "Regular", "Bad"], // Categorías de las evaluaciones
                title: { text: "Evaluation Categories" },
            },
            colors: ["#4CAF50", "#FFC107", "#F44336"], // Colores para las categorías
            legend: { position: "top" },
            chart: { background: "#ffffff" },
        },
    };

    // Gráfico de Mantenimiento & Descomisiones (Barras agrupadas)
    const maintenanceChart = {
        type: "bar", // Cambié a 'bar' para barras agrupadas
        series: selectedTechnicians.map((t) => ({
            name: t,
            data: Object.values(stats[t]?.stats.maintenanceByMonth || {}),
        })),
        options: {
            xaxis: {
                categories: Object.keys(
                    stats[selectedTechnicians[0]]?.stats.maintenanceByMonth ||
                        {},
                ),
                title: { text: "Months" },
            },
            colors: ["#008FFB", "#FF4560", "#00E396", "#775DD0"],
            legend: { position: "top" },
            chart: { background: "#ffffff" },
        },
    };

    // Función para limpiar la selección
    const clearSelection = () => {
        setSelectedTechnicians([]);
    };

    return (
        <Card className="mt-12">
            <CardHeader
                variant="gradient"
                color="gray"
                className="flex items-center justify-between p-6"
            >
                <Typography variant="h5" className="mb-4">
                    Compare Technicians
                </Typography>
            </CardHeader>
            <CardBody>
                {/* Sección de Búsqueda */}
                <div className="mb-6 px-6">
                    <Input
                        type="text"
                        label="Search Technicians"
                        value={searchTerm}
                        onChange={handleSearchChange}
                    />
                </div>

                {/* Tabla de Técnicos */}
                <div className="mb-6 px-6">
                    <Typography variant="h6" className="mb-4">
                        Select Technicians
                    </Typography>
                    <div className="overflow-x-auto max-h-96">
                        <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                            <thead className="bg-gray-800 text-white">
                                <tr>
                                    <th className="border-b px-6 py-3 text-center">
                                        Technician
                                    </th>
                                    <th className="border-b px-6 py-3 text-center">
                                        Select
                                    </th>
                                </tr>
                            </thead>
                            <tbody className="bg-white">
                                {technicians.length > 0 ? (
                                    technicians.map((technician) => (
                                        <tr key={technician.id}>
                                            <td className="border-b px-6 py-3 text-center">
                                                {technician.name}
                                            </td>
                                            <td className="border-b px-6 py-3 text-center">
                                                <Checkbox
                                                    checked={selectedTechnicians.includes(
                                                        technician.id,
                                                    )}
                                                    onChange={() =>
                                                        handleCheckboxChange(
                                                            technician.id,
                                                        )
                                                    }
                                                />
                                            </td>
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td
                                            colSpan="2"
                                            className="px-6 py-3 text-center"
                                        >
                                            No technicians found
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* Paginación */}
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

                {/* Botón para limpiar selección */}
                {selectedTechnicians.length > 0 && (
                    <div className="px-6 mb-4">
                        <Button onClick={clearSelection} color="red">
                            Clear Selection
                        </Button>
                    </div>
                )}

                {loading && <div className="px-6">Loading statistics...</div>}
                {error && <div className="px-6">{error}</div>}

                {/* Sección de Gráficos */}
                {stats && selectedTechnicians.length > 0 && (
                    <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2">
                        <div>
                            <Typography variant="h6" className="mb-4 px-6">
                                Evaluation Comparison
                            </Typography>
                            <div className="px-6">
                                <StatisticsChart chart={evaluationChart} />
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="mb-4 px-6">
                                Maintenance
                            </Typography>
                            <div className="px-6">
                                <StatisticsChart chart={maintenanceChart} />
                            </div>
                        </div>
                    </div>
                )}
            </CardBody>
        </Card>
    );
};

export default TechnicianComparison;
