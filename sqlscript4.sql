SELECT
    U.Id AS UserId,
    U.Name AS UserName,
    COUNT(P.Id) AS TotalPosts
FROM
    Users AS U
LEFT JOIN
    Posts AS P ON U.Id = P.UserId
GROUP BY
    U.Id, U.Name
ORDER BY
    TotalPosts DESC
LIMIT 5;
