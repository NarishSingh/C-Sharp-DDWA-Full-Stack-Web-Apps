USE ShackUp;
GO

-- Cleans Db and fills with sample data
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'DbReset')
    DROP PROCEDURE DbReset
GO

CREATE PROCEDURE DbReset AS
BEGIN
    DELETE FROM Listings;
    DELETE FROM States;
    DELETE FROM BathroomTypes;
    DELETE
    FROM AspNetUsers
    WHERE Id IN ('00000000-0000-0000-0000-000000000000', '11111111-1111-1111-1111-111111111111');

    -- sample data
    INSERT INTO States(StateId, StateName)
    VALUES ('OH', 'Ohio'),
           ('KY', 'Kentucky'),
           ('MN', 'Minnesota');

    SET IDENTITY_INSERT BathroomTypes ON;
    INSERT INTO BathroomTypes(BathroomTypeId, BathroomTypeName)
    VALUES (1, 'Indoor'),
           (2, 'Outdoor'),
           (3, 'None');
    SET IDENTITY_INSERT BathroomTypes OFF;

    INSERT INTO AspNetUsers(Id, EmailConfirmed, PhoneNumberConfirmed, Email, StateId, TwoFactorEnabled, LockoutEnabled,
                            AccessFailedCount, UserName)
    VALUES ('00000000-0000-0000-0000-000000000000', 0, 0, 'test@test.com', 'OH', 0, 0, 0, 'test'),
           ('11111111-1111-1111-1111-111111111111', 0, 0, 'test2@test.com', 'OH', 0, 0, 0, 'test2');

    SET IDENTITY_INSERT Listings ON;
    INSERT INTO Listings(ListingID, UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric,
                         HasHeat, ImageFileName, ListingDescription)
    VALUES (1, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 1', 'Cleveland', 100, 400, 0, 1,
            'placeholder.png', 'Description'),
           (2, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 2', 'Cleveland', 110, 410, 0, 1,
            'placeholder.png', null),
           (3, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 3', 'Cleveland', 120, 420, 0, 1,
            'placeholder.png', null),
           (4, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 4', 'Cleveland', 130, 430, 0, 1,
            'placeholder.png',
            'Experience the chill of Cleveland winters in this leaky shack.  Poorly insulated and weather worn, you will truly appreciate returning home from your stay here.'),
           (5, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 5', 'Columbus', 140, 440, 0, 1,
            'placeholder.png', null),
           (6, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 6', 'Cleveland', 150, 450, 0, 1,
            'placeholder.png', null)
    SET IDENTITY_INSERT Listings OFF;
END
GO

EXEC DbReset;

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