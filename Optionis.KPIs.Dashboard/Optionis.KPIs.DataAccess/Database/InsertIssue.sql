INSERT INTO Issues(ReleaseId, IssueId, Link, Title)
VALUES (@releaseId, @issueId, @link, @title);

SELECT SCOPE_IDENTITY();