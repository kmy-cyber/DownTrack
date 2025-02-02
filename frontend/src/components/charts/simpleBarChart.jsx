import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

// Componente principal del gráfico de barras
const SimpleBarChart = ({ data, colors }) => {
    return (
        <ResponsiveContainer width="100%" height={400}>
        <BarChart
            data={data}
            margin={{
            top: 5,
            right: 30,
            left: 20,
            bottom: 5,
            }}
            barSize={20}
        >
            <XAxis dataKey="name" scale="point" padding={{ left: 10, right: 10 }} />
            <YAxis />
            <Tooltip />
            <Legend />
            <CartesianGrid strokeDasharray="3 3" />
            <Bar dataKey="pv" fill={colors.barFill} background={{ fill: colors.background }} />
        </BarChart>
        </ResponsiveContainer>
    );
};

export default SimpleBarChart;