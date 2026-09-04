-- ============================================================
-- RaceDay Database Creation & Schema Script
-- Module: PROG6212 - Portfolio of Evidence (Part 1)
-- Target RDBMS: Microsoft SQL Server (SSMS)
-- ============================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RaceDayDB')
BEGIN
    CREATE DATABASE RaceDayDB;
END
GO

USE RaceDayDB;
GO

IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL DROP TABLE dbo.Results;
IF OBJECT_ID('dbo.Enrolments', 'U') IS NOT NULL DROP TABLE dbo.Enrolments;
IF OBJECT_ID('dbo.EventCategories', 'U') IS NOT NULL DROP TABLE dbo.EventCategories;
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

-- CREATE TABLES
CREATE TABLE dbo.Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(RoleId)
);

CREATE TABLE dbo.Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserId INT NOT NULL,
    Title VARCHAR(150) NOT NULL,
    Description TEXT NULL,
    Location VARCHAR(150) NOT NULL,
    EventDate DATETIME NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Events_Users FOREIGN KEY (OrganiserId) REFERENCES dbo.Users(UserId)
);

CREATE TABLE dbo.EventCategories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    DistanceKM DECIMAL(5,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_EventCategories_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(EventId) ON DELETE CASCADE
);

CREATE TABLE dbo.Enrolments (
    EnrolmentId INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    PaymentStatus VARCHAR(20) DEFAULT 'Pending' CHECK (PaymentStatus IN ('Pending', 'Paid', 'Cancelled')),
    CONSTRAINT FK_Enrolments_Users FOREIGN KEY (ParticipantId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Enrolments_EventCategories FOREIGN KEY (CategoryId) REFERENCES dbo.EventCategories(CategoryId),
    CONSTRAINT UQ_Participant_Category UNIQUE (ParticipantId, CategoryId)
);

CREATE TABLE dbo.Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentId INT NOT NULL UNIQUE,
    FinishTime TIME NULL,
    OverallPosition INT NULL,
    CategoryPosition INT NULL,
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES dbo.Enrolments(EnrolmentId) ON DELETE CASCADE
);
GO

-- SEED SAMPLE DATA
INSERT INTO dbo.Roles (RoleName) 
VALUES ('Organiser'), ('Participant');

INSERT INTO dbo.Users (RoleId, FullName, Email, PasswordHash, PhoneNumber)
VALUES 
(1, 'Sipho Zulu', 'sipho@eventmasters.co.za', 'hashed_pass_1', '+27821234567'),
(1, 'Anika van der Merwe', 'anika@raceorganisers.co.za', 'hashed_pass_2', '+27839876543'),
(2, 'Thabo Mokoena', 'thabo.mokoena@gmail.com', 'hashed_pass_3', '+27711122334'),
(2, 'Sarah Jenkins', 'sarah.j@yahoo.com', 'hashed_pass_4', '+27725556677');

INSERT INTO dbo.Events (OrganiserId, Title, Description, Location, EventDate)
VALUES 
(1, 'Soweto Marathon 2026', 'The People’s Race through historic Soweto.', 'Soweto, Johannesburg', '2026-11-01 06:00:00'),
(1, 'Cape Town Cycle Tour 2026', 'World famous scenic cycle race around the Cape Peninsula.', 'Cape Town', '2026-10-15 06:30:00'),
(2, 'Durban City Beach Walk', 'A community walk along the Durban beachfront promenade.', 'Durban Beachfront', '2026-12-05 08:00:00');

INSERT INTO dbo.EventCategories (EventId, CategoryName, DistanceKM, EntryFee)
VALUES 
(1, '42.2km Full Marathon', 42.20, 350.00),
(1, '21.1km Half Marathon', 21.10, 250.00),
(1, '10km Open Run', 10.00, 150.00),
(2, '109km Main Cycle Race', 109.00, 550.00),
(3, '5km Family Walk', 5.00, 80.00);

INSERT INTO dbo.Enrolments (ParticipantId, CategoryId, PaymentStatus)
VALUES 
(3, 1, 'Paid'),
(3, 4, 'Paid'),
(4, 2, 'Paid'),
(4, 5, 'Pending');

INSERT INTO dbo.Results (EnrolmentId, FinishTime, OverallPosition, CategoryPosition)
VALUES 
(1, '03:22:15', 45, 12),
(3, '01:55:40', 112, 34);
GO-- Database Schema Script for RaceDay System
