-- DROP DATABASE `db-assist`;
CREATE SCHEMA `db-assist` ;
USE `db-assist`;
-- Creación de tablas maestras

-- DROP TABLE IF EXISTS EventStatus;
CREATE TABLE EventStatus (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);

-- DROP TABLE IF EXISTS EventType;

CREATE TABLE EventType (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    Category VARCHAR(100)
);

CREATE TABLE VoucherStatus (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);

CREATE TABLE BillStatus (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255)
);

CREATE TABLE EventProviderStatus (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255)
);
-- Creación de tablas de negocio
-- DROP TABLE IF EXISTS Vouchers;
CREATE TABLE Vouchers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    VoucherName VARCHAR(100) NOT NULL,
    Plan VARCHAR(100),
    DateOfIssue DATETIME NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IssueName VARCHAR(100),
    IdVoucherStatus INT,
    Destination VARCHAR(100),
    IsCoPayment Boolean,
    CONSTRAINT fk_voucher_status FOREIGN KEY (IdVoucherStatus) REFERENCES VoucherStatus(Id)
);

-- DROP TABLE IF EXISTS CustomerTrip;
CREATE TABLE CustomerTrip (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdClinicHistory INT,
    Names VARCHAR(100) NOT NULL,
    LastNames VARCHAR(100) NOT NULL,
    CountryOfBirth VARCHAR(100),
	Gender VARCHAR(50) NOT NULL,
    DateOfBirth DATE   
);

CREATE TABLE Bill (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdDocument INT,
    IdBillStatus INT NOT NULL,
    Value DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdBillStatus) REFERENCES BillStatus(Id)
);

-- DROP TABLE IF EXISTS Guarantees;
CREATE TABLE Guarantees (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CreatedDate DATETIME NOT NULL,                      
    UpdateDate DATETIME,                                
    EndDate DATETIME,                                   
    LocalCurrencyValue DECIMAL(10,2) NOT NULL,          
    LocalCurrency VARCHAR(3) NOT NULL,                  
    ExchangeRateUSD DECIMAL(10,4) NOT NULL,             
    GuaranteeValueUSD DECIMAL(10,2) GENERATED ALWAYS AS (LocalCurrencyValue * ExchangeRateUSD) STORED, 
    Email VARCHAR(255) NOT NULL,                        
    IdDocument VARCHAR(255) NOT NULL,                       
    CreatedBy VARCHAR(255) NOT NULL,                     
    RolUser VARCHAR(255)     
);

-- Creación de tablas relacionadas

CREATE TABLE Events (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CreatedDate DATETIME NOT NULL,
    UpdateDate DATETIME,
    EndDate DATETIME,
    IdEventStatus INT NOT NULL,
    IdEventType INT NOT NULL,
    IdVoucher INT NOT NULL,
    IssueName VARCHAR(100),
    IdCustomerTrip INT NOT NULL,
    Country VARCHAR(100),
    State VARCHAR(100),
    City VARCHAR(100),
    NearTo VARCHAR(255),
    Address VARCHAR(255),
    Longitude DECIMAL(9,6),
    Latitude DECIMAL(9,6),
    EventPriority VARCHAR(50),
    FOREIGN KEY (IdEventStatus) REFERENCES EventStatus(Id),
    FOREIGN KEY (IdEventType) REFERENCES EventType(Id),
    FOREIGN KEY (IdVoucher) REFERENCES Vouchers(Id),
    FOREIGN KEY (IdCustomerTrip) REFERENCES CustomerTrip(Id)
);

CREATE TABLE ContactEmergency (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdCustomerTrip INT NOT NULL,
    Names VARCHAR(100) NOT NULL,
    LastNames VARCHAR(100) NOT NULL,
    Emails VARCHAR(255),
    Phones VARCHAR(50),
    IsPrincipal BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (IdCustomerTrip) REFERENCES CustomerTrip(Id)
);

-- DROP TABLE IF EXISTS ContactInformation;
CREATE TABLE ContactInformation (
    IdCustomerTrip INT NOT NULL,
    Type VARCHAR(50) NOT NULL,
    DataType VARCHAR(50) NOT NULL,
    Data VARCHAR(50) NOT NULL,
    FOREIGN KEY (IdCustomerTrip) REFERENCES CustomerTrip(Id)
);

-- DROP TABLE IF EXISTS EventProvider;
CREATE TABLE EventProvider (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    IdEvent INT NOT NULL,
    IdProvider INT NOT NULL,
    IdProviderLocation INT NOT NULL,
    StartDate DATETIME NOT NULL,
    UpdateDate DATETIME,
    EndDate DATETIME,
    Details TEXT,
    CoPayment INT,
    IdEventProviderStatus INT,
    FOREIGN KEY (IdEventProviderStatus) REFERENCES EventProviderStatus(Id)
);

CREATE TABLE `devassists`.`PhoneConsultation` (
  `Id` INT NOT NULL,
  `IdEvent` INT NULL,
  `IdPhoneConsultationSk` VARCHAR(100) NULL,
  `IdUser` INT NULL,
  `NameDoctor` VARCHAR(50) NULL,
  `EmailDoctor` VARCHAR(50) NULL,
  `CreatedRequest` DATETIME NULL,
  `CreatedEnd` DATETIME NULL,
  PRIMARY KEY (`Id`));

-- DROP TABLE IF EXISTS EventProviderGuarantees;
CREATE TABLE EventProviderGuarantees (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    IdGuarantees INT NOT NULL,
    IdEvent INT NOT NULL,
    IdProvider INT NOT NULL
    
    -- FOREIGN KEY (IdGuarantees) REFERENCES Guarantees(Id),
    -- FOREIGN KEY (IdEvent) REFERENCES Events(Id)
    -- No se define clave foránea para IdProvider
);

CREATE TABLE BillsGuarantees (
    IdBill INT NOT NULL,
    IdGuarantees INT NOT NULL,
    PRIMARY KEY (IdBill, IdGuarantees),
    FOREIGN KEY (IdBill) REFERENCES Bill(Id),
    FOREIGN KEY (IdGuarantees) REFERENCES Guarantees(Id)
);

