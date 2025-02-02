import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    Typography,

} from "@material-tailwind/react";
import api from "@/middlewares/api";
import { Refresh } from "@mui/icons-material";
import { toast } from "react-toastify";

export const SectionCreationForm = ({ sectionData=null, onSave=() => {}, onCancel=() => {}, formType="create" }) => {
    const [isLoading, setIsLoading] = useState(false);

    const [userManagerList, setUserManagerList] = useState([]);
    const [onEdit, setOnEdit] = useState(false);


    const [formData, setFormData] = useState(
        sectionData ? {
            id: sectionData ? sectionData.id : "",
            name: sectionData ? sectionData.name : "",
            sectionManagerId: sectionData ? sectionData.sectionManagerId : "",
            sectionManagerUserName: sectionData ? sectionData.sectionManagerUserName : "",
        } : {
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

            if (formType === 'create') {
                setFormData((prev) => ({ ...prev, ['usernameSectionM']: filteredData[0].id }));
            } else {
                setFormData((prev) => ({ ...prev, ['usernameSectionM']: sectionData.sectionManagerId }));
            }

            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching sections:", error);
            setUserManagerList([]);
            setIsLoading(false);
        }
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault();

        if (formType === "create") {
            await handleCreate();
        } else {
            await handleEdit();
        }
    };

    const handleCreate = async () => {
        console.log("Section created:", formData);
        
        setIsLoading(true);
        try {
            const response = await api("/Section/POST", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: formData.name,
                    sectionManagerId: formData.usernameSectionM,
                }),
            });
            
            if (response.ok) {
                toast.success("Successful registration")
                //setAlertMessage("Successful registration");
                setFormData({ id: "", name: "", usernameSectionM: formData.usernameSectionM });
            } else {
                toast.error("Failed to login");
            }
        } catch (error) {
            toast.error('An error occurred during the login process');
        } finally {
            setIsLoading(false);
        }
    };
    
    const handleEdit = async () => {    
        console.log("Section edited:", formData);

        try {
            const response = await api(`/Section/PUT`, {
                method: 'PUT',
                body: JSON.stringify({
                    'id': formData.id,
                    'name': formData.name,
                    'sectionManagerId' : formData.sectionManagerId
                })
            });
    
        if (!response.ok) {
            toast.error('Failed to edit section');
            throw new Error('Failed to edit section');
        }
        else
        {
            toast.success('Edit completed successfully');
            onSave(formData);
        }
        } catch (error) {
            toast.error('Error editing section:');
        }
    };

    return (
        <>
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
                        <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                            Section Manager Username
                        </label>
                        <select
                            id="usernameSectionM"
                            name="usernameSectionM"
                            value={
                                formType === 'create' 
                                    ? formData.usernameSectionM
                                    : formData.sectionManagerId
                            }
                            onChange={(e) => {
                                const { name, value } = e.target;

                                if (formType === 'create') {
                                    setFormData((prev) => ({ ...prev, [name]: value }));
                                } else {
                                    const currentSectionM = userManagerList.find((s) => s.id === parseInt(value));

                                    setFormData((prev) => ({ 
                                        ...prev, 
                                        ["sectionManagerId"]: value,
                                        ["sectionManagerUserName"]: currentSectionM.userName,
                                    }));
                                }
                            }}
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
                    <div className="flex gap-4">
                        <button
                            type="submit"
                            className="mt-6 w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                            disabled={isLoading}
                        >
                            {isLoading ? 'Submit' : ' Accept'}
                        </button>
                        {formType === 'edit' && (
                            <button
                                type="button"
                                onClick={() => onCancel()}
                                className="mt-6 w-full bg-gray-600 text-white py-2 px-4 rounded-md hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
                                >
                            Cancel
                            </button>
                        )}
                    </div>
                    </form>
                </CardBody>
            </div>
        </>
    );
};

export default SectionCreationForm;
