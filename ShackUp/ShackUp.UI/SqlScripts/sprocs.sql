USE ShackUp;
GO

-- verify reset and data population
/*
SELECT *
FROM States;
SELECT *
FROM BathroomTypes;
SELECT *
FROM AspNetUsers;
SELECT *
FROM Listings;
GO
 */

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

-- ListingSelectRecent
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingsSelectRecent')
    DROP PROCEDURE ListingsSelectRecent
GO

CREATE PROCEDURE ListingsSelectRecent AS
BEGIN
    SELECT TOP 5 ListingID, UserId, Rate, City, StateId, ImageFileName
    FROM Listings
    ORDER BY CreatedDate DESC
END
GO

-- ListingSelectDetails
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingsSelectDetails')
    DROP PROCEDURE ListingsSelectDetails
GO

CREATE PROCEDURE ListingsSelectDetails(
    @ListingId INT
) AS
BEGIN
    SELECT ListingID,
           UserId,
           Nickname,
           City,
           StateId,
           Rate,
           SquareFootage,
           HasElectric,
           HasHeat,
           L.BathroomTypeId,
           BathroomTypeName,
           ImageFileName,
           L.ListingDescription
    FROM Listings L
             INNER JOIN BathroomTypes BT ON L.BathroomTypeId = BT.BathroomTypeId
    WHERE ListingID = @ListingId
END
GO

-- ListingsSelectFavorites
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingsSelectFavorites')
    DROP PROCEDURE ListingsSelectFavorites
GO

CREATE PROCEDURE ListingsSelectFavorites(
    @UserId NVARCHAR(128)
) AS
BEGIN
    SELECT L.ListingID,
           L.City,
           L.StateId,
           L.Rate,
           L.SquareFootage,
           L.UserId,
           L.HasElectric,
           L.HasHeat,
           L.BathroomTypeId,
           BT.BathroomTypeName
    FROM Favorites F
             INNER JOIN Listings L on L.ListingID = F.ListingId
             INNER JOIN BathroomTypes BT on BT.BathroomTypeId = L.BathroomTypeId
    WHERE F.UserId = @UserId
END
GO

-- ListingsSelectContacts
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingsSelectContacts')
    DROP PROCEDURE ListingsSelectContacts
GO

CREATE PROCEDURE ListingsSelectContacts(
    @UserId NVARCHAR(128)
) AS
BEGIN
    SELECT L.ListingID,
           U.Email,
           U.Id AS UserId,
           L.Nickname,
           L.City,
           L.StateId,
           L.Rate
    FROM Listings L
             INNER JOIN Contacts C ON L.ListingID = C.ListingId
             INNER JOIN AspNetUsers U ON C.UserId = U.Id
    WHERE L.UserId = @UserId
END
GO

-- ListingsSelectContacts
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'ListingsSelectByUser')
    DROP PROCEDURE ListingsSelectByUser
GO

CREATE PROCEDURE ListingsSelectByUser(
    @UserId NVARCHAR(128)
) AS
BEGIN
    SELECT ListingID,
           UserId,
           Nickname,
           City,
           StateId,
           Rate,
           SquareFootage,
           HasElectric,
           HasHeat,
           L.BathroomTypeId,
           BathroomTypeName,
           ImageFileName,
           L.ListingDescription
    FROM Listings L
             INNER JOIN BathroomTypes BT ON L.BathroomTypeId = BT.BathroomTypeId
    WHERE UserId = @UserId
END
GO

-- FavoritesInsert
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'FavoritesInsert')
    DROP PROCEDURE FavoritesInsert
GO

CREATE PROCEDURE FavoritesInsert(
    @UserId NVARCHAR(128),
    @ListingId INT
) AS
BEGIN
    INSERT INTO Favorites(ListingId, UserId)
    VALUES (@ListingId, @UserId);
END
GO

-- FavoritesDelete
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'FavoritesDelete')
    DROP PROCEDURE FavoritesDelete
GO

CREATE PROCEDURE FavoritesDelete(
    @UserId NVARCHAR(128),
    @ListingId INT
) AS
BEGIN
    DELETE
    FROM Favorites
    WHERE UserId = @UserId
      AND ListingId = @ListingId
END
GO