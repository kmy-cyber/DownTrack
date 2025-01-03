import React from 'react';
import { CardBody, Typography, Button } from "@material-tailwind/react";

const TransferInfoForm = ({ transfer, onClose  }) => {
    return (
        <div className="p-6">
            <CardBody>
                <Typography variant="h5" className="mb-4">
                    Transfer Information
                </Typography>
                <div className="space-y-4">
                    <div className="p-4 border rounded-md shadow-sm">
                        <Typography variant="h6" className="font-semibold">
                            {transfer.equipment}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Source Section: {transfer.sourceSection}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Section Boss: {transfer.requestingOfficer}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Department: {transfer.department}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Date: {transfer.date}
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

export default TransferInfoForm;
