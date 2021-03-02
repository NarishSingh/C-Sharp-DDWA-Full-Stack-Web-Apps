USE master;
GO

IF EXISTS(SELECT *
          FROM sys.databases
          WHERE name = 'MovieCatalog')
    DROP DATABASE MovieCatalog
GO

CREATE DATABASE MovieCatalog
GO

-- Drops
USE MovieCatalog
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Movie')
    DROP TABLE Movie
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Rating')
    DROP TABLE Rating
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Genre')
    DROP TABLE Genre
GO

-- Create tables
CREATE TABLE Genre
(
    GenreId   INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    GenreType VARCHAR(50)                    NOT NULL
);

CREATE TABLE Rating
(
    RatingId   INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    RatingName VARCHAR(50)                    NOT NULL
);

-- Rating fk is nullable
CREATE TABLE Movie
(
    MovieId  INT IDENTITY (1,1) PRIMARY KEY                 NOT NULL,
    RatingId INT FOREIGN KEY REFERENCES Rating (RatingId)   NULL,
    GenreId  INT FOREIGN KEY REFERENCES Genre (GenreId) NOT NULL,
    Title    VARCHAR(50)                                    NOT NULL
);