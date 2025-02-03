import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { CubeIcon, MinusCircleIcon } from "@heroicons/react/24/solid";
import api from "@/middlewares/api";
import { CustomPieChart } from "@/components/charts";
import { StatisticsChart } from "@/components/charts/statistics-chart"; // Importamos el nuevo componente

export function Home() {
  const [inventoryData, setInventoryData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [totalEquipment, setTotalEquipment] = useState(0);
  const [totalDecommissions, setTotalDecommissions] = useState(0);
  const [decommissionedData, setDecommissionedData] = useState([]);

  useEffect(() => {
    fetchInventoryData();
    fetchDecommissionedData();
  }, []);

  const fetchInventoryData = async () => {
    try {
      const response = await api(
        "/Equipment/GetPaged?PageNumber=1&PageSize=99999",
      );
      if (!response.ok) throw new Error("Failed to fetch inventory");
      const data = await response.json();
      setInventoryData(processInventoryData(data.items || []));
      setTotalEquipment(data.totalCount);
    } catch (err) {
      setError("Failed to load inventory data");
    } finally {
      setLoading(false);
    }
  };

  const fetchDecommissionedData = async () => {
    try {
      // http://localhost:5217/api/EquipmentDecommissioning/Get_Paged_Accepted?PageNumber=1&PageSize=99999
      const response = await api(
        "/EquipmentDecommissioning/Get_Paged_Accepted?PageNumber=1&PageSize=99999",
      );
      if (!response.ok) throw new Error("Failed to fetch decommissioned data");
      const data = await response.json();
      console.log(data.items);
      const processedData = processDecommissionedData(data.items);
      setDecommissionedData(processedData);
      setTotalDecommissions(data.totalCount);
    } catch (err) {
      console.error("Error fetching decommissioned data:", err);
    }
  };

  const processInventoryData = (data) => {
    if (!Array.isArray(data) || data.length === 0) return [];
    const statusCount = data.reduce((acc, item) => {
      if (item.status) acc[item.status] = (acc[item.status] || 0) + 1;
      return acc;
    }, {});
    return [
      { name: "Active", value: statusCount.Active || 0, color: "#4A90E2" },
      {
        name: "Maintenance",
        value: statusCount.UnderMaintenance || 0,
        color: "#FFBB28",
      },
      { name: "Inactive", value: statusCount.Inactive || 0, color: "#E94E77" },
    ];
  };

  const processDecommissionedData = (data) => {
    if (!Array.isArray(data) || data.length === 0)
      return { months: [], values: [] };

    const decommissionedCount = {};

    data.forEach((item) => {
      const date = new Date(item.date);
      const monthYear = `${date.toLocaleString("default", {
        month: "short",
      })} ${date.getFullYear()}`;

      if (!decommissionedCount[monthYear]) {
        decommissionedCount[monthYear] = 0;
      }
      decommissionedCount[monthYear]++;
    });

    const sortedData = Object.entries(decommissionedCount).sort(
      ([a], [b]) => new Date(a) - new Date(b),
    );

    // Devolvemos los datos formateados para el gráfico
    return {
      months: sortedData.map(([monthYear]) => monthYear),
      values: sortedData.map(([, count]) => count),
    };
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  // Configuración de charts
  const chartsConfig = {
    xaxis: {
      labels: {
        rotate: -45,
      },
    },
  };

  return (
    <div className="mt-12">
      <div className="mb-12 grid gap-x-3 gap-y-10 xl:grid-cols-2">
        <StatisticsCard
          color="gray"
          title="Equipments:"
          value={totalEquipment - totalDecommissions}
          icon={<CubeIcon className="h-6 w-6" />}
          footer={<Typography>Total Active/Under Maintenance Equipment</Typography>}
        />
        <StatisticsCard
          color="gray"
          title=" Decommissions"
          value={totalDecommissions}
          icon={<MinusCircleIcon className="h-6 w-6"></MinusCircleIcon>}
          footer={<Typography> Decommissions overall</Typography>}
        />
      </div>

      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-2">
        <Card className="p-4">
          <Typography variant="h6" color="blue-gray" className="mb-4">
            Inventory Status Pie Chart
          </Typography>
          <CustomPieChart
            data={inventoryData}
            colors={["#4A90E2", "#FFBB28", "#E94E77"]}
          />
          <Typography variant="body2" color="blue-gray" className="mb-4">
            This chart represents the distribution of the equipment. The
            categories are: Active, Maintenance, and Inactive. Each segment of
            the pie chart shows the percentage of equipment in each of these
            states.
          </Typography>
        </Card>

        {/* Nuevo gráfico de descomisiones */}
        <StatisticsChart
          key="Decomissions"
          color="white"
          title=" Decommissions by Month"
          description="This metric reflects the number of decommissioning proposals accepted each month. It helps track the decommissioning trend over time, providing insights into the equipment lifecycle."
          chart={{
            type: "line",
            height: 330,
            series: [
              {
                name: "Decommissioned",
                data: decommissionedData.values,
              },
            ],
            options: {
              ...chartsConfig,
              colors: ["#2C3E50"],
              stroke: {
                lineCap: "round",
              },
              markers: {
                size: 5,
              },
              xaxis: {
                ...chartsConfig.xaxis,
                categories: decommissionedData.months,
              },
            },
          }}
        />
      </div>
    </div>
  );
}

export default Home;
