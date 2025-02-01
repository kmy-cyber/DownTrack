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
import { CustomPieChart} from "@/components/charts/CustomPieChart";
import CustomBarChart from "@/components/charts/customBarChart";
import { chartsConfig } from "@/configs";
import { CheckCircleIcon, ClockIcon, UserGroupIcon } from "@heroicons/react/24/solid";
import { Business, HomeWorkRounded} from '@mui/icons-material';

export function Home() {

    const dataRole = [
        { name: 'Administrators', value: 5 , color: '#34495E'},
        { name: 'Section Managers', value: 50, color:'#34495E'  },
        { name: 'Technicians', value: 300 , color:'#34495E' },
        { name: 'Shipping Supervisors', value: 200, color:'#34495E' },
        { name: 'Receptors', value: 100, color:'#34495E' },
        { name: 'Directors', value: 1, color:'#34495E' },
        
    ];

    const data = [
        { name: 'Page A', uv: 4000, pv: 2400, amt: 2400 },
        { name: 'Page B', uv: 3000, pv: 1398, amt: 2210 },
        { name: 'Page C', uv: 2000, pv: 9800, amt: 2290 },
        { name: 'Page D', uv: 2780, pv: 3908, amt: 2000 },
        { name: 'Page E', uv: 1890, pv: 4800, amt: 2181 },
        { name: 'Page F', uv: 2390, pv: 3800, amt: 2500 },
        { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
    ];
    
    const colors = {
        barFill: '#34495E',  
        barFillSecondary: '#000000', 
    };


    return (
        <div className="mt-12">
        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">

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
        {/* <div className="mb-6 grid grid-cols-1 gap-y-12 gap-x-6 md:grid-cols-2 xl:grid-cols-3">
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

        </div> */}

        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-2 p-4">
            <Card className="border border-blue-gray-100 shadow-sm">
                <CardHeader variant="gradient" color="white" floated={false} shadow={false}>
                    Employees by role
                </CardHeader>
                <CardBody className="px-6 pt-0">
                <Typography variant="h6" color="blue-gray">
                    <CustomPieChart data={dataRole} width={5000} height={5000} />
                </Typography>
                {/* <Typography variant="small" className="font-normal text-blue-gray-600">
                    {description}
                </Typography> */}
                </CardBody>
                {/* {footer && (
                <CardFooter className="border-t border-blue-gray-50 px-6 py-5">
                    {footer}
                </CardFooter>
                )} */}
            </Card>
            
            <Card className="border border-blue-gray-100 shadow-sm">
                <CardHeader variant="gradient" color="white" floated={false} shadow={false}>
                    Departments and Sections per Month
                </CardHeader>
                <CardBody className="px-6 pt-0">
                <Typography variant="h6" color="blue-gray">
                    <CustomBarChart data={data} colors={colors} />
                </Typography>
                {/* <Typography variant="small" className="font-normal text-blue-gray-600">
                    {description}
                </Typography> */}
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