INSERT INTO EventType (Id, Name, Description, Category) VALUES
(1, 'Asistencia médica', 'Servicio de atención médica inmediata para situaciones de emergencia o consulta médica general.', 'Asistencia Médica'),
(2, 'Odontología', 'Servicios odontológicos de urgencia y rutinarios en caso de necesidad durante un viaje.', 'Asistencia Médica'),
(3, 'Asistencia psicológica', 'Apoyo y asesoramiento psicológico accesible en momentos de crisis o por necesidades continuas.', 'Asistencia Médica'),
(4, 'Regreso anticipado', 'Coordinación y gestión del regreso anticipado a casa por razones médicas graves.', 'Asistencia Médica'),
(5, 'Medicamentos', 'Provisión o reembolso de medicamentos recetados necesarios durante el viaje.', 'Asistencia Médica'),
(6, 'Repatriación de restos', 'Gestión de la repatriación de restos mortales a su país de origen en caso de fallecimiento.', 'Asistencia Médica'),
(7, 'Traslado de un familiar', 'Organización del viaje de un familiar para acompañar o cuidar a la persona asegurada en caso de hospitalización.', 'Asistencia Médica'),
(8, 'Traslado sanitario', 'Transporte médico especializado, incluyendo ambulancia aérea si es necesario.', 'Asistencia Médica'),
(9, 'Repatriación sanitaria', 'Repatriación médica a su país de residencia para continuar tratamiento.', 'Asistencia Médica'),
(10, 'Hotel por convalecencia', 'Alojamiento temporal para convalecencia después de una intervención médica.', 'Asistencia Médica'),
(11, 'Cuarentena Médica', 'Cobertura de gastos adicionales por alojamiento debido a cuarentena médica obligatoria.', 'Asistencia Médica'),
(12, 'Hotel de un familiar', 'Pago de los gastos de hotel para un familiar que acompaña al asegurado durante su hospitalización.', 'Asistencia Médica'),
(13, 'Demora de equipaje', 'Gestión de reclamaciones y compensación por demora en la entrega de equipaje.', 'Asistencia de equipajes'),
(14, 'Daño de equipaje', 'Asistencia y compensación por daño al equipaje durante un viaje.', 'Asistencia de equipajes'),
(15, 'Pérdida de equipaje', 'Asistencia en la localización y compensación por pérdida de equipaje.', 'Asistencia de equipajes'),
(16, 'Localización de equipaje', 'Servicios para rastrear y recuperar equipaje perdido.', 'Asistencia de equipajes'),
(17, 'Vuelo demorado', 'Gestión de compensaciones y arreglos necesarios debido a demoras significativas en vuelos.', 'Asistencia de vuelos'),
(18, 'Pérdida de vuelo', 'Asistencia para reprogramar vuelos perdidos y gestión de los costes implicados.', 'Asistencia de vuelos'),
(19, 'Cancelación de viaje', 'Reembolso de gastos de viaje no reembolsables en caso de cancelación justificada.', 'Asistencia de vuelos'),
(20, 'Reprogramación de viaje', 'Ayuda con la reprogramación de viajes en caso de interrupción o necesidad de cambio.', 'Asistencia de vuelos'),
(21, 'Interrupción de viaje', 'Cobertura económica por interrupción inesperada del viaje debido a emergencias.', 'Asistencia de vuelos'),
(22, 'Repatriación quiebra de aerolínea', 'Soporte en caso de quiebra de la aerolínea, incluyendo reprogramación de vuelos o reembolsos.', 'Asistencia de vuelos'),
(23, 'Concierge', 'Servicios de concierge para asistencia general, reservas y recomendaciones personalizadas.', 'Otras asistencias'),
(24, 'Pérdida de pasaporte', 'Asistencia en la gestión de la pérdida o robo de pasaportes, incluyendo trámites consulares.', 'Otras asistencias'),
(25, 'Informaciones', 'Acceso a información útil y asesoramiento para planificar y disfrutar de un viaje sin contratiempos.', 'Otras asistencias'),
(26, 'Seguro de electrónicos', 'Cobertura de seguro para dispositivos electrónicos en caso de daño o robo durante el viaje.', 'Otras asistencias'),
(27, 'Pet Hotel', 'Alojamiento y cuidado de mascotas durante viajes del asegurado.', 'Otras asistencias'),
(28, 'Asistencia para la mascota', 'Servicios de emergencia y cuidado veterinario para mascotas mientras viajan con su dueño.', 'Otras asistencias'),
(29, 'Extravío de documentos', 'Asistencia para recuperar o reemplazar documentos importantes extraviados durante el viaje.', 'Otras asistencias'),
(30, 'Sustitución de ejecutivos', 'Gestión de reemplazo temporal de ejecutivos en caso de emergencias que impidan sus responsabilidades laborales.', 'Otras asistencias'),
(31, 'Compra protegida', 'Seguro que cubre las compras realizadas durante el viaje frente a robo o daño accidental.', 'Otras asistencias'),
(32, 'Sin asignar', '(Descripción no asignada; este tipo de asistencia no tiene servicios específicos listados.)', 'Otras asistencias'),
(33, 'Asistencia legal', 'Asesoramiento legal y representación en situaciones que requieran intervención jurídica durante un viaje.', 'Otras asistencias'),
(34, 'Transferencia de fondos', 'Servicios para la transferencia segura de fondos en caso de emergencia financiera.', 'Otras asistencias'),
(35, 'Responsabilidad Civil', 'Cobertura por daños a terceros causados por el asegurado durante el viaje.', 'Otras asistencias');

ALTER TABLE EventType AUTO_INCREMENT = 36;

INSERT INTO EventStatus (Id, Name, Description) VALUES
(1, 'Evento en Borrador', 'El Grupo de Asistencias comienza a crear el evento recopilando la información inicial del viajero y la situación.'),
(2, 'Evento Creado', 'Una vez recopilada toda la información, se asigna un número de evento, y el Grupo de Asistencias toma el control.'),
(3, 'Evento en Proveedor', 'El evento ha sido asignado a un proveedor médico externo para asistencia física.'),
(4, 'Evento en TMO', 'El evento ha sido asignado a TMO para realizar consultas médicas telefónicas.'),
(5, 'Evento en Proceso', 'El evento está siendo gestionado activamente por el Grupo de Asistencias para resolución, seguimiento o reasignación.'),
(6, 'Evento Cerrado', 'El evento ha sido cerrado y todas las gestiones necesarias han sido completadas.');

