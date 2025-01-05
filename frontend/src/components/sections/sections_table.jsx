import { useState } from "react";
import { useNavigate } from "react-router-dom"; 
import { Card, CardHeader, CardBody, Typography, Button, Input } from "@material-tailwind/react";
import { sectionData } from "@/data/sections-data"; 
import { departmentData } from "@/data/department-data";
import { ChevronDownIcon, ChevronUpIcon } from "@heroicons/react/24/solid";
import { userListData } from "@/data/users-table-data";

export function SectionsTable() {
    const navigate = useNavigate();

    const [expandedSectionId, setExpandedSectionId] = useState(null);
    const [searchTerm, setSearchTerm] = useState("");

    const handleSectionInventoryClick = (sectionId) => {
        navigate(`/dashboard/director/sections/${sectionId}/inventory`);
    };

    const handleDepartmentInventoryClick = (departmentId) => {
        navigate(`/dashboard/director/departments/${departmentId}/inventory`);
    };

    const toggleSectionDetails = (sectionId) => {
        setExpandedSectionId((prev) => (prev === sectionId ? null : sectionId));
    };

    const filteredSections = sectionData.filter((section) =>
        section.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="mt-12 mb-8 flex flex-col gap-8">
            <Card>
                <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Sections
                    </Typography>
                    <div className="relative mt-4">
                        <Input 
                            type="text"
                            placeholder="Search sections..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            className="w-full border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:outline-none"
                            style={{
                                transition: "none", 
                                padding: "0.5rem 0.75rem",
                                height: "40px",
                                color: "white",
                            }}
                        />
                    </div>
                </CardHeader>
                <CardBody className="px-4 py-2">
                    <div className="space-y-4">
                        {filteredSections.map((section) => {
                            const departments = departmentData.filter(
                                (dept) => dept.sectionId === section.id
                            );
                            const sectionManager = userListData.find(
                                (user) => user.role === "section_manager" && user.sectionId === section.id
                            );

                            const isExpanded = expandedSectionId === section.id;

                            return (
                                <div 
                                    key={section.id} 
                                    className="border border-gray-200 rounded-lg shadow-sm hover:bg-gray-200 hover:shadow-md transition duration-300"
                                >
                                    <div
                                        className="flex items-center justify-between p-4 bg-gray-100 cursor-pointer"
                                        onClick={() => toggleSectionDetails(section.id)}
                                    >
                                        <Typography variant="h6" color="blue-gray">
                                            {section.name}
                                        </Typography>
                                        {isExpanded ? (
                                            <ChevronUpIcon className="w-5 h-5 text-gray-600" />
                                        ) : (
                                            <ChevronDownIcon className="w-5 h-5 text-gray-600" />
                                        )}
                                    </div>
                                    {isExpanded && (
                                        <div id={`section-${section.id}`} className="p-4">
                                            <Typography variant="small" className="text-gray-700 mb-2">
                                                <span className="font-semibold">Manager:</span> {sectionManager ? sectionManager.name : "No section manager assigned."}
                                            </Typography>
                                            <Typography variant="small" className="text-gray-700 mb-2">
                                                <span className="font-semibold">Description:</span> {section.description}
                                            </Typography>
                                            <Typography variant="small" className="text-gray-700 mb-4">
                                                <span className="font-semibold">Departments:</span>
                                            </Typography>
                                            <ul className="list-none space-y-2">
                                                {departments.map((dept) => (
                                                    <li 
                                                        key={dept.id} 
                                                        className="flex items-center justify-between p-2 border border-gray-200 rounded-md hover:bg-gray-100 transition"
                                                    >
                                                        <Typography variant="small" color="blue-gray">
                                                            {dept.name}
                                                        </Typography>
                                                        <Button
                                                            variant="text"
                                                            color="gray"
                                                            size="sm"
                                                            onClick={() => handleDepartmentInventoryClick(dept.id)}
                                                        >
                                                            View Inventory
                                                        </Button>
                                                    </li>
                                                ))}
                                            </ul>
                                            <Button
                                                variant="text"
                                                color="gray"
                                                size="sm"
                                                className="mt-4"
                                                onClick={() => handleSectionInventoryClick(section.id)}
                                            >
                                                View Section Inventory
                                            </Button>
                                        </div>
                                    )}
                                </div>
                            );
                        })}
                        {filteredSections.length === 0 && (
                            <Typography variant="small" color="gray">
                                No sections match your search criteria.
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
