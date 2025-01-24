-- Insertar datos de prueba en Employee
INSERT INTO Employee (Id, Name, UserRole) VALUES
(1, 'Alice Johnson', 'Administrator'),
(2, 'Bob Smith', 'Technician'),
(3, 'Carol White', 'SectionManager'),
(4, 'Dave Brown', 'EquipmentReceptor'),
(5, 'Eve Davis', 'Director'),
(6, 'Frank Moore', 'ShippingSupervisor');

-- Insertar datos de prueba en Sections
INSERT INTO Sections (Id, Name) VALUES
(1, 'IT'),
(2, 'Maintenance'),
(3, 'Logistics');

-- Insertar datos de prueba en Departments
INSERT INTO Departments (Id, SectionId, Name) VALUES
(1, 1, 'Software Development'),
(2, 1, 'Hardware Support'),
(3, 2, 'Preventive Maintenance'),
(4, 3, 'Shipping Coordination');

-- Insertar datos de prueba en Equipments
INSERT INTO Equipments (Id, Name, Type, Status, DateOfAdquisition) VALUES
(1, 'Laptop A', 'Electronics', 'activo', '2021-06-15'),
(2, 'Printer B', 'Peripherals', 'mantenimiento', '2020-12-01'),
(3, 'Server C', 'Electronics', 'baja', '2018-03-20');

-- Insertar datos de prueba en Maintenances
INSERT INTO Maintenances (Id, Type) VALUES
(1, 'Software Update'),
(2, 'Hardware Repair'),
(3, 'System Diagnostics');

-- Insertar datos de prueba en Technician
INSERT INTO Technician (Id, Specialty, Salary, ExpYears) VALUES
(2, 'Computer Hardware', 50000, 5),
(3, 'Software Engineering', 60000, 7);


-- Insertar datos de prueba en TransferRequests
INSERT INTO TransferRequests (EmployeeId, EquipmentId, Date, DepartmentId, SectionId) VALUES
(1, 1, '2025-01-10 09:00:00', 1, 1), -- Alice Johnson, Laptop A, Software Development, IT
(2, 2, '2025-01-11 10:30:00', 2, 1), -- Bob Smith, Printer B, Hardware Support, IT
(3, 3, '2025-01-12 14:45:00', 3, 2), -- Carol White, Server C, Preventive Maintenance, Maintenance
(4, 1, '2025-01-13 08:00:00', 4, 3), -- Dave Brown, Laptop A, Shipping Coordination, Logistics
(5, 2, '2025-01-14 11:15:00', 1, 1); -- Eve Davis, Printer B, Software Development, IT

