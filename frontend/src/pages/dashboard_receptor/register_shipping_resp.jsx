import React, { useState, useEffect } from 'react';
import { Card, CardBody, Typography, Button, Input } from "@material-tailwind/react";
import api from "@/middlewares/api";

const RegisterForm = ({ onAccept, onCancel }) => {
    const [assignedPerson, setAssignedPerson] = useState("");
    const [error, setError] = useState("");
    const [responsibles, setResponsibles] = useState([]);
    const [isExpanded, setIsExpanded] = useState(false);


    useEffect(() => {
        fetchResponsibles();
    }, []);  

    const fetchResponsibles = async () => {
        try {
            const response = await api('/Employee/GetAllShippingSupervisor', {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            console.log("Responsibles data", data);
            setResponsibles(data);
        } catch (error) {
            console.error("Error fetching responsibles:", error);
        }
    };

    const handleAccept = () => {
        if (assignedPerson.trim() === "") {
            setError("This field is required.");
        } else {
            setError("");
            onAccept(assignedPerson);
        }
    };



    return (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
            <Card className={`transition-all duration-300 ${isExpanded ? 'w-96' : 'w-96'}`}>
                <CardBody>
                    <Typography variant="h5" className="mb-4">
                        Shipping Personnel Responsible
                    </Typography>
                    <div className="relative">
                        <select
                            value={assignedPerson}
                            onChange={(e) => setAssignedPerson(e.target.value)}
                            onFocus={() => setIsExpanded(true)}
                            onBlur={() => setIsExpanded(false)}
                            className={`mb-4 ${error ? 'border-red-500' : ''} w-full`}
                            required
                            size={isExpanded ? 8 : 1}
                            style={{ overflowY: 'auto' }}
                        >
                            <option value="" disabled>Select a responsible</option>
                            {responsibles.map((responsible) => (
                                <option key={responsible.id} value={responsible.id}>
                                    {responsible.name}
                                </option>
                            ))}
                        </select>
                    </div>
                    {error && (
                        <Typography variant="small" color="red" className="mt-1">
                            {error}
                        </Typography>
                    )}
                    <div className="flex justify-end gap-3 mt-4">
                        <Button onClick={onCancel} color="gray">
                            Cancel
                        </Button>
                        <Button onClick={handleAccept} color="green">
                            Accept
                        </Button>
                    </div>
                </CardBody>
            </Card>
        </div>
    );
};

export default RegisterForm;