import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { CubeIcon, MinusCircleIcon, WrenchScrewdriverIcon } from "@heroicons/react/24/solid";
import api from "@/middlewares/api";
import { CustomPieChart } from "@/components/charts";
import { StatisticsChart } from "@/components/charts/statistics-chart";

export function Home() {
  const [metricsData, setMetricsData] = useState(null);
  const [equipmentData, setEquipmentData] = useState(null); // Nuevo estado para equipos
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchMetricsData();
    fetchEquipmentData(); // Llamado adicional para obtener los equipos
  }, []);

  const fetchMetricsData = async () => {
    try {
      const response = await api("/Statistics/Director");
      if (!response.ok) throw new Error("Failed to fetch metrics data");
      const data = await response.json();
      setMetricsData(data);
    } catch (err) {
      setError("Failed to load metrics data");
    } finally {
      setLoading(false);
    }
  };

  const fetchEquipmentData = async () => {
    try {
      const response = await api("/Equipment/GetPaged?PageNumber=1&PageSize=99999");
      if (!response.ok) throw new Error("Failed to fetch equipment data");
      const data = await response.json();
      setEquipmentData(data.items); // Guardamos los datos de los equipos
    } catch (err) {
      setError("Failed to load equipment data");
    }
  };

  const processDataForChart = (data) => {
    const months = Object.keys(data);
    const values = months.map((month) => data[month]);
    return { months, values };
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  const {
    numberOfEquipments,
    acceptedDecommissionsByMonth,
    transfersByMonth,
    maintenanceCostByMonth,
  } = metricsData || {};

  const decommissionData = processDataForChart(acceptedDecommissionsByMonth);
  const transfersData = processDataForChart(transfersByMonth);
  const maintenanceCostData = processDataForChart(maintenanceCostByMonth);

  // Contamos los equipos por estado
  const countEquipmentStatuses = (equipmentData) => {
    const statusCounts = { Active: 0, "UnderMaintenance": 0, Inactive: 0 };

    equipmentData.forEach((equipment) => {
      if (statusCounts[equipment.status] !== undefined) {
        statusCounts[equipment.status]++;
      }
    });

    return statusCounts;
  };

  const equipmentStatuses = equipmentData ? countEquipmentStatuses(equipmentData) : {};

  // Calculamos los porcentajes
  const totalEquipment = equipmentData ? equipmentData.length : 0;
  const activePercentage = totalEquipment ? (equipmentStatuses.Active / totalEquipment) * 100 : 0;
  const maintenancePercentage = totalEquipment ? (equipmentStatuses["UnderMaintenance"] / totalEquipment) * 100 : 0;
  const inactivePercentage = totalEquipment ? (equipmentStatuses.Inactive / totalEquipment) * 100 : 0;

  const acceptedDecommissions = Object.values(acceptedDecommissionsByMonth).reduce((acc, value) => acc + value, 0)
  console.log(equipmentStatuses);
  console.log(equipmentData);

  return (
    <div className="mt-12">
      <div className="mb-12 grid gap-x-3 gap-y-10 xl:grid-cols-3">
        <StatisticsCard
          color="gray"
          title="Equipment:"
          value={numberOfEquipments - acceptedDecommissions}
          icon={<CubeIcon className="h-6 w-6" />}
          footer={<Typography>Active Equipment</Typography>}
        />
        <StatisticsCard
          color="gray"
          title="Decommissions"
          value={acceptedDecommissions}
          icon={<MinusCircleIcon className="h-6 w-6" />}
          footer={<Typography>Total Decommissions</Typography>}
        />
        <StatisticsCard
          color="gray"
          title="Equipment Under Maintenance:"
          value={equipmentStatuses["UnderMaintenance"]}
          icon={<WrenchScrewdriverIcon className="h-6 w-6" />}
          footer={<Typography>Total Equipment Under Maintenance</Typography>}
        />
      </div>

      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-3">
        <Card className="p-4">
          <Typography variant="h6" color="blue-gray" className="mb-4">
            Equipment Status
          </Typography>
          <CustomPieChart
            data={[
              { name: "Active", value: activePercentage, color: "#4A90E2" },
              { name: "Maintenance", value: maintenancePercentage, color: "#FFBB28" },
              { name: "Inactive", value: inactivePercentage, color: "#E94E77" },
            ]}
            colors={["#4A90E2", "#FFBB28", "#E94E77"]}
          />
          <Typography variant="body2" color="blue-gray" className="mb-4">
            This chart represents the distribution of the equipment. The categories are: Active, Maintenance, and Inactive. Each segment of the pie chart shows the percentage of equipment in each of these states.
          </Typography>
        </Card>

        <StatisticsChart
          key="Decommissions"
          color="white"
          title="Decommissions by Month"
          description="This metric reflects the number of decommissioning proposals accepted each month. It helps track the decommissioning trend over time, providing insights into the equipment lifecycle."
          chart={{
            type: "line",
            height: 330,
            series: [
              { name: "Decommissioned", data: decommissionData.values },
            ],
            options: {
              colors: ["#2C3E50"],
              stroke: { lineCap: "round" },
              markers: { size: 5, colors: ["#FF0000"] },
              xaxis: { categories: decommissionData.months },
            },
          }}
        />
        
        <StatisticsChart
          key="Transfers"
          color="white"
          title="Transfers by Month"
          description="Shows the number of transfers made each month, helping track the transfer activity across the equipment lifecycle."
          chart={{
            type: "line",
            height: 330,
            series: [
              { name: "Transfers", data: transfersData.values },
            ],
            options: {
              colors: ["#27AE60"],
              stroke: { lineCap: "round" },
              markers: { size: 5, colors: ["#FF9800"] },
              xaxis: { categories: transfersData.months },
            },
          }}
        />
        
        <StatisticsChart
          key="MaintenanceCost"
          color="white"
          title="Maintenance Costs by Month"
          description="Tracks the total cost of maintenance performed on equipment each month."
          chart={{
            type: "line",
            height: 330,
            series: [
              { name: "Maintenance Cost", data: maintenanceCostData.values },
            ],
            options: {
              colors: ["#8E44AD"],
              stroke: { lineCap: "round" },
              markers: { size: 5, colors: ["#FFC107"] },
              xaxis: { categories: maintenanceCostData.months },
            },
          }}
        />
      </div>
    </div>
  );
}

export default Home;
