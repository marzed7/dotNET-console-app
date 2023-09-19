CREATE PROCEDURE GetPostTitleAndBody
    @PostId INT
AS
BEGIN
    DECLARE @PostTitle NVARCHAR(MAX)
    DECLARE @PostBody NVARCHAR(MAX)

    SELECT @PostTitle = Title, @PostBody = Body
    FROM Posts
    WHERE Id = @PostId

    IF @PostTitle IS NOT NULL
    BEGIN
        SELECT @PostTitle AS PostTitle, @PostBody AS PostBody
    END
    ELSE
    BEGIN
        RAISEERROR ('Post not found.', 16, 1)
    END
END;
