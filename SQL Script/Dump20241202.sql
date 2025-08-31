-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: assist-database-1.clcy8gyq614l.us-east-2.rds.amazonaws.com    Database: devassists
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ '';

--
-- Table structure for table `Categories`
--

DROP TABLE IF EXISTS `Categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Categories` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `IsConfigurationField` tinyint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Categories`
--

LOCK TABLES `Categories` WRITE;
/*!40000 ALTER TABLE `Categories` DISABLE KEYS */;
INSERT INTO `Categories` VALUES (1,'DocumentType',NULL,1),(2,'ContactType',NULL,1),(3,'Medical Assistance',NULL,NULL),(4,'Luggage Assistance',NULL,NULL),(5,'Flight Assistance',NULL,NULL),(6,'Other Assistance',NULL,NULL);
/*!40000 ALTER TABLE `Categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ContactEmergency`
--

DROP TABLE IF EXISTS `ContactEmergency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ContactEmergency` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EventId` int NOT NULL,
  `Names` varchar(100) NOT NULL,
  `LastNames` varchar(100) NOT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `IsPrimary` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `ContactEmergency_Events` (`EventId`),
  CONSTRAINT `ContactEmergency_Events` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ContactEmergency`
--

LOCK TABLES `ContactEmergency` WRITE;
/*!40000 ALTER TABLE `ContactEmergency` DISABLE KEYS */;
INSERT INTO `ContactEmergency` VALUES (1,1,'Tom','Jones','tom.jones@example.com','+0987654321',1),(2,2,'Tom','Jones','tom.jones@example.com','+0987654321',1),(3,3,'Tom','Jones','tom.jones@example.com','+0987654321',1),(4,4,'Tom','Jones','tom.jones@example.com','+0987654321',1),(5,5,'Tom','Jones','tom.jones@example.com','+0987654321',1),(6,6,'Tom','Jones','tom.jones@example.com','+0987654321',1),(7,7,'Tom','Jones','tom.jones@example.com','+0987654321',1),(8,8,'Tom','Jones','tom.jones@example.com','+0987654321',1),(9,9,'Tom','Jones','tom.jones@example.com','+0987654321',1),(10,10,'Tom','Jones','tom.jones@example.com','+0987654321',1),(11,11,'Tom','Jones','tom.jones@example.com','+0987654321',1),(12,12,'Tom','Jones','tom.jones@example.com','+0987654321',1),(13,13,'Tom','Jones','tom.jones@example.com','+0987654321',1),(14,14,'Tom','Jones','tom.jones@example.com','+0987654321',1),(15,15,'Tom','Jones','tom.jones@example.com','+0987654321',1),(16,16,'Tom','Jones','tom.jones@example.com','+0987654321',1),(17,17,'Tom','Jones','tom.jones@example.com','+0987654321',1),(18,18,'Tom','Jones','tom.jones@example.com','+0987654321',1),(19,19,'Tom','Jones','tom.jones@example.com','+0987654321',1),(20,20,'Tom','Jones','tom.jones@example.com','+0987654321',1),(21,21,'Tom','Jones','tom.jones@example.com','+0987654321',1),(22,24,'Tom','Jones','tom.jones@example.com','+0987654321',1),(23,25,'Tom','Jones','tom.jones@example.com','+0987654321',1),(24,27,'Tom','Jones','tom.jones@example.com','+0987654321',1),(25,28,'Tom','Jones','tom.jones@example.com','+0987654321',1),(26,29,'Tom','Jones','tom.jones@example.com','+0987654321',1);
/*!40000 ALTER TABLE `ContactEmergency` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ContactInformation`
--

