INSERT INTO Releases(Application, Comments, Created, CreatedBy, Title)
VALUES (@application, @comments, @created, @createdBy, @title);

SELECT SCOPE_IDENTITY();