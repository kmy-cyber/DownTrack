import React, { useState } from 'react';
import { Card, CardBody, Typography, Button, Input } from "@material-tailwind/react";

const RegisterForm = ({ onAccept, onCancel }) => {
    const [assignedPerson, setAssignedPerson] = useState("");

    const handleAccept = () => {
        if (assignedPerson.trim() !== "") {
            onAccept(assignedPerson);
        } else {
            alert("Please enter the shipping personnel responsible.");
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
                        placeholder=" Enter shipping responsible ID"
                        value={assignedPerson}
                        onChange={(e) => setAssignedPerson(e.target.value)}
                        className="mb-4"
                    />
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