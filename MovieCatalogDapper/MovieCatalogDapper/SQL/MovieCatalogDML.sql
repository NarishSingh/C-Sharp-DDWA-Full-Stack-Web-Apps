USE MovieCatalog;
GO

INSERT INTO Genre(GenreType)
VALUES ('Action'),
       ('Horror'),
       ('Kids'),
       ('Mystery'),
       ('Romance'),
       ('Sci-Fi');

INSERT INTO Rating (RatingName)
VALUES ('G'),
       ('PG'),
       ('PG-13'),
       ('R');

INSERT INTO Movie (RatingId, GenreId, Title)
VALUES (1, 3, 'The Lion King'),
       (4, 6, 'Terminator'),
       (4, 2, 'Friday the 13th'),
       (null, 6, 'This movie has no rating');

SELECT * FROM Rating;
SELECT * FROM Genre;
SELECT * FROM Movie;