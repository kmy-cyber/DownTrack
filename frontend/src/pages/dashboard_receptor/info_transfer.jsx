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
                <Typography variant="h4" className="mb-4 flex items-center justify-center">
                    Transfer Information
                </Typography>
                <div className="space-y-4">
                    <div className="p-4 border rounded-md shadow-sm">
                        <Typography variant="h5" className="font-semibold">
                            {transfer.equipmentName}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Equipment Type: {transfer.equipmentType}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Equipment Status: {transfer.equipmentStatus}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Source Section: {transfer.requestSectionName}
                        </Typography>
                        <Typography variant="body2" className="text-gray-600">
                            Source Department: {transfer.requestDepartmentName}
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
