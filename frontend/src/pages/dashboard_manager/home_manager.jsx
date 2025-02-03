import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { CubeIcon, ArrowRightIcon } from "@heroicons/react/24/solid";  // Usamos un icono para las transferencias
import { StatisticsChart } from "@/components/charts";
import api from "@/middlewares/api";
import { CustomPieChart } from "@/components/charts";
import { useAuth } from "@/context/AuthContext";

export function Home() {
  const [inventoryData, setInventoryData] = useState([]);
  const [transferData, setTransferData] = useState([]); // Estado para las transferencias
  const [inactiveEquipments, setInactiveEquipments] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [total, setTotal] = useState(0);
  const { user } = useAuth();

  useEffect(() => {
    fetchData();
    // fetchTransferData(); // Cargar datos de transferencias
  }, []);

  const fetchData = async () => {
    try {
      const response = await api(
        `/Equipment/equipments/section-manager/${user.id}?PageNumber=1&PageSize=99999`,
      );

      if (!response.ok) {
        throw new Error("Failed to fetch inventory");
      }

      const data = await response.json();
      const processedData = processInventoryData(data.items || []);
      setInactiveEquipments(data.items.filter(item => item.status === "Inactive").length)
      console.log(inactiveEquipments);
      setInventoryData(processedData);
      setTotal(data.totalCount);
    } catch (err) {
      setError("Failed to load inventory data");
    } finally {
      setLoading(false);
    }
  };

  const fetchTransferData = async () => {
    try {
      // Simulación de la API para obtener datos de transferencias
      const response = await api(
        `/Transfers/GetPaged?PageNumber=1&PageSize=99999`, // URL de ejemplo para obtener las transferencias
      );

      if (!response.ok) {
        throw new Error("Failed to fetch transfer data");
      }

      const data = await response.json();
      const processedTransferData = processTransferData(data.items || []);
      setTransferData(processedTransferData);
    } catch (err) {
      setError("Failed to load transfer data");
    }
  };

  const processInventoryData = (data) => {
    if (!Array.isArray(data) || data.length === 0) return [];

    const statusCount = data.reduce((acc, item) => {
      if (item.status) {
        acc[item.status] = (acc[item.status] || 0) + 1;
      }
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

  const processTransferData = (data) => {
    if (!Array.isArray(data) || data.length === 0)
      return { months: [], values: [] };

    const transferCount = {};

    data.forEach((item) => {
      const date = new Date(item.transferDate);
      const monthYear = `${date.toLocaleString("default", { month: "short" })} ${date.getFullYear()}`;

      if (!transferCount[monthYear]) {
        transferCount[monthYear] = 0;
      }
      transferCount[monthYear]++;
    });

    const sortedData = Object.entries(transferCount).sort(
      ([a], [b]) => new Date(a) - new Date(b),
    );

    return {
      months: sortedData.map(([monthYear]) => monthYear),
      values: sortedData.map(([, count]) => count),
    };
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

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
          value={total-inactiveEquipments}
          icon={<CubeIcon className="h-6 w-6" />}
          footer={<Typography>Total Active/Under Maintenance Equipments</Typography>}
        />
      </div>

      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-2">
        {/* Pie Chart Component */}
        <div>
          <Card className="p-4">
            <Typography variant="h6" color="blue-gray" className="mb-4">
              Inventory Status Pie Chart
            </Typography>
            <CustomPieChart
              data={inventoryData}
              colors={["#4A90E2", "#FFBB28", "#E94E77"]}
            />
            <footer>
              This chart represents the distribution of the equipment. The
              categories are: Active, Maintenance, and Inactive. Each segment of
              the pie chart shows the percentage of equipment in each of these
              states.
            </footer>
          </Card>
        </div>

        {/* New Transfer Chart */}
        <div>
          <Card className="p-4">
            <Typography variant="h6" color="blue-gray" className="mb-4">
              Transfers by Month
            </Typography>
            <StatisticsChart
              key="TransfersByMonth"
              color="white"
              title="Transfers by Month"
              description="This chart shows the number of transfers made each month, providing insights into equipment movement."
              chart={{
                type: "line",
                height: 330,
                series: [
                  {
                    name: "Transfers",
                    data: transferData.values,
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
                    categories: transferData.months,
                  },
                },
              }}
            />
          </Card>
        </div>
      </div>
    </div>
  );
}

export default Home;
