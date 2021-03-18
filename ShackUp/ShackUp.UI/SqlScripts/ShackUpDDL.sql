USE master;
GO

IF EXISTS(SELECT *
          FROM sys.databases
          WHERE name = 'ShackUp')
    DROP DATABASE ShackUp;
GO

CREATE DATABASE ShackUp;
GO

USE ShackUp;
GO

-- STATES
IF EXISTS(SELECT *
          FROM sys.tables
          WHERE name = 'States')
    DROP TABLE States
GO

CREATE TABLE States
(
    StateId   CHAR(2) PRIMARY KEY NOT NULL,
    StateName VARCHAR(15)         NOT NULL
);

-- BATHROOM TYPES
IF EXISTS(SELECT *
          FROM sys.tables
          WHERE name = 'BathroomTypes')
    DROP TABLE BathroomTypes
GO

CREATE TABLE BathroomTypes
(
    BathroomTypeId   INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    BathroomTypeName VARCHAR(15)                    NOT NULL
);

-- LISTINGS
IF EXISTS(SELECT *
          FROM sys.tables
          WHERE name = 'Listings')
    DROP TABLE Listings
GO

-- NVARCHAR coming from AspNetUsers, not a FK as if ASP Id changes, this can change. Also, if the user is deleted you get to keep their data
CREATE TABLE Listings
(
    ListingID          INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    UserId             NVARCHAR(128)                  NOT NULL,
    StateId            CHAR(2)                        NOT NULL,
    BathroomTypeId     INT                            NULL FOREIGN KEY
        REFERENCES BathroomTypes (BathroomTypeId),              -- nullable FK
    Nickname           NVARCHAR(50)                   NOT NULL,
    City               NVARCHAR(50)                   NOT NULL,
    Rate               DECIMAL(7, 2)                  NOT NULL, -- max $99,999.99
    SquareFootage      DECIMAL(7, 2)                  NOT NULL,
    HasElectric        BIT                            NOT NULL,
    HasHeat            BIT                            NOT NULL,
    ListingDescription VARCHAR(500)                   NULL,
    ImageFileName      VARCHAR(50)                    NULL,
    CreatedDate        DATETIME2                      NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT Fk_State_Listings FOREIGN KEY (StateId)
        REFERENCES States (StateId)
);

-- Contacts bridge table
IF EXISTS(SELECT *
          FROM sys.tables
          WHERE name = 'Contacts')
    DROP TABLE Contacts
GO

CREATE TABLE Contacts
(
    ListingId INT           NOT NULL,
    UserId    NVARCHAR(128) NOT NULL,
    PRIMARY KEY (ListingId, UserId),
    CONSTRAINT Fk_Listings_Contacts FOREIGN KEY (ListingId)
        REFERENCES Listings (ListingID)
);

-- Favorites bridge table
IF EXISTS(SELECT *
          FROM sys.tables
          WHERE name = 'Favorites')
    DROP TABLE Favorites
GO

CREATE TABLE Favorites
(
    ListingId INT           NOT NULL,
    UserId    NVARCHAR(128) NOT NULL,
    PRIMARY KEY (ListingId, UserId),
    CONSTRAINT Fk_Listings_Favorites FOREIGN KEY (ListingId)
        REFERENCES Listings (ListingID)
);