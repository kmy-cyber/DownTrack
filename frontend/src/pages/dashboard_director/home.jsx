import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { StatisticsChart } from "@/components/charts";
import { ClockIcon } from "@heroicons/react/24/solid";
import api from "@/middlewares/api";
import DonutChart from "@/components/charts/customDonutChart";

export function Home() {
  const [inventoryData, setInventoryData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await api("/Equipment/GetPaged?PageNumber=1&PageSize=99999");

      if (!response.ok) {
        throw new Error("Failed to fetch inventory");
      }

      const data = await response.json();
      console.log(data.items);
      const processedData = processInventoryData(data.items || []);
      setInventoryData(processedData);
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

    const total = Object.values(statusCount).reduce((sum, count) => sum + count, 0);

    let result = Object.entries(statusCount).map(([status, count]) => ({
      name: status,
      value: parseFloat(((count / total) * 100).toFixed(2)), // Convertir a porcentaje con 2 decimales
      total: count, // Almacenamos el total de cada categoría
    }));

    // Asegurar que siempre haya al menos dos categorías para evitar errores en el gráfico
    while (result.length < 3) {
      result.push({ name: `Category ${result.length + 1}`, value: 0, total: 0 });
    }

    return result;
  };

  return (
    <div className="mt-12">
      <div className="mb-12 grid gap-x-6 gap-y-10 md:grid-cols-2 xl:grid-cols-4">
        <StatisticsCard color="gray" title="Recently added" value="COMING SOON" />
        <StatisticsCard color="gray" title="Recently added" value="COMING SOON" />
        <StatisticsCard color="gray" title="Recently added" value="COMING SOON" />
      </div>

      {/* Donut Chart para estado del inventario */}
      <div className="mb-12 grid grid-cols-1 gap-6 md:grid-cols-2 xl:grid-cols-3">
        <Card className="p-6 shadow-lg">
          <Typography variant="h6" color="blue-gray" className="mb-4 text-center">
            Estado del Inventario
          </Typography>
          {loading ? (
            <Typography className="text-center">Cargando...</Typography>
          ) : error ? (
            <Typography className="text-center text-red-500">{error}</Typography>
          ) : (
            <DonutChart data={inventoryData} />
          )}
        </Card>
      </div>

      {/* Gráfico de línea de ejemplo */}
      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-3">
        <StatisticsChart
          key="Added last month"
          color="white"
          title="Added last month"
          description="Info coming soon"
          chart={{
            type: "line",
            height: 220,
            series: [{ name: "Equipments", data: [50, 40, 300, 320, 500, 350, 200, 230, 500] }],
            options: {
              colors: ["#0288d1"],
              stroke: { lineCap: "round" },
              markers: { size: 5 },
              xaxis: {
                categories: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
              },
            },
          }}
          footer={
            <Typography variant="small" className="flex items-center font-normal text-blue-gray-600">
              <ClockIcon strokeWidth={2} className="h-4 w-4 text-blue-gray-400" />
              &nbsp;{"..."}
            </Typography>
          }
        />
      </div>
    </div>
  );
}

export default Home;
