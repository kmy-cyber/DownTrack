import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { ClockIcon, CubeIcon } from "@heroicons/react/24/solid";
import { StatisticsChart } from "@/components/charts";
import api from "@/middlewares/api";
import { CustomPieChart } from "@/components/charts";
import { useAuth } from "@/context/AuthContext";

export function Home() {
  const [inventoryData, setInventoryData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [total, setTotal] = useState(0);
  const {user} = useAuth();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      // http://localhost:5217/api/Equipment/equipments/section-manager/362?PageNumber=1&PageSize=5
      const response = await api(
        `/Equipment/equipments/section-manager/${user.id}?PageNumber=1&PageSize=99999`,
      );

      if (!response.ok) {
        throw new Error("Failed to fetch inventory");
      }

      const data = await response.json();
      const processedData = processInventoryData(data.items || []);
      setInventoryData(processedData);
      setTotal(data.totalCount);
    } catch (err) {
      setError("Failed to load inventory data");
    } finally {
      setLoading(false);
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

    // Agregando el nuevo estado 'Inactive'
    return [
      { name: "Active", value: statusCount.Active || 0, color: "#4A90E2" },
      { name: "Maintenance", value: statusCount.UnderMaintenance || 0, color: "#FFBB28" },
      { name: "Inactive", value: statusCount.Inactive || 0, color: "#E94E77" }, // Nuevo estado
    ];
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="mt-12">
      <div className="mb-12 grid gap-x-3 gap-y-10 xl:grid-cols-2">
        <StatisticsCard
          color="gray"
          title="Equipment:"
          value={total}
          icon={<CubeIcon className="h-6 w-6"/>}
          footer={<Typography>Total Equipment</Typography>}
        />
        <StatisticsCard
          color="gray"
          title="Recently added"
          value="COMING SOON"
        />
      </div>

      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-3">
        {/* Pie Chart Component */}
        <div>
          <Card className="p-4">
            <Typography variant="h6" color="blue-gray" className="mb-4">
              Inventory Status Pie Chart
            </Typography>
            <CustomPieChart
              data={inventoryData}
              colors={["#4A90E2", "#FFBB28", "#E94E77"]} // Añadiendo el nuevo color
            />
          </Card>
        </div>
      </div>
    </div>
  );
}

export default Home;
