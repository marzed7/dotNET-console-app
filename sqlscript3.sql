CREATE TRIGGER InsertPostOnCreate
ON Posts
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PostId INT
    SELECT @PostId = Id FROM INSERTED

    UPDATE Posts
    SET CreatedDateTime = GETDATE()
    WHERE Id = @PostId
END;
