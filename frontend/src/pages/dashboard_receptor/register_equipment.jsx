import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import {equipmentData} from "@/data/equipment-data"
export const EquipmentRegisterForm = () => {
    const [formData, setFormData] = useState({
        id: "",
        name: " ",
        type: "",
        status: " ",
        acquisitionDate: "",
        location: "",
        section: ""
    });
    const [startDate, setStartDate] = useState("");

    const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
    e.preventDefault();
    const currentDate = new Date().toISOString().split('T')[0];
    date: currentDate
    console.log("User created:", formData);
    };

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
                <label htmlFor="departament" className="block text-sm font-medium text-gray-700">
                    Assing Departament
                </label>
                <select
                    id="departament"
                    name="departament"
                    value={formData.department}
                    onChange={(e) => setFormData({ ...formData, department: e.target.value })}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                >
                    <option value="departament1">Departament1</option>
                    <option value="departament2">Departament2</option>
                    <option value="departament3">Departament3</option>
                </select>
                </div>

                <div>
                <label htmlFor="section" className="block text-sm font-medium text-gray-700">
                    Assing Section
                </label>
                <select
                    id="departament"
                    name="departament"
                    value={formData.department}
                    onChange={(e) => setFormData({ ...formData, section: e.target.value })}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                >
                    <option value="sec1">section1</option>
                    <option value="sec2">section2</option>
                    <option value="sec3">section3</option>
                </select>
                </div>

                <div>
                    <label htmlFor="Status" className="block text-sm font-medium text-gray-700">
                    Status
                </label><select
                    id="status"
                    name="status"
                    value={formData.department}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                >
                    <option value="active">Active</option>
                    <option value="under_maintenance"> Under maintenance</option>
                    <option value="inactive">Inactive</option>
                </select>
                </div>

                <div>
                <label htmlFor="date" className="block text-sm font-medium text-gray-700">
                    Date
                </label>
                <input
                    type="text"
                    id="date"
                    name="date"
                    value={startDate}
                    placeholder="Enter equipment type"
                    readOnly
                    className="mt-1 block w-full px-3 py-2 border  text-gray-900 border-gray-300 rounded-md shadow-sm"
                    required
                />
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
