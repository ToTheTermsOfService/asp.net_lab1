IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'UtilityServicesDB')
BEGIN
    CREATE DATABASE UtilityServicesDB;
END
GO

USE UtilityServicesDB;
GO
IF OBJECT_ID('dbo.Tenant_Service', 'U') IS NOT NULL DROP TABLE dbo.Tenant_Service;
IF OBJECT_ID('dbo.Service', 'U') IS NOT NULL DROP TABLE dbo.Service;
IF OBJECT_ID('dbo.Tenant', 'U') IS NOT NULL DROP TABLE dbo.Tenant;
GO

CREATE TABLE Tenant (
    tenant_id INT IDENTITY(1,1) PRIMARY KEY,
    last_name NVARCHAR(50) NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    middle_name NVARCHAR(50) NULL,
    personal_account NVARCHAR(20) UNIQUE NOT NULL,
    address NVARCHAR(100) NOT NULL,
    residents_count INT NOT NULL,
    apartment_area DECIMAL(6,2) NOT NULL
);

CREATE TABLE Service (
    service_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    billing_type NVARCHAR(20) CHECK (billing_type IN ('area','person')) NOT NULL,
    tariff DECIMAL(10,2) NOT NULL
);

CREATE TABLE Tenant_Service (
    tenant_service_id INT IDENTITY(1,1) PRIMARY KEY,
    tenant_id INT NOT NULL FOREIGN KEY REFERENCES Tenant(tenant_id),
    service_id INT NOT NULL FOREIGN KEY REFERENCES Service(service_id),
    calculated_amount DECIMAL(10,2) NULL
);
GO

INSERT INTO Tenant (last_name, first_name, middle_name, personal_account, address, residents_count, apartment_area)
VALUES
('Smith','John','Edward','ACC001','12 Green St, Apt 1',3,55.5),
('Johnson','Emily','Grace','ACC002','15 Oak St, Apt 2',2,48.0),
('Williams','David','Michael','ACC003','22 Pine St, Apt 5',4,62.3),
('Brown','Sophia','Anne','ACC004','35 Maple St, Apt 7',1,40.0),
('Jones','Daniel','Thomas','ACC005','44 Elm St, Apt 3',5,75.2),
('Garcia','Olivia','Maria','ACC006','56 Birch St, Apt 9',2,53.1),
('Miller','James','Robert','ACC007','61 Cedar St, Apt 11',3,58.7),
('Davis','Ava','Louise','ACC008','72 Walnut St, Apt 4',4,69.4),
('Rodriguez','Ethan','Paul','ACC009','88 Chestnut St, Apt 6',2,47.6),
('Martinez','Isabella','Rose','ACC010','95 Ash St, Apt 8',3,64.8);

INSERT INTO Service (name, billing_type, tariff)
VALUES
('Heating','area',12.50),
('Gas','area',7.80),
('Water Supply','person',50.00),
('Waste Disposal','person',20.00),
('Elevator Service','person',15.00);

INSERT INTO Tenant_Service (tenant_id, service_id, calculated_amount)
SELECT t.tenant_id, s.service_id,
       CASE s.billing_type
            WHEN 'area' THEN t.apartment_area * s.tariff
            WHEN 'person' THEN t.residents_count * s.tariff
       END
FROM Tenant t
JOIN Service s ON s.service_id IN (1,3,4);


INSERT INTO Tenant_Service (tenant_id, service_id, calculated_amount)
SELECT t.tenant_id, s.service_id,
       CASE s.billing_type
            WHEN 'area' THEN t.apartment_area * s.tariff
            WHEN 'person' THEN t.residents_count * s.tariff
       END
FROM Tenant t
JOIN Service s ON s.service_id IN (2,5);