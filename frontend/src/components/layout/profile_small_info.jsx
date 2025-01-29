import React from "react";
import {
    useMaterialTailwindController,
    setOpenUserInfo,
} from "@/context";
import { Button } from "@material-tailwind/react";
import { ArrowRightOnRectangleIcon } from "@heroicons/react/24/solid";
import { useAuth } from "@/context/AuthContext";

export const UserInfoSidebar = ({ id, name, role }) => {
    const [controller, dispatch] = useMaterialTailwindController();
    const { openUserInfo } = controller;
    const { logout } = useAuth();

    return (
        <div
            className={`fixed top-5 right-0 z-50 w-72 bg-white shadow-lg rounded-l-lg border border-gray-200 transform ${
            openUserInfo ? "translate-x-0" : "translate-x-full"
            } transition-transform duration-300 ease-in-out`}
        >
            {/* Header */}
            <div className="flex justify-between items-center p-4 border-b border-gray-200 bg-clip-border mt-4 mx-4 rounded-xl overflow-hidden bg-gradient-to-tr from-gray-900 to-gray-800 text-white shadow-gray-900/20">
            <h2 className="text-sm font-medium text-white-800">User Info</h2>
            <button
                className="text-gray-500 hover:text-gray-700 focus:outline-none"
                onClick={() => setOpenUserInfo(dispatch, false)}
            >
                <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={2}
                stroke="currentColor"
                className="w-5 h-5"
                >
                <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
                </svg>
            </button>
            </div>
        
            {/* User Info */}
            <div className="p-4 space-y-4">
            {/* User ID */}
            <div className="flex items-center">
                <label className="text-sm font-medium text-gray-700">ID:</label>
                <span className="ml-2 text-sm text-gray-800">{id}</span>
            </div>
            {/* User Name */}
            <div className="flex items-center">
                <label className="text-sm font-medium text-gray-700">Name:</label>
                <span className="ml-2 text-sm text-gray-800">{name}</span>
            </div>
            {/* User Role */}
            <div className="flex items-center">
                <label className="text-sm font-medium text-gray-700">Role:</label>
                <span className="ml-2 text-sm text-gray-800">{role}</span>
            </div>
            <hr />
            <button
                className="w-full hover:bg-gray-100 rounded p-2 flex items-center justify-start"
                onClick={() => logout()}
            >
                <ArrowRightOnRectangleIcon className="w-5 h-5 text-gray-500" />
                <p className="ml-5">Logout</p>
            </button>

            </div>
        </div>
    );
};

export default UserInfoSidebar;
