import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,
    Dialog,
    DialogHeader,
    DialogBody,
    Button,
    DialogFooter,
    Input,
} from "@material-tailwind/react";
import api from "@/middlewares/api";
import { useAuth } from "@/context/AuthContext";
import { ChevronDownIcon } from "@heroicons/react/24/solid";
import { useParams } from "react-router-dom";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

export const LeaveCreationForm = () => {
    const { id, name: nameEquipment, type } = useParams();

    const navigate = useNavigate();
    const { user } = useAuth();

    const [formData, setFormData] = useState({
        id: "",
        technicianId: user.id,
        equipmentId: id,
        equipmentName: nameEquipment + " - " + type,
        receptorId: "",
        receptorName: "",
        receptorUsername: "",
        date: "",
        cause: "",
        status: "Pending",
    });
    const [searchQuery, setSearchQuery] = useState("");
    const [filteredReceptor, setFilteredReceptor] = useState([]);
    const [startDate, setStartDate] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [dateFormat, setDateFormat] = useState("");
    const [receptorList, setReceptorList] = useState([]);
    const [showReceptors, setShowReceptors] = useState(false);

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
        const month = String(currentDate.getMonth() + 1).padStart(2, "0");
        const day = String(currentDate.getDate()).padStart(2, "0");
        const hours = String(currentDate.getHours()).padStart(2, "0");
        const minutes = String(currentDate.getMinutes()).padStart(2, "0");
        const seconds = String(currentDate.getSeconds()).padStart(2, "0");
        const milliseconds = String(currentDate.getMilliseconds()).padStart(
            3,
            "0",
        );

        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}.${milliseconds}Z`;
    };

    // Establecer la fecha al cargar el componente
    useEffect(() => {
        setIsLoading(true);
        const currentDate = generateDateTime();
        setStartDate(currentDate);
        const dateInFormat = inFormatDate();
        setDateFormat(dateInFormat);
        fetchReceptors();
        setIsLoading(false);
    }, []);

    const fetchReceptors = async () => {
        try {
            const response = await api(`/EquipmentReceptor/GetAll`, {
                method: "GET",
            });
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setReceptorList(data);
            setFormData((prev) => ({
                ...prev,
                ["receptorId"]: data[0].id,
                ["receptorName"]: data[0].name,
                ["receptorUsername"]: data[0].userName,
            }));
            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching departments:", error);
            setReceptorList([]);
            setIsLoading(false);
        }
    };

    const submitLeave = async () => {
        console.log("Submit leave:", formData);
        try {
            const response = await api("/EquipmentDecommissioning/POST/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },

                body: JSON.stringify({
                    technicianId: user.id,
                    equipmentId: formData.equipmentId,
                    receptorId: formData.receptorId,
                    date: dateFormat,
                    cause: formData.cause,
                    status: formData.status,
                }),
            });

            if (response.ok) {
                toast.success("Successful registration");
                navigate("/dashboard/technic/equipment_inventory");
            } else {
                toast.error("Failed to login");
            }
        } catch (error) {
            console.error("Error logging in:", error);
            toast.error("An error occurred during the login process");
        } finally {
            setIsLoading(false);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Baja propuesta:", formData);
        const currentDate = new Date().toISOString().split("T")[0];
        date: currentDate;
        setIsLoading(true);
        await submitLeave();
    };

    const handleSearch = (e) => {
        const query = e.target.value;
        setSearchQuery(query);

        if (query) {
            setFilteredReceptor(
                receptorList.filter((receptor) =>
                    receptor.name.toLowerCase().includes(query.toLowerCase()),
                ),
            );
        } else {
            setFilteredReceptor(receptorList);
        }
    };

    const handleShowROpen = () => {
        setShowReceptors(true);
    };

    const handleShowRClose = () => {
        setShowReceptors(false);
    };

    const handleReceptorSelect = (receptorId, receptorName) => {
        setFormData((prev) => ({
            ...prev,
            ["receptorId"]: receptorId,
            ["receptorName"]: receptorName,
        }));
        handleShowRClose("receptor");
    };

    return (
        <>
            <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
                <CardHeader
                    variant="gradient"
                    color="gray"
                    className="mb-8 p-6"
                >
                    <Typography variant="h6" color="white">
                        Register Technical Decommissioning
                    </Typography>
                </CardHeader>
                <CardBody>
                    <form onSubmit={handleSubmit}>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            {/* General Fields */}
                            <div>
                                <label
                                    htmlFor="equipmentId"
                                    className="block text-sm font-medium text-gray-700"
                                >
                                    Equipment
                                </label>
                                <input
                                    type="equipmentId"
                                    id="equipmentId"
                                    name="equipmentId"
                                    value={formData.equipmentName}
                                    onChange={handleChange}
                                    placeholder="Enter the equipment"
                                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                                    required
                                    disabled
                                />
                            </div>

                            <div>
                                <label
                                    htmlFor="username"
                                    className="block text-sm font-medium text-gray-700"
                                >
                                    Cause
                                </label>
                                <input
                                    type="text"
                                    id="cause"
                                    name="cause"
                                    value={formData.cause}
                                    onChange={handleChange}
                                    placeholder="Enter cause"
                                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                                    required
                                />
                            </div>

                            <div>
                                <label
                                    htmlFor="username"
                                    className="block text-sm font-medium text-gray-700"
                                >
                                    Date
                                </label>
                                <input
                                    type="text"
                                    id="date"
                                    name="date"
                                    value={startDate}
                                    onChange={handleChange}
                                    placeholder=""
                                    className="mt-1 block w-full px-3 py-2 border  text-gray-900 border-gray-300 rounded-md shadow-sm"
                                    required
                                    disabled
                                />
                            </div>

                            <div>
                                <label
                                    htmlFor="destiny"
                                    className="block text-sm font-medium text-gray-700"
                                >
                                    Final destiny
                                </label>
                                <select
                                    id="destiny"
                                    name="destiny"
                                    value={formData.destiny}
                                    onChange={handleChange}
                                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                                    required
                                >
                                    <option value="storage">Storage</option>
                                    <option value="disposal">Disposal</option>
                                    <option value="transfer">
                                        Transfer to another Unit
                                    </option>
                                </select>
                            </div>

                            <div>
                                <label
                                    htmlFor="receptorId"
                                    className="block text-sm font-medium text-gray-700"
                                >
                                    Receptor
                                </label>
                                <div className="relative">
                                    <input
                                        type="text"
                                        id="receptorId"
                                        name="receptorId"
                                        value={formData.receptorName}
                                        onClick={handleShowROpen}
                                        placeholder="Enter receptor"
                                        className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm pr-10"
                                        required
                                        readOnly
                                    />
                                    <ChevronDownIcon
                                        className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none"
                                        style={{
                                            width: "2rem",
                                            height: "2rem",
                                            color: "gray",
                                        }}
                                    />
                                </div>
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

                {/* Modal de selección de equipo */}
                <Dialog open={showReceptors} handler={() => handleShowROpen()}>
                    <DialogHeader>Select Receptor</DialogHeader>
                    <DialogBody>
                        <Input
                            type="text"
                            placeholder="Search Receptor"
                            value={searchQuery}
                            onChange={(e) => handleSearch(e, "receptor")}
                            className="mb-4 w-full"
                        />
                        <div className="max-h-72 overflow-y-auto mt-3">
                            {filteredReceptor.map((receptor) => (
                                <div
                                    key={receptor.id}
                                    className="p-2 cursor-pointer hover:bg-gray-100"
                                    onClick={() =>
                                        handleReceptorSelect(
                                            receptor.id,
                                            receptor.name,
                                        )
                                    }
                                >
                                    {receptor.name} - @{receptor.userName}
                                </div>
                            ))}
                        </div>
                    </DialogBody>
                    <DialogFooter>
                        <Button
                            onClick={() => handleShowRClose()}
                            variant="outlined"
                        >
                            Close
                        </Button>
                    </DialogFooter>
                </Dialog>

                <div></div>
            </div>
        </>
    );
};

export default LeaveCreationForm;
