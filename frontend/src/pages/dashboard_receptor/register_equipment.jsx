import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import {equipmentData} from "@/data/equipment-data";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
export const EquipmentRegisterForm = () => {
    const [formData, setFormData] = useState({
        id: "",
        name: " ",
        type: "",
        status: "Active",
        acquisitionDate: "",
        section: "",
        department: ""
    });
    const [startDate, setStartDate] = useState("");
    const [alertMessage, setAlertMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [alertType, setAlertType] = useState('success');
    const [dateFormat, setDateFormat] = useState("");

    const {user} = useAuth();

    const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    };

    // "2025-01-24T15:34:15.298Z"

    // Generar la fecha y hora en formato YYYY-MM-DD HH:MM:SS
        const generateDateTime = () => {
            const currentDate = new Date();
            return currentDate
                .toISOString()
                .slice(0, 19) // Recorta para obtener la fecha y hora en formato YYYY-MM-DDTHH:MM:SS
                .replace("T", " "); // Reemplaza "T" con espacio para formato MySQL
        };
    
        const inFormatDate = () => {
            const currentDate = new Date();
            const year = currentDate.getFullYear();
            const month = String(currentDate.getMonth() + 1).padStart(2, '0');
            const day = String(currentDate.getDate()).padStart(2, '0');
            const hours = String(currentDate.getHours()).padStart(2, '0');
            const minutes = String(currentDate.getMinutes()).padStart(2, '0');
            const seconds = String(currentDate.getSeconds()).padStart(2, '0');
            const milliseconds = String(currentDate.getMilliseconds()).padStart(3, '0');
        
            return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}.${milliseconds}Z`;
        };

        // Establecer la fecha al cargar el componente
        useEffect(() => {
            const currentDate = generateDateTime();
            setStartDate(currentDate);
            const dateInFormat = inFormatDate();
            setDateFormat(dateInFormat);
            fetchReceptorInfo(user.id);
        }, []);


        const fetchReceptorInfo = async (receptorId) => {
            try {
                const response = await api(`/EquipmentReceptor/GET?equipmentReceptorId=${receptorId}`, {
                    method: 'GET',
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setFormData((prevData) => ({
                    ...prevData, 
                    section: data.sectionId, 
                    department: data.departmentId
                }));
                setIsLoading(false);
            } catch (error) {
                console.error("Error fetching departments:", error);
                setIsLoading(false);
            }
        };

        const handleSubmit = async (e) => {
            console.log("Register equipment");
            e.preventDefault(); // Previene la recarga de la página
            const currentDate = new Date().toISOString().split('T')[0];
            date: currentDate
            setIsLoading(true);
            setAlertMessage(null);
            try {
                const response = await api("/Equipment/POST/", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        //"Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxODkiLCJnaXZlbl9uYW1lIjoicGVkcm9fc2FuY2hlcyIsImp0aSI6IjFlMDYxMDVmLWNhYzQtNDU2ZC1iMTAxLTRjMzM1MTYyOTlhYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJleHAiOjE3Mzc0NzQ2MDgsImlzcyI6IkRvd25UcmFjayIsImF1ZCI6IkRvd25UcmFjayJ9.MSHvcGnczsqz1HuaHRqvlsSjE7-LyZQjRSCVWEe_kp4"
                    },
                    
                    body: JSON.stringify({
                        "name": formData.name,
                        "type": formData.type,
                        "status": formData.status,
                        "dateOfadquisition": dateFormat,
                        "sectionId": formData.section,
                        "departmentId": formData.department,
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


    return (
    <>
        <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />
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
                        id="name"
                        name="name"
                        value={formData.name}
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
                        <label htmlFor="status" className="block text-sm font-medium text-gray-700">
                        Status
                    </label><select
                        id="status"
                        name="status"
                        value={formData.status}
                        onChange={handleChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    >
                        <option value="Active">Active</option>
                        <option value="UnderMaintenance"> Under maintenance</option>
                        <option value="Inactive">Inactive</option>
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
    </>
    );
};

export default EquipmentRegisterForm;