ALTER TABLE EventStatus AUTO_INCREMENT = 7;

INSERT INTO VoucherStatus (Id, Name, Description) VALUES
(1, 'Emitido', 'Voucher comprado en espera de fechas de vigencia para su uso.'),
(2, 'En Vigencia', 'Voucher disponible para uso y en espera de ser utilizado.'),
(3, 'Caducado', 'Voucher cuya vigencia ha expirado y no puede ser utilizado.'),
(4, 'Anulado', 'Voucher anulado y no disponible para uso.'),
(5, 'En Carencia', 'Voucher emitido como extensión de otro voucher, con un periodo de gracia sin beneficios disponibles.');

-- Ajustar el valor de AUTO_INCREMENT
ALTER TABLE VoucherStatus AUTO_INCREMENT = 6;

-- Insertar datos en la tabla EventProviderStatus
INSERT INTO EventProviderStatus (Name, Description) VALUES
('Teledoctor', 'Consulta médica realizada a través de videollamada o teléfono, sin necesidad de desplazamiento físico.'),
('Clinical Appointment', 'Cita clínica programada en un centro de salud para evaluación, diagnóstico o tratamiento.'),
('Hospitalization', 'Ingreso del paciente en un hospital para recibir atención médica continua o tratamiento especializado.'),
('House Call', 'Visita médica realizada en el domicilio del paciente para consultas, tratamientos o seguimiento.'),
('Hospital Appointment', 'Cita programada dentro de un hospital para procedimientos, consultas especializadas o tratamientos específicos.'),
('Ambulance', 'Servicio de transporte médico de emergencia para trasladar a pacientes que requieren atención urgente.');


INSERT INTO Vouchers (VoucherName, Plan, DateOfIssue, StartDate, EndDate, IssueName, IdVoucherStatus, Destination) VALUES
('MEX2405084212BC', 'Plan Básico', '2023-01-15 10:00:00', '2023-01-15 10:00:00','2023-01-15 10:00:00','SURA', 1, 'México'),
('COL2405084212BC', 'Plan Premium', '2023-02-20 12:30:00','2023-01-15 10:00:00', '2023-01-15 10:00:00','MILES', 2, 'Colombia'),
('ARG2405084212BC', 'Plan Familiar', '2023-03-05 09:15:00','2023-01-15 10:00:00','2023-01-15 10:00:00','PALIC', 3,'Argentina'),
('BRA2405084212BC', 'Plan Empresarial', '2023-04-10 14:45:00','2023-01-15 10:00:00','2023-01-15 10:00:00','SURA', 4,'Brasil'),
('BRA2505084212BC', 'Plan Estudiante', '2023-05-25 08:20:00','2023-01-15 10:00:00','2023-01-15 10:00:00','PALIC', 5,'Brasil'),
('BRA2605084212BC', 'Plan Senior', '2023-06-30 16:50:00','2023-01-15 10:00:00','2023-01-15 10:00:00','SURA', 2,'Brasil'),
('BRA2705084212BC', 'Plan Viajero', '2023-07-15 11:05:00','2023-01-15 10:00:00','2023-01-15 10:00:00','PALIC', 1,'Brasil'),
('BRA2805084212BC', 'Plan Aventura', '2023-08-20 13:25:00','2023-01-15 10:00:00','2023-01-15 10:00:00','SURA', 2,'Brasil'),
('BRA2905084212BC', 'Plan Salud', '2023-09-10 15:40:00','2023-01-15 10:00:00','2023-01-15 10:00:00','PALIC', 3,'Brasil'),
('BRA2305084212BC', 'Plan VIP', '2023-10-05 17:55:00','2023-01-15 10:00:00','2023-01-15 10:00:00','SURA', 4,'Brasil');

INSERT INTO CustomerTrip (IdClinicHistory, Names, LastNames, Gender, CountryOfBirth, DateOfBirth) VALUES
(1001, 'Carlos', 'García López', 'Female', 'España', '1985-04-12'),
(1002, 'María', 'Rodríguez Fernández', 'Female','México', '1990-08-25'),
(1003, 'Juan', 'Pérez Sánchez', 'Female','Argentina', '1978-12-05'),
(1004, 'Ana', 'Martínez Gómez', 'Female','Colombia', '1982-03-17'),
(1005, 'Luis', 'Hernández Díaz', 'Female','Perú', '1995-06-30'),
(1006, 'Elena', 'González Ruiz',  'Female','Chile', '1988-11-22'),
(1007, 'Miguel', 'Torres Ramírez',  'Female','Uruguay', '1975-01-09'),
(1008, 'Laura', 'Sánchez Romero',  'Female','Ecuador', '1992-09-14'),
(1009, 'Diego', 'Flores Jiménez',  'Female','Guatemala', '1980-05-27'),
(1010, 'Isabel', 'Moreno Vargas',  'Female','Costa Rica', '1987-07-19');

