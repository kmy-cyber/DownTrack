import React, { useState, useEffect } from "react";
import {
    CardHeader,
    CardBody,
    Typography,
} from "@material-tailwind/react";
import { toast } from "react-toastify";
import api from "@/middlewares/api";

export const DepartmentCreationForm = () => {
    const [isLoading, setIsLoading] = useState(true);

    const [sectionList, setSectionList] = useState([]);
    const [formData, setFormData] = useState({
        name: "",
        section: "",
        sectionName: "",
    });

    useEffect(() => {
        setIsLoading(true);
        fetchSections();
        setIsLoading(false);
    }, []);
    
    const fetchSections = async () => {
        try {
            const response = await api('/Section/GET_ALL', {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setSectionList(data);
            if(data.length === 0){
                navigate('/dashboard/admin/add_section');
            }
            console.log(data);
            setFormData((prev) => ({ ...prev, ['section']: data[0].id }));
            setFormData((prev) => ({ ...prev, ['sectionName']: data[0].name }));
            
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
        try {

            console.log(formData.name, formData.section);
            const response = await api("/Department/POST", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: formData.name,
                    sectionId: formData.section,
                    sectionName: formData.sectionName,
                }),
            });


            if (response.ok) {
                toast.success("Successful registration");
                setFormData({ name: "", section: formData.section, sectionName: formData.sectionName });
            } else {
                toast.error("Failed to login");
            }
        } catch (error) {
            toast.error('An error occurred during the login process');
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
