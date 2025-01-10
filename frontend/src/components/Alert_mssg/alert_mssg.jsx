import React, { useEffect, useState } from 'react';
import { Typography, Button } from "@material-tailwind/react";

const MessageAlert = ({ message, type = "success", onClose }) => {
const [isVisible, setIsVisible] = useState(false);

useEffect(() => {
    if (message) {
        setIsVisible(true);
        setTimeout(() => {
            setIsVisible(false);
        }, 3000); // Hide the alert after 1 second
    }
}, [message]);

const getBackgroundColor = () => {
    switch(type) {
        case "success":
            return "bg-green-100";
        case "error":
            return "bg-red-100";
        case "warning":
            return "bg-yellow-100";
        default:
            return "bg-blue-100";
    }
};

const getTextColor = () => {
    switch(type) {
        case "success":
            return "text-green-600";
        case "error":
            return "text-red-600";
        case "warning":
            return "text-yellow-600";
        default:
        return "text-blue-600";
    }
};

const getMessageColor = () => {
    switch(type) {
        case "success":
            return "text-green-600";
        case "error":
            return "text-red-600";
        case "warning":
            return "text-yellow-600";
        default:
            return "text-blue-600";
    }
};

return (
    isVisible && (
        <div    className="fixed top-4 left-1/2 transform -translate-x-1/2 z-50">
            <div  className={`bg-white p-4 rounded-lg shadow-md w-96 ${getBackgroundColor()} ring-1 ring-gray-200`}>
                <div className="flex justify-between items-center">
                    <Typography variant="small" className={getTextColor()}>
                        {type === "success" ? "Success" : type === "error" ? "Error" : type === "warning" ? "Warning" : "Message"}
                    </Typography>
                </div>
                <Typography variant="small" className="mt-2">
                    {message}
                </Typography>
            </div>
        </div>
    )
);
};

export default MessageAlert;
