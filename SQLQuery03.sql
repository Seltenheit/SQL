/* SELECT column_name, column_name2 FROM table_name [WHERE expression]
SELECT * FROM 
* - all columns 
SELECT ProductName, UnitPrice FROM Products 
SELECT Products.ProductName FROM Products 
SELECT ProductName, UnitPrice FROM Products WHERE NOT(UnitPrice > 100) 
occurrence of strings:
SELECT ProductName, UnitPrice FROM Products WHERE ProductName LIKE '%tt%' 
SELECT ProductName, UnitPrice FROM Products WHERE CategoryID = 3 AND UnitPrice > 15 
SELECT ProductName AS 'Product Name', UnitPrice * UnitsInStock AS 'Total stock value' FROM Products WHERE UnitsInStock > 0 
SELECT DISTINCT CustomerID FROM Orders 
SELECT COUNT(DISTINCT CustomerID) FROM Orders */
