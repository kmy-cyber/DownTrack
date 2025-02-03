import React, { useState, useEffect } from "react";
import { Card, Typography } from "@material-tailwind/react";
import Select from "react-select";
import StatisticsChart from "@/components/charts/StatisticsChart";
import api from "@/middlewares/api";

const TechnicianComparison = () => {
  const [technicians, setTechnicians] = useState([]);
  const [selectedTechnicians, setSelectedTechnicians] = useState([]);
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchTechnicians();
  }, []);

  const fetchTechnicians = async () => {
    try {
      const response = await api("/Technicians");
      if (!response.ok) throw new Error("Failed to fetch technicians");
      const data = await response.json();
      setTechnicians(data);
    } catch (err) {
      setError("Error loading technicians");
    }
  };

  const fetchStats = async () => {
    if (selectedTechnicians.length === 0) return;
    setLoading(true);
    try {
      const ids = selectedTechnicians.map((t) => t.value).join(",");
      const response = await api(`/TechnicianComparison?ids=${ids}`);
      if (!response.ok) throw new Error("Failed to fetch stats");
      const data = await response.json();
      setStats(data);
    } catch (err) {
      setError("Error loading statistics");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchStats();
  }, [selectedTechnicians]);

  const evaluationChart = {
    type: "bar",
    series: ["Good", "Regular", "Bad"].map((category) => ({
      name: category,
      data: selectedTechnicians.map((t) => stats?.[t.value]?.evaluations[category] || 0),
    })),
    options: {
      xaxis: {
        categories: selectedTechnicians.map((t) => t.label),
      },
      colors: ["#4CAF50", "#FFC107", "#F44336"],
      legend: { position: "top" },
    },
  };

  const maintenanceChart = {
    type: "line",
    series: selectedTechnicians.map((t) => ({
      name: t.label,
      data: stats?.[t.value]?.maintenanceHistory || [],
    })),
    options: {
      xaxis: {
        categories: stats?.dates || [],
        title: { text: "Time" },
      },
      colors: ["#008FFB", "#FF4560", "#00E396", "#775DD0"],
      legend: { position: "top" },
    },
  };

  return (
    <div className="mt-8">
      <Typography variant="h5" className="mb-4">Compare Technicians</Typography>
      <Select
        isMulti
        options={technicians.map((t) => ({ value: t.id, label: t.name }))}
        onChange={setSelectedTechnicians}
        className="mb-6"
        placeholder="Select technicians..."
      />
      {loading && <div>Loading statistics...</div>}
      {error && <div>{error}</div>}
      {stats && (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <Card className="p-4">
            <Typography variant="h6" className="mb-4">Evaluation Comparison</Typography>
            <StatisticsChart chart={evaluationChart} />
          </Card>
          <Card className="p-4">
            <Typography variant="h6" className="mb-4">Maintenance History</Typography>
            <StatisticsChart chart={maintenanceChart} />
          </Card>
        </div>
      )}
    </div>
  );
};

export default TechnicianComparison;
