USE MovieCatalog;
GO

-- We can create output parameters for our SP's
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