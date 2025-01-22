import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,
} from "@material-tailwind/react";
import api from "@/middlewares/api";

export const EditUserForm = ({ userData, onSave, onCancel }) => {
    const [formData, setFormData] = useState({
        id: "",
        name: "",
        role: "",
        email:"",
        password: "",
        salary: 0,
        specialty: "",
        expYears: 0,
        departamentId: 0
    });

    useEffect(() => {
        if (userData) {
            setFormData(userData);
        }
    }, [userData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    }; 

    const handleSubmit = async (e) => {
        e.preventDefault();

    try {
        console.log("HERE",)
        console.log(formData)
        const response = await api(`/Authentication/PUT`, {
        method: 'PUT',
        body: JSON.stringify({
            'id': formData.id,
            'name': formData.name,
            'email': formData.email,
            'password': formData.password,
            'userRole': formData.role,
            'specialty': formData.specialty,
            'salary': formData.salary,
            'expYears': formData.expYears,
            'departamentId': formData.departamentId
        })
        });
        if (!response.ok) {
            setAlertMessage('Failed to edit user');
            setAlertType('error');
            throw new Error('Failed to edit user');
        }
        else
        {
            setAlertMessage('Edit completed successfully');
            setAlertType('success');
            onSave(formData);
        }
    } catch (error) {
        setAlertMessage('Error editing user:');
        setAlertType('error');
        console.log(error);
    }
    };

    return (
        <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
            <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Edit Employee
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
                    placeholder="Enter full name"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                    />
                </div>

                <div>
                    <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                    Email
                    </label>
                    <input
                    type="text"
                    id="email"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    placeholder="Enter identification number"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required

                    />
                </div>

                <div>
                    <label htmlFor="role" className="block text-sm font-medium text-gray-700">
                    Role
                    </label>
                    <input
                    id="role"
                    name="role"
                    value={formData.userRole}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    required
                    disabled
                    readOnly
                    />
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
                        value={formData.departamentId}
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
                        value={formData.expYears}
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
                        placeholder="Salary"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        />
                    </div>
                    </>
                ) : null}
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

export default EditUserForm;
