import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,
    } from "@material-tailwind/react";

export const EditMaintenanceForm = ({ maintenanceData, onSave, onCancel }) => {
    const [formData, setFormData] = useState({
        equipment: "",
        maintenanceType: "",
        technician: "",
        date: "",
        cost: "",
    });

    useEffect(() => {
        if (maintenanceData) {
            setFormData(maintenanceData);
        }
    }, [maintenanceData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
        console.log("Updated user:", formData);
    };

    return (
        <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
            <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Edit MaintenanceData
                    </Typography>
            </CardHeader>
        <CardBody>
            <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* General Fields */}
                <div>
                    <label htmlFor="equipment" className="block text-sm font-medium text-gray-700">
                    Equipment
                    </label>
                    <input
                    type="text"
                    id="equipment"
                    name="equipment"
                    value={formData.equipment}
                    onChange={handleChange}
                    placeholder="Enter equipment's name"
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
                    value={formData.maintenanceType}
                    onChange={handleChange}
                    placeholder="Enter maintenance's type"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required

                    />
                </div>

                <div>
                <label htmlFor="technician" className="block text-sm font-medium text-gray-700">
                    Technician
                </label>
                <input
                    type="text"
                    id="technician"
                    name="technician"
                    value={formData.technician}
                    onChange={handleChange}
                    placeholder="Enter technician"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                <div>
                <label htmlFor="date" className="block text-sm font-medium text-gray-700">
                    Date
                </label>
                <input
                    type="date"
                    id="date"
                    name="date"
                    value={formData.date}
                    onChange={handleChange}
                    placeholder="Enter date"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                <div>
                <label htmlFor="cost" className="block text-sm font-medium text-gray-700">
                    Cost
                </label>
                <input
                    type="number"
                    id="cost"
                    name="cost"
                    value={formData.cost}
                    onChange={handleChange}
                    placeholder="Enter cost"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                />
                </div>

                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <button
                        type="submit"
                        className="mt-6 w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    >
                    Save Changes
                    </button>
                    <button
                        type="button"
                        onClick={() => onCancel()}
                        className="mt-6 w-full bg-gray-600 text-white py-2 px-4 rounded-md hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
                    >
                    Cancel
                    </button>
                </div>
            </form>
        </CardBody>
        </div>
    );
};

export default EditMaintenanceForm;
