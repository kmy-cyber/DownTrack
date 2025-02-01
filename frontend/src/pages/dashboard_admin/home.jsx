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
import { CustomPieChart } from "@/components/charts/CustomPieChart";
import { chartsConfig } from "@/configs";
import { CheckCircleIcon, ClockIcon, UserGroupIcon } from "@heroicons/react/24/solid";
import { Business, HomeWorkRounded} from '@mui/icons-material';

export function Home() {

    const data = [
        { name: 'Group A', value: 400 },
        { name: 'Group B', value: 300 },
        { name: 'Group C', value: 300 },
        { name: 'Group D', value: 200 },
    ];


    return (
        <div className="mt-12">
        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-4">

            <StatisticsCard
                color= "gray" 
                key= "Employees"
                title= "Employees"
                value = "VALUE"
                icon={React.createElement(UserGroupIcon, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total number of employees."}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "Sections"
                title= "Sections"
                value = "VALUE"
                icon={React.createElement(Business, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total number of sections"}
                </Typography>
                }
            />

            <StatisticsCard
                color= "gray" 
                key= "Department"
                title= "Department"
                value = "Value"
                icon={React.createElement(HomeWorkRounded, {
                className: "w-6 h-6 text-white",
                })}
                footer={
                <Typography className="font-normal text-green-gray-600">
                    <strong className={"text-green-500"}>{""}</strong>
                    &nbsp;{"Total number of departments."}
                </Typography>
                }
            />


        </div>
        <div className="mb-6 grid grid-cols-1 gap-y-12 gap-x-6 md:grid-cols-2 xl:grid-cols-3">
            <StatisticsChart
                key ="Added last month"
                color ="white"
                title = "Added last month"
                description = "Info comming soon"
                chart = { 
                    {
                    type: "line",
                    height: 220,
                    series: [
                        {
                        name: "",
                        data: [50, 40, 300, 320, 500, 350, 200, 230, 500],
                        },
                    ],
                    options: {
                        ...chartsConfig,
                        colors: ["#0288d1"],
                        stroke: {
                        lineCap: "round",
                        },
                        markers: {
                        size: 5,
                        },
                        xaxis: {
                        ...chartsConfig.xaxis,
                        categories: [
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
                footer={
                <Typography
                    variant="small"
                    className="flex items-center font-normal text-blue-gray-600"
                >
                    <ClockIcon strokeWidth={2} className="h-4 w-4 text-blue-gray-400" />
                    &nbsp;{"..."}
                </Typography>
                }
            />

            <StatisticsChart
                key ="Erased last month"
                color ="white"
                title = "Erased last month"
                description = "Info comming soon"
                chart = { 
                    {
                    type: "line",
                    height: 220,
                    series: [
                        {
                        name: "",
                        data: [50, 40, 300, 320, 500, 350, 200, 230, 500],
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
                        categories: [
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
                footer={
                <Typography
                    variant="small"
                    className="flex items-center font-normal text-blue-gray-600"
                >
                    <ClockIcon strokeWidth={2} className="h-4 w-4 text-blue-gray-400" />
                    &nbsp;{"..."}
                </Typography>
                }
            />

        </div>

        <div className="p-4">
            <h3 className="text-lg font-semibold mb-4">Gráfico de Pie</h3>
            <CustomPieChart data={data} width={500} height={500} />
        </div>

        </div>
    );
}

export default Home;
