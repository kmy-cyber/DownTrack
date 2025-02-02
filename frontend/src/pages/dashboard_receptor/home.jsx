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
import CustomBarChart from "@/components/charts/customBarChart";

import { chartsConfig } from "@/configs";
import { CheckCircleIcon, ClockIcon, ArrowsRightLeftIcon } from "@heroicons/react/24/solid";
import { AdfScanner, ArrowCircleDown, Business, HomeWorkRounded, TransferWithinAStationOutlined} from '@mui/icons-material';
import SimpleBarChart from "@/components/charts/simpleBarChart";

export function Home() {

    const data = [
        { name: 'Jan', pv: 100 },
        { name: 'Feb', pv: 500 },
        { name: 'Mar', pv: 200 },
        { name: 'Apr', pv: 400 },
        { name: 'May', pv: 30 },
        { name: 'Jun', pv: 550 },
        { name: 'Jul', pv: 200 },
        { name: 'Aug', pv: 390 },
        { name: 'Sep', pv: 300 },
        { name: 'Oct', pv: 690 },
        { name: 'Nov', pv: 190 },
        { name: 'Dec', pv: 360 },
    ];
    
    const colors = {
        barFill: '#34495E',       
        background: '#eee',      
    };

    return (

        <div className="mt-12">

        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">
            <StatisticsCard
                color= "gray" 
                key= "Pending transfers"
                title= ""
                value = "VALUE"
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
                title= ""
                value = "VALUE"
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
                title= ""
                value = "VALUE"
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
                        data: [50, 40, 300, 320, 500, 350, 200, 230, 500,100,400,280],
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
                <CardHeader variant="gradient" color="white" floated={false} shadow={false}>
                    Total registered transfers per month
                </CardHeader>
                <CardBody className="px-6 pt-0">
                <Typography variant="h6" color="blue-gray">
                    <SimpleBarChart data={data} colors={colors} />
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
