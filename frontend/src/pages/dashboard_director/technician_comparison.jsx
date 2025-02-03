import React, { useState, useEffect } from "react";
import {
    Card,
    CardBody,
    CardHeader,
    Typography,
    Checkbox,
    Button,
    IconButton,
    Input,
} from "@material-tailwind/react";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";
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
    const [isSearching, setIsSearching] = useState(false);
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

    const searchByUserName = async () => {
        setLoading(true);
        try {
            // Asegúrate de que PageSize sea 5
            const response = await api(
                `/Technician/Search_By_UserName?username=${searchTerm}`,
            );
            if (!response.ok) throw new Error("Failed to fetch technician");
            const user = await response.json();
            console.log(`USER ${user}`);
            setTechnicians([user]);
        } catch (err) {
            setError("Error searching technician");
        } finally {
            setLoading(false);
        }
    };

    const resetSearch = () => {
        setSearchTerm("");
        setIsSearching(false);
        setIsSearching(false);
        fetchTechnicians();
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

    const handleKeyDown = (e) => {
        if (e.key === "Enter") {
            e.preventDefault();
            searchByUserName();
            setIsSearching(true);
        }
    };
    // Gráfico de Evaluaciones (Barra)
    // Gráfico de Evaluaciones (Barra)
    const evaluationChart = {
        type: "bar",
        series: selectedTechnicians.map((technicianId) => {
            // Obtenemos las evaluaciones para este técnico
            const evaluations = stats[technicianId]?.evaluations || [];
            const technician = technicians.find((t) => t.id === technicianId);

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
                name: technician
                    ? technician.userName
                    : `Technician ${technicianId}`,
                data: [good, regular, bad], // Datos de las evaluaciones por categoría
            };
        }),
        options: {
            chart: {
                background: "#ffffff",
                stacked: true, // Esto permite apilar las barras
            },
            xaxis: {
                categories: ["Good", "Regular", "Bad"], // Categorías de las evaluaciones
                title: { text: "Evaluation Criteria" },
            },
            yaxis: {
                title: { text: "Number of Evaluations" },
            },
            colors: ["#4CAF50", "#FFC107", "#F44336"], // Colores para las categorías
            legend: { position: "top" },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: "60%", // Ajusta el ancho de las barras
                },
            },
            tooltip: {
                shared: true,
                intersect: false,
                y: {
                    formatter: (val) => `${val} evaluations`, // Muestra la cantidad de evaluaciones en el tooltip
                },
            },
        },
    };

    // Gráfico de Mantenimiento & Descomisiones (Barras agrupadas)
    const maintenanceChart = {
        type: "bar",
        series: selectedTechnicians.map((technicianId) => {
            const technician = technicians.find((t) => t.id === technicianId);
            return {
                name: technician
                    ? technician.userName
                    : `Technician ${technicianId}`, // Mostramos el userName
                data: Object.values(
                    stats[technicianId]?.stats.maintenanceByMonth || {},
                ),
            };
        }),
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
                <Typography variant="h5" className="mb-4">
                    Compare Technicians
                </Typography>
            </CardHeader>
            <CardBody>
                {/* Sección de Búsqueda */}
                <div className="mb-6 px-6">
                    <Input
                        type="text"
                        label="Search Technician"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        onKeyDown={handleKeyDown}
                    />
                </div>

                {/* Tabla de Técnicos */}
                <div className="mb-6 px-6">
                    <Typography variant="h6" className="mb-4">
                        Select Technicians
                    </Typography>
                    <div className="overflow-x-auto max-h-72">
                        {" "}
                        {/* Añadir desplazamiento horizontal y altura limitada */}
                        <table className="min-w-full table-auto border-collapse text-sm text-gray-900">
                            <thead className="bg-gray-200 text-gray-700">
                                <tr>
                                    <th className="border-b px-4 py-2 text-left">
                                        Technician
                                    </th>
                                    <th className="border-b px-4 py-2 text-center">
                                        Select
                                    </th>
                                </tr>
                            </thead>
                            <tbody className="bg-white">
                                {technicians.length > 0 ? (
                                    technicians.map((technician) => (
                                        <tr
                                            key={technician.id}
                                            className="hover:bg-gray-100"
                                        >
                                            <td className="border-b px-4 py-2 text-left">
                                                {technician.userName}
                                            </td>
                                            <td className="border-b px-4 py-2 text-center">
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
                                            className="px-4 py-2 text-center text-gray-500"
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
                {!loading && !error && !isSearching && totalPages > 1 && (
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
                        <Button onClick={clearSelection} color="gray">
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
                                <StatisticsChart
                                    color="white"
                                    chart={evaluationChart}
                                />
                            </div>
                            <Typography
                                variant="small"
                                className="text-center text-gray-600 mt-2 px-6"
                            >
                                Comparación de evaluaciones de técnicos en base
                                a calificaciones de desempeño.
                            </Typography>
                        </div>
                        <div>
                            <Typography variant="h6" className="mb-4 px-6">
                                Maintenance Count
                            </Typography>
                            <div className="px-6">
                                <StatisticsChart
                                    color="white"
                                    chart={maintenanceChart}
                                />
                            </div>
                            <Typography
                                variant="small"
                                className="text-center text-gray-600 mt-2 px-6"
                            >
                                Registro de mantenimientos realizados por
                                técnicos a lo largo de los meses.
                            </Typography>
                        </div>
                    </div>
                )}
            </CardBody>
        </Card>
    );
};

export default TechnicianComparison;
