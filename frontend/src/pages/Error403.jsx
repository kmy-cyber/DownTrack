import React from "react";
import { Typography, Button, Container, Box } from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { useNavigate } from "react-router-dom"; // Para redireccionar al usuario
import { useAuth } from "@/context/AuthContext";

const Error403 = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    console.log(user.role);
    const handleGoHome = () => {
        let url = "/dashboard/";
        switch (user.role.toLowerCase()) {
            case "director":
                url += "director/home";
                break;
            case "sectionmanager":
                url += "manager/home";
                break;
            case "technician":
                url += "technic/home";
                break;
            case "administrator":
                url += "admin/home";
                break;
            case "equipmentreceptor":
                url += "receptor/home";
                break;
        }
        navigate(url); // Redirige al usuario a la página de inicio
    };

    return (
        <Container
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center",
                height: "100vh",
                textAlign: "center",
                backgroundColor: "#f5f5f5",
            }}
        >
            <Box
                sx={{
                    backgroundColor: "white",
                    padding: "40px",
                    borderRadius: "10px",
                    boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.1)",
                }}
            >
                <LockOutlinedIcon
                    sx={{
                        fontSize: "80px",
                        color: "#ff4444",
                        marginBottom: "20px",
                    }}
                />
                <Typography
                    variant="h4"
                    sx={{
                        fontWeight: "bold",
                        color: "#333",
                        marginBottom: "10px",
                    }}
                >
                    Error 403: Forbidden
                </Typography>
                <Typography
                    variant="body1"
                    sx={{ color: "#666", marginBottom: "30px" }}
                >
                    You have no access to this page.
                </Typography>
                <Button
                    variant="contained"
                    onClick={handleGoHome}
                    sx={{
                        backgroundColor: "#ff4444",
                        color: "white",
                        fontWeight: "bold",
                        padding: "10px 30px",
                        borderRadius: "5px",
                        "&:hover": {
                            backgroundColor: "#cc0000",
                        },
                    }}
                >
                    Back to Home
                </Button>
            </Box>
        </Container>
    );
};

export default Error403;
