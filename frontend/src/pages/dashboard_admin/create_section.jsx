import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
import api from "@/middlewares/api";

export const SectionCreationForm = () => {
    const [alertMessage, setAlertMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [alertType, setAlertType] = useState('success');

    const [userManagerList, setUserManagerList] = useState([]);
    const [formData, setFormData] = useState({
        name: "",
        usernameSectionM: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    useEffect(() => {
            setIsLoading(true);
            fetchSectionManager();
            setIsLoading(false);
        }, []);
        

    const fetchSectionManager = async () => {
        try {
            const response = await api('/Employee/GET_ALL', {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            const filteredData = data.filter(employee => employee.userRole === 'SectionManager');
            setUserManagerList(filteredData);
            if(filteredData.length === 0){
                navigate('/dashboard/admin/add_employee');
            }
            setFormData((prev) => ({ ...prev, ['usernameSectionM']: filteredData[0].id }));
            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching sections:", error);
            setUserManagerList([]);
            setIsLoading(false);
        }
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Section created:", formData);

        setIsLoading(true);
        setAlertMessage(null);
        try {
            const response = await api("/Section/POST", {
                method: "POST",
                body: JSON.stringify({
                    name: formData.name,
                    sectionManagerId: formData.usernameSectionM,
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
                setFormData({ id: "", name: "", usernameSectionM: "" });
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
                        Register Section
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
                        <label htmlFor="usernaname" className="block text-sm font-medium text-gray-700">
                            Section Manager Username
                        </label>
                        <select
                            id="usernameSectionM"
                            name="usernameSectionM"
                            value={formData.usernameSectionM}
                            onChange={handleChange}
                            placeholder="Enter Section Manager Username"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                            required
                        >
                                {userManagerList.map(sm => (
                                <option key={sm.id} value={sm.id}>{sm.name}</option>
                        ))}
                        </select>
                        </div>
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

export default SectionCreationForm;
