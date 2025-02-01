import React from 'react';
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { Pagination } from '@mui/material';
import { useState, useEffect } from "react";
import api from "@/middlewares/api";
import { WrenchIcon } from '@heroicons/react/24/solid';
import DropdownMenu from "@/components/DropdownMenu";
import { ArrowDownwardSharp } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';

export function EquipmentInventory() {
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [isLoading, setIsLoading] = useState(true);
    const [currentItems, setCurrentItems] = useState([]);
    const navigate = useNavigate();

    // Inicializa los valores. Es lo que se ejecuta al cargar el componente
    useEffect(()=>{
        fetchEquipments(1);
    }, []);

    const options =(id, name, type) => [
        { 
            label: 'Do Maintenance', 
            className: 'text-gray-500 h-5 w-5', 
            icon: WrenchIcon,
            action: () => {
                navigate(`/dashboard/technic/insert_maintenance/${id}/${name}/${type}`);
            }
        },
        { 
            label: 'Decommission', 
            className: 'text-red-500 h-5 w-5', 
            icon: ArrowDownwardSharp,
            action: () => {
                navigate(`/dashboard/technic/insert_technical_leave/${id}/${name}/${type}`);
            }
        },
    ];

    // funcion que se llama cada vez que se cambia de pagina
    const handlePageChange = async (event, newPage) => {
        setCurrentPage(newPage);
        await fetchEquipments(newPage);
    };

    const fetchEquipments = async (page) => {
        try {
            const response = await api(`/Equipment/active equipment?PageNumber=${page}&PageSize=10`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            
            setCurrentItems(data.items);
            setTotalPages(Math.ceil(data.totalCount / data.pageSize));

            setIsLoading(false);
        } catch (error) {
            console.error("Error fetching inventory:", error);
            setCurrentItems([]);
            setIsLoading(false);
        }
    };

    return (
        <>
            {(<div className={`mt-12 mb-8 flex flex-col gap-12 `}>
                <Card>
                    <CardHeader variant="gradient" color="gray" className="mb-8 p-6">
                        <Typography variant="h6" color="white">
                            Equipment Inventory
                        </Typography>
                    </CardHeader>
                    <CardBody className="overflow-x-scroll px-0 pt-0 pb-2">
                        <table className="w-full min-w-[640px] table-auto">
                            <thead>
                                <tr>
                                    {["ID", "Equipment", "type",""].map((el) => (
                                        <th
                                            key={el}
                                            className="border-b border-r border-blue-gray-50 py-3 px-5 text-left last:border-r-0 bg-gray-800"
                                        >
                                            <Typography
                                                variant="small"
                                                className="text-[11px] font-extrabold uppercase text-white"
                                            >
                                                {el}
                                            </Typography>
                                        </th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody >
                                {currentItems.map(
                                    (equipment, index) => {
                                        const className = `py-3 px-5 ${index === currentItems.length - 1
                                                ? ""
                                                : "border-b border-blue-gray-50"
                                            }`;
                                        return (
                                            <tr key={equipment.id} >
                                                <td className="py-3 px-5 border-b border-r border-blue-gray-50 last:border-r-0">
                                                    <div className="flex items-center gap-4">
                                                        <div>
                                                            <Typography className="text-xs font-semibold text-blue-gray-600">
                                                                {equipment.id}
                                                            </Typography>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="py-3 px-5 border-b border-r border-blue-gray-50 last:border-r-0">
                                                    <div className="flex items-center gap-4">
                                                        <div>
                                                            <Typography
                                                                variant="small"
                                                                color="blue-gray"
                                                                className="font-semibold"
                                                            >
                                                                {equipment.name}
                                                            </Typography>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className={className}>
                                                    <Typography className="text-xs font-semibold text-blue-gray-600">
                                                        {equipment.type}
                                                    </Typography>
                                                </td>
                                                <td className={className + "items-center text-center"}>
                                                    <DropdownMenu options={options(equipment.id, equipment.name, equipment.type)} />
                                                </td>
                                            </tr>
                                        );
                                    }
                                )}
                            </tbody>
                        </table>
                    </CardBody>
                </Card>
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    className="self-center"
                />
            </div>)
            }
        </>
    );
}

export default EquipmentInventory;
