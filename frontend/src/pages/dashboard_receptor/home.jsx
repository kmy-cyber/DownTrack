import React, { useState, useEffect } from "react";
import {
    Typography,
    Card,
    CardHeader,
    CardBody,
    IconButton,
    Menu,
    MenuHandler,
    MenuList,
    MenuItem,
    Avatar,
    Tooltip,
    Progress,
} from "@material-tailwind/react";
import {
    EllipsisVerticalIcon,
    ArrowUpIcon,
    PlusIcon,
} from "@heroicons/react/24/outline";
import { StatisticsCard } from "@/components/cards";
import { StatisticsChart } from "@/components/charts";
import CustomBarChart from "@/components/charts/customBarChart";
import api from "@/middlewares/api";
import { chartsConfig } from "@/configs";
import { CheckCircleIcon, ClockIcon, ArrowsRightLeftIcon } from "@heroicons/react/24/solid";
import { AdfScanner, ArrowCircleDown, Business, HomeWorkRounded, TransferWithinAStationOutlined} from '@mui/icons-material';
import SimpleBarChart from "@/components/charts/simpleBarChart";
import { useAuth } from "@/context/AuthContext";

export function Home() {

    const [currentItems, setCurrentItems] = useState({})
    const [acceptedDecommissions, setAcceptedDecommissions] = useState([])
    const [processedTransfers, setProcessedTransfers] = useState([])

{/*"pendingTransfers": 4,
  "pendingDecommissions": 1,
  "totalEquipments": 4,
  "acceptedDecommissionsPerMonth": {
    "2-2025": 1
  },
  "processedTransfersPerMonth": {}
}*/}

    const {user} = useAuth();
    
    const colors = {
        barFill: '#34495E',       
        background: '#eee',      
    };

    useEffect(()=>{
        fetchStatistics();
    },[])

    const fetchStatistics = async () => {
        try {
            const response = await api(`/Statistics/Receptor?receptorId=${user.id}`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            
            setCurrentItems(data);
            setAcceptedDecommissions(transformData(data.acceptedDecommissionsPerMonth));
            setProcessedTransfers(transformData(data.processedTransfersPerMonth));

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

    return (

        <div className="mt-12">

        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">
            <StatisticsCard
                color= "gray" 
                key= "Pending transfers"
                title= "Pending transfers"
                value = {currentItems.pendingTransfers}
                icon={React.createElement(ArrowsRightLeftIcon, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Pending transfers"}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "Pending decommission proposals"
                title= "Pending decommission"
                value = {currentItems.pendingDecommissions}
                icon={React.createElement(ArrowCircleDown, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Pending decommission proposals"}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "Total of inserted equipment"
                title= "Equipments"
                value = {currentItems.totalEquipments}
                icon={React.createElement(AdfScanner, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total of inserted equipment"}
                </Typography>
                }
            />


        </div>

        <div className="mb-6 grid grid-cols-1 gap-y-12 gap-x-6 md:grid-cols-2 xl:grid-cols-2">

            <StatisticsChart
                key ="Total accepted decommissions per month"
                color ="white"
                title = "Total accepted decommissions per month"
                description = "the number of decommission requests approved each month"
                chart = { 
                    {
                    type: "line",
                    height: 390,
                    series: [
                        {
                        name: "",
                        data: acceptedDecommissions.values
                        },
                    ],
                    options: {
                        ...chartsConfig,
                        colors: ["#a52a2a",],
                        stroke: {
                        lineCap: "round",
                        },
                        markers: {
                        size: 5,
                        },
                        xaxis: {
                        ...chartsConfig.xaxis,
                        categories: acceptedDecommissions.months,
                        },
                    },
                    }
                }
            />

            <Card className="border border-blue-gray-100 shadow-sm">
                <CardHeader variant="gradient" color="white" floated={false} shadow={false}>
                    Total registered transfers per month
                </CardHeader>
                <CardBody className="px-6 pt-0">
                <Typography variant="h6" color="blue-gray">
                    <SimpleBarChart data={processedTransfers} colors={colors} />
                </Typography>
                <Typography variant="small" className="font-normal text-blue-gray-600">
                    {"The number of equipment transfers recorded each month"}
                </Typography>
                </CardBody>
                {/* {footer && (
                <CardFooter className="border-t border-blue-gray-50 px-6 py-5">
                    {footer}
                </CardFooter>
                )} */}
            </Card>

        </div>

        
        </div>
    );
}

export default Home;
