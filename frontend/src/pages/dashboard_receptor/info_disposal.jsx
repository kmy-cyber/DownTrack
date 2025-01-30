import React from 'react';
import { CardBody, Typography, Button } from "@material-tailwind/react";
import {InformationCircleIcon} from "@heroicons/react/24/outline";
const DisposalInfoForm = ({ disposal, onClose  }) => {
    
    return (
        <div className="p-6 ">
            <CardBody>
                <div className="flex items-center justify-center">
                    <InformationCircleIcon className="h-20 w-120 text-blue-800" />
                </div>
                <Typography variant="h4" className="mb-4 flex items-center justify-center">
                    Low-Technical Suggestion Information
                </Typography>
                <div className="space-y-4">
                    <div className="p-4 border rounded-md shadow-sm">
                        <Typography variant="h5" className="font-semibold">
                            {disposal.equipmentName}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Type: {disposal.equipmentType}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Status: {disposal.status}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Technician: {disposal.technicianUserName}
                        <Typography variant="body2" className="text-gray-600">
                            Date: {disposal.date}
                        </Typography>
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Reason for Removal: {disposal.cause}
                        </Typography>
                    </div>
                </div>
                <Button onClick={onClose } className="mt-4">
                    Cancel
                </Button>
            </CardBody>
        </div>
    );
};

export default DisposalInfoForm;
