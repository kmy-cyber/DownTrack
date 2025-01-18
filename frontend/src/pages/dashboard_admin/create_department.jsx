import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";

export const DepartmentCreationForm = () => {
    const [isLoading, setIsLoading] = useState(true);
    const[alertMessage, setAlertMessage] = useState('');
    const [alertType, setAlertType] = useState('success');

    const [sectionList, setSectionList] = useState([]);
    const [formData, setFormData] = useState({
        name: "",
        id: "",
        section: "",
    });

    useEffect(() => {
        setIsLoading(true);
        fetchSections();
        setIsLoading(false);
    }, []);
    
    const fetchSections = async () => {
        try {
            const response = await fetch('http://localhost:5217/api/Section/GET_ALL', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                },
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            
            setSectionList(data);
            setFormData((prev) => ({ ...prev, ['section']: data[0].id }));
            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching sections:", error);
            setSectionList([]);
            setIsLoading(false);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Department created:", formData);

        setIsLoading(true);
        setAlertMessage(null);
        try {
            const response = await fetch("http://localhost:5217/api/Department/POST", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: formData.name,
                    sectionId: formData.section,
                }),
            });

            if (!response.ok) { 
                if (response.status === 400) {
                    setAlertType('error');
                    setAlertMessage("Something fail, validation error please review the fields");
                    setAlertMessage("Validation error. Please review the fields.");
                } else if (response.status === 500) {
                    setAlertType('error');
                    setAlertMessage("Something fail,a server error occurred. Try again later",'error');
                    setAlertMessage("A server error occurred. Try again later.");
                }
            }
            else if (response.ok) {
                setAlertType('success');
                setAlertMessage("Successful registration");
                setFormData({ id: "", name: "" });
            } else {
                setAlertMessage("Failed to login");
            }
        } catch (error) {
            setAlertType('error');
            console.error("Error logging in:", error);
            setAlertMessage('An error occurred during the login process');
        } finally {
            setIsLoading(false);
        }
    };

    if(isLoading) {
        return (
            <div className='w-full h-full item-center align-center'>
                <h2>Loading...</h2>
            </div>
        );
    }

    return (
        <>
            <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />
            <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
            <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                <Typography variant="h6" color="white">
                    Register Department
                </Typography>
            </CardHeader>
            <CardBody>
                <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    {/* General Fields */}
                    <div>
                    <label htmlFor="name" className="block text-sm font-medium text-gray-700">
                        Name
                    </label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        placeholder="Enter departament's name"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    />
                    </div>

                    <div>
                    <label htmlFor="section" className="block text-sm font-medium text-gray-700">
                        Section
                    </label>
                    <select
                        id="section"
                        name="section"
                        value={formData.section}
                        onChange={handleChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    >
                        {sectionList.map(s => (
                            <option key={s.id} value={s.id}>{s.name}</option>
                        ))}
                    </select>
                    </div>

                    {formData.role === "section_manager" || formData.role === "receptor" ? (
                    <div>
                        <label htmlFor="department" className="block text-sm font-medium text-gray-700">
                        Department
                        </label>
                        <input
                        type="text"
                        id="department"
                        name="department"
                        value={formData.department}
                        onChange={handleChange}
                        placeholder="Enter department"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                        />
                    </div>
                    ) : null}

                    {formData.role === "technician" ? (
                    <>
                        <div>
                        <label htmlFor="experience" className="block text-sm font-medium text-gray-700">
                            Years of Experience
                        </label>
                        <input
                            type="number"
                            id="experience"
                            name="experience"
                            value={formData.experience}
                            onChange={handleChange}
                            placeholder="Enter years of experience"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                        </div>

                        <div>
                        <label htmlFor="specialty" className="block text-sm font-medium text-gray-700">
                            Specialty
                        </label>
                        <input
                            type="text"
                            id="specialty"
                            name="specialty"
                            value={formData.specialty}
                            onChange={handleChange}
                            placeholder="Enter specialty"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        />
                        </div>

                        <div>
                        <label htmlFor="salary" className="block text-sm font-medium text-gray-700">
                            Salary
                        </label>
                        <input
                            type="number"
                            id="salary"
                            name="salary"
                            value={formData.salary}
                            onChange={handleChange}
                            placeholder="Enter money to pay"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        />
                        </div>
                    </>
                    ) : null}
                </div>

                <button
                    type="submit"
                    className="mt-6 w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                >
                    Accept
                </button>
                </form>
            </CardBody>
        </div>
        </>
    );
};

export default DepartmentCreationForm;
