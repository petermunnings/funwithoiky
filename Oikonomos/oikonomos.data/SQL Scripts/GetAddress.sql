/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @pa AS NVARCHAR(255)
DECLARE @address AS NVARCHAR(255)
DECLARE @ordinal int
DECLARE @oldOrdinal int
DECLARE @address1 AS NVARCHAR(255)
DECLARE @address2 AS NVARCHAR(255)
DECLARE @address3 AS NVARCHAR(255)

SELECT @pa = PhysicalAddress FROM [oikonomos].[nccb].[Family] WHERE FamilyId = 5
PRINT @pa

SET @ordinal = 0
SET @oldOrdinal = -1
WHILE(@ordinal IS NOT NULL)
BEGIN
	SELECT TOP 1 @Ordinal = Ordinal, @address = StringValue FROM dbo.Split (@pa, '\r\n') WHERE Ordinal > @ordinal
	IF(@oldOrdinal = @ordinal)
	BEGIN
	  SET @ordinal = NULL
	END
	ELSE
	BEGIN 
	  SET @oldOrdinal = @ordinal
	  IF(@address1 IS NULL)
	     SET @address1 = @address
	  ELSE IF(@address2 IS NULL)
	     SET @address2 = @address
	  ELSE IF (@address3 IS NULL)
	     SET @address3 = @address
	END
END

PRINT @address1
PRINT @address2
PRINT @address3

