import React from "react";
import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip, Bar, Legend } from "recharts";

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent, index }) => {
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
  const x = cx + radius * Math.cos(-midAngle * RADIAN);
  const y = cy + radius * Math.sin(-midAngle * RADIAN);

    return (
        <text x={x} y={y} fill="white" textAnchor={x > cx ? "start" : "end"} dominantBaseline="central">
          {`${(percent * 100).toFixed(0)}%`}
        </text>
    );
};

const PieChartComponent = ({ data }) => {
    return (
        <ResponsiveContainer width="100%" height={400}>
            <PieChart>
                <Pie
                    data={data}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={renderCustomizedLabel}
                    outerRadius={80}
                    fill="#8884d8"
                    dataKey="value"
                >
                    {data.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color || "#8884d8"} />
                    ))}
                </Pie>
                <Legend
                    iconSize={10}
                    verticalAlign="bottom"
                    align="middle"
                    layout="vertical"
                    
                />
            </PieChart>
        </ResponsiveContainer>

    );
};

export default PieChartComponent;
