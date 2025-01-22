import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
import api from "@/middlewares/api";

export const UserCreationForm = () => {
    const [alertMessage, setAlertMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [alertType, setAlertType] = useState('success');

    const [formData, setFormData] = useState({
        name: "",
        username: "",
        role: "Administrator",
        department: 0,
        experience: "",
        specialty: "",
        salary: 0,
        password: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    let globalId = localStorage.getItem("globalId")?parseInt(localStorage.getItem("globalId")):192;
    
    const handleSubmit = async (e) => {
            console.log("Register ");
            e.preventDefault(); // Previene la recarga de la página
            setIsLoading(true);
            setAlertMessage(null);
            try {
                const response = await api("/Authentication/register/", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        //"Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxODkiLCJnaXZlbl9uYW1lIjoicGVkcm9fc2FuY2hlcyIsImp0aSI6IjFlMDYxMDVmLWNhYzQtNDU2ZC1iMTAxLTRjMzM1MTYyOTlhYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJleHAiOjE3Mzc0NzQ2MDgsImlzcyI6IkRvd25UcmFjayIsImF1ZCI6IkRvd25UcmFjayJ9.MSHvcGnczsqz1HuaHRqvlsSjE7-LyZQjRSCVWEe_kp4"
                    },
                    
                    body: JSON.stringify({
                        id: globalId,
                        name: formData.name,
                        userName: formData.username,
                        email: formData.email,
                        password: formData.password,
                        userRole: formData.role,
                        specialty: formData.specialty,
                        salary: parseFloat(formData.salary),
                        expYears: formData.expYears,
                        departamentId: parseInt(formData.department),
                        sectionId: formData.sectionId
                    }),
                });
                
                console.log(globalId);
                globalId++;
                localStorage.setItem("globalId",globalId);
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
                    Register Employee
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


                    {formData.role !== "ShippingSupervisor" ? (
                    <>
                    <div>
                    <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                        Username
                    </label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        placeholder="Enter username to assign"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    />
                    </div>

                    <div>
                    <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                        Password
                    </label>
                    <input
                        type="text"
                        id="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        placeholder="Enter password to assign"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    />
                    </div>

                
                    </>
                    ) : null}
                
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
                            placeholder="Enter email"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        />
                    </div>
                    <div>
                    <label htmlFor="role" className="block text-sm font-medium text-gray-700">
                        Role
                    </label>
                    <select
                        id="role"
                        name="role"
                        value={formData.role}
                        onChange={handleChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        required
                    >
                        <option value="Administrator">Administrator</option>
                        <option value="SectionManager">Section Manager</option>
                        <option value="Technician">Technician</option>
                        <option value="EquipmentReceptor">Equipment Receptor</option>
                        <option value="Director">Center Director</option>
                        <option value="ShippingSupervisor">Shipping Supervisor</option>
                    </select>
                    </div>


                    {formData.role === "EquipmentReceptor" ? (
                        <div>
                            <label htmlFor="section" className="block text-sm font-medium text-gray-700">
                            Section
                            </label>
                            <input
                            type="text"
                            id="section"
                            name="section"
                            value={formData.section}
                            onChange={handleChange}
                            placeholder="Enter section"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                            />
                        </div>
                    ) : null}

                    {formData.role === "EquipmentReceptor" ? (
                        <div>
                            <label htmlFor="department" className="block text-sm font-medium text-gray-700">
                            Department
                            </label>
                            <input
                            type="number"
                            id="department"
                            name="department"
                            value={formData.section}
                            onChange={handleChange}
                            placeholder="Enter department"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                            />
                        </div>
                    ) : null}

                    {formData.role === "Technician" ? (
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
                    disabled={isLoading}
                >
                    {isLoading ? 'Submit' : ' Accept'}
                </button>
                </form>
            </CardBody>
        </div>

    </>
    );
};

export default UserCreationForm;
