import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const CustomTooltip = ({ active, payload, label }) => {
    if (active && payload && payload.length) {
        return (
        <div className="custom-tooltip" style={{ backgroundColor: '#fff', padding: '10px', border: '1px solid #ccc' }}>
            <p className="label">{`${label}`}</p>
            <p className="value1">{`${payload[0].dataKey}: ${payload[0].value}`}</p>
            <p className="value2">{`${payload[1].dataKey}: ${payload[1].value}`}</p>
        </div>
        );
    }

    return null;
};

const CustomBarChart = ({ data, colors, dataKey1, dataKey2 }) => {
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
            <Bar dataKey={dataKey1} barSize={20} fill={colors.barFill} />
            <Bar dataKey={dataKey2} barSize={20} fill={colors.barFillSecondary} />
        </BarChart>
        </ResponsiveContainer>
    );
};

export default CustomBarChart;