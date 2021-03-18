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

    INSERT INTO States(StateId, StateName)
    VALUES ('OH', 'Ohio'),
           ('KY', 'Kentucky'),
           ('MN', 'Minnesota');
END
GO

EXEC DbReset;

SELECT *
FROM States;
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