USE Northwind;
GO

-- no param
CREATE PROCEDURE sp_ProductsToReorder
AS
SELECT ProductName, UnitsInStock
FROM Products
WHERE UnitsInStock < 5
ORDER BY UnitsInStock DESC, ProductName
GO

EXEC sp_ProductsToReorder

-- params
CREATE PROCEDURE sp_ProductsToReorder2(
    @UnitsInStock INT
)
AS
SELECT ProductName, UnitsInStock
FROM Products
WHERE UnitsInStock < @UnitsInStock
ORDER BY UnitsInStock DESC, ProductName
GO

EXEC sp_ProductsToReorder2 4

-- alter an sp
ALTER PROCEDURE sp_ProductsToReorder
AS
SELECT ProductName, UnitsInStock
FROM Products
WHERE UnitsInStock < 10
ORDER BY UnitsInStock DESC, ProductName
GO

EXEC sp_ProductsToReorder

-- drops
-- exists...drop...create pattern is sometimes preferred over using alters
IF EXISTS(
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'ProductsToReorder2'
    )
BEGIN
    DROP PROCEDURE ProductsToReorder2
END
GO

CREATE PROCEDURE ProductsToReorder2(
    @UnitsInStock int
)
AS

SELECT ProductName, UnitsInStock
FROM Products
WHERE UnitsInStock < @UnitsInStock
ORDER BY UnitsInStock DESC, ProductName
GO