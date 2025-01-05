import { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography, Button, Select, Option } from "@material-tailwind/react";

export function Reports() {
    const [reportType, setReportType] = useState("inventory");
    const [startDate, setStartDate] = useState("");
    const [reportData, setReportData] = useState([]);

    // Generar la fecha y hora en formato YYYY-MM-DD HH:MM:SS
    const generateDateTime = () => {
        const currentDate = new Date();
        return currentDate
            .toISOString()
            .slice(0, 19) // Recorta para obtener la fecha y hora en formato YYYY-MM-DDTHH:MM:SS
            .replace("T", " "); // Reemplaza "T" con espacio para formato MySQL
    };

    // Establecer la fecha al cargar el componente
    useEffect(() => {
        const currentDate = generateDateTime();
        setStartDate(currentDate);
    }, []);

    const generateReport = () => {
        // To Do API call - replace with backend call.
        const simulatedData = {
            inventory: [
                { id: 1, name: "Item A", status: "Active", quantity: 10 },
                { id: 2, name: "Item B", status: "Inactive", quantity: 5 },
            ],
            "technical-downtime": [
                { id: 1, equipment: "Machine X", cause: "Broken", downtime: "5 hours" },
                { id: 2, equipment: "Machine Y", cause: "Maintenance", downtime: "2 hours" },
            ],
            "staff-effectiveness": [
                { id: 1, name: "John Doe", tasksCompleted: 15, efficiency: "90%" },
                { id: 2, name: "Jane Smith", tasksCompleted: 10, efficiency: "85%" },
            ],
        };

        setReportData(simulatedData[reportType]);
    };

    return (
        <div className="mt-12 mb-8 flex flex-col gap-12">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
                    <Typography variant="h6" color="white">
                        Report Generator
                    </Typography>
                </CardHeader>
                <CardBody>
                    {/* Report Parameters */}
                    <div className="flex flex-col gap-6 mb-6">
                        <div className="flex flex-col md:flex-row gap-4">
                            <Select
                                label="Select Report Type"
                                value={reportType}
                                onChange={(value) => setReportType(value)}
                            >
                                <Option value="inventory">Inventory Status</Option>
                                <Option value="technical-downtime">Technical Downtime</Option>
                                <Option value="staff-effectiveness">Staff Effectiveness</Option>
                            </Select>
                            <input
                                type="text"
                                value={startDate}
                                readOnly
                                className="border rounded-lg px-4 py-2 bg-gray-100 text-gray-700"
                                placeholder="Start Date"
                            />
                        </div>
                        <Button color="gray" onClick={generateReport}>
                            Generate Report
                        </Button>
                    </div>

                    {/* Report Table */}
                    {reportData.length > 0 && (
                        <div className="overflow-x-auto">
                            <table className="w-full table-auto border-collapse border border-gray-200">
                                <thead>
                                    <tr>
                                        {Object.keys(reportData[0]).map((key) => (
                                            <th
                                                key={key}
                                                className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700"
                                            >
                                                {key.charAt(0).toUpperCase() + key.slice(1)}
                                            </th>
                                        ))}
                                    </tr>
                                </thead>
                                <tbody>
                                    {reportData.map((row, index) => (
                                        <tr key={index} className="hover:bg-gray-50">
                                            {Object.values(row).map((value, i) => (
                                                <td key={i} className="border border-gray-200 px-4 py-2">
                                                    {value}
                                                </td>
                                            ))}
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </CardBody>
            </Card>
        </div>
    );
}

export default Reports;
