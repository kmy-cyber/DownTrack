import React, { useState } from 'react';
import { Card, CardBody, Typography, Button, Input } from "@material-tailwind/react";

const RegisterForm = ({ onAccept, onCancel }) => {
    const [assignedPerson, setAssignedPerson] = useState("");
    const [error, setError] = useState("");

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
            <Card className="w-96">
                <CardBody>
                    <Typography variant="h5" className="mb-4">
                        Shipping Personnel Responsible
                    </Typography>
                    <Input
                        type="text"
                        value={assignedPerson}
                        onChange={(e) => setAssignedPerson(e.target.value)}
                        className={`mb-4 ${error ? 'border-red-500' : ''}`}
                        required
                    />
                    {error && (
                        <Typography variant="small" color="red" className="mt-1">
                            {error}
                        </Typography>
                    )}
                    <div className="flex justify-end gap-3 mt-4">
                        <Button onClick={onCancel} color="gray">
                            Cancelar
                        </Button>
                        <Button onClick={handleAccept} color="green">
                            Aceptar
                        </Button>
                    </div>
                </CardBody>
            </Card>
        </div>
    );
};

export default RegisterForm;
