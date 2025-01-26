import { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import api from "@/middlewares/api";

export function SectionsTable() {
    const [sectionList, setSectionList] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetchSections();
    }, []);
    
    const fetchSections = async () => {
        setLoading(true); // Inicia el loading
        setError(null); // Reinicia el error
        try {
            const response = await api('/Section/GET_ALL', {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setSectionList(data);
        } catch (error) {
            console.error("Error fetching sections:", error);
            setError("Failed to fetch sections");
            setSectionList([]);
        } finally {
            setLoading(false); // Finaliza el loading
        }
    };

    return (
        <div className="mt-12 mb-8 flex flex-col gap-8">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Sections and Managers
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
                        {sectionList.map((section) => (
                            <div
                                className="border border-gray-200 rounded-lg shadow-sm p-4 hover:bg-gray-200 hover:shadow-md transition duration-300"
                            >
                                <Typography variant="h6" color="blue-gray">
                                    Section: {section.name}
                                </Typography>
                                <Typography variant="small" color="gray">
                                    Manager ID: {section.sectionManagerId}
                                </Typography>
                            </div>
                        ))}
                        {!loading && sectionList.length === 0 && (
                            <Typography variant="small" color="gray">
                                No sections available.
                            </Typography>
                        )}
                    </div>
                </CardBody>
            </Card>
        </div>
    );
}

SectionsTable.displayName = "/src/pages/dashboard_director/sections_table.jsx";
export default SectionsTable;
