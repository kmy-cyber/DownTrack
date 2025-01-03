import React from 'react';
import { CardBody, Typography, Button } from "@material-tailwind/react";

const DisposalInfoForm = ({ disposal, onClose  }) => {
    
    return (
        <div className="p-6">
            <CardBody>
                <Typography variant="h5" className="mb-4">
                    Low-Technical Suggestion Information
                </Typography>
                <div className="space-y-4">
                    <div className="p-4 border rounded-md shadow-sm">
                        <Typography variant="h6" className="font-semibold">
                            {disposal.equipment}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Technician: {disposal.technician}
                        <Typography variant="body2" className="text-gray-600">
                            Date: {disposal.date}
                        </Typography>
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Reason for Removal: {disposal.reasonForRemoval}
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
