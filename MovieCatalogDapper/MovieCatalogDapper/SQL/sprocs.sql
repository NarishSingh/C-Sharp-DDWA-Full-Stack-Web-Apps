USE MovieCatalog;
GO

-- Read all movies
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'MovieSelectAll')
    DROP PROCEDURE MovieSelectAll
GO

CREATE PROCEDURE MovieSelectAll
AS
SELECT M.MovieId, M.Title, G.GenreType, R.RatingName
FROM Movie M
         JOIN Genre G on G.GenreId = M.GenreId
         LEFT JOIN Rating R ON M.RatingId = R.RatingId
ORDER BY Title
GO

-- Read by Id
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'MovieSelectById')
    DROP PROCEDURE MovieSelectById
GO

CREATE PROCEDURE MovieSelectById(
    @MovieId INT
)
AS
SELECT MovieId, Title, GenreId, RatingId
FROM Movie
WHERE MovieId = @MovieId
GO

-- Create Movie
-- We can create output parameters for our SP's
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'MovieInsert')
    DROP PROCEDURE MovieInsert
GO

CREATE PROCEDURE MovieInsert(
    @MovieId INT OUTPUT,
    @GenreId INT,
    @RatingId INT,
    @Title VARCHAR(50)
)
AS
INSERT INTO Movie(GenreId, RatingId, Title)
VALUES (@GenreId, @RatingId, @Title)

    SET @MovieId = SCOPE_IDENTITY()
GO

-- Update movie
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'MovieUpdate')
    DROP PROCEDURE MovieUpdate
GO

CREATE PROCEDURE MovieUpdate(
    @MovieId INT,
    @GenreId INT,
    @RatingId INT,
    @Title VARCHAR(50)
)
AS
UPDATE Movie
SET GenreId  = @GenreId,
    RatingID = @RatingId,
    Title    = @Title
WHERE MovieId = @MovieId
GO

-- Delete Movie
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'MovieDelete')
    DROP PROCEDURE MovieDelete
GO

CREATE PROCEDURE MovieDelete(
    @MovieId INT
)
AS
DELETE
FROM Movie
WHERE MovieId = @MovieId
GO

-- Read all rating
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'RatingSelectAll')
    DROP PROCEDURE RatingSelectAll
GO

CREATE PROCEDURE RatingSelectAll
AS
SELECT RatingId, RatingName
FROM Rating
ORDER BY RatingName
GO

-- Read all genre
IF EXISTS(SELECT *
          FROM INFORMATION_SCHEMA.ROUTINES
          WHERE ROUTINE_NAME = 'GenreSelectAll')
    DROP PROCEDURE GenreSelectAll
GO

CREATE PROCEDURE GenreSelectAll
AS
SELECT GenreId, GenreType
FROM Genre
ORDER BY GenreType
GO