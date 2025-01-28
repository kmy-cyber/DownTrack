import React from 'react';
import { Popover, PopoverHandler, PopoverContent, Tooltip } from '@material-tailwind/react';
import { EllipsisVerticalIcon } from '@heroicons/react/24/outline';

const DropdownMenu = ({ options }) => {
    return (
        <Popover placement="left">
        <PopoverHandler>
            <button className="inline-flex justify-center items-center w-10 h-10 rounded-full border shadow-sm  font-medium text-gray-700 hover:bg-gray-50 focus:outline-none">
                <EllipsisVerticalIcon className="h-5 w-5" />
            </button>
        </PopoverHandler>
        <PopoverContent className="flex space-x-2 p-2 bg-transparent shadow-none border border-transparent">
            {options.map((option, index) => (
                <Tooltip key={index} content={option.label} placement="top">
                <button
                    onClick={option.action}
                    className="flex items-center justify-center w-8 h-8 bg-white border border-gray-300 rounded-full shadow-sm hover:bg-gray-100 focus:outline-none"
                    role="menuitem"
                >
                    <option.icon className={option.className} />
                </button>
                </Tooltip>
            ))}
        </PopoverContent>
        </Popover>
    );
};

export default DropdownMenu;