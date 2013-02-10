DECLARE @UserId int
DECLARE @nccbFamilyId int
DECLARE @FamilyId int
DECLARE @PersonId int
DECLARE @nccbRoleId int
DECLARE @roleId int
DECLARE @statusCode NVARCHAR(50)
DECLARE @anniversary DateTime

DECLARE @pa AS NVARCHAR(255)
DECLARE @address AS NVARCHAR(255)
DECLARE @ordinal int
DECLARE @oldOrdinal int
DECLARE @address1 AS NVARCHAR(255)
DECLARE @address2 AS NVARCHAR(255)
DECLARE @address3 AS NVARCHAR(255)
DECLARE @address4 AS NVARCHAR(255)
DECLARE @addressId AS INT


DECLARE user_cursor CURSOR FOR
SELECT UserId FROM nccb.UserInfo

OPEN user_cursor;

FETCH NEXT FROM user_cursor INTO @UserId;

WHILE @@FETCH_STATUS = 0
BEGIN
   --Check for Family Record
   SELECT @nccbFamilyId = FamilyId FROM nccb.FamilyMember WHERE UserId = @UserId
   IF(@nccbFamilyId IS NOT NULL)
   BEGIN
	   SELECT @anniversary = DateOfMarriage FROM nccb.Family WHERE FamilyID = @nccbFamilyId
	   SELECT @FamilyId = FamilyId FROM nccb.FamilyLink WHERE NccbFamilyId = @nccbFamilyId
	   PRINT @FamilyId
	   IF(@FamilyId IS NULL)
	   BEGIN
		   --First create address
		   SELECT @pa = PhysicalAddress FROM [nccb].[Family] WHERE FamilyId = @nccbFamilyId
	       
		   SET @ordinal = 0
			SET @oldOrdinal = -1
			SET @address1 = NULL
			SET @address2 = NULL
			SET @address3 = NULL
			set @address4 = NULL
			set @address = NULL
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
				  ELSE IF (@address4 IS NULL)
					 SET @address4 = @address
				END
			END
			
			INSERT INTO Address (Line1, Line2, Line3, Line4)
			VALUES (ISNULL(@address1, ''), ISNULL(@address2, ''), ISNULL(@address3, ''), ISNULL(@address4, ''))
	       
		   SELECT @addressId = SCOPE_IDENTITY()
	       
		   INSERT INTO dbo.Family (FamilyName, HomePhone, Anniversary, AddressId) 
		   SELECT u.Surname, f.hometel, f.DateOfMarriage, @addressId 
		   FROM nccb.Family f 
		   JOIN nccb.FamilyMember fm
		   on f.FamilyId = fm.FamilyId
		   JOIN nccb.UserInfo u
		   ON fm.UserId = u.UserId
		   WHERE u.UserId = @UserId
	       
		   SELECT @FamilyId = SCOPE_IDENTITY()
	       
		   INSERT INTO nccb.FamilyLink (FamilyId, nccbFamilyId)
		   VALUES (@FamilyId, @nccbFamilyId)
	       
	   END

	   --Create person
	   INSERT INTO Person(Firstname, Email, DateOfBirth, Occupation, FamilyId, Anniversary)
	   SELECT Firstname, EmailAddress, DateOfBirth, Occupation, @FamilyId, @anniversary
	   FROM nccb.UserInfo
	   WHERE UserId = @UserId
	   
	   SELECT @personId = SCOPE_IDENTITY()
	   
	   SELECT @statusCode = StatusCode from nccb.UserInfo
	   WHERE UserId = @UserId
	   		   
	   SET @roleId = 79
		   
	   IF(@statusCode = 'Left' or @statusCode = 'InActive')
	   BEGIN
		   IF(@statusCode = 'Left')
			  SET @roleId = 78
		   IF(@statusCode = 'InActive')
			  SET @roleId = 199
	   END
	   ELSE
	   BEGIN
	   
		   SELECT TOP 1 @nccbRoleId = RoleId FROM nccb.UserRole where UserID = @UserId
		   ORDER BY UserRoleID Desc

		   IF(@nccbRoleId = 104 OR @nccbRoleId = 103 OR @nccbRoleId = 11 OR @nccbRoleId = 10 OR @nccbRoleId = 9 OR @nccbRoleId = 6 OR @nccbRoleId = 5)
			   SET @roleId = 74
		   IF(@nccbRoleId = 102 OR @nccbRoleId = 101 OR @nccbRoleId = 100 OR @nccbRoleId = 7 OR @nccbRoleId = 3)
			   SET @roleId = 73
		   IF(@nccbRoleId = 8 OR @nccbRoleId = 4)
			   SET @roleId = 80
		   IF(@nccbRoleId = 2)
			   SET @roleId = 75
	   END
	   
	   --Create personchurch
	   INSERT INTO PersonChurch (ChurchId, PersonId, RoleId)
	   VALUES (11, @PersonId, @roleId)
	   
	   --create personoptionalfields
	   INSERT INTO PersonOptionalField (PersonId, OptionalFieldId, Value)
	   SELECT @PersonId, 2, ISNULL(WorkTel, '')
	   FROM nccb.UserInfo WHERE UserId = @UserId
	   
	   INSERT INTO PersonOptionalField (PersonId, OptionalFieldId, Value)
	   SELECT @PersonId, 3, ISNULL(MobileTel, '')
	   FROM nccb.UserInfo WHERE UserId = @UserId
	     
	   INSERT INTO PersonOptionalField (PersonId, OptionalFieldId, Value)
	   SELECT @PersonId, 7, ISNULL(Occupation, '')
	   FROM nccb.UserInfo WHERE UserId = @UserId
	   
	   INSERT INTO PersonOptionalField (PersonId, OptionalFieldId, Value)
	   SELECT @PersonId, 14, 
	       CASE Gender 
	           WHEN 'M' THEN 'Male'
	           WHEN 'F' THEN 'Female'
	           ELSE ''
	       END
	   FROM nccb.UserInfo WHERE UserId = @UserId

   END
   SET @FamilyId = NULL
   SET @nccbFamilyId = NULL
   FETCH NEXT FROM user_cursor INTO @UserId;
END

CLOSE user_cursor;
DEALLOCATE user_cursor;
GO