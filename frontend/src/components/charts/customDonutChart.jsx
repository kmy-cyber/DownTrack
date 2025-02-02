import React from "react";
import { PieChart, Pie, Cell, Tooltip, Legend, Label } from "recharts";

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#A28DFF"];

const DonutChart = ({ data }) => {
  // Calcular el total para poder mostrar el porcentaje
  const totalValue = data.reduce((sum, entry) => sum + entry.value, 0);

  return (
    <PieChart width={300} height={300}>
      <Pie
        data={data}
        cx="50%"
        cy="50%"
        innerRadius={70}
        outerRadius={100}
        fill="#8884d8"
        paddingAngle={5}
        dataKey="value"
      >
        {data.map((entry, index) => (
          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
        ))}
        {/* Agregar etiquetas dentro del gráfico para mostrar valor y porcentaje */}
        {data.map((entry, index) => (
          <Label
            key={`label-${index}`}
            position="center"
            fontSize={14}
            fill="#fff"
            value={`${entry.name}: ${entry.value} items (${((entry.value / totalValue) * 100).toFixed(2)}%)`}
          />
        ))}
      </Pie>
      <Tooltip
        formatter={(value, name, props) => [
          `${props.payload.name}: ${value} items`,
          `${((value / totalValue) * 100).toFixed(2)}%`,
        ]}
      />
      <Legend />
    </PieChart>
  );
};

export default DonutChart;
