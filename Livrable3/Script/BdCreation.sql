CREATE DATABASE IF NOT EXISTS `h24_esp_projet_1336289`
/*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */
/*!80016 DEFAULT ENCRYPTION='N' */
;

USE `h24_esp_projet_1336289`;

-- MySQL dump 10.13  Distrib 8.0.32, for Win64 (x86_64)
--
-- Host: sql.decinfo-cchic.ca    Database: h24_esp_projet_1336289
-- ------------------------------------------------------
-- Server version	8.0.21
/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */
;

/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */
;

/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */
;

/*!50503 SET NAMES utf8 */
;

/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */
;

/*!40103 SET TIME_ZONE='+00:00' */
;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */
;

/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */
;

/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */
;

/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */
;

--
-- Table structure for table `auditorium`
--
DROP TABLE IF EXISTS `auditorium`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `auditorium` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `AuditoriumName` varchar(255) NOT NULL,
    `numberOfRows` int NOT NULL,
    `NumberOfColumns` int NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `AuditoriumName` (`AuditoriumName`)
) ENGINE = InnoDB AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `client`
--
DROP TABLE IF EXISTS `client`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `client` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `FirstName` varchar(255) NOT NULL,
    `LastName` varchar(255) NOT NULL,
    `Email` varchar(255) NOT NULL,
    `PasswordHash` blob,
    `PasswordSalt` blob,
    `Phone` varchar(20) DEFAULT NULL,
    `City` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Email` (`Email`)
) ENGINE = InnoDB AUTO_INCREMENT = 11 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `orders`
--
DROP TABLE IF EXISTS `orders`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `orders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ClientId` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `OrderNumber` varchar(255) NOT NULL,
    `OrderDate` datetime NOT NULL,
    `TotalPrice` decimal(10, 2) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `OrderNumber` (`OrderNumber`),
    KEY `ClientId` (`ClientId`),
    CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ClientId`) REFERENCES `client` (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 7 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `representation`
--
DROP TABLE IF EXISTS `representation`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `representation` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ShowId` int NOT NULL,
    `AuditoriumId` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `Date` datetime NOT NULL,
    `RepresentationStatus` varchar(45) NOT NULL,
    PRIMARY KEY (`Id`),
    KEY `ShowId` (`ShowId`),
    KEY `representation_ibfk_2_idx` (`AuditoriumId`),
    CONSTRAINT `representation_ibfk_1` FOREIGN KEY (`ShowId`) REFERENCES `shows` (`Id`),
    CONSTRAINT `representation_ibfk_2` FOREIGN KEY (`AuditoriumId`) REFERENCES `auditorium` (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 3 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `seat`
--
DROP TABLE IF EXISTS `seat`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `seat` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AuditoriumId` int NOT NULL,
    `SeatNumber` int NOT NULL,
    `SectionName` varchar(45) NOT NULL,
    `XCoordinate` int NOT NULL,
    `YCoordinate` int NOT NULL,
    `SeatStatus` varchar(45) NOT NULL,
    `RowName` varchar(5) NOT NULL,
    PRIMARY KEY (`Id`),
    KEY `FK_Seat_Auditorium_idx` (`AuditoriumId`),
    CONSTRAINT `FK_Seat_Auditorium` FOREIGN KEY (`AuditoriumId`) REFERENCES `auditorium` (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 794 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `shows`
--
DROP TABLE IF EXISTS `shows`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `shows` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `ShowName` varchar(255) NOT NULL,
    `Artiste` varchar(255) NOT NULL,
    `Description` text NOT NULL,
    `ShowType` varchar(50) NOT NULL,
    `ImageUrl` varchar(255) NOT NULL,
    `NumberOfTicketsMaxByClient` int NOT NULL,
    `BaseTicketPrice` decimal(10, 2) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `ShowName` (`ShowName`),
    KEY `FK_Show_User` (`UserId`),
    CONSTRAINT `FK_Show_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 5 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `ticket`
--
DROP TABLE IF EXISTS `ticket`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `ticket` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RepresentationId` int NOT NULL,
    `SeatId` int NOT NULL,
    `OrderId` int DEFAULT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `TicketStatus` varchar(50) NOT NULL,
    `ReservationNumber` varchar(20) NOT NULL,
    `QRData` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `TicketNumber` (`ReservationNumber`),
    KEY `RepresentationId` (`RepresentationId`),
    KEY `SeatId` (`SeatId`),
    KEY `ticket_ibfk_3_idx` (`OrderId`),
    CONSTRAINT `ticket_ibfk_1` FOREIGN KEY (`RepresentationId`) REFERENCES `representation` (`Id`),
    CONSTRAINT `ticket_ibfk_2` FOREIGN KEY (`SeatId`) REFERENCES `seat` (`Id`),
    CONSTRAINT `ticket_ibfk_3` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 1587 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Table structure for table `user`
--
DROP TABLE IF EXISTS `user`;

/*!40101 SET @saved_cs_client     = @@character_set_client */
;

/*!50503 SET character_set_client = utf8mb4 */
;

CREATE TABLE `user` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `FirstName` varchar(255) NOT NULL,
    `LastName` varchar(255) NOT NULL,
    `EmployeeNumber` varchar(255) NOT NULL,
    `Email` varchar(255) NOT NULL,
    `Type` varchar(50) NOT NULL,
    `Phone` varchar(20) NOT NULL,
    `PasswordHash` blob,
    `PasswordSalt` blob,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `EmployeeNumber` (`EmployeeNumber`),
    UNIQUE KEY `Email` (`Email`)
) ENGINE = InnoDB AUTO_INCREMENT = 6 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

/*!40101 SET character_set_client = @saved_cs_client */
;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */
;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */
;

/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */
;

/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */
;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */
;

/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */
;

/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */
;

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */
;

-- Dump completed on 2024-03-17 10:11:58