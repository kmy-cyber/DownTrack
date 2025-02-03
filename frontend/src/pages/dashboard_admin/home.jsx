import React, { useEffect, useState } from "react";
import {
    Typography,
    Card,
    CardHeader,
    CardBody,
} from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { StatisticsChart } from "@/components/charts";
import { CustomPieChart} from "@/components/charts/CustomPieChart";
import CustomBarChart from "@/components/charts/customBarChart";
import { chartsConfig } from "@/configs";
import { CheckCircleIcon, ClockIcon, UserGroupIcon } from "@heroicons/react/24/solid";
import { Business, HomeWorkRounded} from '@mui/icons-material';
import api from "@/middlewares/api";

export function Home() {

    const [currentItems, setCurrentItems] = useState([]);
    const [dataRole, setDataRole] = useState([]);
    const [dataDepartmentSections, setDataDepartmentSections] = useState([]);

    useEffect(() => {
        fetchStatistics();
    }, []);

    
    const colors = {
        barFill: '#34495E',  
        barFillSecondary: '#000000', 
    };

    const fetchStatistics = async () => {
        try {
            const response = await api(`/Statistics/Admin`, {
                method: 'GET',
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            
            setCurrentItems(data);
            initDataRole(data.rolesStatistics);
            setDataDepartmentSections(transformData(data));
        } catch (error) {
            console.error("Error fetching departments:", error);
            setCurrentItems([   ]);
        }
    };

    const initDataRole = (rolesStatistics) => {
        setDataRole([
            { name: 'Administrators', value: rolesStatistics.Administrator , color: '#34495E'},
            { name: 'Section Managers', value: rolesStatistics.SectionManager, color:'#34495E'  },
            { name: 'Technicians', value: rolesStatistics.Technician , color:'#34495E' },
            { name: 'Shipping Supervisors', value: rolesStatistics.ShippingSupervisor, color:'#34495E' },
            { name: 'Receptors', value: rolesStatistics.EquipmentReceptor, color:'#34495E' },
            { name: 'Directors', value: rolesStatistics.Director, color:'#34495E' },    
        ]);
    }

    const transformData = (data) => {
        const months = [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
    
        const combinedData = {};
    
        for (const [key, value] of Object.entries(data.departmentsByMonth)) {
            const [month, year] = key.split('-');
            const monthName = months[parseInt(month) - 1];
            const name = `${monthName}-${year.substring(2)}`;
        
            if (!combinedData[name]) {
                combinedData[name] = { name, departments: 0, sections: 0 };
            }
            combinedData[name].departments = value;
        }
    
        for (const [key, value] of Object.entries(data.sectionsByMonth)) {
            const [month, year] = key.split('-');
            const monthName = months[parseInt(month) - 1];
            const name = `${monthName}-${year.substring(2)}`;
        
            if (!combinedData[name]) {
                combinedData[name] = { name, departments: 0, sections: 0 };
            }
            combinedData[name].sections = value;
        }

        console.log(Object.values(combinedData));
    
        return Object.values(combinedData);
    }

    return (
        <div className="mt-12">
        <div className="mb-12 grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">

            <StatisticsCard
                color= "gray" 
                key= "Employees"
                title= "Employees   "
                value = {currentItems.numberEmployee}
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
                title= "Sections  "
                value = {currentItems.numberSections}
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
                title= "Department   "
                value = {currentItems.numberDepartments}
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
                    <CustomBarChart data={dataDepartmentSections} colors={colors} dataKey1="departments" dataKey2="sections" />
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
