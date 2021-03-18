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
    DELETE FROM States;
    DELETE FROM BathroomTypes;

    INSERT INTO States(StateId, StateName)
    VALUES ('OH', 'Ohio'),
           ('KY', 'Kentucky'),
           ('MN', 'Minnesota');
    INSERT INTO BathroomTypes(BathroomTypeName)
    VALUES ('Indoor'),
           ('Outdoor'),
           ('None');
END
GO

EXEC DbReset;

-- verify reset and data population
SELECT *
FROM States;
SELECT *
FROM BathroomTypes;
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