DROP TABLE IF EXISTS `ContactInformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ContactInformation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerTripId` int NOT NULL,
  `GeneralTypesId` int NOT NULL,
  `Value` varchar(50) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `ContactInformation_CustomerTrip` (`CustomerTripId`),
  KEY `ContactInformation_GeneralTypes` (`GeneralTypesId`),
  CONSTRAINT `ContactInformation_CustomerTrip` FOREIGN KEY (`CustomerTripId`) REFERENCES `CustomerTrip` (`Id`),
  CONSTRAINT `ContactInformation_GeneralTypes` FOREIGN KEY (`GeneralTypesId`) REFERENCES `GeneralTypes` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ContactInformation`
--

LOCK TABLES `ContactInformation` WRITE;
/*!40000 ALTER TABLE `ContactInformation` DISABLE KEYS */;
INSERT INTO `ContactInformation` VALUES (1,1,6,'X1238526','2024-11-15 15:19:52'),(2,1,24,'+12345685890','2024-11-15 15:19:52'),(3,1,22,'arthur.pitt@gmail.com','2024-11-15 15:19:52'),(4,2,1,'X1238526','2024-11-19 21:47:13'),(5,2,24,'+12345685890','2024-11-19 21:47:13'),(6,2,22,'arthur.pitt@gmail.com','2024-11-19 21:47:13'),(7,3,1,'1012352119','2024-11-29 16:32:50'),(8,3,24,'3164942933','2024-11-29 16:32:50'),(9,3,22,'mroa@terrawind.com','2024-11-29 16:32:50');
/*!40000 ALTER TABLE `ContactInformation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CustomerTrip`
--

DROP TABLE IF EXISTS `CustomerTrip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CustomerTrip` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdClinicHistory` varchar(100) DEFAULT NULL,
  `Names` varchar(100) NOT NULL,
  `LastNames` varchar(100) NOT NULL,
  `CountryOfBirth` varchar(100) DEFAULT NULL,
  `Gender` smallint NOT NULL,
  `DateOfBirth` date DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CustomerTrip`
--

LOCK TABLES `CustomerTrip` WRITE;
/*!40000 ALTER TABLE `CustomerTrip` DISABLE KEYS */;
INSERT INTO `CustomerTrip` VALUES (1,NULL,'Arthur','Pitt','USA',0,'1985-05-15','2024-11-15 15:19:51'),(2,NULL,'Juan','Ramon','USA',0,'1985-05-15','2024-11-19 21:47:13'),(3,NULL,'Milena','Roa','colombia',0,'1989-04-11','2024-11-29 16:32:47'),(4,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:44:26'),(5,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:46:02'),(6,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:46:25'),(7,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:47:23'),(8,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:56:19'),(9,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 17:56:58'),(10,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 18:07:46'),(11,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 18:09:03'),(12,NULL,'Alex','Broskow','Afghanistan',0,'2024-11-01','2024-11-29 18:11:39');
/*!40000 ALTER TABLE `CustomerTrip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EventCoverages`
--

DROP TABLE IF EXISTS `EventCoverages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EventCoverages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EventId` int NOT NULL,
  `CoverageVoucher` varchar(255) DEFAULT NULL,
  `CoverageVoucherDetails` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `EventCoverages_Events` (`EventId`),
  CONSTRAINT `EventCoverages_Events` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EventCoverages`
--

