import React, { useEffect, useState } from "react";
import { Typography, Card } from "@material-tailwind/react";
import { StatisticsCard } from "@/components/cards";
import { CustomPieChart } from "@/components/charts";
import { StatisticsChart } from "@/components/charts";
import { useAuth } from "@/context/AuthContext";
import {
  ArchiveBoxArrowDownIcon,
  ArrowsRightLeftIcon,
} from "@heroicons/react/24/solid";
import api from "@/middlewares/api";
import { HomeWorkRounded } from "@mui/icons-material";

export function Home() {
  const { user } = useAuth();
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchStatistics();
  }, []);

  const fetchStatistics = async () => {
    try {
      const response = await api(
        `/Statistics/SectionManager?sectionManager=${user.id}`,
      );
      if (!response.ok) throw new Error("Failed to fetch statistics");
      const data = await response.json();
      setStats(data);
      console.log(data);
    } catch (err) {
      setError("Failed to load statistics");
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  const equipmentData = [
    {
      name: "Active",
      value: stats.equipmentsByStatus.Active || 0,
      color: "#4A90E2",
    },
    {
      name: "Maintenance",
      value: stats.equipmentsByStatus.UnderMaintenance || 0,
      color: "#FFBB28",
    },
    {
      name: "Inactive",
      value: stats.equipmentsByStatus.Inactive || 0,
      color: "#E94E77",
    },
  ];

  return (
    <div className="mt-12">
      <div className="mb-12 grid gap-x-3 gap-y-10 xl:grid-cols-3">
        <StatisticsCard
          title="Transfer Requests"
          value={stats.transferRequestsMade}
          icon={<ArchiveBoxArrowDownIcon className="h-6 w-6" />}
          color="gray"
          footer="Total number of transfer requests made."
        />
        <StatisticsCard
          title="Transfers Completed"
          value={stats.transferRequestsCompleted}
          icon={<ArrowsRightLeftIcon className="h-6 w-6" />}
          color="gray"
          footer="Total number of transfer requests completed."
        />
        <StatisticsCard
          title="Departments"
          value={stats.numberOfDepartmentsInSection}
          icon={<HomeWorkRounded className="h-6 w-6" />}
          color="gray"
          footer="Total number of departments in your sections."
        />
      </div>

      <div className="mb-6 grid grid-cols-1 gap-x-6 gap-y-12 md:grid-cols-2 xl:grid-cols-2">
        <Card className="p-4">
          <Typography variant="h6" color="blue-gray" className="mb-4">
            Equipment Status Distribution
          </Typography>
          <CustomPieChart
            data={equipmentData}
            colors={["#4A90E2", "#FFBB28", "#E94E77"]}
          />
          <Typography variant="body2" color="gray" className="mt-2">
            Distribution of equipment based on their
            current status: Active, Maintenance, and Inactive.
          </Typography>
        </Card>

        <Card className="p-4">
          <Typography variant="h6" color="blue-gray" className="mb-4">
            Technician Evaluations By Criteria
          </Typography>
          <StatisticsChart
            color="white"
            chart={{
              type: "bar",
              series: [
                {
                  name: "Evaluations",
                  data: [
                    stats.evaluationsByType?.Good || 0,
                    stats.evaluationsByType?.Regular || 0,
                    stats.evaluationsByType?.Bad || 0,
                  ],
                },
              ],
              options: {
                chart: {
                  stacked: true,
                },
                xaxis: {
                  categories: ["Good", "Regular", "Bad"], // Categories
                },
                colors: ["#4A90E2", "#FF6384", "#FFBB28"], // Differentiated colors
                legend: {
                  position: "top",
                },
                dataLabels: {
                  enabled: true,
                },
                yaxis: {
                  labels: {
                    formatter: function (value) {
                      return Math.floor(value); // Show only integer values
                    },
                  },
                },
                plotOptions: {
                  bar: {
                    columnWidth: "20%", // Adjust the width of the bars here (you can decrease this value further)
                  },
                },
              },
            }}
          />
          <Typography variant="body2" color="gray" className="mt-2">
            Shows the number of technicians evaluated based on
            three criteria: Good, Regular, and Bad.
          </Typography>
        </Card>
      </div>
    </div>
  );
}

export default Home;