INSERT INTO ContactInformation (IdCustomerTrip, Type, DataType, Data) VALUES
-- Customer Trip 1
(1, 'Contact', 'Email', 'carlos.garcia@example.com'),
(1, 'Contact', 'Phone', '+34 600 123 456'),
(1, 'Identification', 'IdCard', '12345678A'),
(1, 'Identification', 'Passport', 'X1234567'),
-- Customer Trip 2
(2, 'Contact', 'Email', 'maria.rodriguez@example.com'),
(2, 'Contact', 'Phone', '+52 55 1234 5678'),
(2, 'Identification', 'IdCard', '87654321B'),
(2, 'Identification', 'Passport', 'Y7654321'),
-- Customer Trip 3
(3, 'Contact', 'Email', 'juan.perez@example.com'),
(3, 'Contact', 'Phone', '+54 9 11 2345 6789'),
(3, 'Identification', 'IdCard', '11223344C'),
(3, 'Identification', 'Passport', 'Z9876543'),
-- Customer Trip 4
(4, 'Contact', 'Email', 'ana.martinez@example.com'),
(4, 'Contact', 'Phone', '+57 300 123 4567'),
(4, 'Identification', 'IdCard', '55667788D'),
(4, 'Identification', 'Passport', 'W1234567'),
-- Customer Trip 5
(5, 'Contact', 'Email', 'luis.hernandez@example.com'),
(5, 'Contact', 'Phone', '+51 987 654 321'),
(5, 'Identification', 'IdCard', '99887766E'),
(5, 'Identification', 'Passport', 'V7654321'),
-- Customer Trip 6
(6, 'Contact', 'Email', 'elena.gonzalez@example.com'),
(6, 'Contact', 'Phone', '+56 9 8765 4321'),
(6, 'Identification', 'IdCard', '44332211F'),
(6, 'Identification', 'Passport', 'U1234987'),
-- Customer Trip 7
(7, 'Contact', 'Email', 'miguel.torres@example.com'),
(7, 'Contact', 'Phone', '+598 99 123 456'),
(7, 'Identification', 'IdCard', '77665544G'),
(7, 'Identification', 'Passport', 'T9876123'),
-- Customer Trip 8
(8, 'Contact', 'Email', 'laura.sanchez@example.com'),
(8, 'Contact', 'Phone', '+593 99 876 5432'),
(8, 'Identification', 'IdCard', '22113344H'),
(8, 'Identification', 'Passport', 'S1237890'),
-- Customer Trip 9
(9, 'Contact', 'Email', 'diego.flores@example.com'),
(9, 'Contact', 'Phone', '+502 5123 4567'),
(9, 'Identification', 'IdCard', '33445566I'),
(9, 'Identification', 'Passport', 'R0987654'),
-- Customer Trip 10
(10, 'Contact', 'Email', 'isabel.moreno@example.com'),
(10, 'Contact', 'Phone', '+506 8888 7777'),
(10, 'Identification', 'IdCard', '66778899J'),
(10, 'Identification', 'Passport', 'Q5678901');

INSERT INTO ContactEmergency (IdCustomerTrip, Names, LastNames, Emails, Phones, IsPrincipal) VALUES
(1, 'Pedro', 'López García', 'pedro.lopez@example.com', '+34 600 987 654', TRUE),
(2, 'Carolina', 'Hernández Ruiz', 'carolina.hernandez@example.com', '+52 55 2345 6789', TRUE),
(3, 'Andrés', 'Martínez Fernández', 'andres.martinez@example.com', '+54 9 11 5432 1098', TRUE),
(4, 'Lucía', 'Gómez Sánchez', 'lucia.gomez@example.com', '+57 320 123 4567', TRUE),
(5, 'Sergio', 'Díaz Pérez', 'sergio.diaz@example.com', '+51 987 123 456', TRUE),
(6, 'Valentina', 'Torres González', 'valentina.torres@example.com', '+56 9 8765 4321', TRUE),
(7, 'Roberto', 'Ramírez López', 'roberto.ramirez@example.com', '+598 98 765 432', TRUE),
(8, 'Marta', 'Flores Jiménez', 'marta.flores@example.com', '+593 97 654 3210', TRUE),
(9, 'José', 'Sánchez Díaz', 'jose.sanchez@example.com', '+502 5132 6543', TRUE),
(10, 'Elena', 'Morales Castillo', 'elena.morales@example.com', '+506 8777 6666', TRUE);


INSERT INTO Events (
    CreatedDate,
    UpdateDate,
    EndDate,
    IdEventStatus,
    IdEventType,
    IdVoucher,    
    IdCustomerTrip,
    Country,
    State,
    City,
    NearTo,
    Address,
    Longitude,
    Latitude,
    EventPriority
) VALUES
-- Evento 1
(NOW(), NULL, NULL, 2, 1, 1, 1, 'España', 'Madrid', 'Madrid', 'Cerca del Museo del Prado', 'Calle de Alcalá, 1', -3.703790, 40.416775, 'Alta'),
-- Evento 2
(NOW(), NULL, NULL, 2, 2, 2,  2, 'México', 'Ciudad de México', 'Ciudad de México', 'Cerca del Zócalo', 'Av. 5 de Mayo, Centro', -99.133208, 19.432608, 'Media'),
-- Evento 3
(NOW(), NULL, NULL, 2, 3, 3,   3, 'Argentina', 'Buenos Aires', 'Buenos Aires', 'Cerca del Obelisco', 'Av. 9 de Julio', -58.381559, -34.603684, 'Alta'),
-- Evento 4
(NOW(), NULL, NULL, 2, 4, 4,  4, 'Colombia', 'Bogotá', 'Bogotá', 'Cerca de Monserrate', 'Carrera 2 Este No. 21-48', -74.063644, 4.598077, 'Media'),
-- Evento 5
(NOW(), NULL, NULL, 2, 5, 5,  5, 'Perú', 'Lima', 'Lima', 'Cerca de Plaza Mayor', 'Jirón de la Unión', -77.028240, -12.046374, 'Baja'),
-- Evento 6
(NOW(), NULL, NULL, 2, 6, 6,  6, 'Chile', 'Santiago', 'Santiago', 'Cerca del Palacio de La Moneda', 'Alameda 1058', -70.669265, -33.448890, 'Alta'),
-- Evento 7
(NOW(), NULL, NULL, 2, 7, 7,  7, 'Uruguay', 'Montevideo', 'Montevideo', 'Cerca de la Rambla', 'Av. 18 de Julio', -56.164532, -34.901113, 'Media'),
-- Evento 8
(NOW(), NULL, NULL, 2, 8, 8,  8, 'Ecuador', 'Quito', 'Quito', 'Cerca de la Mitad del Mundo', 'Calle Benalcázar', -78.467838, -0.180653, 'Alta'),
-- Evento 9
(NOW(), NULL, NULL, 2, 9, 9,  9, 'Guatemala', 'Ciudad de Guatemala', 'Ciudad de Guatemala', 'Cerca del Palacio Nacional', '6A Calle', -90.515134, 14.634915, 'Baja'),
-- Evento 10
(NOW(), NULL, NULL, 2, 10, 10,  10, 'Costa Rica', 'San José', 'San José', 'Cerca del Teatro Nacional', 'Av. 2', -84.078593, 9.932542, 'Media');

