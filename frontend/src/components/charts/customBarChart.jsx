import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

    const CustomTooltip = ({ active, payload, label }) => {
    if (active && payload && payload.length) {
        return (
        <div className="custom-tooltip" style={{ backgroundColor: '#fff', padding: '10px', border: '1px solid #ccc' }}>
            <p className="label">{`${label} : ${payload[0].value}`}</p>
            <p className="desc">Información adicional sobre el punto seleccionado.</p>
        </div>
        );
    }

    return null;
    };

const CustomBarChart = ({ data, colors }) => {
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
        >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip content={<CustomTooltip />} />
            <Legend />
            <Bar dataKey="pv" barSize={20} fill={colors.barFill} />
            <Bar dataKey="uv" barSize={20} fill={colors.barFillSecondary} />
        </BarChart>
        </ResponsiveContainer>
    );
};

export default CustomBarChart;