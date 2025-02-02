import { useState, useEffect } from "react";
import { useAuth } from "@/context/AuthContext";
import {
  Card,
  CardHeader,
  CardBody,
  Typography,
  Button,
  Select,
  Option,
  Input,
} from "@material-tailwind/react";
import api from "@/middlewares/api";

export function Reports() {
  const [reportType, setReportType] = useState("");
  const [startDate, setStartDate] = useState("");
  const [reportData, setReportData] = useState([]);
  const [extraField, setExtraField] = useState("");
  const [sections, setSections] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [technicians, setTechnicians] = useState([]);
  const [equipments, setEquipments] = useState([]);
  const [selectedSection1, setSelectedSection1] = useState("");
  const [selectedSection2, setSelectedSection2] = useState("");
  const [selectedDepartment, setSelectedDepartment] = useState("");
  const { user } = useAuth();

  const userRole = user?.role?.toLowerCase() || "";
  const isDirector = userRole === "director";
  const isManager = userRole === "sectionmanager";

  const reportOptions = [
    ...(isDirector
      ? [
          { value: "inventory", label: "Inventory Status" },
          { value: "staff-effectiveness", label: "Staff Effectiveness" },
          {
            value: "last-year-decommissions",
            label: "Last Year Decommissions",
          },
          {
            value: "transfers-between-sections",
            label: "Transfer Between Sections",
          },
          { value: "technician-evaluations", label: "Technician Evaluations" },
          { value: "equipments-sent-to", label: "Equipments Sent To" },
        ]
      : []),
    ...(isDirector || isManager
      ? [
          { value: "decommissions", label: "Decommissions" },
          {
            value: "technician-interventions",
            label: "Technician Interventions",
          },
        ]
      : []),
    {
      value: "maintenances-performed-on-equipment",
      label: "Maintenances Performed on Equipment",
    },
    { value: "equipments-to-be-replaced", label: "Equipments to be Replaced" },
  ];

  useEffect(() => {
    setStartDate(new Date().toISOString().split("T")[0]);
  }, []);

  useEffect(() => {
    if (reportType === "transfers-between-sections") {
      fetchSections();
    } else if (
      reportType === "technician-evaluations" ||
      reportType === "technician-interventions"
    ) {
      fetchTechnicians();
    } else if (reportType === "equipments-sent-to") {
      fetchSections();
    } else if (reportType === "maintenances-performed-on-equipment") {
      fetchEquipments();
    }
  }, [reportType]);

  const fetchSections = async () => {
    try {
      const response = await api("/Section/GET_ALL");
      const data = await response.json();
      setSections((prevSections) => {
        if (!data.some((s) => s.id === selectedSection1)) {
            setSelectedSection1("");
        }
        if (!data.some((s) => s.id === selectedSection2)) {
            setSelectedSection2("");
        }
        return data;
      });
    } catch (error) {
      console.error("Error fetching sections:", error);
    }
  };

  const fetchDepartments = async (sectionId) => {
    try {
      const response = await api(
        `/Department/GetAllDepartment_In_Section?sectionId=${sectionId}`,
      );
      const data = await response.json();
      setDepartments((prevDepartment) =>{
        if (!data.some((d) => d.id === selectedDepartment)){
            setSelectedDepartment("");
        }
        return data;
      });
    } catch (error) {
      console.error("Error fetching departments:", error);
    }
  };

  const fetchTechnicians = async () => {
    try {
      const response = await fetch("/api/technicians");
      const data = await response.json();
      setTechnicians(data);
    } catch (error) {
      console.error("Error fetching technicians:", error);
    }
  };

  const fetchEquipments = async () => {
    try {
      const response = await fetch("/api/equipments");
      const data = await response.json();
      setEquipments(data);
    } catch (error) {
      console.error("Error fetching equipments:", error);
    }
  };

  const generateReport = async () => {
    let queryParams = `?type=${reportType}`;
    if (extraField) queryParams += `&extra=${extraField}`;

    try {
      const response = await fetch(`/api/reports${queryParams}`);
      const data = await response.json();
      setReportData(data);
    } catch (error) {
      console.error("Error fetching report:", error);
    }
  };

  return (
    <div className="mb-8 mt-12 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Report Generator
          </Typography>
        </CardHeader>
        <CardBody>
          <div className="mb-6 flex flex-col gap-6">
            <div className="flex flex-col gap-4 md:flex-row">
              <Select
                label="Choose a Report"
                value={reportType}
                onChange={(value) => {
                  setReportType(value);
                  setExtraField("");
                  setSections([]);
                  setDepartments([]);
                  setTechnicians([]);
                  setEquipments([]);
                }}
              >
                {reportOptions.map((option) => (
                  <Option key={option.value} value={option.value}>
                    {option.label}
                  </Option>
                ))}
              </Select>
              <input
                type="text"
                value={startDate}
                readOnly
                className="rounded-lg border bg-gray-100 px-4 py-2 text-gray-700"
                placeholder="Start Date"
              />
            </div>

            {reportType === "transfers-between-sections" && (
              <>
                <Select
                  label="Select Section 1"
                  value={extraField}
                  onChange={(value) => setSelectedSection1(value)}
                >
                  {sections.map((section) => (
                    <Option key={section.id} value={section.id}>
                      {section.name}
                    </Option>
                  ))}
                </Select>
                <Select
                  label="Select Section 2"
                  value={extraField}
                  onChange={(value) => setSelectedSection2(value)}
                >
                  {sections.map((section) => (
                    <Option key={section.id} value={section.id}>
                      {section.name}
                    </Option>
                  ))}
                </Select>
              </>
            )}

            {reportType === "technician-evaluations" ||
            reportType === "technician-interventions" ? (
              <Select
                label="Select Technician"
                value={extraField}
                onChange={(value) => setExtraField(value)}
              >
                {technicians.map((technician) => (
                  <Option key={technician.id} value={technician.id}>
                    {technician.name}
                  </Option>
                ))}
              </Select>
            ) : null}

            {reportType === "equipments-sent-to" && (
              <>
                <Select
                  label="Select Section"
                  value={extraField}
                  onChange={(value) => {
                    setSelectedSection1(value);
                    fetchDepartments(value);
                  }}
                >
                  {sections.map((section) => (
                    <Option key={section.id} value={section.id}>
                      {section.name}
                    </Option>
                  ))}
                </Select>
                <Select
                  label="Select Department"
                  value={extraField}
                  onChange={(value) => setSelectedDepartment(value)}
                >
                  {departments.map((department) => (
                    <Option key={department.id} value={department.id}>
                      {department.name}
                    </Option>
                  ))}
                </Select>
              </>
            )}

            {reportType === "maintenances-performed-on-equipment" && (
              <Select
                label="Select Equipment"
                value={extraField}
                onChange={(value) => setExtraField(value)}
              >
                {equipments.map((equipment) => (
                  <Option key={equipment.id} value={equipment.id}>
                    {equipment.name}
                  </Option>
                ))}
              </Select>
            )}

            <Button color="gray" onClick={generateReport}>
              Generate Report
            </Button>
          </div>

          {Array.isArray(reportData) && reportData.length > 0 && (
            <div className="overflow-x-auto">
              <table className="w-full table-auto border-collapse border border-gray-200">
                <thead>
                  <tr>
                    {Object.keys(reportData[0]).map((key) => (
                      <th
                        key={key}
                        className="border border-gray-200 bg-gray-50 px-4 py-2 text-gray-700"
                      >
                        {key.charAt(0).toUpperCase() + key.slice(1)}
                      </th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  {reportData.map((row, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                      {Object.values(row).map((value, i) => (
                        <td
                          key={i}
                          className="border border-gray-200 px-4 py-2"
                        >
                          {value}
                        </td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </CardBody>
      </Card>
    </div>
  );
}

export default Reports;
