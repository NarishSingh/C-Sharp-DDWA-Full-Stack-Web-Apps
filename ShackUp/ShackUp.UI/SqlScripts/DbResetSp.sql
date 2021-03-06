﻿USE ShackUp;
GO

-- Cleans Db and fills with sample data
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'DbReset')
    DROP PROCEDURE DbReset
GO

CREATE PROCEDURE DbReset AS
BEGIN
    DELETE FROM Favorites;
    DELETE FROM Contacts;
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

    -- we need to reseed the identity column, so that it begins from 1 again
    DBCC CHECKIDENT ( 'Listings', RESEED , 1);
    SET IDENTITY_INSERT Listings ON;
    INSERT INTO Listings(ListingID, UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric,
                         HasHeat, ImageFileName, ListingDescription)
    VALUES (1, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 1', 'Cleveland', 100, 400, 0, 1,
            'placeholder.jpg', 'Description'),
           (2, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 2', 'Cleveland', 110, 410, 0, 1,
            'placeholder.jpg', null),
           (3, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 3', 'Cleveland', 120, 420, 0, 1,
            'placeholder.jpg', null),
           (4, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 4', 'Cleveland', 130, 430, 0, 1,
            'placeholder.jpg',
            'Experience the chill of Cleveland winters in this leaky shack.  Poorly insulated and weather worn, you will truly appreciate returning home from your stay here.'),
           (5, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 5', 'Columbus', 140, 440, 0, 1,
            'placeholder.jpg', null),
           (6, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test shack 6', 'Cleveland', 150, 450, 0, 1,
            'placeholder.jpg', null)
    SET IDENTITY_INSERT Listings OFF;

    INSERT INTO Favorites(ListingId, UserId)
    VALUES (1, '11111111-1111-1111-1111-111111111111'),
           (2, '11111111-1111-1111-1111-111111111111');

    INSERT INTO Contacts(ListingId, UserId)
    VALUES (1, '11111111-1111-1111-1111-111111111111'),
           (3, '11111111-1111-1111-1111-111111111111');
END
GO

EXEC DbReset;