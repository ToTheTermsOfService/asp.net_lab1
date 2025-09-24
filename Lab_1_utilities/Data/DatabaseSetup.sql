INSERT INTO Tenant (LastName, FirstName, MiddleName, PersonalAccount, address, ResidentsCount, ApartmentArea)
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

INSERT INTO Service (name, BillingType, tariff)
VALUES
('Heating','area',12.50),
('Gas','area',7.80),
('Water Supply','person',50.00),
('Waste Disposal','person',20.00),
('Elevator Service','person',15.00);

INSERT INTO Tenant_Service (TenantId, ServiceId, CalculatedAmount)
SELECT t.Id, s.Id,
       CASE s.BillingType
            WHEN 'area' THEN t.ApartmentArea * s.tariff
            WHEN 'person' THEN t.ResidentsCount * s.tariff
       END
FROM Tenant t
JOIN Service s ON s.Id IN (1,3,4);


INSERT INTO Tenant_Service (TenantId, ServiceId, CalculatedAmount)
SELECT t.Id, s.Id,
       CASE s.BillingType
            WHEN 'area' THEN t.ApartmentArea * s.tariff
            WHEN 'person' THEN t.ResidentsCount * s.tariff
       END
FROM Tenant t
JOIN Service s ON s.Id IN (2,5);