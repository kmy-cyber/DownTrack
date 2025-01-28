import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import { useParams } from 'react-router-dom';
import { useAuth } from "@/context/AuthContext";


export const MaintenanceCreationForm = () => {
    const { id, name:nameEquipment, type } = useParams();
    console.log("ENTER:",id, nameEquipment, type);

    const{user} = useAuth(); 
    
    const [formData, setFormData] = useState({
    technicianId: user.id,
    type: "",
    equipmentId: id,
    equipmentName: nameEquipment + " - " + type,
    date: "",
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
    console.log("Maintenance", formData);
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
                Register Maintenance
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
                    value={formData.equipmentName}
                    onChange={handleChange}
                    placeholder="Enter the equipment"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                <div>
                <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                    Maintenance's Type
                </label>
                <input
                    type="text"
                    id="type"
                    name="type"
                    value={formData.type}
                    onChange={handleChange}
                    placeholder="Enter type"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
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
                    readOnly
                    className="mt-1 block w-full px-3 py-2 border  text-gray-700 border-gray-300 sel focus:border-gray-300 rounded-md shadow-sm"
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

export default MaintenanceCreationForm;
