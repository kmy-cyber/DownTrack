import React, { useEffect, useState } from "react";
import {
    Typography,
    Card,
    CardHeader,
    CardBody,
} from "@material-tailwind/react";

import { StatisticsCard } from "@/components/cards";
import { StatisticsChart } from "@/components/charts";
import PieChartComponent from "@/components/charts/simplePieChart";
import { chartsConfig } from "@/configs";
import { ArrowDownCircleIcon, CheckCircleIcon, ClockIcon, WrenchIcon } from "@heroicons/react/24/solid";
import { ArrowDownwardSharp, HourglassBottomRounded } from '@mui/icons-material';
import { useAuth } from "@/context/AuthContext";
import api from "@/middlewares/api";


export function Home() {

    const [currentItems, setCurrentItems] = useState({});
    const [dataMaintenance, setDataMaintenance] = useState({months: [], values: []});
    const [dataDecommissions, setDataDecommissions] = useState({months: [], values: []});
    const [dataEquipments, setDataEquipments] = useState([]);

    useEffect(()=>{
        fetchStatistics();
    },[])

    const dataEquipment = [
        { name: 'Active', value: 400 , color: '#34495E'},
        { name: 'Under Maintenance', value: 500, color:'#5D8AA8'  },
        { name: 'Inactive', value: 300 , color:'#4A6FA5' },
    ];

    const {user} = useAuth();

    const fetchStatistics = async () => {
        try {
            const response = await api(`/Statistics/Technician?technicianId=${user.id}`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            
            setCurrentItems(data);
            initDataE(data.equipmentByStatus);
            setDataMaintenance(transformData(data.maintenanceByMonth));
            setDataDecommissions(transformData(data.decomissionsByMonth));

        } catch (error) {
            console.error("Error fetching departments:", error);
            setCurrentItems({});
        }
    };

    const transformData = (data) => {
        const months = [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
    
        const combinedData = {
            months: [],
            values: []
        };
    
        for (const [key, value] of Object.entries(data)) {
            const [month, year] = key.split('-');
            const monthName = months[parseInt(month) - 1];
            const name = `${monthName}-${year.substring(2)}`;
        
            combinedData.months.push(name);
            combinedData.values.push(value);
        }
    
        return combinedData;
    }

    const initDataE = (equipmentByStatus) => {
        setDataEquipments([
            { name: 'Active', value: equipmentByStatus.Active , color: '#34495E'},
            { name: 'Inactive', value: equipmentByStatus.Inactive, color:'#5D8AA8'  },
            { name: 'Under Maintenance', value: equipmentByStatus.UnderMaintenance ,color:'#4A6FA5' },  
        ]);
    }

    

    return (
        <div className="mt-12">
        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">

            <StatisticsCard
                color= "gray" 
                key= "Maintenance"
                title= "Maintenance"
                value = {currentItems.maintenances ?? 0}
                icon={React.createElement(WrenchIcon, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{" total of completed maintenance"}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "MaintenanceProgress"
                title= "Maintenances in progress"
                value = {currentItems.maintenancesInProgress ?? 0}
                icon={React.createElement(HourglassBottomRounded, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total of maintenance in progress"}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "Decommissions"
                title= "Decommissions"
                value = {currentItems.decomissions ?? 0}
                icon={React.createElement(ArrowDownCircleIcon, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total of proposed decommissions"}
                </Typography>
                }
            />


        </div>
        <div className="mb-6 grid grid-cols-1 gap-y-12 gap-x-6 md:grid-cols-2 xl:grid-cols-3">

            <StatisticsChart
                key ="MaintenanceMonth"
                color ="white"
                title = "Completed maintenance by month"
                description = "This metric tracks the number of maintenance tasks completed each month."
                chart = { 
                    {
                    type: "line",
                    height: 330,
                    series: [
                        {
                        name: "",
                        data: dataMaintenance.values,
                        },
                    ],
                    options: {
                        ...chartsConfig,
                        colors: ["#5D8AA8"],
                        stroke: {
                        lineCap: "round",
                        },
                        markers: {
                        size: 5,
                        },
                        xaxis: {
                        ...chartsConfig.xaxis,
                        categories: dataMaintenance.months,
                        },
                    },
                    }
                }
            />

            <StatisticsChart
                key ="ProposedDecomissions"
                color ="white"
                title = "Proposed decommissions by month"
                description = "This metric reflects the number of decommissioning proposals made each month."
                chart = { 
                    {
                    type: "line",
                    height: 330,
                    series: [
                        {
                        name: "",
                        data: dataDecommissions.values,
                        },
                    ],
                    options: {
                        ...chartsConfig,
                        colors: ["#2C3E50",],
                        stroke: {
                        lineCap: "round",
                        },
                        markers: {
                        size: 5,
                        },
                        xaxis: {
                        ...chartsConfig.xaxis,
                        categories: dataDecommissions.months,
                        },
                    },
                    }
                }
                
            />
            <Card className="border border-blue-gray-100 shadow-sm">
                <CardHeader variant="h6" color="white" floated={false} shadow={false}>
                        Equipments by state
                </CardHeader>
                <CardBody className="px-6 pt-0">
                <Typography variant="h6" color="blue-gray">
                    <PieChartComponent data={dataEquipments} width={5000} height={5000} />
                </Typography>
                </CardBody>
            </Card>

        </div>
        </div>
    );
}

export default Home;
