WITH LatestComments AS (
    SELECT
        PostId,
        MAX(CreatedDateTime) AS LatestCommentDate
    FROM
        Comments
    GROUP BY
        PostId
)

SELECT
    P.Id AS PostId,
    P.Title AS PostTitle,
    P.Body AS PostBody,
    U.Id AS UserId,
    U.Name AS UserName
FROM
    Posts AS P
INNER JOIN
    Users AS U ON P.UserId = U.Id
INNER JOIN
    LatestComments AS LC ON P.Id = LC.PostId
INNER JOIN
    Comments AS C ON LC.PostId = C.PostId AND LC.LatestCommentDate = C.CreatedDateTime
ORDER BY
    LC.LatestCommentDate DESC;
