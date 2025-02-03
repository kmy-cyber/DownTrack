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
import { convertDateFormat } from "@/utils/changeDateFormat";
import { PDFDocument, StandardFonts, rgb } from "pdf-lib";

export function Reports() {
    const [reportType, setReportType] = useState("");
    const [startDate, setStartDate] = useState("");
    const [reportData, setReportData] = useState([]);
    const [extraField, setExtraField] = useState("");
    const [sections, setSections] = useState([]);
    const [departments, setDepartments] = useState([]);
    const [technicians, setTechnicians] = useState([]);
    const [equipmentsHistory, setEquipmentsHistory] = useState([]);
    const [equipments, setEquipments] = useState([]);
    const [selectedSection1, setSelectedSection1] = useState("");
    const [selectedSection2, setSelectedSection2] = useState("");
    const [selectedTechnician, setSelectedTechnician] = useState("");
    const [selectedDepartment, setSelectedDepartment] = useState("");
    const [columnWidths, setColumnWidths] = useState([]);
    const { user } = useAuth();
    console.log(selectedSection1);

    const userRole = user?.role?.toLowerCase() || "";
    const isDirector = userRole === "director";
    const isManager = userRole === "sectionmanager";
    const isTechnician = userRole === "technician";
    const isReceptor = userRole === "receptor";

    const reportOptions = [
        ...(isDirector
            ? [
                  { value: "inventory-status", label: "Inventory Status" },
                  {
                      value: "staff-effectiveness",
                      label: "Staff Effectiveness",
                  },
                  {
                      value: "last-year-decommissions",
                      label: "Last Year Decommissions",
                  },
                  {
                    value: "equipment-to-be-replaced",
                    label: "Equipment to be Replaced"
                  },
                  {
                      value: "transfers-between-sections",
                      label: "Transfer Between Sections",
                  },
                  {
                      value: "technician-evaluations",
                      label: "Technician Evaluations",
                  },
                  { value: "equipment-sent-to", label: "Equipments Sent To" },
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
        ...(isTechnician
            ? [
                  {
                      value: "maintenances-performed",
                      label: "Maintenances Performed",
                  },
              ]
            : []),
        // {
        //     value: "maintenances-performed-on-equipment",
        //     label: "Maintenances Performed on Equipment",
        // },
        // {
        //     value: "equipments-to-be-replaced",
        //     label: "Equipments to be Replaced",
        // },
    ];

    useEffect(() => {
        setStartDate(new Date().toISOString().split("T")[0]);
    }, []);

    const handleDateDisplay = (dateString) => {
        return convertDateFormat(dateString);
    };

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
        } else if (reportType === "maintenances-performed") {
            fetchEquipmentsHistory();
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
            setDepartments((prevDepartment) => {
                if (!data.some((d) => d.id === selectedDepartment)) {
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
            const response = await api(`/Technician/GetPaged?PageNumber=1&PageSize=99999`);
            const data = await response.json();
            setTechnicians((prevTechnician) =>{
                if(!data.items.some((t) => t.id === selectedTechnician)){
                    setSelectedTechnician("");
                }
                console.log(`TECHNICIAN: ${selectedTechnician}`);
                return data.items;
            });
        } catch (error) {
            console.error("Error fetching technicians:", error);
        }
    };

    const fetchEquipmentsHistory = async () => {
        try {
            const response = await api(
                `/DoneMaintenance/Get_Maintenances_By_Technician_Status?PageNumber=1&PageSize=1000&technicianId=${user.id}&IsFinish=false`,
                {
                    method: "GET",
                },
            );
            const data = await response.json();
            setEquipmentsHistory(data);
        } catch (error) {
            console.error("Error fetching equipments:", error);
        }
    };

    const fetchEquipments = async () => {
        try {
            const response = await api("/Equipment/GET");
            const data = await response.json();
            setEquipments(data);
        } catch (error) {
            console.error("Error fetching equipments:", error);
        }
    };

    // Función para generar el reporte de mantenimientos realizados
    const generateMaintenancesPerformedReport = async () => {
        try {
            const response = await api(
                `/DoneMaintenance/Get_Maintenances_By_Technician_Status?PageNumber=1&PageSize=1000&technicianId=${user.id}&IsFinish=true`,
            );
            const dataResponse = await response.json();
            const data = dataResponse.items.map((item) => ({
                Technician: item.technicianUserName,
                Type: item.type,
                Equipment: item.equipmentName,
                Date: handleDateDisplay(item.date),
                Cost: item.cost,
            }));

            setColumnWidths([100, 100, 100, 100, 100]);
            setReportData(data);
        } catch (error) {
            console.error("Error fetching maintenances performed:", error);
        }
    };

    // Función para generar el reporte de transferencias entre secciones
    const generateTransfersBetweenSectionsReport = async () => {
        try {
            const response = await api(
                `/Transfer/Get_Transfer_Between_Sections?PageNumber=1&PageSize=10000&sectionId1=${selectedSection1}&sectionId2=${selectedSection2}`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items);
            const data = dataResponse.items.map((item) => ({
                EId: item.equipmentId,
                EName: item.equipmentName,
                Receptor: item.equipmentReceptorUserName,
                Supervisor: item.shippingSupervisorName,
                Date: item.date.split('T')[0]
            }));
            setColumnWidths([100,100,100,100,100])
            setReportData(data);
        } catch (error) {
            console.error("Error fetching transfers between sections:", error);
        }
    };

    // Función para generar el reporte de evaluaciones de técnicos
    const generateTechnicianEvaluationsReport = async () => {
        try {
            const response = await api(
                `/Evaluation/Get_Evaluation_By_TechnicianId?PageNumber=1&PageSize=9999&technicianId=${selectedTechnician}`,
            );
            const dataResponse = await response.json();
            const data = dataResponse.items.map((item) => ({
                Username: item.technicianUserName,
                Evaluator: item.sectionManagerUserName,
                Evaluation: item.description
            }));
            setColumnWidths([100, 100, 100]);
            setReportData(data);
            
        } catch (error) {
            console.error("Error fetching technician evaluations:", error);
        }
    };

     // Función para generar el reporte de equipos a ser reemplazados
     const generateToBeReplacedEquipmentReport = async () => {
        try {
            const response = await api(
                `/Equipment/Equipment_With_More_Than_Three_Maintenances_In_Last_Year?PageNumber=1&PageSize=99999`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items)
            const data = dataResponse.items.map((item) => ({
                Id: item.id,
                Name: item.name,
                Type: item.type,
                AcqDate: item.dateOfadquisition.split('T')[0],
                Department: item.departmentName,
                Section: item.sectionName,
            }));
            setColumnWidths([30, 100, 100, 100, 100]);
            setReportData(data);
            console.log(data)
        } catch (error) {
            console.error("Error fetching equipment to be replaced:", error);
        }
    };

    // Función para generar reporte de bajas de los equipos en el ultimo año
    const generateLastYearDecommissionsReport = async () => {
        try {
            const response = await api(
                `/EquipmentDecommissioning/Get_Decomissions_Last_Year?PageNumber=1&PageSize=100000`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items)
            const data = dataResponse.items.map((item) => ({
                EId: item.equipmentId,
                EName: item.equipmentName,
                EType: item.equipmentType,
                Technician: item.technicianUserName,
                Receptor: item.receptorUserName,
                Cause: item.cause,
                Date: item.date.split('T')[0],
            }));
            setColumnWidths([30, 80, 70, 100, 80, 75, 100]);
            setReportData(data);
            console.log(data)
        } catch (error) {
            console.error("Error fetching equipment to be replaced:", error);
        }
    };

    // Función para generar reporte de intervenciones de un técnico
    const generateTechnicianInterventionsReport = async () => {
        try {
            const response = await api(
                `/DoneMaintenance/Get_Maintenances_By_Technician_Status?PageNumber=1&PageSize=99999&technicianId=${selectedTechnician}&IsFinish=true`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items)
            const data = dataResponse.items.map((item) => ({
                EquipmentId: item.equipmentId,
                EquipmentName: item.equipmentName,
                UserName: item.technicianUserName,
                Cost: item.cost,
                Date: item.date.split('T')[0],
            }));
            setColumnWidths([100, 100, 100, 100, 100]);
            setReportData(data);
            console.log(data)
        } catch (error) {
            console.error("Error fetching equipment to be replaced:", error);
        }
    };

    // Función para generar reporte del estado del inventario
    const generateInventoryStatusReport = async () => {
        try {
            const response = await api(
                `/Equipment/GetPaged?PageNumber=1&PageSize=1000`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items)
            const data = dataResponse.items.map((item) => ({
                EId: item.id,
                EquipmentName: item.name,
                SectionName: item.sectionName,
                Status: item.status,
                Department: item.departmentName,
                Date: item.dateOfadquisition.split('T')[0],
            }));
            setColumnWidths([50, 100, 100, 70, 100]);
            setReportData(data);
            console.log(data)
        } catch (error) {
            console.error("Error fetching equipment to be replaced:", error);
        }
    };

    // Función para generar reporte de las bajas tecnicas
    const generateDecommissionsReport = async () => {
        try {
            const response = await api(
                `/EquipmentDecommissioning/Get_Paged_Accepted?PageNumber=1&PageSize=100000`,
            );
            const dataResponse = await response.json();
            console.log(dataResponse.items);
            const data = dataResponse.items.map((item) => ({
                EId: item.equipmentId,
                EName: item.equipmentName,
                Type: item.equipmentType,
                Technician: item.technicianUserName,
                Receptor: item.receptorUserName,
                Cause: item.cause,
                Date: item.date.split('T')[0]
            }));
            setColumnWidths([30, 100, 60, 100, 80,60,100]);
            setReportData(data);
            console.log(data)
        } catch (error) {
            console.error("Error fetching equipment to be replaced:", error);
        }
    };

    // Función para generar el reporte de equipos enviados a un departamento
    const generateEquipmentSentToReport = async () => {
        try {
            const response = await api(
                `/Equipment/Get_Equipments_Sent_To?departmentId=${selectedDepartment}`,
            );
            const data = await response.json();
            setReportData(data);
        } catch (error) {
            console.error("Error fetching equipments sent to:", error);
        }
    };

    // Función para generar el reporte de mantenimientos realizados en un equipo
    const generateMaintenancesPerformedOnEquipmentReport = async () => {
        try {
            const response = await api(
                `/DoneMaintenance/Get_Maintenances_By_Equipment?equipmentId=${extraField}`,
            );
            const data = await response.json();
            setReportData(data);
        } catch (error) {
            console.error(
                "Error fetching maintenances performed on equipment:",
                error,
            );
        }
    };

    // Función principal para generar el reporte según el tipo seleccionado
    const generateReport = async () => {
        switch (reportType) {
            case "inventory-status":
                await generateInventoryStatusReport();
                break;
            case "last-year-decommissions":
                await generateLastYearDecommissionsReport();
                break;
            case "decommissions":
                await generateDecommissionsReport();
                break;
            case "maintenances-performed":
                await generateMaintenancesPerformedReport();
                break;
            case "transfers-between-sections":
                await generateTransfersBetweenSectionsReport();
                break;
            case "equipment-to-be-replaced":
                await generateToBeReplacedEquipmentReport();
                break;
            case "technician-evaluations":
                await generateTechnicianEvaluationsReport();
                break;
            case "technician-interventions":
                await generateTechnicianInterventionsReport();
                break;
            case "equipment-sent-to":
                await generateEquipmentSentToReport();
                break;
            case "maintenances-performed-on-equipment":
                await generateMaintenancesPerformedOnEquipmentReport();
                break;
            default:
                alert("Tipo de reporte no válido.");
                break;
        }
    };

    const handleExportPDF = async () => {
        if (reportData.length === 0) {
            alert("No hay datos para exportar.");
            return;
        }

        // Crear un nuevo documento PDF
        const pdfDoc = await PDFDocument.create();
        const page = pdfDoc.addPage();
        const { width, height } = page.getSize();

        // Configuración de estilos
        const font = await pdfDoc.embedFont(StandardFonts.Helvetica);
        const font_bold = await pdfDoc.embedFont(StandardFonts.HelveticaBold);
        const fontSize = 10;
        const titleFontSize = 18;
        const headerFontSize = 12;
        const textColor = rgb(0.2, 0.2, 0.2); // Gris oscuro
        const headerColor = rgb(0, 0.5, 0.8); // Azul
        const titleColor = rgb(0, 0.4, 0.6); // Azul más oscuro
        const borderColor = rgb(0, 0, 0); // Negro
        const backgroundColor = rgb(0.95, 0.95, 0.95); // Gris claro

        const margin = 50;
        const borderThickness = 45; // Ancho del borde negro
        let currentY = height - margin;

        // Dibujar borde superior negro
        page.drawRectangle({
            x: 0,
            y: height - borderThickness,
            width: width,
            height: borderThickness,
            color: borderColor,
        });

        // Cargar y dibujar el logo
        const logoUrl = "../../../public/img/logoDT.png"; // URL del logo
        const logoImageBytes = await fetch(logoUrl).then((res) =>
            res.arrayBuffer(),
        );
        const logoImage = await pdfDoc.embedPng(logoImageBytes);

        const logoWidth = 80;
        const logoHeight = 40;
        const logoX = (width - logoWidth) / 2;
        const logoY = height - logoHeight - 2; // Encima del borde negro

        page.drawImage(logoImage, {
            x: logoX,
            y: logoY,
            width: logoWidth,
            height: logoHeight,
        });

        // Ajustar espacio debajo del logo
        currentY = logoY - 30;

        // Título del reporte
        page.drawText("Reporte Oficial: DownTrack", {
            x: margin,
            y: currentY,
            size: titleFontSize,
            font,
            color: titleColor,
        });

        currentY -= titleFontSize + 15;

        // Línea de fecha y emisor
        const reportDate = new Date().toLocaleDateString();
        page.drawText(`Fecha: ${reportDate}`, {
            x: margin,
            y: currentY,
            size: fontSize,
            font_bold,
            color: textColor,
        });
        page.drawText(`Emitido por: ${user.name}`, {
            x: width - margin - 150,
            y: currentY,
            size: fontSize,
            font_bold,
            color: textColor,
        });

        currentY -= fontSize + 50;

        // Nombre de la tabla
        page.drawText(`Datos de ${reportType}`, {
            x: margin,
            y: currentY,
            size: headerFontSize,
            font_bold,
            color: headerColor,
        });

        currentY -= headerFontSize + 20;

        // Encabezado de la tabla
        const headers = Object.keys(reportData[0]);

        // Dibujar fondo del encabezado de la tabla
        page.drawRectangle({
            x: margin,
            y: currentY - 5,
            width: width - 2 * margin,
            height: headerFontSize + 10,
            color: backgroundColor,
        });

        // Dibujar encabezados de la tabla
        headers.forEach((header, index) => {
            page.drawText(header, {
                x:
                    margin +
                    columnWidths.slice(0, index).reduce((a, b) => a + b, 0) +
                    5,
                y: currentY,
                size: headerFontSize,
                font,
                color: headerColor,
            });
        });

        // Dibujar línea debajo del encabezado
        page.drawLine({
            start: { x: margin, y: currentY - 5 },
            end: { x: width - margin, y: currentY - 5 },
            thickness: 1,
            color: borderColor,
        });

        currentY -= headerFontSize + 15;

        // Datos de la tabla
        reportData.forEach((row) => {
            const rowData = headers.map((header) => row[header].toString());

            // Dibujar fondo de la fila
            page.drawRectangle({
                x: margin,
                y: currentY - 5,
                width: width - 2 * margin,
                height: fontSize + 10,
                color: backgroundColor,
            });

            // Dibujar datos de la fila
            rowData.forEach((cell, index) => {
                const cellX =
                    margin +
                    columnWidths.slice(0, index).reduce((a, b) => a + b, 0) +
                    5;
                const cellWidth = columnWidths[index] - 10;

                const lines = splitTextToFitWidth(
                    cell,
                    cellWidth,
                    fontSize,
                    font,
                );

                lines.forEach((line, lineIndex) => {
                    page.drawText(line, {
                        x: cellX,
                        y: currentY - lineIndex * (fontSize + 2),
                        size: fontSize,
                        font,
                        color: textColor,
                    });
                });
            });

            currentY -= fontSize + 15;

            // Si el contenido se sale de la página, agregar una nueva página
            if (currentY < margin + 50) {
                const newPage = pdfDoc.addPage();
                currentY = height - margin;

                // Dibujar encabezado de la tabla en la nueva página
                headers.forEach((header, index) => {
                    newPage.drawText(header, {
                        x:
                            margin +
                            columnWidths
                                .slice(0, index)
                                .reduce((a, b) => a + b, 0) +
                            5,
                        y: currentY,
                        size: headerFontSize,
                        font,
                        color: headerColor,
                    });
                });

                // Dibujar línea debajo del encabezado
                newPage.drawLine({
                    start: { x: margin, y: currentY - 5 },
                    end: { x: width - margin, y: currentY - 5 },
                    thickness: 1,
                    color: borderColor,
                });

                currentY -= headerFontSize + 15;
            }
        });

        // Dibujar borde inferior negro
        page.drawRectangle({
            x: 0,
            y: margin - borderThickness,
            width: width,
            height: borderThickness,
            color: borderColor,
        });

        // Pie de página
        page.drawText("DownTrack - Todos los derechos reservados", {
            x: margin,
            y: margin - 20,
            size: fontSize,
            font,
            color: rgb(0.9, 0.9, 0.9),
        });

        // Guardar el PDF
        const pdfBytes = await pdfDoc.save();
        const blob = new Blob([pdfBytes], { type: "application/pdf" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = `${reportType}_report.pdf`;
        link.click();
    };

    // Función para dividir el texto en varias líneas si es demasiado largo
    const splitTextToFitWidth = (text, maxWidth, fontSize, font) => {
        const words = text.split(" ");
        const lines = [];
        let currentLine = words[0];

        for (let i = 1; i < words.length; i++) {
            const word = words[i];
            const width = font.widthOfTextAtSize(
                currentLine + " " + word,
                fontSize,
            );
            if (width < maxWidth) {
                currentLine += " " + word;
            } else {
                lines.push(currentLine);
                currentLine = word;
            }
        }
        lines.push(currentLine);
        return lines;
    };

    return (
        <div className="mb-8 mt-12 flex flex-col gap-12">
            <Card>
                <CardHeader
                    variant="gradient"
                    color="gray"
                    className="mb-4 p-6"
                >
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
                                    <Option
                                        key={option.value}
                                        value={option.value}
                                    >
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
                                    onChange={(value) =>
                                        setSelectedSection1(value)
                                    }
                                >
                                    {sections.map((section) => (
                                        <Option
                                            key={section.id}
                                            value={section.id}
                                        >
                                            {section.name}
                                        </Option>
                                    ))}
                                </Select>
                                <Select
                                    label="Select Section 2"
                                    value={extraField}
                                    onChange={(value) =>
                                        setSelectedSection2(value)
                                    }
                                >
                                    {sections.map((section) => (
                                        <Option
                                            key={section.id}
                                            value={section.id}
                                        >
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
                                onChange={(value) => setSelectedTechnician(value)}
                            >
                                {technicians.map((technician) => (
                                    <Option
                                        key={technician.id}
                                        value={technician.id}
                                    >
                                        {technician.name}
                                    </Option>
                                ))}
                            </Select>
                        ) : null}

                        {reportType === "equipment-sent-to" && (
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
                                        <Option
                                            key={section.id}
                                            value={section.id}
                                        >
                                            {section.name}
                                        </Option>
                                    ))}
                                </Select>
                                <Select
                                    label="Select Department"
                                    value={extraField}
                                    onChange={(value) =>
                                        setSelectedDepartment(value)
                                    }
                                >
                                    {departments.map((department) => (
                                        <Option
                                            key={department.id}
                                            value={department.id}
                                        >
                                            {department.name}
                                        </Option>
                                    ))}
                                </Select>
                            </>
                        )}

                        {reportType ===
                            "maintenances-performed-on-equipment" && (
                            <Select
                                label="Select Equipment"
                                value={extraField}
                                onChange={(value) => setExtraField(value)}
                            >
                                {equipments.map((equipment) => (
                                    <Option
                                        key={equipment.id}
                                        value={equipment.id}
                                    >
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
                                        {Object.keys(reportData[0]).map(
                                            (key) => (
                                                <th
                                                    key={key}
                                                    className="border border-gray-200 bg-gray-50 px-4 py-2 text-gray-700"
                                                >
                                                    {key
                                                        .charAt(0)
                                                        .toUpperCase() +
                                                        key.slice(1)}
                                                </th>
                                            ),
                                        )}
                                    </tr>
                                </thead>
                                <tbody>
                                    {reportData.map((row, index) => (
                                        <tr
                                            key={index}
                                            className="hover:bg-gray-50"
                                        >
                                            {Object.values(row).map(
                                                (value, i) => (
                                                    <td
                                                        key={i}
                                                        className="border border-gray-200 px-4 py-2"
                                                    >
                                                        {value}
                                                    </td>
                                                ),
                                            )}
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                            <Button
                                color="blue"
                                onClick={handleExportPDF}
                                className="mt-4"
                            >
                                Export to PDF
                            </Button>
                        </div>
                    )}
                </CardBody>
            </Card>
        </div>
    );
}

export default Reports;