INSERT INTO EventProvider (
    IdEvent,
    IdProvider,
    IdProviderLocation,
    StartDate,
    UpdateDate,
    EndDate,
    Details,
    CoPayment,
    IdEventProviderStatus
    
) VALUES
-- Evento 1
(1, 101, 201, NOW(), NULL, NULL, 'Proveedor asignado para atención médica general', 30,5),
-- Evento 2
(2, 102, 202, NOW(), NULL, NULL, 'Proveedor asignado para asistencia odontológica',30,5),
-- Evento 3
(3, 103, 203, NOW(), NULL, NULL, 'Proveedor asignado para asistencia psicológica',30,5),
-- Evento 4
(4, 104, 204, NOW(), NULL, NULL, 'Proveedor asignado para traslado sanitario',30,5),
-- Evento 5
(5, 105, 205, NOW(), NULL, NULL, 'Proveedor asignado para hotel por convalecencia',30,5),
-- Evento 6
(6, 106, 206, NOW(), NULL, NULL, 'Proveedor asignado para repatriación sanitaria',30,5),
-- Evento 7
(7, 107, 207, NOW(), NULL, NULL, 'Proveedor asignado para asistencia legal',30,5),
-- Evento 8
(8, 108, 208, NOW(), NULL, NULL, 'Proveedor asignado para pérdida de equipaje',30,5),
-- Evento 9
(9, 109, 209, NOW(), NULL, NULL, 'Proveedor asignado para vuelo demorado',30,5),
-- Evento 10
(10, 110, 210, NOW(), NULL, NULL, 'Proveedor asignado para transferencia de fondos',30,5);

-- Asegúrate de que la tabla Guarantees tiene las columnas necesarias
-- Si aún no has agregado las columnas Currency, Email y PdfUrl, hazlo primero

-- Insertar garantías de pago para cada evento
-- Insertar garantías de pago para cada evento
INSERT INTO Guarantees (
    CreatedDate, 
    UpdateDate, 
    EndDate, 
    LocalCurrencyValue, 
    LocalCurrency, 
    ExchangeRateUSD, 
    Email, 
    IdDocument, 
    CreatedBy, 
    RolUser
) VALUES
-- Garantía para Evento 1 (España - EUR)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 500.00, 'EUR', 1.0800, 'proveedor1@example.com', 'http://example.com/guarantee/pdf/1.pdf', 'Usuario1', 'Gerente'),

-- Garantía para Evento 2 (México - MXN)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 750.00, 'MXN', 0.0500, 'proveedor2@example.com', 'http://example.com/guarantee/pdf/2.pdf', 'Usuario2', 'Supervisor'),

-- Garantía para Evento 3 (Argentina - ARS)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 1000.00, 'ARS', 0.0100, 'proveedor3@example.com', 'http://example.com/guarantee/pdf/3.pdf', 'Usuario3', 'Coordinador'),

-- Garantía para Evento 4 (Colombia - COP)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 1250.00, 'COP', 0.00026, 'proveedor4@example.com', 'http://example.com/guarantee/pdf/4.pdf', 'Usuario4', 'Analista'),

-- Garantía para Evento 5 (Perú - PEN)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 1500.00, 'PEN', 0.2600, 'proveedor5@example.com', 'http://example.com/guarantee/pdf/5.pdf', 'Usuario5', 'Administrador'),

-- Garantía para Evento 6 (Chile - CLP)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 1750.00, 'CLP', 0.0013, 'proveedor6@example.com', 'http://example.com/guarantee/pdf/6.pdf', 'Usuario6', 'Gerente'),

-- Garantía para Evento 7 (Uruguay - UYU)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 2000.00, 'UYU', 0.0250, 'proveedor7@example.com', 'http://example.com/guarantee/pdf/7.pdf', 'Usuario7', 'Supervisor'),

-- Garantía para Evento 8 (Ecuador - USD)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 2250.00, 'USD', 1.0000, 'proveedor8@example.com', 'http://example.com/guarantee/pdf/8.pdf', 'Usuario8', 'Coordinador'),

-- Garantía para Evento 9 (Guatemala - GTQ)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 2500.00, 'GTQ', 0.1300, 'proveedor9@example.com', 'http://example.com/guarantee/pdf/9.pdf', 'Usuario9', 'Analista'),

-- Garantía para Evento 10 (Costa Rica - CRC)
(NOW(), NULL, DATE_ADD(NOW(), INTERVAL 6 MONTH), 2750.00, 'CRC', 0.0017, 'proveedor10@example.com', 'http://example.com/guarantee/pdf/10.pdf', 'Usuario10', 'Administrador');

INSERT INTO EventProviderGuarantees (
    IdGuarantees,
    IdEvent,
    IdProvider
) VALUES
-- Garantía 1 asociada al Evento 101 y Proveedor 101
(1, 1, 101),
-- Garantía 2 asociada al Evento 102 y Proveedor 102
(2, 2, 102),
-- Garantía 3 asociada al Evento 103 y Proveedor 103
(3, 3, 103),
-- Garantía 4 asociada al Evento 104 y Proveedor 104
(4, 4, 104),
-- Garantía 5 asociada al Evento 105 y Proveedor 105
(5, 5, 105),
-- Garantía 6 asociada al Evento 106 y Proveedor 106
(6, 6, 106),
-- Garantía 7 asociada al Evento 107 y Proveedor 107
(7, 7, 107),
-- Garantía 8 asociada al Evento 108 y Proveedor 108
(8, 8, 108),
-- Garantía 9 asociada al Evento 109 y Proveedor 109
(9, 9, 109),
-- Garantía 10 asociada al Evento 110 y Proveedor 110
(10, 10, 110);

-- DROP TABLE IF EXISTS Notes;
CREATE TABLE Notes (
    IdEvent INT NOT NULL,
    IdUser INT NOT NULL,
    NameUser VARCHAR(50) NOT NULL,
    RolUser VARCHAR(50) NOT NULL,
    CreatedDate DATETIME NOT NULL,
    Description TEXT NOT NULL,
    NoteType VARCHAR(50) NOT NULL,
    FOREIGN KEY (IdEvent) REFERENCES Events(Id)
    -- No se define clave foránea para IdUser
);

