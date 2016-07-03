INSERT INTO Users(Created, UserName)
VALUES (@created, @userName);

SELECT SCOPE_IDENTITY();