import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
export const EquipmentRegisterForm = () => {
    const [formData, setFormData] = useState({
    name: "",
    username: "",
    role: "admin",
    department: "",
    experience: "",
    specialty: "",
    salary: "",
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
                Register Equipment
            </Typography>
        </CardHeader>
        <CardBody>
            <form onSubmit={handleSubmit}>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* General Fields */}
                <div>
                <label htmlFor="name" className="block text-sm font-medium text-gray-700">
                    Equipment
                </label>
                <input
                    type="text"
                    id="equipment"
                    name="equipment"
                    value={formData.equipment}
                    onChange={handleChange}
                    placeholder="Enter the equipment"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                <div>
                <label htmlFor="type" className="block text-sm font-medium text-gray-700">
                    Type
                </label>
                <input
                    type="text"
                    id="type"
                    name="type"
                    value={formData.type}
                    onChange={handleChange}
                    placeholder="Enter equipment type"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                <div>
                <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                    Date
                </label>
                <input
                    type="date"
                    id="date"
                    name="date"
                    value={formData.date}
                    onChange={handleChange}
                    placeholder=""
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>


                <div>
                <label htmlFor="departament" className="block text-sm font-medium text-gray-700">
                    Assing Departament
                </label>
                <select
                    id="departament"
                    name="departament"
                    value={formData.department}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                >
                    <option value="departament1">Departament1</option>
                    <option value="departament2">Departament2</option>
                    <option value="departament3">Departament3</option>
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
    );
};

export default EquipmentRegisterForm;