-- Insertar notas para cada evento, incluyendo el campo RolUser
INSERT INTO Notes (IdEvent, IdUser, NameUser, RolUser, CreatedDate, Description, NoteType) VALUES
-- Notas para el Evento 1
(1, 201, 'María López', 'FrontUser', NOW(), 'Evento creado automáticamente. Se asignó el voucher MEX2405084212BC al cliente Carlos García.', 'CreateEvent'),
(1, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a María López al Evento', 'DetailEvent'),
(1, 201, 'María López', 'FrontUser', NOW(), 'María López recibe el evento.', 'AssignedTo'),
(1, 201, 'María López', 'FrontUser', NOW(), 'Se actualizó el estado del evento a "Evento Creado".', 'DetailEvent'),
(1, 201, 'María López', 'FrontUser', NOW(), 'Asignado proveedor con Id 101 al evento.', 'DetailEventProvider'),
(1, 201, 'María López', 'FrontUser', NOW(), 'Se creó la garantía por valor de $500.00 al proveedor con Id 101.', 'DetailEventProvider'),
(1, 212, 'Carlos Saldaña', 'Líder de Asistencias', NOW()+1, 'Asignó a Cristian Sandoval al Evento', 'DetailEvent'),
(1, 211, 'Cristian Sandoval', 'Coordinador', NOW()+1, 'Cristian Sandoval recibe el evento.', 'AssignedTo'),
(1, 211, 'Cristian Sandoval', 'Coordinador', NOW(), 'Cerró el caso porque ya se terminó.', 'EndEvent'),
(1, 211, 'Cristian Sandoval', 'Coordinador', NOW(), 'Reabrió el caso para seguimiento adicional.', 'ReopenEvent'),

-- Notas para el Evento 2
(2, 202, 'Juan Martínez', 'Agente de Viajes', NOW(), 'Evento creado manualmente. Cliente María Rodríguez reporta demora en su vuelo.', 'CreateEvent'),
(2, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Juan Martínez al Evento', 'DetailEvent'),
(2, 202, 'Juan Martínez', 'Agente de Viajes', NOW(), 'Juan Martínez recibe el evento.', 'AssignedTo'),
(2, 202, 'Juan Martínez', 'Agente de Viajes', NOW(), 'Se actualizó el estado del evento a "En Proceso".', 'StatusUpdate'),
(2, 202, 'Juan Martínez', 'Agente de Viajes', NOW(), 'Proveedor de aerolínea con Id 102 asignado.', 'DetailEventProvider'),
(2, 202, 'Juan Martínez', 'Agente de Viajes', NOW(), 'Se creó la garantía por valor de $750.00 al proveedor con Id 102.', 'GuaranteeCreated'),
(2, 213, 'Sofía Morales', 'Supervisora', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Asignó a Pedro Ramírez al Evento para seguimiento.', 'DetailEvent'),
(2, 214, 'Pedro Ramírez', 'Asistente', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Pedro Ramírez recibe el evento.', 'AssignedTo'),
(2, 214, 'Pedro Ramírez', 'Asistente', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Cliente abordó el vuelo reprogramado. Evento cerrado.', 'EndEvent'),

-- Notas para el Evento 3
(3, 203, 'Laura Gómez', 'Especialista Médico', NOW(), 'Evento creado automáticamente. Juan Pérez solicita asistencia médica urgente.', 'CreateEvent'),
(3, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Laura Gómez al Evento', 'DetailEvent'),
(3, 203, 'Laura Gómez', 'Especialista Médico', NOW(), 'Laura Gómez recibe el evento.', 'AssignedTo'),
(3, 203, 'Laura Gómez', 'Especialista Médico', NOW(), 'Se actualizó el estado del evento a "En Proveedor".', 'StatusUpdate'),
(3, 203, 'Laura Gómez', 'Especialista Médico', NOW(), 'Proveedor médico con Id 103 asignado para atención inmediata.', 'DetailEventProvider'),
(3, 203, 'Laura Gómez', 'Especialista Médico', NOW(), 'Se creó la garantía por valor de $1000.00 al proveedor con Id 103.', 'GuaranteeCreated'),
(3, 215, 'Fernando Ruiz', 'Doctor', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Atendió al cliente en el hospital. Estado estable.', 'CustomerInteraction'),
(3, 203, 'Laura Gómez', 'Especialista Médico', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Evento finalizado tras alta médica del cliente.', 'EndEvent'),

-- Notas para el Evento 4
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', NOW(), 'Evento creado por solicitud de repatriación de Ana Martínez.', 'CreateEvent'),
(4, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Carlos Díaz al Evento', 'DetailEvent'),
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', NOW(), 'Carlos Díaz recibe el evento.', 'AssignedTo'),
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', NOW(), 'Se actualizó el estado del evento a "En Proceso".', 'StatusUpdate'),
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', NOW(), 'Proveedor de transporte con Id 104 asignado para repatriación.', 'DetailEventProvider'),
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', NOW(), 'Se creó la garantía por valor de $1250.00 al proveedor con Id 104.', 'GuaranteeCreated'),
(4, 216, 'Laura Torres', 'Líder Logístico', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Organizó detalles del traslado sanitario.', 'DetailEvent'),
(4, 204, 'Carlos Díaz', 'Coordinador de Asistencias', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Cliente llegó a destino. Evento cerrado.', 'EndEvent'),

-- Notas para el Evento 5
(5, 205, 'Sofía Pérez', 'Asistente Legal', NOW(), 'Evento creado para asistencia legal a Luis Hernández.', 'CreateEvent'),
(5, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Sofía Pérez al Evento', 'DetailEvent'),
(5, 205, 'Sofía Pérez', 'Asistente Legal', NOW(), 'Sofía Pérez recibe el evento.', 'AssignedTo'),
(5, 205, 'Sofía Pérez', 'Asistente Legal', NOW(), 'Se actualizó el estado del evento a "En TMO".', 'StatusUpdate'),
(5, 205, 'Sofía Pérez', 'Asistente Legal', NOW(), 'Proveedor legal con Id 105 asignado.', 'DetailEventProvider'),
(5, 205, 'Sofía Pérez', 'Asistente Legal', NOW(), 'Se creó la garantía por valor de $1500.00 al proveedor con Id 105.', 'GuaranteeCreated'),
(5, 217, 'Diego Martínez', 'Abogado', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Realizó asesoría legal al cliente. Caso resuelto.', 'CustomerInteraction'),
(5, 205, 'Sofía Pérez', 'Asistente Legal', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Evento finalizado exitosamente.', 'EndEvent'),

-- Notas para el Evento 6
(6, 206, 'Miguel Torres', 'Agente de Viajes', NOW(), 'Evento creado por cancelación de vuelo de Elena González.', 'CreateEvent'),
(6, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Miguel Torres al Evento', 'DetailEvent'),
(6, 206, 'Miguel Torres', 'Agente de Viajes', NOW(), 'Miguel Torres recibe el evento.', 'AssignedTo'),
(6, 206, 'Miguel Torres', 'Agente de Viajes', NOW(), 'Se actualizó el estado del evento a "Cancelado".', 'StatusUpdate'),
(6, 206, 'Miguel Torres', 'Agente de Viajes', NOW(), 'Proveedor de aerolínea con Id 106 notificado.', 'DetailEventProvider'),
(6, 206, 'Miguel Torres', 'Agente de Viajes', NOW(), 'Se creó la garantía por valor de $1750.00 al proveedor con Id 106.', 'GuaranteeCreated'),
(6, 218, 'Ana López', 'Supervisora', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Asignó a Juan Pérez al Evento para revisión.', 'DetailEvent'),
(6, 219, 'Juan Pérez', 'Supervisor', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Juan Pérez recibe el evento.', 'AssignedTo'),
(6, 219, 'Juan Pérez', 'Supervisor', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Evento revisado y cerrado.', 'EndEvent'),

-- Notas para el Evento 7
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', NOW(), 'Evento creado por pérdida de equipaje de Miguel Torres.', 'CreateEvent'),
(7, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Lucía Fernández al Evento', 'DetailEvent'),
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', NOW(), 'Lucía Fernández recibe el evento.', 'AssignedTo'),
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', NOW(), 'Se actualizó el estado del evento a "En Proceso".', 'StatusUpdate'),
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', NOW(), 'Proveedor de equipaje con Id 107 asignado.', 'DetailEventProvider'),
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', NOW(), 'Se creó la garantía por valor de $2000.00 al proveedor con Id 107.', 'GuaranteeCreated'),
(7, 220, 'Roberto García', 'Especialista en Equipajes', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Localizó y devolvió el equipaje al cliente.', 'CustomerInteraction'),
(7, 207, 'Lucía Fernández', 'Coordinadora de Equipajes', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Evento cerrado tras resolución satisfactoria.', 'EndEvent'),

-- Notas para el Evento 8
(8, 208, 'Andrés Ruiz', 'Asistente Médico', NOW(), 'Evento creado para atención odontológica de Laura Sánchez.', 'CreateEvent'),
(8, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Andrés Ruiz al Evento', 'DetailEvent'),
(8, 208, 'Andrés Ruiz', 'Asistente Médico', NOW(), 'Andrés Ruiz recibe el evento.', 'AssignedTo'),
(8, 208, 'Andrés Ruiz', 'Asistente Médico', NOW(), 'Se actualizó el estado del evento a "En Proveedor".', 'StatusUpdate'),
(8, 208, 'Andrés Ruiz', 'Asistente Médico', NOW(), 'Proveedor odontológico con Id 108 asignado.', 'DetailEventProvider'),
(8, 208, 'Andrés Ruiz', 'Asistente Médico', NOW(), 'Se creó la garantía por valor de $2250.00 al proveedor con Id 108.', 'GuaranteeCreated'),
(8, 220, 'Daniela Pérez', 'Odontóloga', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Realizó tratamiento dental al cliente.', 'CustomerInteraction'),
(8, 208, 'Andrés Ruiz', 'Asistente Médico', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Cliente satisfecho. Evento finalizado.', 'EndEvent'),

-- Notas para el Evento 9
(9, 209, 'Mariana Gómez', 'Agente de Documentación', NOW(), 'Evento creado por pérdida de pasaporte de Diego Flores.', 'CreateEvent'),
(9, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a Mariana Gómez al Evento', 'DetailEvent'),
(9, 209, 'Mariana Gómez', 'Agente de Documentación', NOW(), 'Mariana Gómez recibe el evento.', 'AssignedTo'),
(9, 209, 'Mariana Gómez', 'Agente de Documentación', NOW(), 'Se actualizó el estado del evento a "En Proceso".', 'StatusUpdate'),
(9, 209, 'Mariana Gómez', 'Agente de Documentación', NOW(), 'Proveedor consular con Id 109 asignado.', 'DetailEventProvider'),
(9, 209, 'Mariana Gómez', 'Agente de Documentación', NOW(), 'Se creó la garantía por valor de $2500.00 al proveedor con Id 109.', 'GuaranteeCreated'),
(9, 221, 'José López', 'Oficial Consular', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Ayudó al cliente a obtener un pasaporte provisional.', 'CustomerInteraction'),
(9, 209, 'Mariana Gómez', 'Agente de Documentación', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Evento cerrado tras emisión del documento.', 'EndEvent'),

-- Notas para el Evento 10
(10, 210, 'José Sánchez', 'Asistente Psicológico', NOW(), 'Evento creado para asistencia psicológica a Isabel Moreno.', 'CreateEvent'),
(10, 0, 'Sistema', 'Sistema', NOW(), 'Asignó a José Sánchez al Evento', 'DetailEvent'),
(10, 210, 'José Sánchez', 'Asistente Psicológico', NOW(), 'José Sánchez recibe el evento.', 'AssignedTo'),
(10, 210, 'José Sánchez', 'Asistente Psicológico', NOW(), 'Se actualizó el estado del evento a "En TMO".', 'StatusUpdate'),
(10, 210, 'José Sánchez', 'Asistente Psicológico', NOW(), 'Proveedor de psicología con Id 110 asignado.', 'DetailEventProvider'),
(10, 210, 'José Sánchez', 'Asistente Psicológico', NOW(), 'Se creó la garantía por valor de $2750.00 al proveedor con Id 110.', 'GuaranteeCreated'),
(10, 222, 'Paula Ramírez', 'Psicóloga', DATE_ADD(NOW(), INTERVAL 1 HOUR), 'Realizó sesión de apoyo emocional con el cliente.', 'CustomerInteraction'),
(10, 210, 'José Sánchez', 'Asistente Psicológico', DATE_ADD(NOW(), INTERVAL 2 HOUR), 'Cliente agradecido por el apoyo. Evento cerrado.', 'EndEvent');

Create view ViewEventsAll as
SELECT
    e.Id AS 'Event',
    v.VoucherName AS 'Voucher',
    et.Name AS 'EventType',
    es.Name AS 'EventStatus',
    vs.Name  AS 'VoucherStatus',
    at.NameUser AS 'AssignedTo',
    v.IssueName AS 'IssueBy',
    e.CreatedDate AS 'EventStart',
    e.EndDate AS 'EventEnd'
FROM Events e
INNER JOIN Vouchers v ON e.IdVoucher = v.Id
INNER JOIN voucherstatus vs ON v.IdVoucherStatus = vs.Id
INNER JOIN EventType et ON e.IdEventType = et.Id
INNER JOIN EventStatus es ON e.IdEventStatus = es.Id
-- Obtener el usuario asignado actualmente al evento
LEFT JOIN (
    SELECT
        n.IdEvent,
        n.NameUser
    FROM Notes n
    INNER JOIN (
        SELECT
            IdEvent,
            MAX(CreatedDate) AS MaxCreatedDate
        FROM Notes
        WHERE NoteType = 'AssignedTo'
        GROUP BY IdEvent
    ) latest ON n.IdEvent = latest.IdEvent AND n.CreatedDate = latest.MaxCreatedDate
    WHERE n.NoteType = 'AssignedTo'
) at ON at.IdEvent = e.Id;

Create View ViewEventDetails AS
WITH PreferredIdentifications AS (
    SELECT
        ci.IdCustomerTrip,
        ci.DataType,
        ci.Data,
        ROW_NUMBER() OVER (
            PARTITION BY ci.IdCustomerTrip
            ORDER BY 
                CASE 
                    WHEN ci.DataType = 'IdCard' THEN 1
                    WHEN ci.DataType = 'Passport' THEN 2
                    ELSE 3
                END,
                ci.IdCustomerTrip -- Asumiendo que 'Id' es una columna única para deshacer empates
        ) AS rn
    FROM ContactInformation ci
    WHERE ci.Type = 'Identification'
),

LastAssignedTo AS (
    SELECT
        n.IdEvent,
        n.NameUser,
        n.RolUser,
        ROW_NUMBER() OVER (
            PARTITION BY n.IdEvent
            ORDER BY n.CreatedDate DESC, n.IdEvent DESC -- Asumiendo que 'Id' es una columna única para deshacer empates
        ) AS rn
    FROM Notes n
    WHERE n.NoteType = 'AssignedTo'
)

SELECT
    e.Id AS 'Event',
    v.VoucherName AS 'Voucher',
    v.Plan AS 'PlanVoucher',
    v.IssueName AS 'IssueBy',
    v.DateOfIssue AS 'DateOfIssueVoucher',
    v.StartDate AS 'StartDateVoucher',
    v.EndDate AS 'EndDateVoucher',
    DATEDIFF(CURDATE(), v.EndDate) AS 'Missing days', -- Calcula los días faltantes
    vs.Name AS 'StatusVoucher',
    v.Destination AS 'DestinationVoucher',
    pi.DataType AS 'TypeIdentification',
    pi.Data AS 'Identification',
    ct.Gender AS 'Gender',
    ct.DateOfBirth AS 'DateOfBirth',
    
    et.Name AS 'EventType',
    es.Name AS 'EventStatus',    
    lat.NameUser AS 'AssignedTo',    
    e.CreatedDate AS 'EventStart',
    e.EndDate AS 'EventEnd'
FROM Events e
INNER JOIN Vouchers v ON e.IdVoucher = v.Id
INNER JOIN VoucherStatus vs ON v.IdVoucherStatus = vs.Id
INNER JOIN EventType et ON e.IdEventType = et.Id
INNER JOIN EventStatus es ON e.IdEventStatus = es.Id
INNER JOIN CustomerTrip ct ON e.IdCustomerTrip = ct.Id
LEFT JOIN PreferredIdentifications pi 
    ON e.IdCustomerTrip = pi.IdCustomerTrip AND pi.rn = 1
LEFT JOIN LastAssignedTo lat 
    ON e.Id = lat.IdEvent AND lat.rn = 1
ORDER BY e.Id;

-------------------------------------------
Create view ViewProviderEvent AS
Select 
e.id AS 'IdEvent',
 ep.StartDate AS 'StartDate',
 ep.EndDate AS 'EndDate',
 'UUID' AS 'IdProvider',
 'UUID' AS 'Idlocation',
 ep.CoPayment AS 'CoPayment',
 eps.Name AS 'StatusProvider'
From events e 
inner join eventprovider ep on e.id = ep.IdEvent
inner join eventproviderstatus eps on ep.IdEventProviderStatus = eps.Id;

-------------------------------------------
/**/
Create view ViewProviderEventGuarantees AS

Select 
	e.id AS 'IdEvent',
    ep.id AS 'IdEventProvider',
    ep.IdProvider AS 'IdProvider',
    g.Id AS 'IdGuarantees',
	g.CreatedDate AS 'StartDateGuarantees',
    g.LocalCurrencyValue AS 'LocalCurrencyValue',
    g.LocalCurrency,
    g.ExchangeRateUSD,
    g.GuaranteeValueUSD,
    g.Email,
    g.IdDocument,
    g.CreatedBy AS 'CreateBy',
    g.RolUser AS 'Rol User'
From events e 
inner join eventprovider ep on e.id = ep.IdEvent
inner join eventproviderguarantees epg on ep.IdEvent = epg.IdEvent and  ep.IdProvider = epg.IdProvider
inner join Guarantees g on epg.IdGuarantees = g.Id;

--------------------------------------------
Select * from ViewEventsAll;
Select * from ViewEventDetails;
Select * from ViewProviderEvent;
Select * from ViewProviderEventGuarantees;