CREATE DATABASE IF NOT EXISTS h24_esp_projet_1336289;

CREATE TABLE `auditorium` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `AuditoriumName` varchar(255) NOT NULL,
    `numberOfRows` int NOT NULL,
    `NumberOfColumns` int NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `AuditoriumName` (`AuditoriumName`)
) CREATE TABLE `client` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `FirstName` varchar(255) NOT NULL,
    `LastName` varchar(255) NOT NULL,
    `Email` varchar(255) NOT NULL,
    `PasswordHash` blob NOT NULL,
    `PasswordSalt` blob NOT NULL,
    `Phone` varchar(20) DEFAULT NULL,
    `City` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Email` (`Email`)
) CREATE TABLE `orders` (
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
) CREATE TABLE `representation` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ShowId` int NOT NULL,
    `AuditoriumId` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `Date` datetime NOT NULL,
    PRIMARY KEY (`Id`),
    KEY `ShowId` (`ShowId`),
    KEY `representation_ibfk_2_idx` (`AuditoriumId`),
    CONSTRAINT `representation_ibfk_1` FOREIGN KEY (`ShowId`) REFERENCES `shows` (`Id`),
    CONSTRAINT `representation_ibfk_2` FOREIGN KEY (`AuditoriumId`) REFERENCES `auditorium` (`Id`)
) CREATE TABLE `seat` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AuditoriumId` int NOT NULL,
    `SeatNumber` int NOT NULL,
    `SectionName` varchar(45) NOT NULL,
    `XCoordinate` int NOT NULL,
    `YCoordinate` int NOT NULL,
    PRIMARY KEY (`Id`),
    KEY `FK_Seat_Auditorium_idx` (`AuditoriumId`),
    CONSTRAINT `FK_Seat_Auditorium` FOREIGN KEY (`AuditoriumId`) REFERENCES `auditorium` (`Id`)
) CREATE TABLE `shows` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `ShowName` varchar(255) NOT NULL,
    `Artiste` varchar(255) NOT NULL,
    `Description` text NOT NULL,
    `ShowType` varchar(50) NOT NULL,
    `ShowStatus` varchar(50) NOT NULL,
    `ImageUrl` varchar(255) NOT NULL,
    `NumberOfTicketsMaxByClient` int NOT NULL,
    `BaseTicketPrice` decimal(10, 2) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `ShowName` (`ShowName`),
    KEY `FK_Show_User` (`UserId`),
    CONSTRAINT `FK_Show_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) 
CREATE TABLE `ticket` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RepresentationId` int NOT NULL,
    `SeatId` int NOT NULL,
    `OrderId` int DEFAULT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `TicketStatus` varchar(50) NOT NULL,
    `ReservationNumber` varchar(20) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `TicketNumber` (`ReservationNumber`),
    KEY `RepresentationId` (`RepresentationId`),
    KEY `SeatId` (`SeatId`),
    KEY `ticket_ibfk_3_idx` (`OrderId`),
    CONSTRAINT `ticket_ibfk_1` FOREIGN KEY (`RepresentationId`) REFERENCES `representation` (`Id`),
    CONSTRAINT `ticket_ibfk_2` FOREIGN KEY (`SeatId`) REFERENCES `seat` (`Id`),
    CONSTRAINT `ticket_ibfk_3` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`)
) 

CREATE TABLE `user` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IsActive` tinyint(1) NOT NULL,
    `FirstName` varchar(255) NOT NULL,
    `LastName` varchar(255) NOT NULL,
    `EmployeeNumber` varchar(255) NOT NULL,
    `Email` varchar(255) NOT NULL,
    `Type` varchar(50) NOT NULL,
    `Phone` varchar(20) NOT NULL,
    `PasswordHash` blob NOT NULL,
    `PasswordSalt` blob NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `EmployeeNumber` (`EmployeeNumber`),
    UNIQUE KEY `Email` (`Email`)
)