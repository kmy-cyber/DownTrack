import React from "react";

const UserInfo = ({ id, name, role, onLogout }) => {
    return (
        <div className="relative bg-white rounded-lg shadow-lg w-80 p-5">
        {/* Close Button */}
        <button className="absolute top-2 right-2 text-gray-500 hover:text-gray-700">
            <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={2}
            stroke="currentColor"
            className="w-6 h-6"
            >
            <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M6 18L18 6M6 6l12 12"
            />
            </svg>
        </button>

        {/* User Info */}
        <div className="space-y-4">
            <div>
            <label className="block text-sm font-medium text-gray-700">ID:</label>
            <input
                type="text"
                value={id}
                readOnly
                className="w-full mt-1 px-3 py-2 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
            </div>
            <div>
            <label className="block text-sm font-medium text-gray-700">Name:</label>
            <input
                type="text"
                value={name}
                readOnly
                className="w-full mt-1 px-3 py-2 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
            </div>
            <div>
            <label className="block text-sm font-medium text-gray-700">Role:</label>
            <input
                type="text"
                value={role}
                readOnly
                className="w-full mt-1 px-3 py-2 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
            </div>
        </div>

        {/* Logout Button */}
        <button
            onClick={onLogout}
            className="mt-6 w-full bg-blue-500 text-white font-medium py-2 rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
            Logout
        </button>
        </div>
    );
};

export default UserInfo;
