import React, { useState, useEffect } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
} from "@material-tailwind/react";
import MessageAlert from "@/components/Alert_mssg/alert_mssg";
import api from "@/middlewares/api";


const EditSectionForm = ({ sectionData, onSave, onCancel }) => {
  const [isLoading, setIsLoading] = useState(true);
  const[alertMessage, setAlertMessage] = useState('');
  const [alertType, setAlertType] = useState('success');

  const [formData, setFormData] = useState({
    id: sectionData ? sectionData.id : "",
    name: sectionData ? sectionData.name : "",
  });

  useEffect(() => {
      if (sectionData) {
          setFormData(sectionData);
      }
    }, [sectionData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
        const response = await api(`/Section/PUT`, {
          method: 'PUT',
          body: JSON.stringify({
            'id': formData.id,
            'name': formData.name,
            'sectionManagerId' : formData.sectionManager
          })
        });

        console.log(formData.sectionManager);
        console.log(formData.id);

    if (!response.ok) {
        setAlertMessage('Failed to edit employee');
        setAlertType('error');
        throw new Error('Failed to edit employee');
    }
    else
    {
        setAlertMessage('Edit completed successfully');
        setAlertType('success');
        onSave(formData);
    }
    } catch (error) {
        setAlertMessage('Error editing employee:');
        setAlertType('error');
        console.log(error);
    }
};

return (
  <>
    <MessageAlert message={alertMessage} type={alertType} onClose={() => setAlertMessage('')} />

        <div className="max-w-3xl mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
            <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                    <Typography variant="h6" color="white">
                        Edit Section
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
  </>
    );
};

export default EditSectionForm;