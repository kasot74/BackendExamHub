IF OBJECT_ID('dbo.NEWSID') IS NOT NULL
    DROP PROCEDURE [dbo].[NEWSID]
GO
CREATE PROCEDURE [dbo].[NEWSID]
(
    @TableName nvarchar(128),
    @ReturnSID nvarchar(20) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON

	
	DECLARE
	@SIDRowName NVARCHAR(20)


    DECLARE 
        @currentYear int,
        @dayOfYear int,
        @secondOfDay int,
        @alphabets char(36),
        @firstDigit char(1),
        @secondDigit char(1),
        @prefix char(2),
        @dayCode char(3),
        @secondCode char(5),
        @sql nvarchar(MAX),
        @randomValue char(10),
        @ParmDefinition nvarchar(500)

    DECLARE @tempTable TABLE
    (
        SID CHAR(20)
    );

    SET @currentYear = YEAR(GETDATE()) - 2000;
    SET @dayOfYear = DATEPART(DAYOFYEAR, GETDATE());
    SET @secondOfDay = DATEPART(SECOND, GETDATE()) + (60 * DATEPART(MINUTE, GETDATE())) + (3600 * DATEPART(HOUR, GETDATE()));
    IF (@currentYear > 1295)
    BEGIN
        SET @currentYear = 1295;
    END;
    SET @alphabets = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    SET @firstDigit = SUBSTRING(@alphabets, (@currentYear / 36) % 36 + 1, 1);
    SET @secondDigit = SUBSTRING(@alphabets, @currentYear % 36 + 1, 1);
    SET @prefix = @firstDigit + @secondDigit;
    SET @dayCode = RIGHT('000' + CONVERT(VARCHAR, @dayOfYear), 3);
    SET @secondCode = RIGHT('00000' + CONVERT(VARCHAR, @secondOfDay), 5);

    -- 尋找 Table 的欄位
    SELECT 
        TOP 1 
        @SIDRowName = STUFF((
            SELECT ', ' + c.name 
            FROM sys.index_columns ic
            JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
            ORDER BY ic.key_ordinal
            FOR XML PATH('')
        ), 1, 2, '') 
    FROM sys.indexes i 
    WHERE i.object_id = OBJECT_ID(@TableName) AND i.index_id > 0  -- 排除堆

    -- 尋找預設的
    WHILE 1 = 1
    BEGIN

        SET @randomValue = RIGHT('0000000000' + CAST(ABS(CAST(CAST(NEWID() AS BINARY(5)) AS BIGINT)) % 10000000000 AS VARCHAR(10)), 10);
        SET @ReturnSID = @prefix + @dayCode + @secondCode + @randomValue;
        SET @sql = N'SELECT @SIDRowNameOUT = ' + QUOTENAME(@SIDRowName) + ' FROM ' + QUOTENAME(@TableName) + ' WHERE ' + QUOTENAME(@SIDRowName) + ' = @ReturnSIDIN';
        SET @ParmDefinition = N'@ReturnSIDIN nvarchar(20), @SIDRowNameOUT nvarchar(20) OUTPUT';

        DELETE FROM @tempTable;
        
        DECLARE @SIDRowNameOUT nvarchar(20)
        EXEC sp_executesql @sql, @ParmDefinition, @ReturnSIDIN = @ReturnSID, @SIDRowNameOUT = @SIDRowNameOUT OUTPUT;
        
        IF @SIDRowNameOUT IS NULL
            BREAK;
    END;
END