LOCK TABLES `EventCoverages` WRITE;
/*!40000 ALTER TABLE `EventCoverages` DISABLE KEYS */;
INSERT INTO `EventCoverages` VALUES (1,13,'Health','Comprehensive'),(2,14,'Health','Comprehensive'),(3,15,'Health','Comprehensive'),(4,16,'Health','Comprehensive'),(5,17,'Health','Comprehensive'),(6,18,'Health','Comprehensive'),(7,19,'Health','Comprehensive'),(8,20,'Health','Comprehensive'),(9,21,'Health','Comprehensive'),(10,24,'Health','Comprehensive'),(11,25,'Health','Comprehensive'),(12,27,'Health','Comprehensive'),(13,28,'Health','Comprehensive'),(14,29,'Health','Comprehensive');
/*!40000 ALTER TABLE `EventCoverages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EventProvider`
--

DROP TABLE IF EXISTS `EventProvider`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EventProvider` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EventId` int NOT NULL,
  `EventProviderStatusId` int NOT NULL,
  `IdProvider` int NOT NULL,
  `IdProviderLocation` int NOT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdateAt` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `Description` text,
  `CoPayment` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `EventProvider_EventProviderStatus` (`EventProviderStatusId`),
  KEY `EventProvider_Events` (`EventId`),
  CONSTRAINT `EventProvider_EventProviderStatus` FOREIGN KEY (`EventProviderStatusId`) REFERENCES `EventProviderStatus` (`Id`),
  CONSTRAINT `EventProvider_Events` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EventProvider`
--

LOCK TABLES `EventProvider` WRITE;
/*!40000 ALTER TABLE `EventProvider` DISABLE KEYS */;
/*!40000 ALTER TABLE `EventProvider` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EventProviderStatus`
--

DROP TABLE IF EXISTS `EventProviderStatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EventProviderStatus` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EventProviderStatus`
--

LOCK TABLES `EventProviderStatus` WRITE;
/*!40000 ALTER TABLE `EventProviderStatus` DISABLE KEYS */;
/*!40000 ALTER TABLE `EventProviderStatus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EventStatus`
--

DROP TABLE IF EXISTS `EventStatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EventStatus` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EventStatus`
--

LOCK TABLES `EventStatus` WRITE;
/*!40000 ALTER TABLE `EventStatus` DISABLE KEYS */;
INSERT INTO `EventStatus` VALUES (1,'En Borrador','El Grupo de Asistencias comienza a crear el evento recopilando la información inicial del viajero y la situación.'),(2,'Creado','Una vez recopilada toda la información, se asigna un número de evento, y el Grupo de Asistencias toma el control.'),(3,'En Proveedor','El evento ha sido asignado a un proveedor médico externo para asistencia física.'),(4,'En TMO','El evento ha sido asignado a TMO para realizar consultas médicas telefónicas.'),(5,'En Proceso','El evento está siendo gestionado activamente por el Grupo de Asistencias para resolución, seguimiento o reasignación.'),(6,'Cerrado','El evento ha sido cerrado y todas las gestiones necesarias han sido completadas.');
/*!40000 ALTER TABLE `EventStatus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Events`
--

DROP TABLE IF EXISTS `Events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Events` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EventStatusId` int NOT NULL,
  `GeneralTypesId` int NOT NULL,
  `VoucherId` int NOT NULL,
  `CustomerTripId` int NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `EndDate` datetime DEFAULT NULL,
  `CountryLocation` varchar(100) DEFAULT NULL,
  `StateLocation` varchar(100) DEFAULT NULL,
  `CityLocation` varchar(100) DEFAULT NULL,
  `NearToLocation` varchar(255) DEFAULT NULL,
  `AddressLocation` varchar(255) DEFAULT NULL,
  `InformationLocation` text,
  `Longitude` decimal(9,6) DEFAULT NULL,
  `Latitude` decimal(9,6) DEFAULT NULL,
  `Description` text,
  `EventPriority` int DEFAULT NULL,
  `CoverageVoucher` varchar(100) DEFAULT NULL,
  `CoverageVoucherDetails` varchar(100) DEFAULT NULL,
  `RequireProvider` tinyint(1) DEFAULT NULL,
  `ProviderDetails` varchar(100) DEFAULT NULL,
  `RequirePhoneMedicalConsultation` tinyint(1) DEFAULT NULL,
  `RequireReviewAssistsTeam` tinyint(1) DEFAULT NULL,
  `ReviewAssistsTeamDetails` varchar(100) DEFAULT NULL,
  `RequireCashBackTeam` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Events_CustomerTrip` (`CustomerTripId`),
  KEY `Events_EventStatus` (`EventStatusId`),
  KEY `Events_GeneralTypes` (`GeneralTypesId`),
  KEY `Events_Voucher` (`VoucherId`),
  CONSTRAINT `Events_CustomerTrip` FOREIGN KEY (`CustomerTripId`) REFERENCES `CustomerTrip` (`Id`),
  CONSTRAINT `Events_EventStatus` FOREIGN KEY (`EventStatusId`) REFERENCES `EventStatus` (`Id`),
  CONSTRAINT `Events_GeneralTypes` FOREIGN KEY (`GeneralTypesId`) REFERENCES `GeneralTypes` (`Id`),
  CONSTRAINT `Events_Voucher` FOREIGN KEY (`VoucherId`) REFERENCES `Vouchers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Events`
--

LOCK TABLES `Events` WRITE;
/*!40000 ALTER TABLE `Events` DISABLE KEYS */;
INSERT INTO `Events` VALUES (1,6,37,1,1,'2024-11-15 15:19:53','2024-11-29 21:53:31',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,'Health','Comprehensive',0,'',1,1,'Review Team A',0),(2,3,37,1,1,'2024-11-19 21:42:51','2024-11-29 19:45:32',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,'Health','Comprehensive',0,'',1,1,'Review Team A',0),(3,4,37,1,2,'2024-11-19 17:18:46','2024-11-19 17:18:46',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(4,4,37,1,2,'2024-11-19 17:21:00','2024-11-19 17:21:00',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(5,4,37,1,2,'2024-11-19 17:21:30','2024-11-19 17:21:30',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(6,4,37,1,2,'2024-11-19 17:23:55','2024-11-19 17:23:55',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(7,4,37,1,2,'2024-11-19 22:26:29','2024-11-19 22:26:29',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(8,4,37,1,2,'2024-11-19 22:26:45','2024-11-19 22:26:45',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(9,4,37,1,2,'2024-11-20 17:21:12','2024-11-20 17:21:12',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(10,4,37,1,2,'2024-11-20 17:23:04','2024-11-20 17:23:04',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(11,4,37,1,2,'2024-11-20 17:25:37','2024-11-20 17:25:37',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(12,4,37,1,2,'2024-11-20 17:27:17','2024-11-20 17:27:17',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(13,4,37,1,2,'2024-11-20 17:28:17','2024-11-20 17:28:17',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(14,4,37,1,2,'2024-11-28 15:17:48','2024-11-28 15:17:48',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(15,4,37,1,2,'2024-11-28 15:40:25','2024-11-28 15:40:25',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(16,4,37,1,2,'2024-11-28 15:40:30','2024-11-28 15:40:30',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(17,4,37,1,2,'2024-11-28 13:49:23','2024-11-28 13:49:23',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(18,4,37,1,2,'2024-11-28 13:50:35','2024-11-28 13:50:35',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(19,4,37,1,2,'2024-11-28 13:51:39','2024-11-28 13:51:39',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(20,4,37,1,2,'2024-11-28 18:53:23','2024-11-28 18:53:23',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(21,4,37,1,2,'2024-11-29 16:26:01','2024-11-29 16:26:01',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(24,4,37,1,2,'2024-11-29 21:48:27','2024-11-29 21:48:27',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(25,4,37,1,2,'2024-11-29 21:57:03','2024-11-29 21:57:03',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(27,4,37,1,2,'2024-11-29 22:12:09','2024-11-29 22:12:09',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(28,6,37,1,2,'2024-11-29 22:14:08','2024-11-29 22:20:57',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0),(29,4,37,1,2,'2024-11-29 22:15:22','2024-11-29 22:15:22',NULL,'France','Ile-de-France','Paris','Louvre Museum','123 Champs-Élysées','Near the Eiffel Tower',2.352200,48.856600,'Urgent medical assistance needed',2,NULL,NULL,0,'',1,1,'Review Team A',0);
/*!40000 ALTER TABLE `Events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `GeneralTypes`
--

DROP TABLE IF EXISTS `GeneralTypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `GeneralTypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `CategoriesId` int NOT NULL,
  `Description` text,
  PRIMARY KEY (`Id`),
  KEY `GeneralTypes_Categories` (`CategoriesId`),
  CONSTRAINT `GeneralTypes_Categories` FOREIGN KEY (`CategoriesId`) REFERENCES `Categories` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `GeneralTypes`
--

LOCK TABLES `GeneralTypes` WRITE;
/*!40000 ALTER TABLE `GeneralTypes` DISABLE KEYS */;
INSERT INTO `GeneralTypes` VALUES (1,'National Identity Card',1,'Official document for national identification'),(2,'Passport',1,'Official document for international travel'),(3,'Driver’s License',1,'Document authorizing vehicle operation'),(4,'Youth ID Card',1,'Identification for minors'),(5,'National Identification Document (DNI)',1,'National ID used in many countries'),(6,'Unique Population Registry Code (CURP)',1,'Identification code for Mexican citizens'),(7,'Foreigners’ Identity Number (NIE)',1,'Identification number for foreign residents in Spain'),(8,'Refugee Document',1,'Identification document for refugees'),(9,'Residence Permit',1,'Permit for residing in a foreign country'),(10,'Green Card (Permanent Resident Card)',1,'Permanent residency document for the U.S.'),(11,'Social Security Number (SSN)',1,'Unique identifier for taxation and employment in the U.S.'),(12,'Tax Identification Number (TIN)',1,'Tax identification used in the European Union'),(13,'Foreigners’ Identification Card',1,'ID for foreign residents in certain countries'),(14,'Work Permit',1,'Document allowing foreigners to work legally'),(15,'University ID Card',1,'Identification used by students'),(16,'Electoral Credential (Voter ID Card)',1,'Document for voting and personal identification'),(17,'Social Security Card',1,'Document for social benefits and pensions'),(18,'Digital ID (Electronic DNI)',1,'Digital version of a national ID'),(19,'Electronic Signature Certificate',1,'Digital identity for online transactions'),(20,'QR Code-Based Identity Document',1,'Identity document with QR code for verification'),(21,'Phone',2,'Phone number for contact purposes'),(22,'Email',2,'Electronic mail address for communication'),(23,'Fax',2,'Facsimile number for document transmission'),(24,'Mobile Phone',2,'Mobile phone number for contact'),(25,'Home Phone',2,'Residential phone number'),(26,'Work Phone',2,'Workplace phone number'),(27,'Social Media',2,'Social media account for contact'),(28,'WhatsApp',2,'WhatsApp number for instant messaging'),(29,'Telegram',2,'Telegram account for contact'),(30,'Skype',2,'Skype ID for communication'),(31,'LinkedIn',2,'LinkedIn profile for professional contact'),(32,'Emergency Contact',2,'Contact for emergencies'),(33,'Postal Address',2,'Physical address for correspondence'),(34,'Contact Form',2,'Online form submission'),(35,'Website',2,'Personal or business website URL'),(36,'Other',2,'Other unspecified contact method'),(37,'Asistencia médica',3,'Servicio de atención médica inmediata para situaciones de emergencia o consulta médica general.'),(38,'Odontología',3,'Servicios odontológicos de urgencia y rutinarios en caso de necesidad durante un viaje.'),(39,'test',3,'Prueba Creación');
/*!40000 ALTER TABLE `GeneralTypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Notes`
--

DROP TABLE IF EXISTS `Notes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Notes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EventId` int NOT NULL,
  `IdUser` int DEFAULT NULL,
  `NameUser` varchar(50) DEFAULT NULL,
  `RoleUser` varchar(50) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `Description` text,
  `NoteType` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Notes_Events` (`EventId`),
  CONSTRAINT `Notes_Events` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Notes`
--

LOCK TABLES `Notes` WRITE;
/*!40000 ALTER TABLE `Notes` DISABLE KEYS */;
/*!40000 ALTER TABLE `Notes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `VoucherStatus`
--

DROP TABLE IF EXISTS `VoucherStatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `VoucherStatus` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `VoucherStatus`
--

LOCK TABLES `VoucherStatus` WRITE;
/*!40000 ALTER TABLE `VoucherStatus` DISABLE KEYS */;
INSERT INTO `VoucherStatus` VALUES (1,'Emitido','Voucher comprado en espera de fechas de vigencia para su uso.'),(2,'En Vigencia','Voucher disponible para uso y en espera de ser utilizado.'),(3,'Caducado','Voucher cuya vigencia ha expirado y no puede ser utilizado.'),(4,'Anulado','Voucher anulado y no disponible para uso.'),(5,'En Carencia','Voucher emitido como extensión de otro voucher, con un periodo de gracia sin beneficios disponibles.');
/*!40000 ALTER TABLE `VoucherStatus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Vouchers`
--

DROP TABLE IF EXISTS `Vouchers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vouchers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `VoucherStatusId` int NOT NULL,
  `Name` varchar(150) NOT NULL,
  `Plan` varchar(150) DEFAULT NULL,
  `IssueName` varchar(100) DEFAULT NULL,
  `DateOfIssue` datetime NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Destination` varchar(100) DEFAULT NULL,
  `IsCoPayment` tinyint(1) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Description` text,
  `UpdatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `Vouchers_VoucherStatus` (`VoucherStatusId`),
  CONSTRAINT `Vouchers_VoucherStatus` FOREIGN KEY (`VoucherStatusId`) REFERENCES `VoucherStatus` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Vouchers`
--

LOCK TABLES `Vouchers` WRITE;
/*!40000 ALTER TABLE `Vouchers` DISABLE KEYS */;
INSERT INTO `Vouchers` VALUES (1,1,'CCD12365GH','Premium change','Premium','2024-10-14 00:00:00','2024-09-15 00:00:00','2024-10-15 00:00:00','Paris',0,'2024-11-15 15:19:51','Full travel coverage','2024-11-15 15:19:51'),(2,1,'8552DGFGHG','Premium change','Premium','2024-10-03 00:00:00','2024-10-03 00:00:00','2024-10-03 00:00:00','españa',1,'2024-11-29 16:32:42','Prueba','2024-11-29 16:32:42'),(3,1,'MEX24112851777A','STUDENT 100 RC 45','BIGTREE','2024-11-28 00:00:00','2025-02-12 00:00:00','2025-07-11 00:00:00',' Sudamérica',0,'2024-11-29 17:44:26','','2024-11-29 17:44:26');
/*!40000 ALTER TABLE `Vouchers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `VwEventDetails`
--

DROP TABLE IF EXISTS `VwEventDetails`;
/*!50001 DROP VIEW IF EXISTS `VwEventDetails`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `VwEventDetails` AS SELECT 
 1 AS `Id`,
 1 AS `Voucher`,
 1 AS `PlanVoucher`,
 1 AS `IssueBy`,
 1 AS `DateOfIssueVoucher`,
 1 AS `StartDateVoucher`,
 1 AS `EndDateVoucher`,
 1 AS `MissingDays`,
 1 AS `StatusVoucher`,
 1 AS `DestinationVoucher`,
 1 AS `TypeIdentification`,
 1 AS `Identification`,
 1 AS `Gender`,
 1 AS `DateOfBirth`,
 1 AS `EventType`,
 1 AS `EventStatus`,
 1 AS `AssignedTo`,
 1 AS `EventStart`,
 1 AS `EventEnd`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `VwEvents`
--

DROP TABLE IF EXISTS `VwEvents`;
/*!50001 DROP VIEW IF EXISTS `VwEvents`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `VwEvents` AS SELECT 
 1 AS `Id`,
 1 AS `VoucherStatusId`,
 1 AS `EventStatusId`,
 1 AS `Voucher`,
 1 AS `Name`,
 1 AS `LastName`,
 1 AS `EventType`,
 1 AS `EventStatus`,
 1 AS `VoucherStatus`,
 1 AS `IssueBy`,
 1 AS `EventStart`,
 1 AS `EventEnd`,
 1 AS `CreatedBy`,
 1 AS `LastDoctorAssigned`,
 1 AS `LastPhoneConsulting`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `VwEventDetails`
--

/*!50001 DROP VIEW IF EXISTS `VwEventDetails`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`admin`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `VwEventDetails` AS select `e`.`Id` AS `Id`,`v`.`Name` AS `Voucher`,`v`.`Plan` AS `PlanVoucher`,`v`.`IssueName` AS `IssueBy`,`v`.`DateOfIssue` AS `DateOfIssueVoucher`,`v`.`StartDate` AS `StartDateVoucher`,`v`.`EndDate` AS `EndDateVoucher`,(to_days(curdate()) - to_days(`v`.`EndDate`)) AS `MissingDays`,`vs`.`Name` AS `StatusVoucher`,`v`.`Destination` AS `DestinationVoucher`,`ci`.`Value` AS `TypeIdentification`,`ci`.`Value` AS `Identification`,`ct`.`Gender` AS `Gender`,`ct`.`DateOfBirth` AS `DateOfBirth`,`et`.`Name` AS `EventType`,`es`.`Name` AS `EventStatus`,`n`.`NameUser` AS `AssignedTo`,`e`.`CreatedAt` AS `EventStart`,`e`.`EndDate` AS `EventEnd` from (((((((`Events` `e` join `Vouchers` `v` on((`e`.`VoucherId` = `v`.`Id`))) join `VoucherStatus` `vs` on((`v`.`VoucherStatusId` = `vs`.`Id`))) join `GeneralTypes` `et` on((`e`.`GeneralTypesId` = `et`.`Id`))) join `EventStatus` `es` on((`e`.`EventStatusId` = `es`.`Id`))) join `CustomerTrip` `ct` on((`e`.`CustomerTripId` = `ct`.`Id`))) left join `ContactInformation` `ci` on(((`e`.`CustomerTripId` = `ci`.`CustomerTripId`) and (`ci`.`GeneralTypesId` = 1)))) left join `Notes` `n` on(((`e`.`Id` = `n`.`EventId`) and (`n`.`NoteType` = 'AssignedTo')))) order by `e`.`Id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `VwEvents`
--

/*!50001 DROP VIEW IF EXISTS `VwEvents`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`admin`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `VwEvents` AS select `e`.`Id` AS `Id`,`vs`.`Id` AS `VoucherStatusId`,`es`.`Id` AS `EventStatusId`,`v`.`Name` AS `Voucher`,`c`.`Names` AS `Name`,`c`.`LastNames` AS `LastName`,`et`.`Name` AS `EventType`,`es`.`Name` AS `EventStatus`,`vs`.`Name` AS `VoucherStatus`,`v`.`IssueName` AS `IssueBy`,`e`.`CreatedAt` AS `EventStart`,`e`.`EndDate` AS `EventEnd`,'Front Agent' AS `CreatedBy`,'Andres Plata' AS `LastDoctorAssigned`,cast(now() as date) AS `LastPhoneConsulting` from (((((`Events` `e` join `Vouchers` `v` on((`e`.`VoucherId` = `v`.`Id`))) join `VoucherStatus` `vs` on((`v`.`VoucherStatusId` = `vs`.`Id`))) join `GeneralTypes` `et` on((`e`.`GeneralTypesId` = `et`.`Id`))) join `EventStatus` `es` on((`e`.`EventStatusId` = `es`.`Id`))) join `CustomerTrip` `c` on((`e`.`CustomerTripId` = `c`.`Id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-02 11:13:08
