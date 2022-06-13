/* Goal: Find products which order value is more than 50 */
SELECT DISTINCT Products.ProductID as 'ID', Products.ProductName as 'Name',
Products.UnitPrice as 'Unit price', [Order Details].UnitPrice 'Order price'
FROM Products, [Order Details]
WHERE [Order Details].UnitPrice > 50 AND [Order Details].ProductID = Products.ProductID; 