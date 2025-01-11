import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
export const DepartmentCreationForm = () => {
    const [formData, setFormData] = useState({
    name: "",
    id: "",
    section: "",
    });

    const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
    e.preventDefault();
    console.log("User created:", formData);
    };

    return (
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
                <label htmlFor="role" className="block text-sm font-medium text-gray-700">
                    Section
                </label>
                <select
                    id="section"
                    name="section"
                    value={formData.role}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                >
                    <option value="admin">Section1</option>
                    <option value="section_manager">Section2</option>
                    <option value="technician">Section3</option>
                    <option value="receptor">Section4</option>
                    <option value="director">Section5</option>
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
    );
};

export default DepartmentCreationForm;
