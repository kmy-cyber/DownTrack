import React from 'react';
import { CardBody, Typography, Button } from "@material-tailwind/react";
import {InformationCircleIcon} from "@heroicons/react/24/outline";

const TransferInfoForm = ({ transfer, onClose  }) => {
    return (
        <div className="p-6">
            <CardBody>
                <div className="flex items-center justify-center">
                    <InformationCircleIcon className="h-20 w-120 text-blue-800" />
                </div>
                <Typography variant="h5" className="mb-4 flex items-center justify-center">
                    Transfer Information
                </Typography>
                <div className="space-y-4">
                    <div className="p-4 border rounded-md shadow-sm">
                        <Typography variant="h6" className="font-semibold">
                            {transfer.equipment.name}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Source Section: {transfer.equipment.sectionName}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Section Boss: {transfer.requestingOfficer}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Department: {transfer.equipment.departmentName}
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
