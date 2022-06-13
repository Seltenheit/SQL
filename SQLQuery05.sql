/* Filter employees by territories 


SELECT EmployeeID as id, COUNT(*) as territory
FROM EmployeeTerritories
GROUP BY EmployeeID
HAVING COUNT(*) > 1

Finding max territory one employee takes */

SELECT MAX(tmp.territory) as 'Max territory'
FROM
(
SELECT EmployeeID as id, COUNT(*) as territory
FROM EmployeeTerritories
GROUP BY EmployeeID
HAVING COUNT(*) > 1
) tmp
