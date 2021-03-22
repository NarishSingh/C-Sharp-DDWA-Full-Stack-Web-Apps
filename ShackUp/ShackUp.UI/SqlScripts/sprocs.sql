USE ShackUp;
GO

-- verify reset and data population
SELECT *
FROM States;
SELECT *
FROM BathroomTypes;
SELECT *
FROM AspNetUsers;
SELECT *
FROM Listings;
GO

-- StatesSelectAll
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'StatesSelectAll')
    DROP PROCEDURE StatesSelectAll
GO

CREATE PROCEDURE StatesSelectAll AS
BEGIN
    SELECT StateId, StateName
    FROM States
END
GO

-- BathroomTypesSelectAll
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'BathroomTypesSelectAll')
    DROP PROCEDURE BathroomTypesSelectAll
GO

CREATE PROCEDURE BathroomTypesSelectAll AS
BEGIN
    SELECT BathroomTypeId, BathroomTypeName
    FROM BathroomTypes
END
GO

-- ListingInsert
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingInsert')
    DROP PROCEDURE ListingInsert
GO

CREATE PROCEDURE ListingInsert(
    @ListingId INT OUTPUT,
    @UserId NVARCHAR(128),
    @StateId CHAR(2),
    @BathroomTypeId INT,
    @Nickname NVARCHAR(50),
    @City NVARCHAR(50),
    @Rate DECIMAL(7, 2),
    @SquareFootage DECIMAL(7, 2),
    @HasElectric BIT,
    @HasHeat BIT,
    @ListingDescription VARCHAR(500),
    @ImageFileName VARCHAR(50)
) AS
BEGIN
    INSERT INTO Listings(UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric,
                         HasHeat, ListingDescription, ImageFileName)
    VALUES (@UserId, @StateId, @BathroomTypeId, @Nickname, @City, @Rate, @SquareFootage, @HasElectric, @HasHeat,
            @ListingDescription, @ImageFileName);

    SET @ListingId = SCOPE_IDENTITY();
END
GO

-- ListingUpdate
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingUpdate')
    DROP PROCEDURE ListingUpdate
GO

CREATE PROCEDURE ListingUpdate(
    @ListingId INT,
    @UserId NVARCHAR(128),
    @StateId CHAR(2),
    @BathroomTypeId INT,
    @Nickname NVARCHAR(50),
    @City NVARCHAR(50),
    @Rate DECIMAL(7, 2),
    @SquareFootage DECIMAL(7, 2),
    @HasElectric BIT,
    @HasHeat BIT,
    @ListingDescription VARCHAR(500),
    @ImageFileName VARCHAR(50)
) AS
BEGIN
    UPDATE Listings
    SET UserId             = @UserId,
        StateId            = @StateId,
        BathroomTypeId     = @BathroomTypeId,
        Nickname           = @Nickname,
        City               = @City,
        Rate               = @Rate,
        SquareFootage      = @SquareFootage,
        HasElectric        = @HasElectric,
        HasHeat            = @HasHeat,
        ListingDescription = @ListingDescription,
        ImageFileName      = @ImageFileName
    WHERE ListingId = @ListingId;
END
GO

-- ListingDelete
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingDelete')
    DROP PROCEDURE ListingDelete
GO

CREATE PROCEDURE ListingDelete(
    @ListingID INT
) AS
BEGIN
    BEGIN TRANSACTION
        DELETE
        FROM Contacts
        WHERE ListingId = @ListingID;
        DELETE
        FROM Favorites
        WHERE ListingId = @ListingID;
        DELETE
        FROM Listings
        WHERE ListingID = @ListingID;
    COMMIT TRANSACTION
END
GO

-- ListingSelect
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingSelect')
    DROP PROCEDURE ListingSelect
GO

CREATE PROCEDURE ListingSelect(
    @ListingID INT
) AS
BEGIN
    SELECT ListingID,
           UserId,
           StateId,
           BathroomTypeId,
           Nickname,
           City,
           Rate,
           SquareFootage,
           HasElectric,
           HasHeat,
           ListingDescription,
           ImageFileName
    FROM Listings
    WHERE ListingID = @ListingID
END
GO