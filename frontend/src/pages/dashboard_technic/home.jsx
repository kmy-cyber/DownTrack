import React from "react";
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
import PieChartComponent from "@/components/charts/simplePieChart";
import {
    statisticsCardsData,
    statisticsChartsData,
    projectsTableData,
    ordersOverviewData,
} from "@/data";
import { chartsConfig } from "@/configs";
import { ArrowDownCircleIcon, CheckCircleIcon, ClockIcon, WrenchIcon } from "@heroicons/react/24/solid";
import { ArrowDownwardSharp, HourglassBottomRounded } from '@mui/icons-material';

export function Home() {

    const dataEquipment = [
        { name: 'Active', value: 400 , color: '#34495E'},
        { name: 'Under Maintenance', value: 500, color:'#5D8AA8'  },
        { name: 'Inactive', value: 300 , color:'#4A6FA5' },
    ];

    return (
        <div className="mt-12">
        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">

            <StatisticsCard
                color= "gray" 
                key= "Maintenance"
                title= ""
                value = "VALUE"
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
                key= "MaintenanceProcess"
                title= ""
                value = "VALUE"
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
                title= ""
                value = "VALUE"
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
                        data: [50, 40, 300, 320, 500, 350, 200, 230, 500, 900, 90, 100],
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
                        categories: [
                            "Jan",
                            "Feb",
                            "Mar",
                            "Apr",
                            "May",
                            "Jun",
                            "Jul",
                            "Aug",
                            "Sep",
                            "Oct",
                            "Nov",
                            "Dec",
                        ],
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
                        data: [50, 40, 300, 320, 500, 350, 200, 230, 500, 90, 19, 80],
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
                        categories: [
                            "Jan",
                            "Feb",
                            "Mar",
                            "Apr",
                            "May",
                            "Jun",
                            "Jul",
                            "Aug",
                            "Sep",
                            "Oct",
                            "Nov",
                            "Dec",
                        ],
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
                    <PieChartComponent data={dataEquipment} width={5000} height={5000} />
                </Typography>
                </CardBody>
            </Card>

        </div>
        </div>
    );
}

export default Home;
